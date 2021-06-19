using Graphing.Position.Generic.Native;
using Graphing.Position.Native;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMeshFramework.NNatives
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




    public static class Tetrahedron
    {
        private const float
            SQRT_6 = SharedUtil.SQRT_6,
            SQRT_2 = SharedUtil.SQRT_2,
            SQRT_3 = SharedUtil.SQRT_3;

        //ALL Shapes have a circumradius of 1, to convert to their original circumradius, multiply by the circumradius, to conver to their mid or inradius, divide by the circumradius (when at 1), then multiply by the inradius 
        //Dont know what these are? Look at this (https://en.wikipedia.org/wiki/Platonic_solid) bout midway down the page
        public const float
            CircumRadius = SQRT_3 / SQRT_2,
            MidRadius = 1 / SQRT_2,
            InRadius = 1 / SQRT_6;

        public static float GetRadius(RadiusType radius)
        {
            switch (radius)
            {
                case RadiusType.Circumradius:
                    return CircumRadius;
                case RadiusType.Midradius:
                    return MidRadius;
                case RadiusType.Inradius:
                    return InRadius;
                case RadiusType.Normalized:
                    return 1f;//This is always 1 because the array is normalized.
                default:
                    throw new ArgumentOutOfRangeException(nameof(radius));
            }
        }

        public const int PolyCount = 4;//4 Polygons
        public const int SoftVertCount = 4;//4 Polygons (Triangles), 3 Half Edges Per Polygon (Triangle)
        public const int PolyVertCount = 3;
        private static readonly float3[] Vertexes = new float3[SoftVertCount]
        {
            math.normalize(new float3(1f, 1f, 1f)),
            math.normalize(new float3(1f, -1f, -1f)),
            math.normalize(new float3(-1f, 1f, -1f)),
            math.normalize(new float3(-1f, -1f, 1f))
        };
        /// <summary>
        /// 4 Triangles ~ [PolygonIndex, VertexIndex]
        /// Order matters if converting to mesh, not converting to graph
        /// </summary>
        private static readonly int[,] _Indexes = new int[PolyCount, PolyVertCount]
        {
             { 0, 1, 2 },// 1st Tri
             { 3, 1, 0 },// 2nd Tri
             {0, 2, 3 },// 3rd Tri
             {3, 2, 1 },// 4th Tri
        };
        private static readonly int[] Indexes = SharedUtil.Flatten(_Indexes);
        private static readonly int[] Twins = SharedUtil.CalculateTwins(Indexes, PolyCount);
        private static readonly int[] NodeEdges = SharedUtil.CalculateNodeEdges(Indexes, SoftVertCount);
        private static readonly int[] PolyEdges = SharedUtil.CalculatePolyEdges(PolyVertCount, PolyCount);


        /// <summary>
        /// Builds a soft graph.
        /// </summary>
        /// <returns></returns>
        public static PositionGraph BuildGraph() => SharedUtil.BuildGraph(Vertexes, Indexes, NodeEdges, PolyEdges, Twins);
    }
    public static class Octahedron
    {
        private const float
            SQRT_2 = SharedUtil.SQRT_2,
            SQRT_3 = SharedUtil.SQRT_3;

        //ALL Shapes have a circumradius of 1, to convert to their original circumradius, multiply by the circumradius, to conver to their mid or inradius, divide by the circumradius (when at 1), then multiply by the inradius 
        //Dont know what these are? Look at this (https://en.wikipedia.org/wiki/Platonic_solid) bout midway down the page
        public const float
            CircumRadius = SQRT_2,
            MidRadius = 1,
            InRadius = SQRT_2 / SQRT_3;

        public static float GetRadius(RadiusType radius)
        {
            switch (radius)
            {
                case RadiusType.Circumradius:
                    return CircumRadius;
                case RadiusType.Midradius:
                    return MidRadius;
                case RadiusType.Inradius:
                    return InRadius;
                case RadiusType.Normalized:
                    return 1f;
                default:
                    throw new ArgumentOutOfRangeException(nameof(radius));
            }
        }

        public const int PolyCount = 8;
        public const int SoftVertCount = 6;
        public const int PolyVertCount = 3;
        private static readonly float3[] Vertexes = new float3[SoftVertCount]
        {
            math.normalize(new float3(1f, 0f, 0f)),
            math.normalize(new float3(0f, 1f, 0f)),
            math.normalize(new float3(0f, 0f, 1f)),
            math.normalize(new float3(-1f, 0f, 0f)),
            math.normalize(new float3(0f, -1f, 0f)),
            math.normalize(new float3(0f, 0f, -1f))
        };
        /// <summary>
        /// 4 Triangles ~ [PolygonIndex, VertexIndex]
        /// Order matters if converting to mesh, not converting to graph
        /// </summary>
        /// <remarks>Changing this will require Twins, NodeEdges, & PolyEdges to be updated.</remarks>
        private static readonly int[,] _Indexes = new int[PolyCount, PolyVertCount]
        {
            {0, 1, 2},
            {0, 2, 4},
            {0, 4, 5},
            {0, 5, 1},
            {3, 2, 1},
            {3, 4, 2},
            {3, 5, 4},
            {3, 1, 5},
        };
        private static readonly int[] Indexes = SharedUtil.Flatten(_Indexes);

        //Hardcoded to avoid writing an algorithm for something that could be calculated once; this does mean this breaks if we change the INDEXES
        private static readonly int[] Twins = SharedUtil.CalculateTwins(Indexes, PolyCount);
        private static readonly int[] NodeEdges = SharedUtil.CalculateNodeEdges(Indexes, PolyCount);
        private static readonly int[] PolyEdges = SharedUtil.CalculatePolyEdges(PolyVertCount, PolyCount);


        /// <summary>
        /// Builds a soft graph.
        /// </summary>
        /// <returns></returns>
        public static PositionGraph BuildGraph() => SharedUtil.BuildGraph(Vertexes, Indexes, NodeEdges, PolyEdges, Twins);
    }
    public static class Cube
    {
        private const float
            SQRT_2 = SharedUtil.SQRT_2,
            SQRT_3 = SharedUtil.SQRT_3;

        //ALL Shapes have a circumradius of 1, to convert to their original circumradius, multiply by the circumradius, to conver to their mid or inradius, divide by the circumradius (when at 1), then multiply by the inradius 
        //Dont know what these are? Look at this (https://en.wikipedia.org/wiki/Platonic_solid) bout midway down the page
        public const float
            CircumRadius = SQRT_3,
            MidRadius = SQRT_2,
            InRadius = 1f;
        public static float GetRadius(RadiusType radius)
        {
            switch (radius)
            {
                case RadiusType.Circumradius:
                    return CircumRadius;
                case RadiusType.Midradius:
                    return MidRadius;
                case RadiusType.Inradius:
                    return InRadius;
                case RadiusType.Normalized:
                    return 1f;
                default:
                    throw new ArgumentOutOfRangeException(nameof(radius));
            }
        }

        public const int PolyCount = 6;
        public const int SoftVertCount = 8;
        public const int PolyVertCount = 4;
        private static readonly float3[] Vertexes = new float3[SoftVertCount]
        {
            math.normalize(new float3(1f, 1f, 1f)),
            math.normalize(new float3(1f, 1f, -1f)),
            math.normalize(new float3(1f, -1f, 1f)),
            math.normalize(new float3(1f, -1f, -1f)),
            math.normalize(new float3(-1f, 1f, 1f)),
            math.normalize(new float3(-1f, 1f, -1f)),
            math.normalize(new float3(-1f, -1f, 1f)),
            math.normalize(new float3(-1f, -1f, -1f))
        };
        /// <summary>
        /// 4 Triangles ~ [PolygonIndex, VertexIndex]
        /// Order matters if converting to mesh, not converting to graph
        /// </summary>
        /// <remarks>Changing this will require Twins, NodeEdges, & PolyEdges to be updated.</remarks>
        private static readonly int[,] _Indexes = new int[PolyCount, PolyVertCount]
        {
            // Right
            {2, 1, 0, 3},
            // Top
            {5, 4, 0, 1 },
            //Forward
            {1, 3, 7, 5},
            //Left
            {4, 5, 7, 6 },
            //Down
            {2, 6, 7, 3 },
            //Back
            {4, 2, 0, 6 }
        };
        private static readonly int[] Indexes = SharedUtil.Flatten(_Indexes);

        //Hardcoded to avoid writing an algorithm for something that could be calculated once; this does mean this breaks if we change the INDEXES
        private static readonly int[] Twins = SharedUtil.CalculateTwins(Indexes, PolyCount);
        private static readonly int[] NodeEdges = SharedUtil.CalculateNodeEdges(Indexes, PolyCount);
        private static readonly int[] PolyEdges = SharedUtil.CalculatePolyEdges(PolyVertCount, PolyCount);


        /// <summary>
        /// Builds a soft graph.
        /// </summary>
        /// <returns></returns>
        public static PositionGraph BuildGraph() => SharedUtil.BuildGraph(Vertexes, Indexes, NodeEdges, PolyEdges, Twins);
    }
    public static class Icosahedron
    {
        private const float
            SQRT_3 = SharedUtil.SQRT_3,
            PHI = SharedUtil.PHI,
            XI = SharedUtil.XI;

        //ALL Shapes have a circumradius of 1, to convert to their original circumradius, multiply by the circumradius, to conver to their mid or inradius, divide by the circumradius (when at 1), then multiply by the inradius 
        //Dont know what these are? Look at this (https://en.wikipedia.org/wiki/Platonic_solid) bout midway down the page
        public const float
            CircumRadius = PHI * PHI / SQRT_3,
            MidRadius = PHI,
            InRadius = XI * PHI;
        public static float GetRadius(RadiusType radius)
        {
            switch (radius)
            {
                case RadiusType.Circumradius:
                    return CircumRadius;
                case RadiusType.Midradius:
                    return MidRadius;
                case RadiusType.Inradius:
                    return InRadius;
                case RadiusType.Normalized:
                    return 1f;
                default:
                    throw new ArgumentOutOfRangeException(nameof(radius));
            }
        }

        public const int PolyCount = 20;
        public const int SoftVertCount = 12;
        public const int PolyVertCount = 3;
        private static readonly float3[] Vertexes = new float3[SoftVertCount]
        {
            math.normalize(new float3(0f, 1f, PHI)),
            math.normalize(new float3(0f, -1f, PHI)),
            math.normalize(new float3(0f, 1f, -PHI)),
            math.normalize(new float3(0f, -1f, -PHI)),

            math.normalize(new float3(1f, PHI, 0f)),
            math.normalize(new float3(-1f, PHI, 0f)),
            math.normalize(new float3(1f, -PHI, 0f)),
            math.normalize(new float3(-1f, -PHI, 0f)),

            math.normalize(new float3(PHI, 0f, 1f)),
            math.normalize(new float3(PHI, 0f, -1f)),
            math.normalize(new float3(-PHI, 0f, 1f)),
            math.normalize(new float3(-PHI, 0f, -1f))
        };
        /// <summary>
        /// 4 Triangles ~ [PolygonIndex, VertexIndex]
        /// Order matters if converting to mesh, not converting to graph
        /// </summary>
        /// <remarks>Changing this will require Twins, NodeEdges, & PolyEdges to be updated.</remarks>
        private static readonly int[,] _Indexes = new int[PolyCount, PolyVertCount]
        {
            
            //One peak, at 8
            {0, 1, 8},
            {4, 0, 8},
            {9, 4, 8},
            {6, 9, 8},
            {1, 6, 8},

            //The other peak, at 11
            {5, 2, 11},
            {2, 3, 11},
            {3, 7, 11},
            {7, 10, 11},
            {10, 5, 11},

            //The Strip
            {0, 10, 1},
            {10, 7, 1},
            {7, 6, 1},
            {7, 3, 6},
            {3, 9, 6},
            {2, 9, 3},
            {2, 4, 9},
            {2, 5, 4 },
            {4, 5, 0 },
            {5, 10, 0 }
        };
        private static readonly int[] Indexes = SharedUtil.Flatten(_Indexes);

        //Hardcoded to avoid writing an algorithm for something that could be calculated once; this does mean this breaks if we change the INDEXES
        private static readonly int[] Twins = SharedUtil.CalculateTwins(Indexes, PolyCount);
        private static readonly int[] NodeEdges = SharedUtil.CalculateNodeEdges(Indexes, PolyCount);
        private static readonly int[] PolyEdges = SharedUtil.CalculatePolyEdges(PolyVertCount, PolyCount);


        /// <summary>
        /// Builds a soft graph.
        /// </summary>
        /// <returns></returns>
        public static PositionGraph BuildGraph() => SharedUtil.BuildGraph(Vertexes, Indexes, NodeEdges, PolyEdges, Twins);
    }
    public static class Dodecahedron
    {
        private const float
            SQRT_2 = SharedUtil.SQRT_2,
            SQRT_3 = SharedUtil.SQRT_3,
            PHI = SharedUtil.PHI,
            INV_PHI = SharedUtil.INV_PHI,
            XI = SharedUtil.XI;

        //ALL Shapes have a circumradius of 1, to convert to their original circumradius, multiply by the circumradius, to conver to their mid or inradius, divide by the circumradius (when at 1), then multiply by the inradius 
        //Dont know what these are? Look at this (https://en.wikipedia.org/wiki/Platonic_solid) bout midway down the page
        public const float
            CircumRadius = PHI * PHI / XI,
            MidRadius = PHI * PHI,
            InRadius = SQRT_3 * PHI;

        public static float GetRadius(RadiusType radius)
        {
            switch (radius)
            {
                case RadiusType.Circumradius:
                    return CircumRadius;
                case RadiusType.Midradius:
                    return MidRadius;
                case RadiusType.Inradius:
                    return InRadius;
                case RadiusType.Normalized:
                    return 1f;
                default:
                    throw new ArgumentOutOfRangeException(nameof(radius));
            }
        }

        public const int PolyCount = 12;
        public const int SoftVertCount = 20;
        public const int PolyVertCount = 5;
        private static readonly float3[] Vertexes = new float3[SoftVertCount]
        {
            math.normalize(new float3(1f, 1f, 1f)), //0
            math.normalize(new float3(1f, 1f, -1f)), //1
            math.normalize(new float3(1f, -1f, 1f)), //2
            math.normalize(new float3(1f, -1f, -1f)), //3
            math.normalize(new float3(-1f, 1f, 1f)), //4
            math.normalize(new float3(-1f, 1f, -1f)), //5
            math.normalize(new float3(-1f, -1f, 1f)), //6
            math.normalize(new float3(-1f, -1f, -1f)), //7

            math.normalize(new float3(0f, INV_PHI, PHI)), //8
            math.normalize(new float3(0f, -INV_PHI, PHI)), //9
            math.normalize(new float3(0f, INV_PHI, -PHI)), //10
            math.normalize(new float3(0f, -INV_PHI, -PHI)), //11

            math.normalize(new float3(INV_PHI, PHI, 0f)), //12
            math.normalize(new float3(-INV_PHI, PHI, 0f)), //13
            math.normalize(new float3(INV_PHI, -PHI, 0f)), //14
            math.normalize(new float3(-INV_PHI, -PHI, 0f)), //15

            math.normalize(new float3(PHI, 0f, INV_PHI)), //16
            math.normalize(new float3(PHI, 0f, -INV_PHI)), //17
            math.normalize(new float3(-PHI, 0f, INV_PHI)), //18
            math.normalize(new float3(-PHI, 0f, -INV_PHI)) //19          
        };
        /// <summary>
        /// 4 Triangles ~ [PolygonIndex, VertexIndex]
        /// Order matters if converting to mesh, not converting to graph
        /// </summary>
        /// <remarks>Changing this will require Twins, NodeEdges, & PolyEdges to be updated.</remarks>
        private static readonly int[,] _Indexes = new int[PolyCount, PolyVertCount]
        {
            {0, 16, 17, 1, 12},
            {2, 16, 0, 8, 9},
            {14, 3, 17, 16, 2},
            {0, 12, 13, 4, 8},
            {1, 10, 5, 13, 12},
            {13, 5, 19, 18, 4},
            {6, 9, 8, 4, 18},
            {2, 9, 6, 15, 14},
            {1, 17, 3, 11, 10},
            {7, 19, 5, 10, 11},
            {3, 14, 15, 7, 11},
            {19, 7, 15, 6, 18}
        };
        private static readonly int[] Indexes = SharedUtil.Flatten(_Indexes);

        //Hardcoded to avoid writing an algorithm for something that could be calculated once; this does mean this breaks if we change the INDEXES
        private static readonly int[] Twins = SharedUtil.CalculateTwins(Indexes, PolyCount);
        private static readonly int[] NodeEdges = SharedUtil.CalculateNodeEdges(Indexes, PolyCount);
        private static readonly int[] PolyEdges = SharedUtil.CalculatePolyEdges(PolyVertCount, PolyCount);


        /// <summary>
        /// Builds a soft graph.
        /// </summary>
        /// <returns></returns>
        public static PositionGraph BuildGraph() => SharedUtil.BuildGraph(Vertexes, Indexes, NodeEdges, PolyEdges, Twins);
    }

    public static class ProceduralPlatonicSolidGenerator
    {
        public static PositionGraph BuildGraph(ShapeType shape)
        {
            switch (shape)
            {
                case ShapeType.Tetrahedron:
                    return Tetrahedron.BuildGraph();
                case ShapeType.Octahedron:
                    return Octahedron.BuildGraph();
                case ShapeType.Cube:
                    return Cube.BuildGraph();
                case ShapeType.Icosahedron:
                    return Icosahedron.BuildGraph();
                case ShapeType.Dodecahedron:
                    return Dodecahedron.BuildGraph();
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape));
            }
        }
        public static float GetConversion(ShapeType shape, RadiusType radius)
        {
            switch (shape)
            {
                case ShapeType.Tetrahedron:
                    return Tetrahedron.GetRadius(radius);
                case ShapeType.Octahedron:
                    return Octahedron.GetRadius(radius);
                case ShapeType.Cube:
                    return Cube.GetRadius(radius);
                case ShapeType.Icosahedron:
                    return Icosahedron.GetRadius(radius);
                case ShapeType.Dodecahedron:
                    return Dodecahedron.GetRadius(radius);
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape));
            }
        }
    }
}