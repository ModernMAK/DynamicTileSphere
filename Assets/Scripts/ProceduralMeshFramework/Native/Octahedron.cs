using Graphing.Position.Native;
using System;
using Unity.Mathematics;

namespace ProceduralMeshFramework.Native
{
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
}