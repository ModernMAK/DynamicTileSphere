using Graphing.Position.Native;
using System;
using Unity.Mathematics;

namespace ProceduralMeshFramework.Native
{
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
}