using Graphing.Planet;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlateParameters
{
    public static PlateParameters Default { get { return new PlateParameters() { Trim = true, MaxHeight = 5, MinHeight = -8, DesiredPlateDepth = 8,/* DesiredPlates = 12,*/ MaxPlates = 16, MinPlateSize = 6 }; } }

    public bool Trim;

    public int MaxHeight;
    public int MinHeight;

    public int DesiredPlateDepth;
    //public int DesiredPlates;
    public int MaxPlates;
    public int MinPlateSize;
}
public class PlanetGraphPopulator
{
    public static void SetRandomTerrain(PlanetGraph graph, PlanetGraphParameters parameters)
    {
        foreach (var poly in graph.Polys)
            poly.TerrainType = Random.Range(0, parameters.TerrainTypes);
    }
    public static void Populate(PlanetGraph graph, PlanetGraphParameters parameters, out PlateGraph tectonicGraph)
    {
        using (TempRandomLock randLock = new TempRandomLock(parameters.Seed))
        {
            tectonicGraph = CreatePlateGraph(graph, parameters.TectonicParameters);
            SetRandomTerrain(graph, parameters);
            CalculateBaseHeights(graph, tectonicGraph);


        }
    }
    private static void CalculateBaseHeights(PlanetGraph graph, PlateGraph tectonicGraph)
    {
        Dictionary<PlatePoly, Vector3> PlatePolyCenter = new Dictionary<PlatePoly, Vector3>();
        foreach (var poly in graph.Polys)
        {
            Vector3 polyCenter = poly.CenterLerp;
            List<KeyValuePair<float, int>> closeList = new List<KeyValuePair<float, int>>();
            KeyValuePair<float, int>? kvp = null;
            foreach (var platePoly in tectonicGraph.Polys)
            {
                //Vector3 platePolyCenter;
                //if (!PlatePolyCenter.TryGetValue(platePoly, out platePolyCenter))
                //    PlatePolyCenter[platePoly] = platePoly.CenterLerp;

                foreach (var plateEdge in platePoly)
                {
                    //Debug.Log("A: " + (plateEdge != null));
                    //Debug.Log("B: " + (plateEdge.Twin != null));
                    //Debug.Log("C: " + (plateEdge.Twin.Poly != null));
                    //Vector3 edgeNormal = plateEdge.Twin.Poly.CenterLerp - polyCenter;
                    //Vector3 edgeCenter = plateEdge.CenterLerp + edgeNormal.normalized / 100f;


                    float delta = (plateEdge.CenterLerp - polyCenter).sqrMagnitude;
                    if (!kvp.HasValue || kvp.Value.Key > delta)
                    {
                        kvp = new KeyValuePair<float, int>(delta, platePoly.Height);
                    }
                }
                closeList.Add(kvp.Value);
            }
            closeList.Sort((x, y) =>
            {
                return x.Key.CompareTo(y.Key);
            });
            poly.Elevation = closeList[0].Value;
        }
    }

    private class PlatePolyIntermediate : List<PlanetPoly>
    {

        public bool ContainsEdge(PlanetEdge edge)
        {
            return Contains(edge.Poly);
        }
        public bool IsBorder(PlanetEdge edge)
        {
            return ContainsEdge(edge) && !ContainsEdge(edge.Twin);
        }

    }
    private static PlateGraph CreatePlateGraph(PlanetGraph graph, PlateParameters parameters)
    {
        Dictionary<PlanetEdge, PlatePolyIntermediate> cache = new Dictionary<PlanetEdge, PlatePolyIntermediate>();
        //Debug.Log("Started");
        //Debug.Break();
        var plates = CreateBasePlateGraph(graph);
        //Debug.Log("Base");
        //Debug.Break();
        MergePlateGraph(plates, parameters, cache);
        //Debug.Log("Merged");
        //Debug.Break();

        //Debug.Log("Final Assert: " + Assert(plates));
        var edges = FetchPlateEdges(plates);
        TrimPlateEdges(plates, edges, cache);
        //Debug.Log("Trimmed");
        //Debug.Break();
        return CreateGraph(plates, edges, parameters);
        //return CreateGraphFromIntermediate(plates, parameters, cache);
    }
    class PlateEdgeIntermediate
    {
        public PlatePolyIntermediate Poly;
        public PlanetEdge Start;
        public PlanetEdge End;
        public PlateEdgeIntermediate Next;
        public PlateEdgeIntermediate Prev;
        public PlateEdgeIntermediate Twin;
    }
    private static List<List<PlateEdgeIntermediate>> FetchPlateEdges(List<PlatePolyIntermediate> plates/*, PlateParameters parameters, Dictionary<PlanetEdge, PlatePolyIntermediate> cache*/)
    {

        HashSet<PlanetEdge> Inspected = new HashSet<PlanetEdge>();
        Dictionary<PlanetEdge, PlateEdgeIntermediate> PlanetToPlateEdges = new Dictionary<PlanetEdge, PlateEdgeIntermediate>();
        List<List<PlateEdgeIntermediate>> Edges = new List<List<PlateEdgeIntermediate>>();


        //List<PlanetEdge> Starts = new List<PlanetEdge>();
        foreach (var plate in plates)
        {
            var plateEdges = new List<PlateEdgeIntermediate>();
            Edges.Add(plateEdges);
            foreach (var poly in plate)
            {
                foreach (var edge in poly)
                {
                    //var edgeTwin = edge.Twin;
                    if (!plate.IsBorder(edge))
                    {
                        Debug.Log("Skipping non border.");
                        continue;
                    }
                    else if (Inspected.Contains(edge))
                    {
                        Debug.Log("Skipping checked edge.");
                        continue;
                    }

                    var startEdge = edge;
                    var endEdge = edge;

                    //Create
                    var currentEdge = startEdge.Prev;
                    var createdEdge = new PlateEdgeIntermediate() { Start = startEdge, End = endEdge, Poly = plate };
                    plateEdges.Add(createdEdge);
                    //Iterate
                    while (currentEdge != endEdge)
                    {
                        currentEdge = currentEdge.Next;
                        Inspected.Add(currentEdge);
                        PlanetToPlateEdges[currentEdge] = createdEdge;
                    }
                }
            }
        }
        foreach (var queue in Edges)
        {
            Queue<PlateEdgeIntermediate> SetupQueue = new Queue<PlateEdgeIntermediate>(queue);
            PlateEdgeIntermediate current = null;
            while (SetupQueue.Count > 0)
            {
                //Get intermediary
                var intermediary = current = SetupQueue.Dequeue();

                //There are several ways to reach a start
                //Either it is the next (after end) or it is the next.twin's.next
                PlateEdgeIntermediate found = null;
                if (PlanetToPlateEdges.TryGetValue(intermediary.End.Next, out found) || PlanetToPlateEdges.TryGetValue(intermediary.End.Next.Twin.Next, out found))
                {
                    intermediary.Next = found;
                    intermediary.Next.Prev = intermediary;
                }
                //To find the twin, we twin the end (to find the start of the next edge
                if (intermediary.Twin == null && PlanetToPlateEdges.TryGetValue(intermediary.End.Twin, out found))
                {
                    intermediary.Twin = found;
                    intermediary.Twin.Twin = intermediary;
                }
            }
            queue.Clear();
            var start = current;
            do
            {
                queue.Add(current);
                current = current.Next;
            } while (start != current);
        }
        return Edges;
    }
    private static void ReverseEdges(List<List<PlateEdgeIntermediate>> edges)
    {
        foreach (var edgeList in edges)
        {
            foreach (var edge in edgeList)
            {
                var temp = edge.Next;
                edge.Next = edge.Prev;
                edge.Prev = temp;

                var planetTemp = edge.Start;
                edge.Start = edge.End;
                edge.End = planetTemp;
            }
        }
    }
    private static bool ShouldMerge(PlateEdgeIntermediate edge, /*List<PlatePolyIntermediate> plates, Dictionary<PlanetEdge, PlatePolyIntermediate> cache,*/ out PlateEdgeIntermediate removed)
    {
        removed = null;
        var prev = edge.Prev;
        var prevBorderPoly = edge.Prev.Twin.Poly;
        var edgeBorderPoly = edge.Twin.Poly;
        if (edge.Start.Poly != prev.Start.Poly || prevBorderPoly != edgeBorderPoly)
            return false;
        removed = prev;
        return true;
    }
    private static bool ShouldMergePrev(PlateEdgeIntermediate edge, List<PlatePolyIntermediate> plates, Dictionary<PlanetEdge, PlatePolyIntermediate> cache, out PlateEdgeIntermediate removed)
    {
        removed = null;
        var prevEdge = edge.Prev;
        //if (prevEdge.Start.Poly != edge.Start.Poly)//Not same planet Poly
        //    return false;
        var prevEdgeBorderPlate = prevEdge.Twin.Poly; GetPlateFromEdge(plates, prevEdge.Start.Twin, cache);
        var edgeBorderPlate = GetPlateFromEdge(plates, edge.Start.Twin, cache);
        if (prevEdgeBorderPlate != edgeBorderPlate)//Not bordering same plate poly
            return false;
        removed = prevEdge;
        //RemoveEdge(removed);
        return true;
    }
    private static bool ShouldMergeNext(PlateEdgeIntermediate edge, List<PlatePolyIntermediate> plates, Dictionary<PlanetEdge, PlatePolyIntermediate> cache, out PlateEdgeIntermediate removed)
    {
        removed = null;
        var nextEdge = edge.Next;
        //if (nextEdge.Start.Poly != edge.Start.Poly)//Not same planet Poly
        //    return false;
        var nextEdgeBorderPlate = GetPlateFromEdge(plates, nextEdge.Start.Twin, cache);
        var edgeBorderPlate = GetPlateFromEdge(plates, edge.Start.Twin, cache);
        if (nextEdgeBorderPlate != edgeBorderPlate)//Not bordering same plate poly
            return false;

        removed = nextEdge;
        //RemoveEdge(removed);
        return false;
    }
    private static void RemoveEdge(PlateEdgeIntermediate edge)
    {
        edge.Prev.Next = edge.Next;
        edge.Next.Prev = edge.Prev;
        edge.Twin.Twin = edge.Prev;

        edge.Prev.End = edge.End;

        edge.Next = edge.Twin = edge.Prev;
        edge.Start = edge.End = null;
    }

    //The good news, this is the only thing that we need to fix
    //Bad news, I dont know how!
    private static void TrimPlateEdges(List<PlatePolyIntermediate> plates, List<List<PlateEdgeIntermediate>> untrimmed, Dictionary<PlanetEdge, PlatePolyIntermediate> cache)
    {
        HashSet<PlateEdgeIntermediate> RemovedEdges = new HashSet<PlateEdgeIntermediate>();
        Queue<PlateEdgeIntermediate> tempQueue = null;
        var mergeQueue = new Queue<PlateEdgeIntermediate>();
        foreach (var edgeList in untrimmed)
        {
            tempQueue = new Queue<PlateEdgeIntermediate>(edgeList);
            while (tempQueue.Count > 0)
            {
                var edge = tempQueue.Dequeue();
                if (RemovedEdges.Contains(edge))
                    continue;
                //if (removed.Contains(edge))
                //    continue;
                var removed = edge;
                //if (ShouldMergePrev(edge, plates, cache, out removed) || ShouldMergeNext(edge, plates, cache, out removed))
                if(ShouldMerge(edge,out removed))
                {
                    //if (edge.Start.Poly == null)
                    //    throw new System.Exception("Found the problem!");
                    //if (nextEdge.Start.Poly == null)
                    //    throw new System.Exception("Found the other problem!");
                    //if (edge.Start.Poly != edge.End.Poly)
                    //    throw new System.Exception("Exception! At least the comp knows too.");

                    RemovedEdges.Add(removed);
                    mergeQueue.Enqueue(removed);
                }
            }


            tempQueue = new Queue<PlateEdgeIntermediate>(edgeList);
            edgeList.Clear();
            foreach (var edge in tempQueue)
                if (!RemovedEdges.Contains(edge))
                    edgeList.Add(edge);
        }
        foreach (var edge in mergeQueue)
            RemoveEdge(edge);

        //return untrimmed;
    }
    private static PlateGraph CreateGraph(List<PlatePolyIntermediate> plates, List<List<PlateEdgeIntermediate>> edges, PlateParameters parameters)
    {
        List<PlatePoly> Polys = new List<PlatePoly>();
        List<PlateEdge> Edges = new List<PlateEdge>();
        List<PlateNode> Nodes = new List<PlateNode>();
        foreach (var plate in plates)
        {
            var poly = new PlatePoly()
            {
                Height = Random.Range(parameters.MinHeight, parameters.MaxHeight),
                Identity = Polys.Count
            };
            Polys.Add(poly);
        }
        Dictionary<PlateEdgeIntermediate, PlateEdge> NextAndTwinLookup = new Dictionary<PlateEdgeIntermediate, PlateEdge>();
        Dictionary<PlanetNode, PlateNode> NodeLookup = new Dictionary<PlanetNode, PlateNode>();
        int i = -1;
        foreach (var edgeQueue in edges)
        {
            i++;
            var createdPoly = Polys[i];
            foreach (var edge in edgeQueue)
            {
                PlateNode createdNode = null;
                var node = edge.Start.Node;
                if (!NodeLookup.TryGetValue(node, out createdNode))
                {
                    NodeLookup[node] = createdNode = new PlateNode()
                    {
                        Position = node.Position,
                        Identity = Nodes.Count
                    };
                    Nodes.Add(createdNode);
                }
                PlateEdge createdEdge = new PlateEdge()
                {
                    Node = createdNode,
                    Poly = createdPoly,
                    Identity = Edges.Count,
                    DebugEdge = edge.Start
                };
                Edges.Add(createdEdge);
                createdPoly.Edge = createdNode.Edge = createdEdge;


                NextAndTwinLookup[edge] = createdEdge;
                PlateEdge temp = null;
                if (createdEdge.Next == null && NextAndTwinLookup.TryGetValue(edge.Next, out temp))
                {
                    createdEdge.Next = temp;
                    createdEdge.Next.Prev = createdEdge;
                }
                if (createdEdge.Prev == null && NextAndTwinLookup.TryGetValue(edge.Prev, out temp))
                {
                    createdEdge.Prev = temp;
                    createdEdge.Prev.Next = createdEdge;
                }
                if (createdEdge.Twin == null && NextAndTwinLookup.TryGetValue(edge.Twin, out temp))
                {
                    createdEdge.Twin = temp;
                    createdEdge.Twin.Twin = createdEdge;
                }
            }
        }
        return new PlateGraph()
        {
            Nodes = Nodes,
            Edges = Edges,
            Polys = Polys
        };
    }
    private static PlateGraph CreateGraphFromIntermediate(List<PlatePolyIntermediate> plates, PlateParameters parameters, Dictionary<PlanetEdge, PlatePolyIntermediate> cache)
    {
        HashSet<PlanetEdge> Inspected = new HashSet<PlanetEdge>();
        Dictionary<PlanetEdge, PlateEdgeIntermediate> PlanetToPlateEdges = new Dictionary<PlanetEdge, PlateEdgeIntermediate>();
        Queue<PlateEdgeIntermediate> PlateEdges = new Queue<PlateEdgeIntermediate>();


        //List<PlanetEdge> Starts = new List<PlanetEdge>();
        foreach (var plate in plates)
        {
            foreach (var poly in plate)
            {
                foreach (var edge in poly)
                {
                    //var edgeTwin = edge.Twin;
                    if (!plate.IsBorder(edge))
                    {
                        Debug.Log("Skipping non border.");
                        continue;
                    }
                    else if (Inspected.Contains(edge))
                    {
                        Debug.Log("Skipping checked edge.");
                        continue;
                    }

                    var startEdge = edge;
                    var endEdge = edge;
                    if (parameters.Trim)
                    {
                        var borderPlate = GetPlateFromEdge(plates, edge.Twin, cache);
                        while (startEdge.Prev.Poly == edge.Poly && GetPlateFromEdge(plates, startEdge.Prev.Twin, cache) == borderPlate)
                            startEdge = startEdge.Prev;
                        while (endEdge.Next.Poly == edge.Poly && GetPlateFromEdge(plates, endEdge.Next.Twin, cache) == borderPlate)
                            endEdge = endEdge.Next;
                    }
                    //Create
                    var currentEdge = startEdge.Prev;
                    var createdEdge = new PlateEdgeIntermediate() { Start = startEdge, End = endEdge };
                    PlateEdges.Enqueue(createdEdge);
                    //Iterate
                    while (currentEdge != endEdge)
                    {
                        currentEdge = currentEdge.Next;
                        Inspected.Add(currentEdge);
                        PlanetToPlateEdges[currentEdge] = createdEdge;
                    }
                }
            }
        }
        List<PlatePoly> Polys = new List<PlatePoly>();
        List<PlateEdge> Edges = new List<PlateEdge>();
        List<PlateNode> Nodes = new List<PlateNode>();

        Dictionary<PlatePolyIntermediate, PlatePoly> PolyLookup = new Dictionary<PlatePolyIntermediate, PlatePoly>();
        Dictionary<PlateEdgeIntermediate, PlateEdge> EdgeLookup = new Dictionary<PlateEdgeIntermediate, PlateEdge>();
        Dictionary<PlanetNode, PlateNode> NodeLookup = new Dictionary<PlanetNode, PlateNode>();

        Queue<PlateEdgeIntermediate> SetupQueue = new Queue<PlateEdgeIntermediate>(PlateEdges);
        while (SetupQueue.Count > 0)
        {
            //Get intermediary
            var intermediary = SetupQueue.Dequeue();

            //There are several ways to reach a start
            //Either it is the next (after end) or it is the next.twin's.next
            PlateEdgeIntermediate found = null;
            if (PlanetToPlateEdges.TryGetValue(intermediary.End.Next, out found) || PlanetToPlateEdges.TryGetValue(intermediary.End.Next.Twin.Next, out found))
            {
                intermediary.Next = found;
                //intermediary.Next.Prev = intermediary;
            }
            //To find the twin, we twin the end (to find the start of the next edge
            if (intermediary.Twin == null && PlanetToPlateEdges.TryGetValue(intermediary.End.Twin, out found))
            {
                intermediary.Twin = found;
                intermediary.Twin.Twin = intermediary;
            }
            var poly = GetPlateFromEdge(plates, intermediary.Start, cache);
            PlatePoly createdPoly;
            if (!PolyLookup.TryGetValue(poly, out createdPoly))
            {
                PolyLookup[poly] = createdPoly = new PlatePoly()
                {
                    Height = Random.Range(parameters.MinHeight, parameters.MaxHeight)
                };
                Polys.Add(createdPoly);
            }

            var node = intermediary.Start.Node;
            PlateNode createdNode;
            if (!NodeLookup.TryGetValue(node, out createdNode))
            {
                NodeLookup[node] = createdNode = new PlateNode()
                {
                    Position = node.Position
                };
                Nodes.Add(createdNode);
            }

            PlateEdge createdEdge = EdgeLookup[intermediary] = new PlateEdge()
            {
                Node = createdNode,
                Poly = createdPoly,
                //Next = next,
                //Twin = twin,
                //Prev = prev,
            };
            Edges.Add(createdEdge);
        }

        while (PlateEdges.Count > 0)
        {
            //Get intermediary
            var intermediary = PlateEdges.Dequeue();
            var createdEdge = EdgeLookup[intermediary];
            PlateEdge temp = null;
            if (createdEdge.Prev == null && EdgeLookup.TryGetValue(intermediary.Next, out temp))
            {
                createdEdge.Prev = temp;
                createdEdge.Prev.Next = createdEdge;
            }
            if (createdEdge.Twin == null && EdgeLookup.TryGetValue(intermediary.Twin, out temp))
            {
                createdEdge.Twin = temp;
                createdEdge.Twin.Twin = createdEdge;
            }
            //if (createdEdge.Prev == null && EdgeLookup.TryGetValue(intermediary.Prev, out temp))
            //{
            //    createdEdge.Prev = temp;
            //    createdEdge.Prev.Next = createdEdge;
            //}

            createdEdge.Poly.Edge = createdEdge.Node.Edge = createdEdge;
        }

        int i = 0;
        foreach (var poly in Polys)
            poly.Identity = i++;
        i = 0;
        foreach (var edge in Edges)
            edge.Identity = i++;
        i = 0;
        foreach (var node in Nodes)
            node.Identity = i++;

        PlateGraph graph = new PlateGraph();
        graph.Polys.AddRange(Polys);
        graph.Edges.AddRange(Edges);
        graph.Nodes.AddRange(Nodes);
        return graph;


    }
    private static List<PlatePolyIntermediate> CreateBasePlateGraph(PlanetGraph graph)
    {
        var plateBaseGraph = new List<PlatePolyIntermediate>();
        //Debug.Log(graph.Polys.Count);
        foreach (var poly in graph.Polys)
        {
            var intermediate = new PlatePolyIntermediate();
            intermediate.Add(poly);
            plateBaseGraph.Add(intermediate);
        }
        return plateBaseGraph;
    }
    private static PlatePolyIntermediate GetPlateFromEdge(List<PlatePolyIntermediate> baseGraph, PlanetEdge edge, Dictionary<PlanetEdge, PlatePolyIntermediate> cache)
    {
        if (edge == null)
            throw new System.Exception("Cant Fetch Poly On Null Edge!");
        PlatePolyIntermediate temp = null;
        if (cache.TryGetValue(edge, out temp))
            return temp;
        foreach (var poly in baseGraph)
            if (poly.ContainsEdge(edge))
                return cache[edge] = poly;//Not equivalence, but assignment
        throw new System.Exception("Edge not contained by any poly.");
    }
    private static bool Assert(List<PlatePolyIntermediate> graph)
    {
        HashSet<PlanetPoly> InspectedPlanet = new HashSet<PlanetPoly>();
        HashSet<PlatePolyIntermediate> InspectedPlate = new HashSet<PlatePolyIntermediate>();
        foreach (var plate in graph)
        {
            if (InspectedPlate.Contains(plate))
            {
                Debug.Log("Plate 2");
                return false;
            }
            else
                foreach (var poly in plate)
                    if (InspectedPlanet.Contains(poly))
                    {
                        Debug.Log("Poly 2");
                        return false;
                    }
                    else
                        InspectedPlanet.Add(poly);
        }
        return true;

    }
    private void MergePlate(List<PlatePolyIntermediate> plates, PlatePolyIntermediate merging, PlatePolyIntermediate absorbing)
    {

    }
    private static void MergePlateGraph(List<PlatePolyIntermediate> plates, PlateParameters parameters, Dictionary<PlanetEdge, PlatePolyIntermediate> cache)
    {
        Debug.Log("Initial Assertion: " + Assert(plates));
        List<PlatePolyIntermediate> SortedPlates = new List<PlatePolyIntermediate>(plates);
        Dictionary<PlatePolyIntermediate, int> NeighborEdges = new Dictionary<PlatePolyIntermediate, int>();
        SortedPlates.Sort((x, y) => { return x.Count.CompareTo(y.Count); });
        while (SortedPlates[0].Count <= 0)
            SortedPlates.RemoveAt(0);
        PlatePolyIntermediate activePlate = null;
        //While a plate is less than the minimimum plate size
        //OR
        //The plate cound is acceptable (below max)
        //Each plate must have at least 3  polys to form the plate,  as two create a possible line border (surrounded by a single other plate), and one creates a possible nonexistant border (surrounded by a single other plate)
        bool smallPlateCondition = (SortedPlates.Count > 1 && SortedPlates[0].Count < Mathf.Max(3, parameters.MinPlateSize));
        bool plateCapacityCondition = (parameters.MaxPlates > 0 && SortedPlates.Count > parameters.MaxPlates);
        int pass = 0;
        while (smallPlateCondition || plateCapacityCondition)
        {
            Debug.Log("Pass " + (pass++));
            int id = 0;
            if (!smallPlateCondition && plateCapacityCondition)//Small plates merge with big plates, so it's id is always 0
                                                               //plateCapacity tends towards small plates, but it
                id = Random.Range(0, Random.Range(0, SortedPlates.Count) + 1);

            NeighborEdges.Clear();
            activePlate = SortedPlates[id];

            foreach (var poly in activePlate)
            {
                foreach (var edge in poly)
                {
                    if (!activePlate.IsBorder(edge))
                    {
                        //Debug.Log("Not Border");
                        continue;
                    }
                    var edgePlate = GetPlateFromEdge(plates, edge.Twin, cache);

                    if (activePlate == edgePlate)
                    {
                        //Debug.Log("Active == Edge");
                        continue;
                    }
                    if (!NeighborEdges.ContainsKey(edgePlate))
                        NeighborEdges[edgePlate] = 1;
                    else
                        NeighborEdges[edgePlate]++;
                }
            }
            KeyValuePair<PlatePolyIntermediate, int>? largeKvp = null;
            if (NeighborEdges.Count <= 0)
            {
                Debug.LogWarning("PlateEdges are foobarred!");
                break;
            }
            else
            {
                foreach (KeyValuePair<PlatePolyIntermediate, int> kvp in NeighborEdges)
                {
                    if (largeKvp == null)
                        largeKvp = kvp;
                    else if (kvp.Value > largeKvp.Value.Value || (kvp.Value == largeKvp.Value.Value && Random.value <= 0.5f))
                    {
                        largeKvp = kvp;
                    }
                }
                //neighborPlates.Sort((PlanetTectonicPlate x, PlanetTectonicPlate y){ return { return x.Count.CompareTo(y.Count); });
                //PlateMerge(graph, activePlate, largeKvp.Value.Key);
                //empties++;//Smallest is now empty
                var largePoly = largeKvp.Value.Key;
                largePoly.AddRange(activePlate);
                activePlate.Clear();
                plates.Remove(activePlate);
                SortedPlates.Remove(activePlate);
                SortedPlates.Sort((x, y) => { return x.Count.CompareTo(y.Count); });
            }
            smallPlateCondition = (SortedPlates.Count > 1 && SortedPlates[0].Count < Mathf.Max(3, parameters.MinPlateSize));
            plateCapacityCondition = (parameters.MaxPlates > 0 && SortedPlates.Count > parameters.MaxPlates);
            Debug.Log("Assertion: " + Assert(plates));
        }
    }




    //private static PlateGraph CreateGraph(PlanetGraph planetGraph, PlateParameters parameters)
    //{
    //    PlateGraph graph = CreateFullGraph(planetGraph, parameters);
    //    int i = 0;
    //    Debug.Log((i++) + "I, " + graph.Nodes.Count + "N, " + graph.Edges.Count + "E, " + graph.Polys.Count + "P");//0
    //    //Fill
    //    PlateFloodFill(graph);
    //    Debug.Log((i++) + "I, " + graph.Nodes.Count + "N, " + graph.Edges.Count + "E, " + graph.Polys.Count + "P");//1
    //    //Clean
    //    PlateRemoveEmptyPolys(graph);
    //    Debug.Log((i++) + "I, " + graph.Nodes.Count + "N, " + graph.Edges.Count + "E, " + graph.Polys.Count + "P");//2
    //    //PlateRemoveExcess(graph);
    //    //Prune
    //    PlatePrune(graph, parameters);
    //    Debug.Log((i++) + "I, " + graph.Nodes.Count + "N, " + graph.Edges.Count + "E, " + graph.Polys.Count + "P");//3
    //    //Clean
    //    PlateRemoveEmptyPolys(graph);
    //    Debug.Log((i++) + "I, " + graph.Nodes.Count + "N, " + graph.Edges.Count + "E, " + graph.Polys.Count + "P");//4
    //    //PlateRemoveExcess(graph);
    //    //Merge
    //    PlateMerge(graph, parameters);
    //    Debug.Log((i++) + "I, " + graph.Nodes.Count + "N, " + graph.Edges.Count + "E, " + graph.Polys.Count + "P");//5
    //    //Clean
    //    PlateRemoveEmptyPolys(graph);
    //    Debug.Log((i++) + "I, " + graph.Nodes.Count + "N, " + graph.Edges.Count + "E, " + graph.Polys.Count + "P");//6
    //    PlateRemoveExcess(graph);
    //    Debug.Log((i++) + "I, " + graph.Nodes.Count + "N, " + graph.Edges.Count + "E, " + graph.Polys.Count + "P");//7
    //    //Trim
    //    TrimPlateSharedEdge(graph);
    //    Debug.Log((i++) + "I, " + graph.Nodes.Count + "N, " + graph.Edges.Count + "E, " + graph.Polys.Count + "P");//8
    //    //We are done!
    //    return graph;
    //}
    //public class BiDictionary<TKey, TValue>
    //{
    //    Dictionary<TKey, TValue> Lookup;
    //    Dictionary<TValue, TKey> ReverseLookup;

    //    public TKey this[TValue key]
    //    {
    //        get
    //        {
    //            return ((IDictionary<TValue, TKey>)ReverseLookup)[key];
    //        }
    //        set
    //        {
    //            ((IDictionary<TValue, TKey>)ReverseLookup)[key] = value;
    //        }
    //    }
    //    public TValue this[TKey key]
    //    {
    //        get
    //        {
    //            return ((IDictionary<TKey, TValue>)Lookup)[key];
    //        }
    //        set
    //        {
    //            ((IDictionary<TKey, TValue>)Lookup)[key] = value;
    //        }
    //    }

    //}
    //private class GraphConvertLookup
    //{
    //    public GraphConvertLookup()
    //    {
    //        Nodes = new BiDictionaryOneToOne<PlateNode, PlanetNode>();
    //        Edges = new BiDictionaryOneToOne<PlateEdge, PlanetEdge>();
    //        Polys = new BiDictionaryOneToOne<PlatePoly, PlanetPoly>();
    //    }
    //    public BiDictionaryOneToOne<PlateEdge, PlanetEdge> Edges { get; private set; }
    //    public BiDictionaryOneToOne<PlateNode, PlanetNode> Nodes { get; private set; }
    //    public BiDictionaryOneToOne<PlatePoly, PlanetPoly> Polys { get; private set; }

    //}
    //private static PlateGraph CreateFullGraph(PlanetGraph planetGraph, PlateParameters parameters, out GraphConvertLookup lookup)
    //{
    //    PlateGraph graph = new PlateGraph();
    //    lookup = new GraphConvertLookup();
    //    //Iniitalize Poly (Except for Edge)
    //    foreach (var refPoly in planetGraph.Polys)
    //    {
    //        var poly = new PlatePoly()
    //        {
    //            Identity = refPoly.Identity,
    //            Height = Random.Range(parameters.MinHeight, parameters.MaxHeight)
    //        };
    //        graph.Polys.Add(poly);
    //        lookup.Polys.Add(poly,refPoly);
    //    }
    //    //Initialize Node (Except for Edge)
    //    foreach (var refNode in planetGraph.Nodes)
    //    {
    //        var node = new PlateNode()
    //        {
    //            Identity = refNode.Identity
    //        };
    //        graph.Nodes.Add(node);
    //        lookup.Nodes.Add(node, refNode);
    //    }

    //    foreach (var refEdge in planetGraph.Edges)
    //    {
    //        var edge = new PlateEdge()
    //        {
    //            Identity = refEdge.Identity
    //        };
    //        graph.Edges.Add(edge);
    //        lookup.Edges.Add(edge, refEdge);
    //    }

    //    foreach (var edge in graph.Edges)
    //    {
    //        PlanetEdge refEdge = null;
    //        refEdge = lookup.Edges.GetByFirst(edge);

    //        edge.Next = lookup.Edges.GetBySecond(refEdge.Next);
    //        edge.Prev = lookup.Edges.GetBySecond(refEdge.Prev);
    //        edge.Twin = lookup.Edges.GetBySecond(refEdge.Twin);
    //    }

    //    return graph;
    //}
    ////Fills the plategraph so that each plate tries to absorb all nearby plates.
    ////Only absorbs plates with one cell (planet poly)
    //private static void PlateFloodFill(PlateGraph graph, GraphConvertLookup lookup)
    //{
    //    //Iterate over each PlatePoly as a queue
    //    //If the Plate has only 1, then add it to the plate

    //    Queue<Queue<PlateEdge>> plateQueue = new Queue<Queue<PlateEdge>>();
    //    foreach (var poly in graph.Polys)
    //    {
    //        var newQueue = new Queue<PlateEdge>();
    //        foreach (var edge in poly)
    //            newQueue.Enqueue(edge);
    //        plateQueue.Enqueue(newQueue);
    //    }
    //    HashSet<PlanetPoly> filled = new HashSet<PlanetPoly>();
    //    Queue<PlateEdge> activeQueue = null;
    //    Queue<List<PlanetPoly>> modified = new Queue<List<PlanetPoly>>();
    //    while (plateQueue.Count > 0)
    //    {
    //        activeQueue = plateQueue.Dequeue();
    //        var activeEdge = activeQueue.Dequeue();
    //        var addingPoly = lookup.Edges.GetByFirst(activeEdge.Twin).Poly;

    //        if (!filled.Contains(addingPoly))
    //        {
    //            filled.Add(addingPoly);

    //            addingPoly.

    //            var edgePoly = addingPoly;
    //            foreach (var edge in addingPoly)
    //            {
    //                //Get neighbor
    //                edgePoly = edge.Twin.Poly;
    //                //If neighbor hasnt been set
    //                if (!filled.Contains(edgePoly))
    //                    activeQueue.Enqueue(lookup.Edges.GetBySecond(edge));//Add to queue
    //            }
    //        }
    //        //If queue not empty
    //        if (activeQueue.Count > 0)
    //        {
    //            //add to process
    //            plateQueue.Enqueue(activeQueue);
    //        }
    //    }
    //    PlateRemoveEmptyPolys(graph);
    //}
    ////Prunes plates so that they do no stray to far from the center
    //private static void PlatePrune(PlateGraph graph, PlateParameters parameters)
    //{
    //    Queue<PlatePoly> unprunedPlates = new Queue<PlatePoly>();
    //    foreach (var poly in graph.Polys)
    //        unprunedPlates.Enqueue(poly);
    //    while (unprunedPlates.Count > 0)
    //        unprunedPlates = PlatePrune(graph, unprunedPlates, parameters);
    //    PlateRemoveEmptyPolys(graph);
    //}
    ////Prunes plates in the unprunedPlates queue so that they do no stray to far from the center
    //private static Queue<PlatePoly> PlatePrune(PlateGraph graph, Queue<PlatePoly> unprunedPlates, PlateParameters parameters)
    //{
    //    Dictionary<PlanetPoly, int> DistanceLookup = new Dictionary<PlanetPoly, int>();
    //    Queue<PlanetPoly> DistanceQueue = new Queue<PlanetPoly>();
    //    Queue<PlatePoly> prunedQueue = new Queue<PlatePoly>();
    //    PlatePoly activePlate = null;
    //    while (unprunedPlates.Count > 0)
    //    {
    //        activePlate = unprunedPlates.Dequeue();
    //        if (activePlate.Count <= 0)
    //            continue;//Skip empty plates
    //        DistanceLookup.Clear();
    //        //Pick an arbitrary center Poly
    //        //We could use the origin Poly, (0)
    //        //But this gives us some interesting variation in the pressure
    //        //activePlate.CenterId = Random.Range(0, activePlate.Count);
    //        //The distance from the center to the center is always 0
    //        DistanceLookup[activePlate[0]] = 0;
    //        //The breadth first queue
    //        DistanceQueue.Enqueue(activePlate[0]);
    //        //While queue is not empty
    //        while (DistanceQueue.Count > 0)
    //        {
    //            PlanetPoly activePoly = DistanceQueue.Dequeue(), edgePoly = null;
    //            if (!activePlate.Contains(activePoly))
    //                continue;//Skip enquement if not same plate
    //            int activeDepth = DistanceLookup[activePoly], nextDepth = activeDepth + 1, edgeDepth = -1;
    //            foreach (PlanetEdge edge in activePoly)
    //            {
    //                edgePoly = edge.Twin.Poly;
    //                edgeDepth = -1;
    //                //Have we reached the edge Poly?
    //                if (!DistanceLookup.TryGetValue(edgePoly, out edgeDepth) || edgeDepth > nextDepth)
    //                {
    //                    //We either,
    //                    //A) have not, so we reached it at the nextDepth
    //                    //B) have, but we found a shorter path
    //                    DistanceLookup[edgePoly] = nextDepth;
    //                    //Now enqueue it
    //                    DistanceQueue.Enqueue(activePoly);
    //                }
    //            }
    //        }
    //        //Now we prune
    //        PlatePoly prunePlate = new PlatePoly() { Identity = graph.Polys.Count, Height = Random.Range(parameters.MinHeight, parameters.MaxHeight + 1) };// PlanetTectonicPlate(plates.Count, Random.Range(parameters.GenerationParameters.ElevationScale.Min, parameters.GenerationParameters.ElevationScale.Max));// { Id = plates.Count, IsOcean = Random.value <= 0.5f };
    //        for (int j = 0; j < activePlate.Count; j++)
    //        {
    //            PlanetPoly activePoly = activePlate[j];
    //            int depth = 0;
    //            if (!(DistanceLookup.TryGetValue(activePoly, out depth)) && depth <= parameters.DesiredPlateDepth)
    //            {
    //                activePlate.Remove(activePoly);
    //                j--;
    //                //activePoly.TectonicPlateId = prunePlate.Id;
    //                prunePlate.Add(activePoly);
    //                activePoly.PlatePoly = prunePlate;
    //            }
    //        }
    //        if (prunePlate.Count > 0)
    //        {
    //            ShufflePlate(prunePlate);
    //            prunedQueue.Enqueue(prunePlate);
    //            //prunePlate.CenterId = Random.Range(0, prunePlate.Count);
    //            graph.Polys.Add(prunePlate);
    //        }
    //    }
    //    return prunedQueue;
    //}
    ////Shuffles the planet poly's within the plate. This is useful for altering the center
    //private static void ShufflePlate(PlatePoly plate)
    //{
    //    int n = plate.Count;
    //    while (n > 1)
    //    {
    //        n--;
    //        int k = Random.Range(0, n + 1);
    //        var value = plate[k];
    //        plate[k] = plate[n];
    //        plate[n] = value;
    //    }
    //}
    ////Removes all empty plates from the plateGraph
    //private static void PlateRemoveEmptyPolys(PlateGraph graph)
    //{
    //    //Clean Plates
    //    for (int i = 0; i < graph.Polys.Count; i++)
    //    {
    //        if (graph.Polys[i].Count <= 0)
    //        {
    //            graph.Polys.RemoveAt(i);
    //            i--;
    //        }
    //    }
    //}
    ////Remoevs nonborder edges and nodes
    //private static void PlateRemoveExcess(PlateGraph graph)
    //{
    //    //Enforce every edge to be set to the proper poly
    //    foreach (PlatePoly platePoly in graph.Polys)
    //    {
    //        foreach (PlanetPoly planetPoly in platePoly.PlanetPolys)
    //        {
    //            foreach (var edge in planetPoly)
    //            {
    //                if (edge.PlateEdge == null)
    //                    Debug.LogWarning("Theres a problem with plateEdge.");
    //                else
    //                    edge.PlateEdge.Poly = platePoly;
    //            }
    //        }
    //    }

    //    HashSet<PlateEdge> Inspected = new HashSet<PlateEdge>();
    //    List<PlateEdge> UsedEdges = new List<PlateEdge>(), UnusedEdges = new List<PlateEdge>();
    //    List<PlateNode> UsedNodes = new List<PlateNode>(), UnusedNodes = new List<PlateNode>();
    //    //Enforce every Poly's Edge to be a border and get border
    //    foreach (var edge in graph.Edges)
    //    {
    //        if (Inspected.Contains(edge))
    //            continue;
    //        if (edge.Twin == null)
    //        {
    //            Debug.LogWarning("Theres a problem with edge.twin.");
    //            continue;
    //        }
    //        if (edge.Twin.Poly != edge.Poly)
    //        {
    //            UsedEdges.Add(edge);
    //            edge.Poly.Edge = edge;
    //            edge.Twin.Poly.Edge = edge.Twin;
    //        }
    //        Inspected.Add(edge);
    //    }
    //    //Find excess and trim edges
    //    foreach (var edge in UsedEdges)
    //    {
    //        if (UsedEdges.Contains(edge.Next))
    //            continue;
    //        MergeEdge(edge);
    //    }
    //    //Remove excess
    //    foreach (var edge in graph.Edges)
    //    {
    //        bool edgeUsed = UsedEdges.Contains(edge);
    //        if (!edgeUsed)
    //        {
    //            UnusedEdges.Add(edge);
    //        }
    //        //EDge used and node not used
    //        if (edgeUsed)
    //        {
    //            if (!UsedNodes.Contains(edge.Node))
    //            {
    //                edge.Node.Edge = edge;//Garntess
    //                UsedNodes.Add(edge.Node);
    //                UnusedNodes.Remove(edge.Node);
    //            }
    //        }
    //        else
    //        {
    //            UnusedNodes.Add(edge.Node);
    //        }
    //    }

    //    foreach (var unusedEdge in UnusedEdges)
    //        graph.Edges.Remove(unusedEdge);
    //    foreach (var unusedNode in UnusedNodes)
    //        graph.Nodes.Remove(unusedNode);
    //}
    ////Merges an edge 
    //static void MergeEdge(PlateEdge edge)
    //{
    //    var edgeNext = edge.Next.Twin.Next;

    //    edge.Next.Prev = null;//Break link from next to this
    //    edge.Next.Twin.Next = null;//Break link from Next Twin to it's Next

    //    edge.Next = edgeNext;
    //    edgeNext.Prev = edge;
    //}
    //Merges plates according to the parameters
    //private static void PlateMerge(PlateGraph graph, PlateParameters parameters)
    //{
    //    List<PlatePoly> SortedPlates = new List<PlatePoly>(graph.Polys);

    //    Dictionary<PlatePoly, int> NeighborEdges = new Dictionary<PlatePoly, int>();
    //    SortedPlates.Sort((x, y){
    //        return { return x.Count.CompareTo(y.Count); });
    //        //int empties = 0;
    //        while (SortedPlates[0].Count <= 0)
    //            SortedPlates.RemoveAt(0);
    //        PlatePoly activePlate = null;
    //        //While a plate is less than the minimimum plate size
    //        //OR
    //        //The plate cound is acceptable (below max)
    //        //Each plate must have at least 3  polys to form the plate,  as two create a possible line border (surrounded by a single other plate), and one creates a possible nonexistant border (surrounded by a single other plate)


    //        bool smallPlateCondition = (SortedPlates.Count > 1 && SortedPlates[0].Count < Mathf.Max(3, parameters.MinPlateSize));
    //        bool plateCapacityCondition = (parameters.MaxPlates > 0 && SortedPlates.Count > parameters.MaxPlates);
    //        while (smallPlateCondition || plateCapacityCondition)
    //        {
    //            int id = 0;
    //            if (!smallPlateCondition && plateCapacityCondition)//Small plates merge with big plates, so it's id is always 0
    //                                                               //plateCapacity tends towards small plates, but it
    //                id = Random.Range(0, Random.Range(0, SortedPlates.Count) + 1);

    //            NeighborEdges.Clear();
    //            activePlate = SortedPlates[id];

    //            foreach (PlateEdge edge in activePlate)
    //            {
    //                PlatePoly edgePlate = edge.Twin.Poly;
    //                if (edgePlate == activePlate)
    //                    throw new System.Exception("There is a problem.");

    //                if (!NeighborEdges.ContainsKey(edgePlate))
    //                    NeighborEdges[edgePlate] = 1;
    //                else
    //                    NeighborEdges[edgePlate]++;
    //            }
    //            KeyValuePair<PlatePoly, int>? largeKvp = null;
    //            if (NeighborEdges.Count <= 0)
    //            {
    //                Debug.LogWarning("PlateEdges are foobarred!");
    //                break;
    //            }
    //            else
    //            {
    //                foreach (KeyValuePair<PlatePoly, int> kvp in NeighborEdges)
    //                {
    //                    if (largeKvp == null)
    //                        largeKvp = kvp;
    //                    else if (kvp.Value > largeKvp.Value.Value || (kvp.Value == largeKvp.Value.Value && Random.value <= 0.5f))
    //                    {
    //                        largeKvp = kvp;
    //                    }
    //                }
    //                //neighborPlates.Sort((PlanetTectonicPlate x, PlanetTectonicPlate y){ return { return x.Count.CompareTo(y.Count); });
    //                PlateMerge(graph, activePlate, largeKvp.Value.Key);
    //                //empties++;//Smallest is now empty
    //                SortedPlates.RemoveAt(id);
    //                SortedPlates.Sort((x, y){
    //                    return { return x.Count.CompareTo(y.Count); });
    //                }
    //                smallPlateCondition = (SortedPlates.Count > 1 && SortedPlates[0].Count < Mathf.Max(3, parameters.MinPlateSize));
    //                plateCapacityCondition = (parameters.MaxPlates > 0 && SortedPlates.Count > parameters.MaxPlates);
    //            }
    //        }
    //Merges the plates
    //private static void PlateMerge(PlateGraph graph, PlatePoly small, PlatePoly big)
    //{
    //    PlateEdge smallFirst = small.Edge;
    //    PlateEdge bigFirst = big.Edge;

    //    bool smallAbsorbed = false, bigAbsorbed = false;
    //    //  While im not absorbing        
    //    //And        
    //    //          Im on the big edge 
    //    //      AND
    //    //          My previous is also on the big edge
    //    //  Or
    //    //      Im not on the big edge

    //    while (!smallAbsorbed && ((smallFirst.Prev.Twin.Poly == big && smallFirst.Twin.Poly == big) || smallFirst.Twin.Poly != big))
    //    {
    //        smallFirst = smallFirst.Prev;
    //        smallAbsorbed |= (smallFirst == small.Edge);//If we are back at start, we are absorbing the small one, A kinda rara case, buta case nonetheless            
    //    }
    //    //While not absorved, and big 
    //    //  While im not absorbing        
    //    //And        
    //    //          Im on the small edge 
    //    //      AND
    //    //          My previous is also on the small edge
    //    //  Or
    //    //      Im not on the small edge
    //    while (!bigAbsorbed && ((bigFirst.Prev.Twin.Poly == small && bigFirst.Twin.Poly == small) || bigFirst.Twin.Poly != small))
    //    {
    //        bigFirst = bigFirst.Prev;
    //        //We shouldnt have to worry about absorbtion here            
    //        bigAbsorbed |= (bigFirst == big.Edge);//But i do NOT ANYMORE
    //                                              //HERES WHY
    //                                              //With a minimum shape of 3, you need...
    //                                              //More than 3, for triangles (Im thinking a tetrahedron, and that means 6 edges required for absorbtion (one division leaves 2 edges on each side for a total of 6, in that case, the small would be 3 being absorved by big
    //                                              //At least 14? for hexagons,
    //                                              //So probably 12 for a dodecahedron
    //                                              //But, somethings nagging at me...
    //    }

    //    //Merge the two, delete the excess
    //    if (!smallAbsorbed && !bigAbsorbed)
    //    {
    //        //List<PlateNode> UnusedNodes = new List<PlateNode>();
    //        List<PlateEdge> UnusedEdges = new List<PlateEdge>();

    //        PlateEdge oldSmallNext = smallFirst.Next;
    //        PlateEdge oldBigNext = bigFirst.Next;//For my sanity we twin this one

    //        do
    //        {
    //            UnusedEdges.Add(oldSmallNext);
    //            UnusedEdges.Add(oldBigNext);
    //            oldSmallNext = oldSmallNext.Next;
    //            oldBigNext = oldBigNext.Next;
    //        } while (oldSmallNext != null && oldBigNext != null);

    //        foreach (var edge in UnusedEdges)
    //        {
    //            graph.Edges.Remove(edge);
    //            graph.Nodes.Remove(edge.Node);
    //        }

    //        MergeEdge(smallFirst);
    //        MergeEdge(bigFirst);
    //    }
    //    else
    //    {
    //        throw new System.Exception("Absorbtion occured!");
    //    }

    //    big.PlanetPolys.AddRange(small);
    //    graph.Polys.Remove(small);
    //    small.Clear();

    //}
    //private static void TrimEdge(PlateGraph graph, PlateEdge edge)
    //{
    //    graph.Edges.Remove(edge);
    //    edge.Prev.Next = edge.Next;
    //    edge.Next.Prev = edge.Prev;
    //    edge.Twin = null;
    //}
    //private static void TrimPlateSharedEdge(PlateGraph graph, PlatePoly poly)
    //{
    //    foreach (var edge in poly)
    //    {
    //        if (edge.PlanetEdge == null)
    //        {
    //            Debug.LogWarning("edge.PlanetEdge has a problem!");
    //            continue;
    //        }
    //        if (edge.Next == null)
    //        {
    //            Debug.LogWarning("edge.Next has a problem!");
    //            continue;
    //        }

    //        //Planet Poly Edge Shared
    //        if (edge.PlanetEdge.Poly == edge.Next.PlanetEdge.Poly)
    //        {
    //            //Plate Poly Edge Shared on Same Plate
    //            if (edge.Twin.Poly == edge.Next.Twin.Poly)
    //            {
    //                TrimEdge(graph, edge);
    //            }
    //        }
    //    }
    //}
    //private static void TrimPlateSharedEdge(PlateGraph graph)
    //{
    //    foreach (PlatePoly poly in graph.Polys)
    //    {
    //        TrimPlateSharedEdge(graph, poly);
    //    }
    //    foreach (PlatePoly poly in graph.Polys)
    //    {
    //        foreach (PlateEdge edge in poly)
    //        {
    //            if (edge.Twin == null)
    //            {
    //                Debug.LogWarning("Edges without twins should be skipped!");
    //                continue;
    //            }
    //            //If the twin of our twin was set to null, that edge was trimmed
    //            if (edge.Twin.Twin == null)
    //            {
    //                //Set my twin to my twin's previous
    //                edge.Twin = edge.Twin.Prev;
    //            }
    //        }
    //    }
    //    //HashSet<PlateEdge> Removed = new HashSet<PlateEdge>();
    //    //foreach (PlateEdge edge in graph.Edges)
    //    //{
    //    //    //if (Removed.Contains(edge))
    //    //    //    continue;
    //    //    //if (edge.Twin == null)
    //    //    //    throw new System.Exception("Twin Missing!");
    //    //    //if (edge.Poly == edge.Twin.Poly)
    //    //    //{
    //    //    //    edge.PlanetEdge.PlateEdge = edge.PlanetEdge.PlateEdge = null;//Erase ourselves

    //    //    //    Removed.Add(edge);
    //    //    //    Removed.Add(edge.Twin);

    //    //    //    var ePrev = edge.Prev;
    //    //    //    var tNext = edge.Next;
    //    //    //    var eNext = edge.Twin.Next;
    //    //    //    var tPrev = edge.Twin.Prev;

    //    //    //    ePrev.Next = eNext;
    //    //    //    eNext.Prev = ePrev;

    //    //    //    tPrev.Next = tNext;
    //    //    //    tNext.Prev = tPrev;
    //    //    //}
    //    //}
    //    //foreach (PlateEdge edge in Removed)
    //    //    graph.Edges.Remove(edge);

    //    //List<PlateNode> NodesLeft = new List<PlateNode>();
    //    //foreach (PlateEdge edge in graph.Edges)
    //    //    if (!NodesLeft.Contains(edge.Node))
    //    //        NodesLeft.Add(edge.Node);
    //    //for (int i = 0; i < graph.Nodes.Count; i++)
    //    //{
    //    //    if (!NodesLeft.Contains(graph.Nodes[i]))
    //    //    {
    //    //        graph.Nodes[i].PlanetNode.PlateNode = null;//Erase ourselves
    //    //        graph.Nodes.RemoveAt(i);
    //    //        i--;
    //    //    }
    //    //}



    //    //return graph;
    //}
}
//public static PlateGraph CreateFullGraph(List<List<PlanetPoly>> plateList, PlanetTectonicPlateParameters parameters)
//{
//    Dictionary<PlanetNode, PlateNode> NodeLookup = new Dictionary<PlanetNode, PlateNode>();
//    //Dictionary<PlanetEdge, PlateEdge> EdgeLookup = new Dictionary<PlanetEdge, PlateEdge>();
//    //Dictionary<PlanetPoly, PlatePoly> PolyLookup = new Dictionary<PlanetPoly, PlatePoly>();
//    PlateGraph graph = new PlateGraph();
//    //Create plate polygons
//    foreach (var polyList in plateList)
//    {
//        //Create
//        var pPoly = new PlatePoly()
//        {
//            Identity = graph.Polys.Count,
//            Height = Random.Range(parameters.MinHeight, parameters.MaxHeight + 1)
//        };
//        graph.Polys.Add(pPoly);
//        for(var refPoly in polyList)
//        {

//        }
//    }

//    //    foreach (var planetPoly in plate)
//    //    {
//    //        planetPoly.PlatePoly = poly;
//    //        poly.PlanetPolys.Add(planetPoly);
//    //        foreach (var planetEdge in planetPoly)
//    //        {
//    //            var planetNode = planetEdge.Node;
//    //            PlateNode plateNode;
//    //            if (!NodeLookup.TryGetValue(planetNode, out plateNode))
//    //            {
//    //                NodeLookup[planetNode] = plateNode = new PlateNode()
//    //                {
//    //                    Identity = graph.Nodes.Count,
//    //                    PlanetNode = planetNode
//    //                };
//    //                graph.Nodes.Add(plateNode);
//    //            }
//    //            PlateEdge plateEdge = planetEdge.PlateEdge = new PlateEdge()
//    //            {
//    //                Identity = graph.Edges.Count,
//    //                PlanetEdge = planetEdge,
//    //                Poly = poly,
//    //                Node = plateNode
//    //            };
//    //            plateNode.Edge = plateEdge;
//    //            graph.Edges.Add(plateEdge);
//    //            poly.Edge = plateEdge;
//    //        }
//    //    }
//    //    foreach (var planetPoly in plate)
//    //    {
//    //        foreach (var planetEdge in planetPoly)
//    //        {
//    //            planetEdge.PlateEdge.Next = planetEdge.Next.PlateEdge;//Set Next
//    //            planetEdge.PlateEdge.Prev = planetEdge.Prev.PlateEdge;//set Prev
//    //            planetEdge.PlateEdge.Twin = planetEdge.Twin.PlateEdge;//Set Twin
//    //            planetEdge.Twin.PlateEdge.Twin = planetEdge.PlateEdge;//Set Twin's Twin
//    //        }
//    //    }
//    //}
//    return graph;
//}
//private static List<PlateEdge> GetPlateBoundary(List<List<PlanetPoly>> plates, out List<PlateEdge> createdEdges)
//{


//    //Dictionary<PlanetPoly, List<PlanetPoly>> PlateLookup = new Dictionary<PlanetPoly, List<PlanetPoly>>();
//    //foreach (var plate in plates)
//    //{
//    //    foreach (var poly in plate)
//    //        PlateLookup[poly] = plate;
//    //}
//    //List<List<PlateEdge>> EdgeBoundaries = new List<List<PlanetEdge>>();
//    HashSet<PlanetEdge> Inspected = new HashSet<PlanetEdge>();
//    //Go Over Each Plate
//    foreach (var plate in plates)
//    {
//        Queue<PlateEdge> EdgeStack = new Queue<PlateEdge>();

//        foreach (var poly in plate)
//        {
//            foreach (var edge in poly)
//            {
//                if (Inspected.Contains(edge))
//                    continue;
//                //Is Border?
//                if (!plate.Contains(edge.Twin.Poly))
//                    EdgeBoundary.Add(edge);
//                Inspected.Add(edge);
//            }
//        }
//        EdgeBoundaries.Add(EdgeBoundary);
//    }
//    //Inspected.Clear();
//    //List<List<PlanetEdge>> NodeBoundaries = new List<List<PlanetEdge>>();
//    //foreach (LinkedList<PlanetEdge> Boundary in EdgeBoundaries)
//    //{
//    //    if (Boundary.Count <= 0)
//    //        continue;
//    //    while (!Inspected.Contains(Boundary.First.Value) && PlateLookup[Boundary.First.Value.Twin.Poly] == PlateLookup[Boundary.Last.Value.Twin.Poly])
//    //    {
//    //        Inspected.Add(Boundary.First.Value);
//    //        Boundary.AddLast(Boundary.First.Value);
//    //        Boundary.RemoveFirst();
//    //    }
//    //    List<PlanetNode> Nodes = new List<PlanetNode>();
//    //    if (Inspected.Contains(Boundary.First.Value))
//    //    {
//    //        //Entire Boundary IS a boundary
//    //        foreach (var edge in Boundary)
//    //            Nodes.Add(edge.Node);
//    //    }
//    //    else
//    //    {
//    //        PlanetEdge startEdge = null;
//    //        foreach (var edge in Boundary)
//    //        {
//    //            if (startEdge == null || PlateLookup[startEdge.Twin.Poly] != PlateLookup[edge.Twin.Poly])
//    //            {
//    //                startEdge = edge;
//    //                Nodes.Add(startEdge.Node);
//    //            }
//    //        }
//    //    }
//    //    NodeBoundaries.Add(Nodes);
//    //}
//    return EdgeBoundaries;
//}
//private static PlateGraph PlateGraphFromBoundary(List<List<PlanetEdge>> Boundaries, PlanetTectonicPlateParameters parameters)
//{
//    Dictionary<PlanetNode, PlateNode> NodeLookup = new Dictionary<PlanetNode, PlateNode>();
//    Dictionary<PlanetEdge, PlateEdge> EdgeLookup = new Dictionary<PlanetEdge, PlateEdge>();
//    Dictionary<PlanetEdge, PlatePoly> PolyLookup = new Dictionary<PlanetEdge, PlatePoly>();
//    List<PlatePoly> Polygons = new List<PlatePoly>();
//    List<PlateNode> Nodes = new List<PlateNode>();
//    List<PlateEdge> Edges = new List<PlateEdge>();
//    //Setup Pol
//    foreach (var Boundary in Boundaries)
//    {
//        PlatePoly platePoly;
//        Polygons.Add(platePoly = new PlatePoly()
//        {
//            Identity = Polygons.Count,
//            Height = Random.Range(parameters.MinHeight, parameters.MaxHeight + 1)
//        });
//        foreach (var refEdge in Boundary)
//        {
//            PolyLookup[refEdge] = platePoly;

//            PlateEdge plateEdge;
//            Edges.Add(refEdge.PlateEdge = platePoly.Edge = plateEdge = EdgeLookup[refEdge] = new PlateEdge()
//            {
//                Identity = Edges.Count,
//                Poly = platePoly,
//                PlanetEdge = refEdge
//            });

//            var refNode = refEdge.Node;
//            PlateNode plateNode = null;
//            if (!NodeLookup.TryGetValue(refNode, out plateNode))
//            {
//                Nodes.Add(plateNode = NodeLookup[refNode] = new PlateNode() { Identity = Nodes.Count, PlanetNode = refNode, Edge = plateEdge });
//            }
//            plateEdge.Node = plateNode;
//        }
//    }
//    foreach (var Boundary in Boundaries)
//    {
//        PlateEdge prevEdge = EdgeLookup[Boundary[Boundary.Count - 1]];
//        foreach (var refEdge in Boundary)
//        {
//            PlateEdge plateEdge = EdgeLookup[refEdge];
//            PlateEdge twinPlateEdge = EdgeLookup[refEdge.Twin];

//            plateEdge.Twin = twinPlateEdge;
//            twinPlateEdge.Twin = plateEdge;

//            plateEdge.Prev = prevEdge;
//            prevEdge.Next = plateEdge;
//            prevEdge = plateEdge;
//        }
//    }
//    //Queue<PlateEdge> Removed = new Queue<PlateEdge>();
//    //foreach (var plate in Polygons)
//    //{
//    //    foreach (var edge in plate)
//    //    {
//    //        //IF this Edge can jump to next across this poly
//    //        if (edge.PlanetEdge.Poly == edge.Next.PlanetEdge.Poly)
//    //        {
//    //            //If this edge shares a border with the same poly
//    //            if (edge.Twin.Poly == edge.Next.Twin.Poly)
//    //            {
//    //                Removed.Enqueue(edge);
//    //            }
//    //        }
//    //    }
//    //}
//    //while (Removed.Count > 0)
//    //{
//    //    var edgeRemoved = Removed.Dequeue();
//    //    foreach (var plate in Polygons)
//    //    {
//    //        foreach (var edge in plate)
//    //        {
//    //            if (edge == edgeRemoved)
//    //            {
//    //                var start = edge.Prev;
//    //                var end = edge.Next;
//    //                //while (Removed.Count > 0 && Removed.Peek() == end)
//    //                //{
//    //                //    end = Removed.Dequeue().Next;
//    //                //}
//    //                start.Next = end;
//    //                end.Prev = start;
//    //            }
//    //        }
//    //    }
//    //}
//    //foreach (var Boundary in Boundaries)
//    //{
//    //    int l = Boundary.Count;
//    //    for (int j = 0; j < l; j++)
//    //    {
//    //        var edge = Boundary[j];
//    //        var nextEdge = Boundary[(j + 1) % l];
//    //        if (edge != nextEdge && PolyLookup[edge.Twin] == PolyLookup[nextEdge.Twin])
//    //        {
//    //            var plateEdge = EdgeLookup[edge];
//    //            var nextPlateEdge = EdgeLookup[nextEdge];

//    //            plateEdge.Next = nextPlateEdge.Next;
//    //            nextPlateEdge.Prev 

//    //        }
//    //    }
//    //}
//    //HashSet<PlanetEdge> Inspected = new HashSet<PlanetEdge>();
//    //foreach (var Boundary in Boundaries)
//    //{
//    //    int l = Boundary.Count;
//    //    //Queue<TectonicPlateEdge> Removed = new Queue<TectonicPlateEdge>();
//    //    for (int j = 0; j < l; j++)
//    //    {
//    //        var edge = Boundary[j];
//    //        var skipEdge = Boundary[(j + 1) % l];
//    //        //var nextEdge = Boundary[(j + 2) % l];
//    //        //if (Inspected.Contains(nextEdge))
//    //        //    continue;
//    //        //Same polygon and Same border
//    //        if (edge != skipEdge && edge.Poly == skipEdge.Poly && PolyLookup[edge.Twin] == PolyLookup[skipEdge.Twin])
//    //        {
//    //            var plateEdge = EdgeLookup[edge];
//    //            var skipPlateEdge = EdgeLookup[skipEdge];
//    //            Edges.Remove(skipPlateEdge);
//    //            //var nextPlateEdge = EdgeLookup[nextEdge];
//    //            //Debug.Log("Merging " + plateEdge.Identity + " -> " + nextPlateEdge.Identity + "\n" + "Skipping " + skipPlateEdge.Identity);
//    //            plateEdge.Next = skipPlateEdge.Next;
//    //            plateEdge.Next.Prev = plateEdge;

//    //            //var plateEdgeTwin = plateEdge.Twin.Prev;
//    //            //var skipPlateEdgeTwin = skipPlateEdge.Twin.Prev;

//    //            //plateEdgeTwin.Next = skipPlateEdgeTwin.Next;
//    //            //plateEdgeTwin.Next.Prev = plateEdgeTwin;

//    //            //plateEdge.Twin = plateEdgeTwin;
//    //            //plateEdgeTwin.Twin = plateEdge;

//    //            //Inspected.Add(nextEdge);
//    //            //Removed.Enqueue(EdgeLookup[nextEdge]);
//    //            //var plateEdge = EdgeLookup[nextEdge];
//    //            //var prevPlateEdge = plateEdge.Prev;
//    //            //var nextPlateEdge = plateEdge.Next;
//    //            //nextPlateEdge.Prev = prevPlateEdge;
//    //            //prevPlateEdge.Next = nextPlateEdge;
//    //        }
//    //        //if (edge.)
//    //    }
//    //}
//    //    //if (Removed.Count == l)
//    //    //    Removed.Clear();
//    //    //while (Removed.Count > 0)
//    //    //{
//    //    //    var plateEdge = Removed.Dequeue();
//    //    //    var prevPlateEdge = plateEdge.Prev;
//    //    //    var nextPlateEdge = plateEdge.Next;
//    //    //    nextPlateEdge.Prev = prevPlateEdge;
//    //    //    prevPlateEdge.Next = nextPlateEdge;
//    //    //}
//    //}
//    //if (Removed.Count == l)
//    //    Removed.Clear();
//    //while (Removed.Count > 0)
//    //{
//    //    var removing = Removed.Dequeue();
//    //    if (!Inspected.Contains(removing))
//    //    {
//    //        var plateEdge = EdgeLookup[removing];
//    //        //var plateTwinEdge = plateEdge.Twin.Next;
//    //        //We remove the node we are pointing to (which is this node), meaning...
//    //        //We remove this
//    //        //We remove our twin's next

//    //        Edges.Remove(plateEdge);
//    //        //Edges.Remove(plateTwinEdge);


//    //        plateEdge.Prev.Next = plateEdge.Next;
//    //        plateEdge.Next.Prev = plateEdge.Prev;

//    //        //plateTwinEdge.Prev.Next = plateTwinEdge.Next;
//    //        //plateTwinEdge.Next.Prev = plateTwinEdge.Prev;

//    //        Inspected.Add(removing);
//    //        //Inspected.Add(removing.Twin.Next);
//    //    }
//    //}
//    //}
//    //HashSet<PlanetEdge> Inspected = new HashSet<PlanetEdge>();
//    //foreach (var Boundary in Boundaries)
//    //{
//    //    int l = Boundary.Count;
//    //    Queue<PlanetEdge> Removed = new Queue<PlanetEdge>();
//    //    for (int j = 0; j < l; j++)
//    //    {
//    //        var edge = Boundary[j];
//    //        var nextEdge = Boundary[(j + 1) % l];

//    //        if (edge.Poly == nextEdge.Poly && PolyLookup[edge] == PolyLookup[nextEdge])
//    //        {
//    //            Removed.Enqueue(nextEdge);
//    //        }
//    //        //if (edge.)
//    //    }
//    //    if (Removed.Count == l)
//    //        Removed.Clear();
//    //    while (Removed.Count > 0)
//    //    {
//    //        var removing = Removed.Dequeue();
//    //        if (!Inspected.Contains(removing))
//    //        {
//    //            var plateEdge = EdgeLookup[removing];
//    //            var plateTwinEdge = plateEdge.Twin.Next;
//    //            //We remove the node we are pointing to (which is this node), meaning...
//    //            //We remove this
//    //            //We remove our twin's next

//    //            Edges.Remove(plateEdge);
//    //            Edges.Remove(plateTwinEdge);


//    //            plateEdge.Prev.Next = plateEdge.Next;
//    //            plateEdge.Next.Prev = plateEdge.Prev;

//    //            plateTwinEdge.Prev.Next = plateTwinEdge.Next;
//    //            plateTwinEdge.Next.Prev = plateTwinEdge.Prev;

//    //            Inspected.Add(removing);
//    //            Inspected.Add(removing.Twin.Next);
//    //        }
//    //    }
//    //}
//    int i = 0;
//    foreach (var edge in Edges)
//    {
//        edge.Identity = i;
//        edge.Poly.Edge = edge;
//        edge.Node.Edge = edge;
//        i++;
//    }
//    i = 0;
//    foreach (var node in Nodes)
//    {
//        node.Identity = i;
//        i++;
//    }
//    i = 0;
//    foreach (var poly in Polygons)
//    {
//        poly.Identity = i;
//        i++;
//    }

//    return new PlateGraph()
//    {
//        Edges = Edges,
//        Nodes = Nodes,
//        Polys = Polygons
//    };
//}
//            

//       
//        
//        //public static int GetSign(float f)
//        //{
//        //    return (f > 0f ? 1 : (f < 0f ? -1 : 0));
//        //}
//        private static int GetCollisionType(Vector3 p1, Vector3 v1, Vector3 p2, Vector3 v2)
//        {
//            float dot = Vector3.Dot(v2 - v1, p2 - p1);
//            return (dot > 0f ? -1 : (dot < 0f ? 1 : 0));
//        }
//        private static void CalculatePolyMovement(PlanetGraph graph, PlanetTectonicPlateParameters parameters)
//        {

//            float AxisSpin = Random.Range(parameters.MovementParameters.AxisSpin.Min, parameters.MovementParameters.AxisSpin.Max);

//            foreach (PlanetTectonicPlate plate in graph.PlateLookup)
//                plate.Spin = Random.Range(parameters.MovementParameters.PlateSpin.Min, parameters.MovementParameters.PlateSpin.Max);

//            foreach (PlanetPoly Poly in graph.Polys)
//            {
//                Vector3 pN = Poly.Position.normalized;
//                Vector3 planetMovement = Quaternion.AngleAxis(AxisSpin, Vector3.up) * pN;
//                Vector3 plateMovement = Vector3.zero;
//                plateMovement = Quaternion.AngleAxis(Poly.TectonicPlate.Spin, Poly.TectonicPlate.Center.Position.normalized) * pN;

//                //Poly.TectonicPlateVector =// Vector3.ProjectOnPlane(
//                //    Vector3.Lerp(plateMovement - pN, planetMovement - pN, parameters.AxisWeight).normalized;
//                ////, pN);//This is fine
//                Poly.TectonicPlateVector = Vector3.ProjectOnPlane(Vector3.Lerp(plateMovement - pN, planetMovement - pN, parameters.MovementParameters.AxisWeight).normalized, pN);//This is fine
//                if (Poly.TectonicPlateVector.sqrMagnitude > 1f)
//                    throw new System.Exception("Poly Error : " + Poly.TectonicPlateVector);
//            }
//        }
//        private static void CalculateMovement(PlanetGraph graph, PlanetTectonicPlateParameters parameters)
//        {
//            CalculatePolyMovement(graph, parameters);
//            CalculateBoundaries(graph, parameters);
//        }
//        private static void CalculateBoundaries(PlanetGraph graph, PlanetTectonicPlateParameters parameters)
//        {
//            //Boundaries are calculated a little wierdly
//            //FIRST
//            //All boundaries between a Poly and a plate ARE THE SAME
//            //NOTE, not between a Poly anda Poly, a Poly and a plate
//            //This enforces the idea that the boundaries are fault lines along the Poly, as apposed to the Poly edges
//            HashSet<int> InspectedEdges = new HashSet<int>();
//            Queue<List<PlanetEdge>> EdgeQueue = new Queue<List<PlanetEdge>>();
//            foreach (PlanetEdge edge in graph.EdgeLookup)
//            {
//                if (!edge.IsPlateBoundary)
//                    continue;
//                if (InspectedEdges.Contains(edge.Id))
//                    continue;
//                List<PlanetEdge> edgeList = new List<PlanetEdge>();
//                PlanetEdge edgeStart = edge;
//                while (edgeStart.Prev.Twin.Poly.TectonicPlate == edgeStart.Twin.Poly.TectonicPlate)
//                {
//                    if (edgeStart.Prev == edge)
//                        break;
//                    edgeStart = edgeStart.Prev;
//                }
//                PlanetEdge edgeEnd = edgeStart;
//                do
//                {

//                    edgeList.Add(edgeEnd);
//                    InspectedEdges.Add(edgeEnd.Id);
//                    if (edgeEnd.Next == edgeStart)
//                        break;
//                    edgeEnd = edgeEnd.Next;

//                } while (edgeEnd.Next.Twin.Poly.TectonicPlate == edgeEnd.Twin.Poly.TectonicPlate);
//                if (edgeList.Count > 0)
//                    EdgeQueue.Enqueue(edgeList);

//            }

//            while (EdgeQueue.Count > 0)
//            {
//                List<PlanetEdge> edgeList = EdgeQueue.Dequeue();
//                //Vector3 up = Vector3.zero;
//                Vector3 boundaryVector = Vector3.zero;
//                Vector3 boundaryOrigin = Vector3.zero;
//                LinkedList<PlanetEdge> linkedEdges = new LinkedList<PlanetEdge>(edgeList);


//                foreach (PlanetEdge edge in edgeList)
//                {
//                    boundaryVector += edge.Twin.Poly.TectonicPlateVector;
//                }
//                //up /= edges.Count;
//                //up.Normalize();
//                boundaryVector /= linkedEdges.Count;
//                int strength = 1;
//                while (linkedEdges.Count > 0)
//                {
//                    PlanetEdge leftEdge = linkedEdges.First.Value;
//                    PlanetEdge rightEdge = linkedEdges.Last.Value;

//                    if (linkedEdges.Count > 1)//Do we have more than 1?
//                    {
//                        boundaryOrigin += strength * (rightEdge.Origin + rightEdge.Twin.Origin) / 2f;
//                        linkedEdges.RemoveLast();
//                    }
//                    boundaryOrigin += strength * (leftEdge.Origin + leftEdge.Twin.Origin) / 2f;
//                    linkedEdges.RemoveFirst();
//                }
//                boundaryOrigin.Normalize();
//                int collisionType = GetCollisionType(edgeList[0].Poly.Position, edgeList[0].Poly.TectonicPlateVector, boundaryOrigin, boundaryVector);
//                Vector3 right = (edgeList[0].Origin - boundaryOrigin).normalized;
//                Vector3 up = ((edgeList[0].Origin + boundaryOrigin) / 2f).normalized;//Average of two ups form the correct up
//                Vector3 forward = (edgeList[0].Poly.Position - boundaryOrigin).normalized;

//                Vector3 pressure = Vector3.ProjectOnPlane((edgeList[0].Poly.TectonicPlateVector - boundaryVector) / 2f, up);
//                Vector3 tension = Vector3.Project(pressure, forward) / 2f;
//                Vector3 shear = Vector3.Project(pressure, right) / 2f;

//                foreach (PlanetEdge edge in edgeList)
//                    edge.BoundaryPressure = new Vector2(shear.magnitude, collisionType * tension.magnitude);
//                //edgeList[0].BoundaryPressure = new Vector2(shear.magnitude, collisionType * tension.magnitude);

//                //activeEdge = edgeList;
//                //while (activeEdge.Twin.Poly.TectonicPlate == edgeList.Twin.Poly.TectonicPlate)
//                //{
//                //    activeEdge.BoundaryPressure = edgeList.BoundaryPressure;
//                //    activeEdge = activeEdge.Next;
//                //    if (activeEdge == edgeList)
//                //        break;//Incase the plate is surrounded by another plate, if this happens, we will reach this flag
//                //}

//            }


//            ////This needs to be fixed
//            //foreach (PlanetEdge edge in graph.EdgeLookup)
//            //{
//            //    if (!edge.IsPlateBoundary)
//            //        continue;
//            //    Vector3 right = (edge.Origin - edge.Twin.Origin).normalized;
//            //    Vector3 up = ((edge.Origin + edge.Twin.Origin) / 2f).normalized;//Average of two ups form the correct up
//            //    Vector3 forward = (edge.Poly.Position - edge.Twin.Poly.Position).normalized;

//            //    //Vector3 PolyTension = Vector3.Project(edge.Poly.TectonicPlateVector, forward);
//            //    //Vector3 PolyShear = Vector3.Project(edge.Poly.TectonicPlateVector, right);

//            //    //Vector3 twinTension = Vector3.Project(edge.Twin.Poly.TectonicPlateVector, forward);
//            //    //Vector3 twinShear = Vector3.Project(edge.Twin.Poly.TectonicPlateVector, right);
//            //    //int collisionType = GetCollisionType(edge.Poly.Position, edge.Poly.TectonicPlateVector, edge.Twin.Poly.Position, edge.Twin.Poly.TectonicPlateVector);

//            //    ////X is Shear, and ranges from (0_None to 1_Full)
//            //    ////Y is Pressure, and ranges from (0_None to 1_Full)
//            //    //edge.BoundaryPressure = new Vector2((PolyShear - twinShear).magnitude / 2f, collisionType * (PolyTension - twinTension).magnitude / 2f);

//            //    ////Z is CollisonType and ranges from (-1_Colliding to 0_Shearing to 1_Seperating), note that while 0 implies pressure is 0 and shear is 1, this may not occur ( pressure will be 0 but shear may not be 1)

//            //    ////edge.Twin.BoundaryPressure = -1f * edge.BoundaryPressure;

//            //    Vector3 boundaryVector = Vector3.ProjectOnPlane((edge.Poly.TectonicPlateVector - edge.Twin.Poly.TectonicPlateVector) / 2f, up);
//            //    if (boundaryVector.sqrMagnitude > 1f)
//            //        throw new System.Exception("Calculating Boundary Error : " + boundaryVector);


//            //    Vector3 tension = Vector3.Project(boundaryVector, forward) / 2f;

//            //    Vector3 shear = Vector3.Project(boundaryVector, right) / 2f;
//            //    int collisionType = -GetCollisionType(edge.Poly.Position, edge.Poly.TectonicPlateVector, edge.Twin.Poly.Position, edge.Twin.Poly.TectonicPlateVector);

//            //    //X is Shear, and ranges from (0_None to 1_Full)
//            //    //Y is Pressure, and ranges from (0_None to 1_Full)
//            //    edge.BoundaryPressure = new Vector2(shear.magnitude, collisionType * tension.magnitude);
//            //    if (edge.BoundaryPressure.sqrMagnitude > 1f)
//            //    {
//            //        Debug.Log("BV:" + boundaryVector + " -> " + boundaryVector.magnitude + "\nT:" + tension + " -> " + tension.magnitude + "\nS:" + shear + " -> " + shear.magnitude + "\nBP:" + edge.BoundaryPressure + " -> " + edge.BoundaryPressure.magnitude);
//            //        throw new System.Exception("Edge Boundary Error : " + edge.BoundaryPressure);
//            //    }
//            //}
//        }


//        public static void TectonicPlateGenerator(PlanetGraph graph, PlanetTectonicPlateParameters parameters)
//        {
//            List<PlanetTectonicPlate> Plates = new List<PlanetTectonicPlate>();
//            Queue<PlateQueue> processQueue = new Queue<PlateQueue>();
//            Queue<PlanetTectonicPlate> unprunedPlates = new Queue<PlanetTectonicPlate>();
//            PlateQueue activeQueue = null;
//            PlanetPoly activePoly = null;
//            //Debug.Log("A:" + Plates.Count);
//            for (int i = 0; i < parameters.GenerationParameters.DesiredPlates; i++)
//            {
//                activeQueue = new PlateQueue(Plates.Count, Random.Range(parameters.GenerationParameters.ElevationScale.Min, parameters.GenerationParameters.ElevationScale.Max));
//                activePoly = graph.PolyLookup[Random.Range(0, graph.PolyLookup.Length)];
//                Plates.Add(activeQueue.Plate);
//                unprunedPlates.Enqueue(activeQueue.Plate);
//                activeQueue.Enqueue(activePoly);
//                processQueue.Enqueue(activeQueue);
//            }
//            //Debug.Log("B:" + Plates.Count);
//            TectonicPlateFloodFill(Plates, processQueue, parameters);
//            //Debug.Log("C:" + Plates.Count);
//            //Clean plates for Pruning
//            TectonicPlateClean(Plates);
//            foreach (PlanetTectonicPlate plate in Plates)
//                ShufflePlate(plate);//Shuffle the order of the Polys, randomly assigns a new center. This will also make the fact that we dont do a depth check in our flood fill useful
//                                    //Dont shuffle after pruning
//                                    //Debug.Log("D:" + Plates.Count);
//            do
//            {
//                unprunedPlates = TectonicPlatePrune(Plates, unprunedPlates, parameters);
//            }
//            while (unprunedPlates.Count > 0);
//            //Debug.Log("E:" + Plates.Count);
//            //Clean plates for merging
//            TectonicPlateClean(Plates);
//            //Debug.Log("F:" + Plates.Count);
//            TectonicPlateMerge(Plates, parameters);
//            //Debug.Log("G:" + Plates.Count);
//            //Clean plates for finalizing
//            TectonicPlateClean(Plates);
//            //Debug.Log("H:" + Plates.Count);
//            graph.PlateLookup = Plates.ToArray();
//            CalculateMovement(graph, parameters);
//        }


//public static PlateGraph CreateGraph(PlanetGraph graph, PlanetTectonicPlateParameters parameters)
//{
//    PlateGraph plateGraph = CreateFullGraph(graph, parameters);
//    Queue<Queue<PlatePoly>> plateQueue = new Queue<Queue<PlatePoly>>();
//    Queue<List<PlatePoly>> pruned = new Queue<List<PlatePoly>>();

//    for (int i = 0; i < parameters.DesiredPlates; i++)
//    {

//        var plate = plateGraph.Polys[Random.Range(0, plateGraph.Polys.Count)];
//        plateGraph.Polys.Add(plate);
//    }

//    PlateFloodFill(plateQueue);
//    PlateClean(plates);
//    foreach (var plate in plates)
//        pruned.Enqueue(plate);
//    do
//    {
//        pruned = PlatePrune(plates, pruned, parameters);
//    } while (pruned.Count > 0);
//    PlateClean(plates);
//    PlateMerge(plates, parameters);
//    //return RemoveExPlateGraphFromBoundary(GetPlateBoundary(plates), parameters);
//    //HashSet<PlanetPoly>
//    return RemoveExcessFromGraph(CreateFullGraph(plates, parameters));
//}
//private class PlateQueue : Queue<PlanetPoly>
//{
//    public PlateQueue(PlatePoly plate)
//    {
//        Plate = plate;
//    }
//    public PlatePoly Plate { get; private set; }
//}
//for (int i = 0; i < graph.Polys.Count; i++)
//{
//    graph.Polys[i].Identity = i;
//    if (graph.Polys[i].Count <= 0)
//    {
//        graph.Polys[i].RemoveAt(i);
//        i--;
//    }
//}
//filled.Add(activePoly);
//var edgePoly = activePoly;
//foreach (var edge in activePoly)
//{
//    //Get neighbor
//    edgePoly = edge.Twin.Poly;
//    //If neighbor hasnt been set
//    if (!filled.Contains(edgePoly))
//        activeQueue.Enqueue(edgePoly);//Add to queue
//}
////If queue not empty
//if (activeQueue.Count > 0)
//{
//    //add to process
//    plateQueue.Enqueue(activeQueue);
//}