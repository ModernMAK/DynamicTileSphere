using Graphing.Position.Native;
using System;
using Unity.Mathematics;

namespace ProceduralMeshFramework.Native
{
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
}