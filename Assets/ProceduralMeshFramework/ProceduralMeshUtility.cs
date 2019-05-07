using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMeshUtility
{

    public static bool SoftEquals(Vector3 a, Vector3 b, float precision)
    {
        Vector3 delta = a - b;

        return delta.sqrMagnitude <= precision * precision;
    }
    private static bool SoftEquals(ProceduralVertex a, ProceduralVertex b, float precision = 0f)
    {
        if (precision < 0f)
            precision = 0f;
        //{
        //    return
        //        a.Position
        //}
        //throw new System.NotImplementedException("There is no SoftEquals for a negative or zero precision");
        //return a.SoftEquals(b);

        return SoftEquals(a.Position, b.Position, precision);
    }

    public static void Spherize(ProceduralMeshBuilder PMB, float radius = 1f)
    {
        ProceduralVertex pVertex;
        float sqrRad = radius * radius;
        for (int i = 0; i < PMB.Verticies.Count; i++)
        {
            pVertex = PMB.Verticies[i];
            if (pVertex.Position.sqrMagnitude != sqrRad)
            {
                pVertex.Position = (pVertex.Position.normalized * radius);
                PMB.Verticies[i] = pVertex;
            }

        }
    }
    public static void ModifyRadius(ProceduralMeshBuilder PMB, float scale)
    {
        ProceduralVertex pVertex;
        for (int i = 0; i < PMB.Verticies.Count; i++)
        {
            pVertex = PMB.Verticies[i];
            //if (pVertex.Position.sqrMagnitude != sqrRad)
            //{
            pVertex.Position = (pVertex.Position.normalized * scale);
            PMB.Verticies[i] = pVertex;
            //}

        }
    }
    public static void SpherizeUV(ProceduralMeshBuilder PMB)
    {
        ProceduralVertex pVertex;
        Vector4 sphereUv = Vector4.zero;
        Vector3 tempPos;
        sphereUv.w = 1;
        for (int i = 0; i < PMB.Verticies.Count; i++)
        {
            pVertex = PMB.Verticies[i];
            tempPos = pVertex.Position.normalized;

            sphereUv.x = (1f + (Mathf.Atan2(tempPos.z, tempPos.x) / Mathf.PI)) / 2f;
            sphereUv.y = 1f - Mathf.Acos(tempPos.y) / Mathf.PI;
            //Debug.Log(sphereUv);
            pVertex.Uv = sphereUv;

            PMB.Verticies[i] = pVertex;
        }
    }
    public static void Harden(ProceduralMeshBuilder PMB)
    {
        ProceduralVertex[] vertices = PMB.Verticies.ToArray();
        ProceduralTriangle[] triangles = PMB.Triangles.ToArray();
        PMB.Clear();
        foreach (ProceduralTriangle triangle in triangles)
        {
            PMB.AddTriangle(vertices[triangle.Pivot], vertices[triangle.Left], vertices[triangle.Right]);
        }
    }

    private static int SoftLookup(ProceduralVertex vert, Dictionary<ProceduralVertex, int> vertLookup, float precision = 0.0001f)
    {
        if (vertLookup.ContainsKey(vert))
            return vertLookup[vert];

        foreach (KeyValuePair<ProceduralVertex, int> kvp in vertLookup)
        {
            if (SoftEquals(kvp.Key, vert, precision))
                return kvp.Value;
        }

        return -1;
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
                SubdivisionEdgeHelper edge = (SubdivisionEdgeHelper)obj;
                return
                    (SoftEquals(To, edge.To) && SoftEquals(From, edge.From));
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
        public SubdivisionEdgeHelper Flip { get { return new SubdivisionEdgeHelper(From, To); } }
    }
    public static void Subdivide(ProceduralMeshBuilder builder, int divisions, bool slerp = false, float precision = 0.001f, bool hard = false)
    {
        if (hard)
            SubdivideHard(builder, divisions, slerp, precision);
        else
            SubdivideSoft(builder, divisions, slerp, precision);
    }
    public static void SubdivideHard(ProceduralMeshBuilder builder, int divisions, bool slerp = false, float precision = 0.001f)
    {
        if (divisions <= 0)
            return;

        ProceduralTriangle[] triangles = builder.Triangles.ToArray();
        ProceduralVertex[] verts = builder.Verticies.ToArray();
        builder.Triangles.Clear();

        //Vector3[] vertsPositions = new Vector3[verts.Length];
        //for (int i = 0; i < verts.Length; i++)
        //    vertsPositions[i] = verts[i].Position;

        //Okay... How do we do?
        //Well, Original Verticies are garunteed to be the same, so sort via those guidelines...

        //Dictionary<SubdivisionEdgeHelper, Dictionary<int, int>> Lookup = new Dictionary<SubdivisionEdgeHelper, Dictionary<int, int>>();


        foreach (ProceduralTriangle triangle in triangles)
        {
            ProceduralVertex
                pivot = verts[triangle.Pivot],
                left = verts[triangle.Left],
                right = verts[triangle.Right];

            int ySteps = divisions + 1;
            ProceduralVertex[][] tempVerticies = new ProceduralVertex[ySteps + 1][];
            for (int y = 0; y <= ySteps; y++)
            {
                float yScale = 1f / ySteps;//No need for a plus 1
                ProceduralVertex
                    xL = slerp ? ProceduralVertex.Slerp(pivot, left, y * yScale) : ProceduralVertex.Lerp(pivot, left, y * yScale),// SlerpOrLerp(pivot, left, y * yScale, slerp),
                    xR = slerp ? ProceduralVertex.Slerp(pivot, right, y * yScale) : ProceduralVertex.Lerp(pivot, right, y * yScale);//SlerpOrLerp(pivot, right, y * yScale, slerp);

                int xSteps = y;
                tempVerticies[y] = new ProceduralVertex[xSteps + 1];
                for (int x = 0; x <= xSteps; x++)
                {
                    float xScale = 1f / ((xSteps > 0) ? xSteps : 1);
                    ProceduralVertex mv = slerp ? ProceduralVertex.Slerp(xL, xR, x * xScale) : ProceduralVertex.Lerp(xL, xR, x * xScale); //SlerpOrLerp(xL, xR, x * xScale, slerp);
                                                                                                                                          //int index = -1;
                                                                                                                                          //int revY = ySteps - y;
                                                                                                                                          //int revX = xSteps - x;

                    //if (index == -1)
                    //    index = builder.AddVertex(mv);
                    tempVerticies[y][x] = mv;
                }
            }
            ySteps = divisions;
            for (int y = 0; y <= ySteps; y++)
            {
                int yN = y + 1;
                int xSteps = y;
                for (int x = 0; x <= xSteps; x++)
                {
                    int xN = x + 1;
                    builder.AddTriangle(tempVerticies[y][x], tempVerticies[yN][x], tempVerticies[yN][xN]);
                    if (xN <= y)
                        builder.AddTriangle(tempVerticies[y][x], tempVerticies[yN][xN], tempVerticies[y][xN]);
                }
            }
        }
    }
    public static void SubdivideSoft(ProceduralMeshBuilder builder, int divisions, bool slerp = false, float precision = 0.001f)
    {
        if (divisions <= 0)
            return;

        ProceduralTriangle[] triangles = builder.Triangles.ToArray();
        ProceduralVertex[] verts = builder.Verticies.ToArray();
        builder.Triangles.Clear();

        //Vector3[] vertsPositions = new Vector3[verts.Length];
        //for (int i = 0; i < verts.Length; i++)
        //    vertsPositions[i] = verts[i].Position;

        //Okay... How do we do?
        //Well, Original Verticies are garunteed to be the same, so sort via those guidelines...

        Dictionary<SubdivisionEdgeHelper, Dictionary<int, int>> Lookup = new Dictionary<SubdivisionEdgeHelper, Dictionary<int, int>>();


        foreach (ProceduralTriangle triangle in triangles)
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
            //if (!hard)
            //{
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

            Lookup[originLeft][reversePivotLeft ? (divisions + 1) : 0] = triangle.Pivot;
            Lookup[originLeft][reversePivotLeft ? 0 : (divisions + 1)] = triangle.Left;

            Lookup[originRight][reversePivotRight ? (divisions + 1) : 0] = triangle.Pivot;
            Lookup[originRight][reversePivotRight ? 0 : (divisions + 1)] = triangle.Right;

            Lookup[leftRight][reverseLeftRight ? (divisions + 1) : 0] = triangle.Left;
            Lookup[leftRight][reverseLeftRight ? 0 : (divisions + 1)] = triangle.Right;
            //}
            int ySteps = divisions + 1;
            int[][] tempVerticies = new int[ySteps + 1][];
            for (int y = 0; y <= ySteps; y++)
            {
                float yScale = 1f / ySteps;//No need for a plus 1
                ProceduralVertex
                    xL = slerp ? ProceduralVertex.Slerp(pivot, left, y * yScale) : ProceduralVertex.Lerp(pivot, left, y * yScale),// SlerpOrLerp(pivot, left, y * yScale, slerp),
                    xR = slerp ? ProceduralVertex.Slerp(pivot, right, y * yScale) : ProceduralVertex.Lerp(pivot, right, y * yScale);//SlerpOrLerp(pivot, right, y * yScale, slerp);

                int xSteps = y;
                tempVerticies[y] = new int[xSteps + 1];
                for (int x = 0; x <= xSteps; x++)
                {
                    float xScale = 1f / ((xSteps > 0) ? xSteps : 1);
                    ProceduralVertex mv = slerp ? ProceduralVertex.Slerp(xL, xR, x * xScale) : ProceduralVertex.Lerp(xL, xR, x * xScale); //SlerpOrLerp(xL, xR, x * xScale, slerp);
                    int index = -1;
                    int revY = ySteps - y;
                    int revX = xSteps - x;

                    //if (!hard)
                    //{
                    if (x == 0 && Lookup[originLeft].ContainsKey(reversePivotLeft ? revY : y))
                        index = Lookup[originLeft][reversePivotLeft ? revY : y];

                    if (x == xSteps && Lookup[originRight].ContainsKey(reversePivotRight ? revY : y))
                        index = Lookup[originRight][reversePivotRight ? revY : y];

                    if (y == ySteps && Lookup[leftRight].ContainsKey(reverseLeftRight ? revX : x))
                        index = Lookup[leftRight][reverseLeftRight ? revX : x];
                    //}
                    if (index == -1)
                        index = builder.AddVertex(mv);
                    //if (!hard)
                    //{
                    if (x == 0)
                        Lookup[originLeft][reversePivotLeft ? revY : y] = index;

                    if (x == xSteps)
                        Lookup[originRight][reversePivotRight ? revY : y] = index;

                    if (y == ySteps)
                        Lookup[leftRight][reverseLeftRight ? revX : x] = index;
                    //}
                    tempVerticies[y][x] = index;
                }
            }
            ySteps = divisions;
            for (int y = 0; y <= ySteps; y++)
            {
                int yN = y + 1;
                int xSteps = y;
                for (int x = 0; x <= xSteps; x++)
                {
                    int xN = x + 1;
                    builder.AddTriangle(tempVerticies[y][x], tempVerticies[yN][x], tempVerticies[yN][xN]);
                    if (xN <= y)
                        builder.AddTriangle(tempVerticies[y][x], tempVerticies[yN][xN], tempVerticies[y][xN]);


                }
            }
        }
    }
    public static IEnumerator AsyncSubdivide(ProceduralMeshBuilder builder, int divisions, bool slerp = false, float precision = 0.001f, bool hard = false)
    {
        if (divisions <= 0)
            yield break;

        ProceduralTriangle[] triangles = builder.Triangles.ToArray();
        ProceduralVertex[] verts = builder.Verticies.ToArray();
        builder.Triangles.Clear();

        Vector3[] vertsPositions = new Vector3[verts.Length];
        for (int i = 0; i < verts.Length; i++)
            vertsPositions[i] = verts[i].Position;

        //Okay... How do we do?
        //Well, Original Verticies are garunteed to be the same, so sort via those guidelines...

        Dictionary<SubdivisionEdgeHelper, Dictionary<int, int>> Lookup = new Dictionary<SubdivisionEdgeHelper, Dictionary<int, int>>();


        foreach (ProceduralTriangle triangle in triangles)
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
            if (!hard)
            {
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

                Lookup[originLeft][reversePivotLeft ? (divisions + 1) : 0] = triangle.Pivot;
                Lookup[originLeft][reversePivotLeft ? 0 : (divisions + 1)] = triangle.Left;

                Lookup[originRight][reversePivotRight ? (divisions + 1) : 0] = triangle.Pivot;
                Lookup[originRight][reversePivotRight ? 0 : (divisions + 1)] = triangle.Right;

                Lookup[leftRight][reverseLeftRight ? (divisions + 1) : 0] = triangle.Left;
                Lookup[leftRight][reverseLeftRight ? 0 : (divisions + 1)] = triangle.Right;
            }
            int ySteps = divisions + 1;
            int[][] tempVerticies = new int[ySteps + 1][];
            for (int y = 0; y <= ySteps; y++)
            {
                float yScale = 1f / ySteps;//No need for a plus 1
                ProceduralVertex
                    xL = slerp ? ProceduralVertex.Slerp(pivot, left, y * yScale) : ProceduralVertex.Lerp(pivot, left, y * yScale),// SlerpOrLerp(pivot, left, y * yScale, slerp),
                    xR = slerp ? ProceduralVertex.Slerp(pivot, right, y * yScale) : ProceduralVertex.Lerp(pivot, right, y * yScale);//SlerpOrLerp(pivot, right, y * yScale, slerp);

                int xSteps = y;
                tempVerticies[y] = new int[xSteps + 1];
                for (int x = 0; x <= xSteps; x++)
                {
                    float xScale = 1f / ((xSteps > 0) ? xSteps : 1);
                    ProceduralVertex mv = slerp ? ProceduralVertex.Slerp(xL, xR, x * xScale) : ProceduralVertex.Lerp(xL, xR, x * xScale); //SlerpOrLerp(xL, xR, x * xScale, slerp);
                    int index = -1;
                    int revY = ySteps - y;
                    int revX = xSteps - x;

                    if (!hard)
                    {
                        if (x == 0 && Lookup[originLeft].ContainsKey(reversePivotLeft ? revY : y))
                            index = Lookup[originLeft][reversePivotLeft ? revY : y];

                        if (x == xSteps && Lookup[originRight].ContainsKey(reversePivotRight ? revY : y))
                            index = Lookup[originRight][reversePivotRight ? revY : y];

                        if (y == ySteps && Lookup[leftRight].ContainsKey(reverseLeftRight ? revX : x))
                            index = Lookup[leftRight][reverseLeftRight ? revX : x];
                    }
                    if (index == -1)
                        index = builder.AddVertex(mv);
                    if (!hard)
                    {
                        if (x == 0)
                            Lookup[originLeft][reversePivotLeft ? revY : y] = index;

                        if (x == xSteps)
                            Lookup[originRight][reversePivotRight ? revY : y] = index;

                        if (y == ySteps)
                            Lookup[leftRight][reverseLeftRight ? revX : x] = index;
                    }
                    tempVerticies[y][x] = index;
                }
            }
            //yield return null;//Wait after every triangle is Calculated (Before continuing)
            ySteps = divisions;
            for (int y = 0; y <= ySteps; y++)
            {
                int yN = y + 1;
                int xSteps = y;
                for (int x = 0; x <= xSteps; x++)
                {
                    int xN = x + 1;
                    builder.AddTriangle(tempVerticies[y][x], tempVerticies[yN][x], tempVerticies[yN][xN]);
                    if (xN <= y)
                        builder.AddTriangle(tempVerticies[y][x], tempVerticies[yN][xN], tempVerticies[y][xN]);


                }
                //yield return null;//Wait after every triangle is Added (Before continuing)
            }
        }
        yield return null;//Yield when done

    }

}

