using System.Collections.Generic;
using UnityEngine;

namespace ProceduralMeshFramework
{
    public class ProceduralMeshUtility
    {
        public static bool SoftEquals(Vector3 a, Vector3 b, float precision)
        {
            var delta = a - b;

            return delta.sqrMagnitude <= precision * precision;
        }

        private static bool SoftEquals(ProceduralVertex a, ProceduralVertex b, float precision = 0f)
        {
            if (precision < 0f)
                precision = 0f;
            return SoftEquals(a.Position, b.Position, precision);
        }

        public static void Spherize(ProceduralMeshBuilder PMB, float radius = 1f)
        {
            ProceduralVertex pVertex;
            var sqrRad = radius * radius;
            for (var i = 0; i < PMB.Verticies.Count; i++)
            {
                pVertex = PMB.Verticies[i];
                if (pVertex.Position.sqrMagnitude != sqrRad)
                {
                    pVertex.Position = pVertex.Position.normalized * radius;
                    PMB.Verticies[i] = pVertex;
                }
            }
        }

        public static void ModifyRadius(ProceduralMeshBuilder PMB, float scale)
        {
            ProceduralVertex pVertex;
            for (var i = 0; i < PMB.Verticies.Count; i++)
            {
                pVertex = PMB.Verticies[i];
                pVertex.Position = pVertex.Position.normalized * scale;
                PMB.Verticies[i] = pVertex;
            }
        }

        public static void Subdivide(ProceduralMeshBuilder builder, int divisions, bool slerp = false,
            float precision = 0.001f, bool hard = false)
        {
            if (hard)
                SubdivideHard(builder, divisions, slerp, precision);
            else
                SubdivideSoft(builder, divisions, slerp, precision);
        }

        public static void SubdivideHard(ProceduralMeshBuilder builder, int divisions, bool slerp = false,
            float precision = 0.001f)
        {
            if (divisions <= 0)
                return;

            var triangles = builder.Triangles.ToArray();
            var verts = builder.Verticies.ToArray();
            builder.Triangles.Clear();


            foreach (var triangle in triangles)
            {
                ProceduralVertex
                    pivot = verts[triangle.Pivot],
                    left = verts[triangle.Left],
                    right = verts[triangle.Right];

                var ySteps = divisions + 1;
                var tempVerticies = new ProceduralVertex[ySteps + 1][];
                for (var y = 0; y <= ySteps; y++)
                {
                    var yScale = 1f / ySteps; //No need for a plus 1
                    ProceduralVertex
                        xL = slerp
                            ? ProceduralVertex.Slerp(pivot, left, y * yScale)
                            : ProceduralVertex.Lerp(pivot, left, y * yScale),
                        xR = slerp
                            ? ProceduralVertex.Slerp(pivot, right, y * yScale)
                            : ProceduralVertex.Lerp(pivot, right, y * yScale);

                    var xSteps = y;
                    tempVerticies[y] = new ProceduralVertex[xSteps + 1];
                    for (var x = 0; x <= xSteps; x++)
                    {
                        var xScale = 1f / (xSteps > 0 ? xSteps : 1);
                        var mv = slerp
                            ? ProceduralVertex.Slerp(xL, xR, x * xScale)
                            : ProceduralVertex.Lerp(xL, xR, x * xScale);
                        tempVerticies[y][x] = mv;
                    }
                }

                ySteps = divisions;
                for (var y = 0; y <= ySteps; y++)
                {
                    var yN = y + 1;
                    var xSteps = y;
                    for (var x = 0; x <= xSteps; x++)
                    {
                        var xN = x + 1;
                        builder.AddTriangle(tempVerticies[y][x], tempVerticies[yN][x], tempVerticies[yN][xN]);
                        if (xN <= y)
                            builder.AddTriangle(tempVerticies[y][x], tempVerticies[yN][xN], tempVerticies[y][xN]);
                    }
                }
            }
        }

        public static void SubdivideSoft(ProceduralMeshBuilder builder, int divisions, bool slerp = false,
            float precision = 0.001f)
        {
            if (divisions <= 0)
                return;

            var triangles = builder.Triangles.ToArray();
            var verts = builder.Verticies.ToArray();
            builder.Triangles.Clear();
            var Lookup = new Dictionary<SubdivisionEdgeHelper, Dictionary<int, int>>();


            foreach (var triangle in triangles)
            {
                ProceduralVertex
                    pivot = verts[triangle.Pivot],
                    left = verts[triangle.Left],
                    right = verts[triangle.Right];

                SubdivisionEdgeHelper
                    originLeft = new SubdivisionEdgeHelper(pivot, left),
                    originRight = new SubdivisionEdgeHelper(pivot, right),
                    leftRight = new SubdivisionEdgeHelper(left, right);

                bool
                    reversePivotLeft = false,
                    reversePivotRight = false,
                    reverseLeftRight = false;

                if (!Lookup.ContainsKey(originLeft))
                {
                    reversePivotLeft = Lookup.ContainsKey(originLeft.Flip);
                    if (!reversePivotLeft)
                        Lookup[originLeft] = new Dictionary<int, int>();
                    else
                        originLeft = originLeft.Flip;
                }

                if (!Lookup.ContainsKey(originRight))
                {
                    reversePivotRight = Lookup.ContainsKey(originRight.Flip);
                    if (!reversePivotRight)
                        Lookup[originRight] = new Dictionary<int, int>();
                    else
                        originRight = originRight.Flip;
                }

                if (!Lookup.ContainsKey(leftRight))
                {
                    reverseLeftRight = Lookup.ContainsKey(leftRight.Flip);
                    if (!reverseLeftRight)
                        Lookup[leftRight] = new Dictionary<int, int>();
                    else
                        leftRight = leftRight.Flip;
                }

                Lookup[originLeft][reversePivotLeft ? divisions + 1 : 0] = triangle.Pivot;
                Lookup[originLeft][reversePivotLeft ? 0 : divisions + 1] = triangle.Left;

                Lookup[originRight][reversePivotRight ? divisions + 1 : 0] = triangle.Pivot;
                Lookup[originRight][reversePivotRight ? 0 : divisions + 1] = triangle.Right;

                Lookup[leftRight][reverseLeftRight ? divisions + 1 : 0] = triangle.Left;
                Lookup[leftRight][reverseLeftRight ? 0 : divisions + 1] = triangle.Right;
                //}
                var ySteps = divisions + 1;
                var tempVerticies = new int[ySteps + 1][];
                for (var y = 0; y <= ySteps; y++)
                {
                    var yScale = 1f / ySteps; //No need for a plus 1
                    ProceduralVertex
                        xL = slerp
                            ? ProceduralVertex.Slerp(pivot, left, y * yScale)
                            : ProceduralVertex.Lerp(pivot, left, y * yScale),
                        xR = slerp
                            ? ProceduralVertex.Slerp(pivot, right, y * yScale)
                            : ProceduralVertex.Lerp(pivot, right, y * yScale);

                    var xSteps = y;
                    tempVerticies[y] = new int[xSteps + 1];
                    for (var x = 0; x <= xSteps; x++)
                    {
                        var xScale = 1f / (xSteps > 0 ? xSteps : 1);
                        var mv = slerp
                            ? ProceduralVertex.Slerp(xL, xR, x * xScale)
                            : ProceduralVertex.Lerp(xL, xR, x * xScale);
                        var index = -1;
                        var revY = ySteps - y;
                        var revX = xSteps - x;


                        if (x == 0 && Lookup[originLeft].ContainsKey(reversePivotLeft ? revY : y))
                            index = Lookup[originLeft][reversePivotLeft ? revY : y];

                        if (x == xSteps && Lookup[originRight].ContainsKey(reversePivotRight ? revY : y))
                            index = Lookup[originRight][reversePivotRight ? revY : y];

                        if (y == ySteps && Lookup[leftRight].ContainsKey(reverseLeftRight ? revX : x))
                            index = Lookup[leftRight][reverseLeftRight ? revX : x];
                        if (index == -1)
                            index = builder.AddVertex(mv);
                        if (x == 0)
                            Lookup[originLeft][reversePivotLeft ? revY : y] = index;

                        if (x == xSteps)
                            Lookup[originRight][reversePivotRight ? revY : y] = index;

                        if (y == ySteps)
                            Lookup[leftRight][reverseLeftRight ? revX : x] = index;
                        tempVerticies[y][x] = index;
                    }
                }

                ySteps = divisions;
                for (var y = 0; y <= ySteps; y++)
                {
                    var yN = y + 1;
                    var xSteps = y;
                    for (var x = 0; x <= xSteps; x++)
                    {
                        var xN = x + 1;
                        builder.AddTriangle(tempVerticies[y][x], tempVerticies[yN][x], tempVerticies[yN][xN]);
                        if (xN <= y)
                            builder.AddTriangle(tempVerticies[y][x], tempVerticies[yN][xN], tempVerticies[y][xN]);
                    }
                }
            }
        }

        private struct SubdivisionEdgeHelper
        {
            public SubdivisionEdgeHelper(ProceduralVertex t, ProceduralVertex f) : this()
            {
                To = t;
                From = f;
            }

            public override bool Equals(object obj)
            {
                if (obj is SubdivisionEdgeHelper)
                {
                    var edge = (SubdivisionEdgeHelper) obj;
                    return
                        SoftEquals(To, edge.To) && SoftEquals(From, edge.From);
                }

                return false;
            }

            public override int GetHashCode()
            {
                return
                    To.GetHashCode() * 23 +
                    From.GetHashCode();
            }

            public ProceduralVertex To { get; private set; }
            public ProceduralVertex From { get; private set; }

            public SubdivisionEdgeHelper Flip
            {
                get { return new SubdivisionEdgeHelper(From, To); }
            }
        }
    }
}