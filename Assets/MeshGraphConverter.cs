using System;
using System.Collections.Generic;

using Graphing.Position;
public class MeshGraphConverter
{


    public class KeyCompare<K, V> : IComparer<KeyValuePair<K, V>>
        where K : IComparable<K>
    {
        public int Compare(KeyValuePair<K, V> x, KeyValuePair<K, V> y)
        {
            return x.Key.CompareTo(y.Key);
        }
    }
    public static class BinarySearch
    {
        public static V BinaryKeyValue<K, V>(KeyValuePair<K, V>[] inputArray, K key)
             where K : IComparable<K>
        {
            return BinaryKeyValue(inputArray, key, 0, inputArray.Length - 1);
        }
        public static V BinaryKeyValue<K, V>(KeyValuePair<K, V>[] inputArray, K key, int min, int max)
             where K : IComparable<K>
        {
            int index = BinaryKeySearch(inputArray, key, min, max);
            if (index == -1)
                throw new KeyNotFoundException("Key : " + key.ToString());
            return inputArray[index].Value;
        }

        public static int BinaryKeySearch<K, V>(KeyValuePair<K, V>[] inputArray, K key)
            where K : IComparable<K>
        {
            return BinaryKeySearch(inputArray, key, 0, inputArray.Length - 1);
        }
        public static int BinaryKeySearch<K, V>(KeyValuePair<K, V>[] inputArray, K key, int min, int max)
            where K : IComparable<K>
        {
            while (min <= max)
            {
                int mid = (min + max) / 2;
                int delta = key.CompareTo(inputArray[mid].Key);
                if (delta > 0)
                {
                    min = mid + 1;
                }
                else if (delta < 0)
                {
                    max = mid - 1;
                }
                else return mid;
            }
            return -1;
        }

    }
    public static class QuickSortExtensions
    {
        public static void Quicksort<T, C>(T[] input, C comparar) where C : IComparer<T>
        {
            Quicksort(input, 0, input.Length - 1, comparar);
        }
        public static void Quicksort<T, C>(T[] input, int low, int high, C comparar) where C : IComparer<T>

        {
            int pivot_loc = 0;
            Stack<int> stack = new Stack<int>(input.Length);
            stack.Push(low);
            stack.Push(high);

            while (stack.Count > 0)
            {
                high = stack.Pop();
                low = stack.Pop();

                pivot_loc = Partition(input, low, high, comparar);
                if (pivot_loc - 1 > low)
                {
                    stack.Push(low);
                    stack.Push(pivot_loc - 1);
                }


                if (pivot_loc + 1 < high)
                {
                    stack.Push(pivot_loc + 1);
                    stack.Push(high);
                }

            }
            //Recursive
            //if (low < high)
            //{
            //    pivot_loc = Partition(input, low, high, comparar);
            //    Quicksort(input, low, pivot_loc - 1, comparar);
            //    Quicksort(input, pivot_loc + 1, high, comparar);
            //}
        }
        private static int Partition<T, C>(T[] input, int low, int high, C comparar) where C : IComparer<T>
        {
            T pivot = input[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                int comp = comparar.Compare(input[j], pivot);
                if (comp <= 0)
                {
                    i++;
                    Swap(input, i, j);
                }
            }
            Swap(input, i + 1, high);
            return i + 1;
        }


        public static void Quicksort<T>(T[] input) where T : IComparable<T>
        {
            Quicksort(input, 0, input.Length - 1);
        }
        public static void Quicksort<T>(T[] input, int low, int high) where T : IComparable<T>
        {

            int pivot_loc = 0;
            Stack<int> stack = new Stack<int>(input.Length);
            stack.Push(low);
            stack.Push(high);

            while (stack.Count > 0)
            {
                high = stack.Pop();
                low = stack.Pop();

                pivot_loc = Partition(input, low, high);
                if (pivot_loc - 1 > low)
                {
                    stack.Push(low);
                    stack.Push(pivot_loc - 1);
                }


                if (pivot_loc + 1 < high)
                {
                    stack.Push(pivot_loc + 1);
                    stack.Push(high);
                }

            }

        }
        private static int Partition<T>(T[] input, int low, int high) where T : IComparable<T>
        {
            T pivot = input[high];
            int i = low - 1;

            for (int j = low; j < high - 1; j++)
            {
                int comp = input[j].CompareTo(pivot);
                if (comp <= 0)
                {
                    i++;
                    Swap(input, i, j);
                }
            }
            Swap(input, i + 1, high);
            return i + 1;
        }
        private static void Swap<T>(T[] ar, int a, int b)
        {
            T temp = ar[a];
            ar[a] = ar[b];
            ar[b] = temp;
        }

    }
    public static VD_Graph CreateGraph(ProceduralMeshBuilder pmb)
    {
        return CreateGraph(pmb.Verticies.ToArray(), pmb.Triangles.ToArray());
    }
    public static VD_Graph CreateGraph(ProceduralVertex[] mv, ProceduralTriangle[] tris)
    {
        KeyValuePair<long, int>[] dEdgeLookup = new KeyValuePair<long, int>[tris.Length * 3];
        long[] dTwin = new long[tris.Length * 3];
         graph = new VD_Graph()
        {
            DNodeLookup = new VD_DNode[mv.Length],
            VPolyLookup = new VD_VPoly[mv.Length],

            DEdgeLookup = new VD_DEdge[tris.Length * 3],
            VEdgeLookup = new VD_VEdge[tris.Length * 3],

            DPolyLookup = new VD_DPoly[tris.Length],
            VNodeLookup = new VD_VNode[tris.Length],
        };
        for (int i = 0; i < mv.Length; i++)
        {

            graph.DNodeLookup[i] = new VD_DNode()
            {
                Id = i,
                Position = mv[i].Position,
                Graph = graph
            };

            graph.VPolyLookup[i] = new VD_VPoly()
            {
                Id = i,
                Graph = graph
            };

            //graph.VPolyLookup[i].Id = i;
            //graph.VPolyLookup[i].Graph = graph;
        }
        for (int i = 0; i < tris.Length; i++)
        {
            ProceduralTriangle triangle = tris[i];

            graph.VNodeLookup[i] = new VD_VNode(i, graph);

            graph.DPolyLookup[i] = new VD_DPoly()
            {
                Id = i,
                Graph = graph,
                Position = (mv[triangle.Pivot].Position + mv[triangle.Left].Position + mv[triangle.Right].Position) / 3f,
            };

            for (int j = 0; j < 3; j++)
            {

                int vertJ = triangle[j];
                int vertJn = triangle[(j + 1) % 3];
                int edgeId = i * 3 + j;

                long
                    key = ((long)vertJ << 32) + (long)vertJn,
                    revKey = ((long)vertJn << 32) + (long)vertJ;

                dEdgeLookup[edgeId] = new KeyValuePair<long, int>(key, edgeId);
                dTwin[edgeId] = revKey;

                graph.DEdgeLookup[edgeId] = new VD_DEdge()
                {
                    Id = edgeId,
                    Graph = graph,
                    OriginId = vertJ,
                    PolygonId = i,
                    NextId = (i * 3) + ((j + 1) % 3),
                    PrevId = (i * 3) + ((j + 2) % 3),

                };

                graph.VEdgeLookup[edgeId] = new VD_VEdge(edgeId, graph);

                graph.DPolyLookup[i].EdgeId = graph.DNodeLookup[vertJ].EdgeId = edgeId;
            }
        }

        QuickSortExtensions.Quicksort(dEdgeLookup, new KeyCompare<long, int>());

        for (int i = 0; i < tris.Length * 3; i++)
        {
            int twinId = BinarySearch.BinaryKeyValue(dEdgeLookup, dTwin[i]);
            graph.DEdgeLookup[i].TwinId = twinId;
        }

        return graph;
    }


}
