using Graphing.Position.Native;
using System;
using Unity.Mathematics;

namespace ProceduralMeshFramework.Native
{
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
}