using Graphing.Planet;
using Graphing.Position;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Graphing.Planet;
public class MeshGraphConverter
{    
    public static PositionGraph CreateGraph(ProceduralMeshBuilder pmb, bool createDual = false)
    {
        return CreateGraph(pmb.Verticies.ToArray(), pmb.Triangles.ToArray(), createDual);
    }
    private class PositionTwinEdgeComparar : IEqualityComparer<PositionEdge>
    {
        public bool Equals(PositionEdge x, PositionEdge y)
        {
            var xNow = x.Node;
            var yNow = y.Node;
            var xNext = x.Next.Node;
            var yNext = y.Next.Node;
            return (xNow == yNext && xNext == yNow);
        }

        public int GetHashCode(PositionEdge obj)
        {
            var objNow = obj.Node;
            var objNext = obj.Next.Node;
            return objNow.GetHashCode() ^ objNext.GetHashCode();
        }
    }
    public static PositionGraph CreateGraph(ProceduralVertex[] mv, ProceduralTriangle[] tris, bool createDual = false)
    {
        //KeyValuePair<long, int>[] dEdgeLookup = new KeyValuePair<long, int>[tris.Length * 3];
        //long[] dTwin = new long[tris.Length * 3];
        PositionGraph graph = new PositionGraph(mv.Length, tris.Length * 3, tris.Length);
        Dictionary<PositionEdge, int> TwinLookup = new Dictionary<PositionEdge, int>(new PositionTwinEdgeComparar());
        //Initialize Nodes
        for (int i = 0; i < mv.Length; i++)
            graph.Nodes.Add(new PositionNode());
        //Initialize Polygons and Edges
        for (int i = 0; i < tris.Length; i++)
        {
            graph.Polys.Add(new PositionPoly());
            for (int j = 0; j < 3; j++)
                graph.Edges.Add(new PositionEdge());
        }
        //Finalize Nodes (Stage 1)
        for (int i = 0; i < mv.Length; i++)
        {
            var node = graph.Nodes[i];
            node.Identity = i;
            node.Position = mv[i].Position;
        }
        //Finalize Polygons and Nodes (Stage 2)
        for (int i = 0; i < tris.Length; i++)
        {
            var poly = graph.Polys[i];
            poly.Identity = i;
            ProceduralTriangle triangle = tris[i];
            for (int j = 0; j < 3; j++)
            {
                var edge = graph.Edges[i * 3 + (j + 0) % 3];
                var nextEdge = graph.Edges[i * 3 + (j + 2) % 3];
                var prevEdge = graph.Edges[i * 3 + (j + 1) % 3];
                var node = graph.Nodes[triangle[j]];

                edge.Identity = i * 3 + j;

                prevEdge.Next = nextEdge.Prev = poly.Edge = node.Edge = edge;
                edge.Next = nextEdge;
                edge.Prev = prevEdge;
                edge.Poly = poly;
                edge.Node = node;

            }
            for (int j = 0; j < 3; j++)
            {
                var edge = graph.Edges[i * 3 + (j + 0) % 3];
                int value;
                if (!TwinLookup.TryGetValue(edge, out value))
                {
                    TwinLookup.Add(edge, i * 3 + j);
                }
                else
                {
                    var twin = graph.Edges[value];
                    twin.Twin = edge;
                    edge.Twin = twin;
                }
            }
        }
        //AssertPMBGraph(graph);
        //Debug.Log("Before Possible Dual : " + graph.Nodes.Count + ", " + graph.Edges.Count + ", " + graph.Polys.Count);
        if (createDual)
        {
            var tempGraph = graph.Dual<PositionGraph>();
            //AssertPMBGraph(tempGraph);
            //AssertDualGraph(graph, tempGraph);
            graph = tempGraph;
            //Debug.Log("After Dual : " + graph.Nodes.Count + ", " + graph.Edges.Count + ", " + graph.Polys.Count);
        }

        //foreach (var node in graph.Nodes)
        //    Debug.Log("Node : " + node.Position);
        //foreach (var edge in graph.Edges)
        //    Debug.Log("Edge : " + edge.Center);
        //foreach (var poly in graph.Polys)
        //    Debug.Log("Poly : " + poly.Center);


        return graph;
    }
    public static PositionGraph FetchGraph(int subdivisions, ShapeType shape = ShapeType.Icosahedron, bool slerp = false, bool createDual = false, bool clean = false)
    {
        string fName = (createDual ? "Dual" : "") + (slerp ? "Slerped" : "NormalLerped") + shape.ToString() + "_D" + subdivisions;
        string fPath = Application.persistentDataPath;
        string fExt = ".pos";
        string fullPath = Path.ChangeExtension(Path.Combine(fPath, fName), fExt);
        PositionGraph graph;
        if (File.Exists(fullPath) && !clean)
        {
            Debug.Log("Loaded : " + fullPath);
            graph = new PositionGraph();
            using (var fStream = new FileStream(fullPath, FileMode.Open))
            {
                using (var bReader = new BinaryReader(fStream))
                {
                    graph.Deserialize(bReader);
                }
            }
        }
        else
        {
            Debug.Log("Saved : " + fullPath);
            graph = CreateGraph(subdivisions, shape, slerp, createDual);
            using (var fStream = new FileStream(fullPath, FileMode.Create))
            {
                using (var bWriter = new BinaryWriter(fStream))
                {
                    graph.Serialize(bWriter);
                }
            }
        }
        return graph;
    }
    private static PositionGraph CreateGraph(int subdivisions, ShapeType shape, bool slerp = false, bool createDual = false)
    {
        ProceduralMeshBuilder pmb = new ProceduralMeshBuilder();
        ProceduralPlatonicSolidGenerator.AddToMeshBuilder(pmb, shape, RadiusType.Normalized);
        ProceduralMeshUtility.Subdivide(pmb, subdivisions, slerp);
        if (!slerp)
            ProceduralMeshUtility.Spherize(pmb);//Normalizing means we spherize the graph, this puts the normalized, in normalized slerp
        return CreateGraph(pmb, createDual);
    }



    public static PlanetGraph FetchEmptyPlanetGraph(int subdivisions, ShapeType shape = ShapeType.Icosahedron, bool slerp = false, bool createDual = false, int partitionSize = 32, bool clean = false)
    {
        return CreateEmptyPlanetGraph(FetchGraph(subdivisions, shape, slerp, createDual, clean), partitionSize);
    }
    public static PlanetGraph CreateEmptyPlanetGraph(PositionGraph position, int partitionSize = 32)
    {
        int nCount = position.Nodes.Count, eCount = position.Edges.Count, pCount = position.Polys.Count, partCount = pCount / partitionSize + (pCount % partitionSize > 0 ? 1 : 0);
        PlanetGraph graph = new PlanetGraph(nCount, eCount, pCount, partitionSize);

        //Debug.Log("Upgrading : " + nCount + ", " + eCount + ", " + pCount + ", " + partCount);

        for (int i = 0; i < nCount; i++)
            graph.Nodes.Add(
                new PlanetNode()
                {
                    Identity = i
                }
            );

        for (int i = 0; i < eCount; i++)
            graph.Edges.Add(
                new PlanetEdge()
                {
                    Identity = i
                }
            );

        for (int i = 0; i < pCount; i++)
            graph.Polys.Add(
                new PlanetPoly()
                {
                    Identity = i
                }
            );
        for (int i = 0; i < partCount; i++)
            graph.Partitions.Add(new PlanetPartition(partitionSize));

        for (int i = 0; i < nCount || i < eCount || i < pCount; i++)
        {
            if (i < nCount)
            {
                var node = graph.Nodes[i];
                var refNode = position.Nodes[i];
                node.Edge = graph.Edges[refNode.Edge.Identity];
                node.Position = refNode.Position;
            }
            if (i < eCount)
            {
                var edge = graph.Edges[i];
                var refEdge = position.Edges[i];
                edge.Next = refEdge.Next != null ? graph.Edges[refEdge.Next.Identity] : null;
                edge.Twin = refEdge.Twin != null ? graph.Edges[refEdge.Twin.Identity] : null;
                edge.Prev = refEdge.Prev != null ? graph.Edges[refEdge.Prev.Identity] : null;
                edge.Node = graph.Nodes[refEdge.Node.Identity];
                edge.Poly = graph.Polys[refEdge.Poly.Identity];
            }
            if (i < pCount)
            {
                var poly = graph.Polys[i];
                var refPoly = position.Polys[i];
                poly.Edge = graph.Edges[refPoly.Edge.Identity];
                graph.Partitions[i / partitionSize].Add(poly);
            }
        }
        //Debug.Log("Graph Done");
        return graph;
    }
}
//private static void AssertPMBGraph(PositionGraph graph)
//{
//    Debug.Log("Asserting PMB Graph");
//    for (int i = 0; i < graph.Nodes.Count; i++)
//    {
//        var node = graph.Nodes[i];
//        if (node.Identity != i)
//            throw new Exception("Node Id Invalid!\nID:" + node.Identity + "\nI:" + i);
//        if (node.Edge == null)
//            throw new Exception("Node Edge Null!\nID:" + node.Identity);
//    }

//    for (int i = 0; i < graph.Polys.Count; i++)
//    {
//        var poly = graph.Polys[i];
//        if (poly.Identity != i)
//            throw new Exception("LeftPoly Id Invalid!\nID:" + poly.Identity + "\nI:" + i);
//        if (poly.Edge == null)
//            throw new Exception("Poly Edge Null!\nID:" + poly.Identity);
//    }

//    for (int i = 0; i < graph.Edges.Count; i++)
//    {
//        var edge = graph.Edges[i];
//        if (edge.Identity != i)
//            throw new Exception("Edge Id Invalid!\nID:" + edge.Identity + "\nI:" + i);
//        if (edge.Next == edge)
//            throw new Exception("Edge Next Cyclic!\nID:" + edge.Identity);
//        if (edge.Prev == edge)
//            throw new Exception("Edge Previous Cyclic!\nID:" + edge.Identity);
//        if (edge.Next == null)
//            throw new Exception("Edge Next Null!\nID:" + edge.Identity);
//        if (edge.Prev == null)
//            throw new Exception("Edge Previous Null!\nID:" + edge.Identity);
//        if (edge.Twin == null)
//            throw new Exception("Edge Twin Null!\nID:" + edge.Identity);
//        if (edge.Node == null)
//            throw new Exception("Edge Node Null!\nID:" + edge.Identity);
//        if (edge.Poly == null)
//            throw new Exception("Edge Poly Null!\nID:" + edge.Identity);
//    }

//    Debug.Log("PMB Graph Asserted");
//}
//private static void AssertDualGraph(PositionGraph graph, PositionGraph dual)
//{
//    Debug.Log("Asserting Dual");
//    for (int i = 0; i < graph.Polys.Count; i++)
//    {
//        var poly = graph.Polys[i];
//        var node = dual.Nodes[i];

//        if (node.Position.normalized != poly.CenterLerp.normalized)
//            throw new Exception("Poly->Node Shape Mismatch!\nNID:" + node.Identity + "\nPID:" + poly.Identity + "\nNS:" + node.Position.normalized + "\nPS:" + poly.CenterLerp.normalized);
//    }
//    //for (int i = 0; i < graph.Nodes.Count; i++)
//    //{
//    //    var node = graph.Nodes[i];
//    //    var poly = dual.Polys[i];

//    //    if (node.Position.normalized != poly.CenterLerp.normalized)
//    //        throw new Exception("Node->Poly Shape Mismatch!\nNID:" + node.Identity + "\nPID:" + poly.Identity + "\nNS:" + node.Position.normalized + "\nPS:" + poly.CenterLerp.normalized);
//    //}
//    for (int i = 0; i < graph.Edges.Count; i++)
//    {
//        var edge = graph.Edges[i];
//        var dualEdge = dual.Edges[i];

//        if (dualEdge.CenterSlerp.normalized != edge.CenterSlerp.normalized)
//            throw new Exception("Edge->Edge Slerp Shape Mismatch!\nDEID:" + dualEdge.Identity + "\nEID:" + edge.Identity + "\nDES:" + dualEdge.CenterSlerp.normalized + "\nES:" + edge.CenterSlerp.normalized);
//        if (dualEdge.CenterLerp.normalized != edge.CenterLerp.normalized)
//            throw new Exception("Edge->Edge Lerp Shape Mismatch!\nDEID:" + dualEdge.Identity + "\nEID:" + edge.Identity + "\nDES:" + dualEdge.CenterLerp.normalized + "\nES:" + edge.CenterLerp.normalized);
//        if (dualEdge.Poly.Identity != edge.Node.Identity)
//            throw new Exception("DualEdgePoly->EdgeNode Mismatch!\nDEPID:" + dualEdge.Poly.Identity + "\nENID:" + edge.Node.Identity);
//        if (dualEdge.Node.Identity != edge.Poly.Identity)
//            throw new Exception("DualEdgeNode->EdgePoly Mismatch!\nDENID:" + dualEdge.Node.Identity + "\nEPID:" + edge.Poly.Identity);
//    }
//    Debug.Log("Dual Asserted");
//}
//public static void AssertGraph()
//{
//    int subdivisions = 0;
//    ShapeType shape = ShapeType.Icosahedron;
//    bool slerp = false;
//    bool createDual = false;
//    string fName = (createDual ? "Dual" : "") + (slerp ? "Slerped" : "NormalLerped") + shape.ToString() + "_D" + subdivisions;
//    string fPath = Application.persistentDataPath;
//    string fExt = ".pos";
//    string fullPath = Path.ChangeExtension(Path.Combine(fPath, fName), fExt);



//    var originalGraph = CreateGraph(subdivisions, shape, slerp, createDual);
//    using (var fStream = new FileStream(fullPath, FileMode.Create))
//    {
//        using (var bWriter = new BinaryWriter(fStream))
//        {
//            originalGraph.Serialize(bWriter);
//        }
//    }
//    var serializedGraph = new PositionGraph();
//    using (var fStream = new FileStream(fullPath, FileMode.Open))
//    {
//        using (var bReader = new BinaryReader(fStream))
//        {
//            serializedGraph.Deserialize(bReader);
//        }
//    }
//    AssertGraph(originalGraph, serializedGraph);
//}
//private static void AssertGraph(PositionGraph left, PositionGraph right)
//{
//    if (left.Nodes.Count != right.Nodes.Count)
//        throw new Exception("Node Count Error!");
//    if (left.Edges.Count != right.Edges.Count)
//        throw new Exception("Edge Count Error!");
//    if (left.Polys.Count != right.Polys.Count)
//        throw new Exception("Poly Count Error!");

//    for (int i = 0; i < left.Nodes.Count; i++)
//    {
//        var leftNode = left.Nodes[i];
//        var rightNode = right.Nodes[i];
//        if (leftNode.Identity != i)
//            throw new Exception("LeftNode Id Invalid!");
//        if (rightNode.Identity != i)
//            throw new Exception("RightNode Id Invalid!");
//        if (leftNode.Identity != rightNode.Identity)
//            throw new Exception("Node Id Mismatch!");
//        if (leftNode.Position != rightNode.Position)
//            throw new Exception("Node Position Mismatch!");
//        if (rightNode.Edge.Identity != leftNode.Edge.Identity)
//            throw new Exception("Node Edge Id Mismatch!");
//    }

//    for (int i = 0; i < left.Polys.Count; i++)
//    {
//        var leftPoly = left.Polys[i];
//        var rightPoly = right.Polys[i];
//        if (leftPoly.Identity != i)
//            throw new Exception("LeftPoly Id Invalid!\nLI:" + leftPoly.Identity + "\nI:" + i);
//        if (rightPoly.Identity != i)
//            throw new Exception("RightPoly Id Invalid!\nRI:" + rightPoly.Identity + "\nI:" + i);
//        if (leftPoly.Identity != rightPoly.Identity)
//            throw new Exception("Poly Id Mismatch!\nLI:" + leftPoly.Identity + "\nRI:" + rightPoly.Identity);
//        if (rightPoly.Edge.Identity != leftPoly.Edge.Identity)
//            throw new Exception("Poly Edge Id Mismatch!\nLEI:" + leftPoly.Edge.Identity + "\nREI:" + rightPoly.Edge.Identity);
//    }

//    for (int i = 0; i < left.Edges.Count; i++)
//    {
//        var leftEdge = left.Edges[i];
//        var rightEdge = right.Edges[i];
//        if (leftEdge.Identity != i)
//            throw new Exception("LeftEdge Id Invalid!");
//        if (rightEdge.Identity != i)
//            throw new Exception("RightEdge Id Invalid!");
//        if (leftEdge.Identity != rightEdge.Identity)
//            throw new Exception("Edge Id Mismatch!");
//        if (rightEdge.Next.Identity != leftEdge.Next.Identity)
//            throw new Exception("Edge Next Id Mismatch!");
//        if (rightEdge.Prev.Identity != leftEdge.Prev.Identity)
//            throw new Exception("Edge Previous Id Mismatch!");
//        if (rightEdge.Twin == null || leftEdge.Twin == null)
//            throw new Exception("Edge Twin Invalid!");
//        if (rightEdge.Twin.Identity != leftEdge.Twin.Identity)
//            throw new Exception("Edge Twin Id Mismatch!");
//        if (rightEdge.Poly.Identity != leftEdge.Poly.Identity)
//            throw new Exception("Edge Poly Id Mismatch!");
//        if (rightEdge.Node.Identity != leftEdge.Node.Identity)
//            throw new Exception("Edge Node Id Mismatch!");
//    }
//}
//public class KeyCompare<K, V> : IComparer<KeyValuePair<K, V>>
//    where K : IComparable<K>
//{
//    public int Compare(KeyValuePair<K, V> x, KeyValuePair<K, V> y)
//    {
//        return x.Key.CompareTo(y.Key);
//    }
//}
//public static class BinarySearch
//{
//    public static V BinaryKeyValue<K, V>(KeyValuePair<K, V>[] inputArray, K key)
//         where K : IComparable<K>
//    {
//        return BinaryKeyValue(inputArray, key, 0, inputArray.Length - 1);
//    }
//    public static V BinaryKeyValue<K, V>(KeyValuePair<K, V>[] inputArray, K key, int min, int max)
//         where K : IComparable<K>
//    {
//        int index = BinaryKeySearch(inputArray, key, min, max);
//        if (index == -1)
//            throw new KeyNotFoundException("Key : " + key.ToString());
//        return inputArray[index].Value;
//    }

//    public static int BinaryKeySearch<K, V>(KeyValuePair<K, V>[] inputArray, K key)
//        where K : IComparable<K>
//    {
//        return BinaryKeySearch(inputArray, key, 0, inputArray.Length - 1);
//    }
//    public static int BinaryKeySearch<K, V>(KeyValuePair<K, V>[] inputArray, K key, int min, int max)
//        where K : IComparable<K>
//    {
//        while (min <= max)
//        {
//            int mid = (min + max) / 2;
//            int delta = key.CompareTo(inputArray[mid].Key);
//            if (delta > 0)
//            {
//                min = mid + 1;
//            }
//            else if (delta < 0)
//            {
//                max = mid - 1;
//            }
//            else return mid;
//        }
//        return -1;
//    }

//}
//public static class QuickSortExtensions
//{
//    public static void Quicksort<T, C>(T[] input, C comparar) where C : IComparer<T>
//    {
//        Quicksort(input, 0, input.Length - 1, comparar);
//    }
//    public static void Quicksort<T, C>(T[] input, int low, int high, C comparar) where C : IComparer<T>

//    {
//        int pivot_loc = 0;
//        Stack<int> stack = new Stack<int>(input.Length);
//        stack.Push(low);
//        stack.Push(high);

//        while (stack.Count > 0)
//        {
//            high = stack.Pop();
//            low = stack.Pop();

//            pivot_loc = Partition(input, low, high, comparar);
//            if (pivot_loc - 1 > low)
//            {
//                stack.Push(low);
//                stack.Push(pivot_loc - 1);
//            }


//            if (pivot_loc + 1 < high)
//            {
//                stack.Push(pivot_loc + 1);
//                stack.Push(high);
//            }

//        }
//        //Recursive
//        //if (low < high)
//        //{
//        //    pivot_loc = Partition(input, low, high, comparar);
//        //    Quicksort(input, low, pivot_loc - 1, comparar);
//        //    Quicksort(input, pivot_loc + 1, high, comparar);
//        //}
//    }
//    private static int Partition<T, C>(T[] input, int low, int high, C comparar) where C : IComparer<T>
//    {
//        T pivot = input[high];
//        int i = low - 1;

//        for (int j = low; j < high; j++)
//        {
//            int comp = comparar.Compare(input[j], pivot);
//            if (comp <= 0)
//            {
//                i++;
//                Swap(input, i, j);
//            }
//        }
//        Swap(input, i + 1, high);
//        return i + 1;
//    }


//    public static void Quicksort<T>(T[] input) where T : IComparable<T>
//    {
//        Quicksort(input, 0, input.Length - 1);
//    }
//    public static void Quicksort<T>(T[] input, int low, int high) where T : IComparable<T>
//    {

//        int pivot_loc = 0;
//        Stack<int> stack = new Stack<int>(input.Length);
//        stack.Push(low);
//        stack.Push(high);

//        while (stack.Count > 0)
//        {
//            high = stack.Pop();
//            low = stack.Pop();

//            pivot_loc = Partition(input, low, high);
//            if (pivot_loc - 1 > low)
//            {
//                stack.Push(low);
//                stack.Push(pivot_loc - 1);
//            }


//            if (pivot_loc + 1 < high)
//            {
//                stack.Push(pivot_loc + 1);
//                stack.Push(high);
//            }

//        }

//    }
//    private static int Partition<T>(T[] input, int low, int high) where T : IComparable<T>
//    {
//        T pivot = input[high];
//        int i = low - 1;

//        for (int j = low; j < high - 1; j++)
//        {
//            int comp = input[j].CompareTo(pivot);
//            if (comp <= 0)
//            {
//                i++;
//                Swap(input, i, j);
//            }
//        }
//        Swap(input, i + 1, high);
//        return i + 1;
//    }
//    private static void Swap<T>(T[] ar, int a, int b)
//    {
//        T temp = ar[a];
//        ar[a] = ar[b];
//        ar[b] = temp;
//    }

//}