using Graphing.Position.Native;
using System;
using Unity.Mathematics;

namespace ProceduralMeshFramework.Native
{
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
}