using Graphing.Position.Generic.Native;
using Graphing.Position.Native;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ProceduralMeshFramework.Native
{
	public static class SharedUtil
    {
        public static int[] CalculateTwins(IList<int> Indexes, int PolyCount)
        {
            int PolyVertCount = Indexes.Count / PolyCount;
            var twins = new int[Indexes.Count];
            for (var i = 0; i < twins.Length; i++)
                twins[i] = -1; // Init to -1 to allow skipping on 0

            // This algo is a mess; but simply put
            //  ~ ITERATE OVER EACH POLYGON (PolyA)
            //      ~ ITERATE OVER EACH EDGE (PolyA Edge) ... we use current and next index of the triangle as an 'edge'
            //      > SKIP IF EDGE HAS TWIN           
            //      > FIND TWIN
            //          ~ ITERATE OVER EACH POLY (PolyB) ...skip PolyA so PolyA != PolyB
            //              ~ ITERATE OVER EACH EDGE (PolyB Edge)
            //              > SKIP IF EDGE HAS TWIN
            //                  ~ COMPARE EDGE 
            //                  > If Found Go to next PolyA Edge (break)

            for (var i = 0; i < PolyCount; i++)
            {
                var iOffset = i * PolyVertCount;
                for (var j = 0; j < PolyVertCount; j++)
                {
                    if (twins[iOffset + j] != -1)
                        continue;

                    var jNext = (j + 1) % PolyVertCount;

                    var a = Indexes[iOffset + j];
                    var b = Indexes[iOffset + jNext];

                    var found = false;
                    for (var k = 0; k < PolyCount && k != i && !found; k++)
                    {
                        var kOffset = k * PolyVertCount;
                        for (var l = 0; l < PolyVertCount && !found; l++)
                        {
                            if (twins[kOffset + l] != -1)
                                continue;

                            var lNext = (l + 1) % PolyVertCount;

                            var c = Indexes[kOffset + l];
                            var d = Indexes[kOffset + lNext];

                            if (a == d && b == c)
                            {
                                twins[iOffset + j] = kOffset + l;
                                twins[kOffset + l] = iOffset + j;
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }
            return twins;
        }

        public static int[] CalculatePolyEdges(int PolyVertCount, int PolyCount)
        {
            var polys = new int[PolyCount];
            for (var i = 0; i < polys.Length; i++)
                polys[i] = PolyVertCount * i;
            return polys;
        }
        public static int[] CalculateNodeEdges(IList<int> Indexes, int SoftVertexCount)
        {
            var nodes = new int[SoftVertexCount];
            var found = 0;
            for (var i = 0; i < nodes.Length; i++)
                nodes[i] = -1;
            for (var i = 0; i < Indexes.Count && found < SoftVertexCount; i++)
            {
                var index = Indexes[i];
                if (nodes[index] == -1)
                {
                    nodes[index] = i;
                    found++;
                }
            }
            return nodes;
        }
        public static int[] Flatten(int[,] arr)
        {
            var u = arr.GetLength(0);
            var v = arr.GetLength(1);
            var flat = new int[u * v];
            for (var i = 0; i < flat.Length; i++)
            {
                var j = i / u;
                var k = i % u;
                flat[i] = arr[j, k];
            }
            return flat;
        }

        public static PositionGraph BuildGraph(IList<float3> Vertexes, IList<int> Indexes, IList<int> Twins, IList<int> NodeEdges, IList<int> PolyEdges)
        {
            var PolyCount = PolyEdges.Count;
            var PolyVertCount = Indexes.Count / PolyCount;

            var graph = new PositionGraph(Vertexes.Count, PolyCount * PolyVertCount, PolyCount);
            //Initilaize Vertexes
            for (var i = 0; i < Vertexes.Count; i++)
            {
                var node = graph.Nodes[i];
                node.Data = new PositionData { Position = Vertexes[i] };
                node.Edge = NodeEdges[i];
                graph.Nodes[i] = node;
            }
            // Initilalize Polygons & Edges
            for (var polyIndex = 0; polyIndex < PolyCount; polyIndex++)
            {
                var poly = graph.Polygons[polyIndex];
                poly.Edge = PolyEdges[polyIndex];
                graph.Polygons[polyIndex] = poly;


                var edgeOffset = polyIndex * PolyVertCount;
                //Initialize Edges
                for (var edgeIndex = 0; edgeIndex < PolyVertCount; edgeIndex++)
                {
                    var edge = graph.Edges[edgeOffset + edgeIndex];

                    edge.Next = edgeOffset + (edgeIndex + 1) % PolyVertCount;
                    edge.Prev = edgeOffset + (edgeIndex + PolyVertCount - 1) % PolyVertCount;

                    var fullIndex = polyIndex * PolyVertCount + edgeIndex;

                    edge.Twin = Twins[fullIndex];
                    edge.Node = Indexes[fullIndex];

                    edge.Poly = polyIndex;

                    graph.Edges[edgeOffset + edgeIndex] = edge;
                }

            }
            return graph;
        }

        public const float
            PHI = 1.61803398875f,
            INV_PHI = 1 / PHI,

            //These 4 numbers are used for the Cirucm/Mid/In radius stuff
            XI = 1.17557050458f,
            SQRT_6 = 2.44948974278f,
            SQRT_2 = 1.41421356237f,
            SQRT_3 = 1.73205080757f;

    }
}