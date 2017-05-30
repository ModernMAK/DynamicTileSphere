//using Graphing.Planet;
//using System.Collections.Generic;
//using UnityEngine;
//public class PlanetGraphMeshBuilder
//{
    

//    //public static PlanetGraph CreatePlanetGraph(/*RadiusType radius = RadiusType.Normalized,*//* float scale = 1, */int divisions = 0, bool useSlerp = true, int chunkSize = 32)
//    //{

//    //    //ProceduralMeshBuilder pmb = new ProceduralMeshBuilder();
//    //    ////myEdgeList.Clear();
//    //    //ProceduralPlatonicSolidGenerator.AddToMeshBuilder(pmb, ShapeType.Icosahedron, radius, scale);
//    //    //ProceduralMeshUtility.Subdivide(pmb, divisions, useSlerp);

//    //    return CreatePlanetGraph(divisions,useSlerp, chunkSize);
//    //}
//    public PlanetGraphMeshBuilder()
//    {
//        //myVoronoiBuilder = new ProceduralMeshBuilder();
//        myEdgeList = new HashSet<Polygone>(new EdgeDataComparar());
//        myCornerList = new HashSet<PlanetPolygonCorner>(new PolygonCornerComparar());
//        //myBuilder = new ProceduralMeshBuilder();
//    }
//    //private ProceduralMeshBuilder /*myBuilder,*/ myVoronoiBuilder;
//    private HashSet<EdgeData> myEdgeList;
//    private HashSet<PolygonCorner> myCornerList;
//    //public RadiusType Radius;
//    public float Scale;
//    public int Divisions;
//    public bool UseSlerp;
//    //protected void Clear()
//    //{
//    //    myBuilder.Clear();
//    //    myVoronoiBuilder.Clear();
//    //}
//    //protected override MeshBuilder Builder
//    //{
//    //    get
//    //    {
//    //        return myBuilder;
//    //    }
//    //}

//    //public static CellGraph CreateCell(ProceduralMeshBuilder pmb)
//    //{
//    //    return CellGraphBuilder.CreateGraph(VD_GraphBuilder.CreateGraph(pmb.Verticies.ToArray(), pmb.Triangles.ToArray()));
//    //}
//    //public static CellGraph CreateCell(int divisions, bool slerp)
//    //{
//    //    return CellGraphGenerator.Generate(divisions, slerp);
//    //}
//    //public static PlanetGraph CreatePlanetGraph(ProceduralMeshBuilder pmb, int chunkSize = 32)
//    //{
//    //    return PlanetGraphBuilder.CreateGraph(CreateCell(pmb), chunkSize);
//    //}
//    public static PlanetGraph CreatePlanetGraph(int divisions, bool slerp = true, int chunkSize = 32)
//    {
//        return PlanetGraphBuilder.CreateGraph(CellGraphGenerator.Generate(divisions, slerp), chunkSize);
//    }
//    public static void SetTerrains(PlanetGraph pg, int weight = 10)
//    {
//        foreach (PlanetCell cell in pg.Cells)
//            cell.TerrainType = -1;
//        int range = 5;
//        int[] basic = new int[range];
//        for (int i = 0; i < range; i++)
//            basic[i] = i;
//        List<int> pool = new List<int>(basic);
//        foreach (PlanetCell cell in pg.Cells)
//        {
//            PlanetEdge edge = cell.Edge, startEdge = edge;
//            pool.Clear();
//            pool.AddRange(basic);
//            do
//            {
//                if (edge.Twin.Cell.TerrainType != -1)
//                    for (int i = 0; i < weight; i++)
//                        pool.Add(edge.Twin.Cell.TerrainType);
//                edge = edge.Next;
//            } while (edge != startEdge);
//            cell.TerrainType = pool[Random.Range(0, pool.Count)];
//        }
//    }
//    public static void GenerateCellMesh(ProceduralMeshBuilder pmb, PlanetCell cell, PlatonicGraphParameters parameters)
//    {
//        List<ProceduralVertex> verticies = new List<ProceduralVertex>(6);
//        ProceduralVertex midPoint = new ProceduralVertex();
//        PlanetEdge edge = cell.Edge;
//        PlanetEdge startEdge = edge;
//        //Vector3 midPos = Vector3.zero;
//        ProceduralVertex v = new ProceduralVertex().SetColor(Color.red);
//        do
//        {
//            //midPos += pos;//.SetNormal(pos.normalized).SetTangent(tan).SetColor(Color.red);
//            verticies.Add(v.SetPosition(parameters.Radius * Vector3.Lerp(cell.Position, edge.Origin, parameters.Solidity)));
//            edge = edge.Next;
//        } while (edge != startEdge);
//        //midPos /= verticies.Count;
//        Vector3 norm = cell.Position.normalized;
//        //Vector2 midUv = UvFromSphereScaled(cell.Position, uvScale);
//        Vector3 tan = Quaternion.FromToRotation(Vector3.up, norm) * Vector3.right;
//        //verticies.Add(v.SetPosition(scale * cell.Position));
//        midPoint = midPoint.SetColor(Color.red).SetPosition(parameters.Radius * cell.Position).SetNormal(norm).SetTangent(tan).SetUv3(Vector3.one * cell.TerrainType);

//        int[] indicies = new int[verticies.Count + 1];
//        Vector2 center = Vector2.zero;
//        for (int i = 0; i < verticies.Count; i++)
//        {
//            //Vector2 uv = UvFromSphereScaled(verticies[i].Position, uvScale);
//            Vector2 uv = UvFromShapeScaled(i, verticies.Count, parameters.CellScale);
//            center += uv;
//            //const float Leniancy = 0.5f;
//            //float deltaX = midUv.x - uv.x;
//            ////IsLeft
//            //if (deltaX < 0f && deltaX > -Leniancy)
//            //{
//            //    //int dir = 1;//Shift Left (If Too Right)
//            //    //if (midUv.x - uv.x > Leniancy)//Too Left
//            //    //    dir *= -1;//Shift Right
//            //    //uv = uv + dir * Vector2.right;
//            //    uv.x++;
//            //}
//            ////IsRight
//            //else if (deltaX > 0f && deltaX < Leniancy)
//            //{
//            //    uv.x--;
//            //    //int dir = 1;//Shift Left (If Too Right)
//            //    //if (midUv.x - uv.x > Leniancy)//Too Left
//            //    //    dir *= -1;//Shift Right
//            //    //uv = uv + dir * Vector2.right;
//            //}

//            //Vector2 gridUv = Vector2.zero;//= GridUvFromShape((cell.IsHex ? 2 : 3), i);
//            //uv = new Vector4(uv.x * uvScale.x, uv.y * uvScale.y, gridUv.x, gridUv.y);
//            ////indicies[i] = pmb.AddVertex(
//            verticies[i] = verticies[i].SetNormal(norm).SetTangent(tan).SetUv(uv)/*.SetUv2(gridUv)*/.SetUv3(Vector3.one * cell.TerrainType);//);
//            indicies[i] = pmb.AddVertex(verticies[i]);
//            //verticies[i] = verticies[i].SetNormal(norm).SetTangent(tan).SetUv(uv)/*.SetUv2(gridUv)*/.SetUv3(Vector3.one * cell.TerrainType));
//        }
//        center /= verticies.Count;
//        int l = indicies.Length - 1;
//        midPoint = midPoint.SetUv(center);
//        indicies[l] = pmb.AddVertex(midPoint);
//        for (int i = 0; i < l; i++)
//        {
//            pmb.AddTriangle(indicies[l], indicies[(i + 1) % l], indicies[i]);
//            //pmb.AddTriangle(verticies[l], verticies[(i + 1) % l], verticies[i]);
//        }
//    }
//    private static float Map(float v, float vMin, float vMax, float oMin, float oMax)
//    {
//        float t = ((v - vMin) / (vMax - vMin));
//        return t * (oMax - oMin) + oMin;
//    }
//    public static Vector2 UvFromShape(float i, float s, int o = 0)
//    {
//        float angle = Mathf.PI * 2f * ((i + o) / s);
//        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
//    }
//    public static Vector2 UvFromShapeScaled(float i, float s, Vector2 scale, int o = 0)
//    {
//        Vector2 uv = UvFromShape(i, s, o);
//        return new Vector2(uv.x * scale.x, uv.y * scale.y);
//    }
//    public static Vector2 UvFromSphere(Vector3 v)
//    {
//        float theta = Mathf.Acos(v.z / v.magnitude);
//        float rho = Mathf.Atan2(v.y, v.x);
//        //Debug.Log(string.Format("T:{0}\nR:{1}", theta, rho));
//        float x = Map(theta, 0f, Mathf.PI, 1f, 0f);
//        float y = Map(rho, -Mathf.PI, Mathf.PI, 0f, 1f);
//        if (x > 1f || x < 0f)
//            Debug.Log(string.Format("T:{0}\nR:{1}", theta, rho));
//        else if (y > 1f || y < 0f)
//            Debug.Log(string.Format("T:{0}\nR:{1}", theta, rho));
//        //Debug.Log(string.Format("X:{0}\nY:{1}", x, y));
//        return new Vector2(x, y);//, rho / Mathf.PI - 1f);
//    }
//    public static Vector2 UvFromSphereScaled(Vector3 v, Vector2 scale)
//    {
//        Vector2 uv = UvFromSphere(v);
//        if (uv.sqrMagnitude > 2f)
//            throw new System.Exception();
//        return new Vector2(uv.x * scale.x, uv.y * scale.y);
//    }
//    //public static Vector2 GridUvFromShape(int cell, int index)
//    //{
//    //    int l = 1;
//    //    switch (cell)
//    //    {
//    //        case 0:
//    //            l = 4;
//    //            break;
//    //        case 1:
//    //            l = 3;
//    //            break;
//    //        default:
//    //            l = 1;
//    //            break;
//    //    }
//    //    float angle = Mathf.PI / (2 * l) * 2f * (2 * index );
//    //    Vector2 uv = new Vector2(Mathf.Cos(angle) / 2f, Mathf.Sin(angle) / 2f);
//    //    switch (cell)
//    //    {
//    //        case 0:
//    //            break;
//    //        case 1:
//    //            uv += new Vector2(0.5f, 0f);
//    //            break;
//    //        case 2:
//    //            uv += new Vector2(0f, 0.5f);
//    //            break;
//    //        case 3:
//    //            uv += new Vector2(0.5f, 0.5f);
//    //            break;
//    //    }
//    //    return uv;
//    //}
//    public static void GenerateEdgeMesh(ProceduralMeshBuilder pmb, PlanetEdge edge, PlatonicGraphParameters parameters)
//    {
//        //if (solidity == 1f)
//        //    return;//Nothing to do if edge is solid.

//        ProceduralVertex[] verticies = new ProceduralVertex[4];
//        Vector3
//            edgeMid = edge.Cell.Position,
//            twinMid = edge.Twin.Cell.Position,
//            norm = (edge.Origin + edge.Twin.Origin).normalized,
//            tan = Quaternion.FromToRotation(Vector3.up, norm) * Vector3.right;

//        Vector3
//            terrainUv = new Vector3(edge.Cell.TerrainType, edge.Twin.Cell.TerrainType, edge.Cell.TerrainType);

//        verticies[0] = new ProceduralVertex(parameters.Radius * Vector3.Lerp(edgeMid, edge.Origin, parameters.Solidity)).SetColor(Color.red).SetUv3(terrainUv);
//        verticies[1] = new ProceduralVertex(parameters.Radius * Vector3.Lerp(edgeMid, edge.Twin.Origin, parameters.Solidity)).SetColor(Color.red).SetUv3(terrainUv);
//        verticies[2] = new ProceduralVertex(parameters.Radius * Vector3.Lerp(twinMid, edge.Twin.Origin, parameters.Solidity)).SetColor(Color.green).SetUv3(terrainUv);
//        verticies[3] = new ProceduralVertex(parameters.Radius * Vector3.Lerp(twinMid, edge.Origin, parameters.Solidity)).SetColor(Color.green).SetUv3(terrainUv);

//        int[] indicies = new int[4];

//        for (int i = 0; i < 4; i++)
//        {
//            //Vector2 uv = UvFromSphereScaled(verticies[i].Position, uvScale);
//            Vector2 uv = UvFromShapeScaled(i * 2, verticies.Length * 2, parameters.EdgeScale, 1);
//            //if (i == verticies.Length - 1)
//            //    uv = Vector2.one / 2f;
//            //Vector2 gridUv = GridUvFromShape(0, i);
//            //uv = new Vector4(uv.x * uvScale.x, uv.y * uvScale.y, gridUv.x, gridUv.y);
//            //indicies[i] = pmb.AddVertex(
//            verticies[i] = verticies[i].SetNormal(norm).SetTangent(tan).SetUv(uv);///*.SetUv2(gridUv)*/);
//            indicies[i] = pmb.AddVertex(verticies[i]);
//        }

//        //pmb.AddTriangle(verticies[0], verticies[1], verticies[2]);
//        //pmb.AddTriangle(verticies[2], verticies[3], verticies[0]);
//        pmb.AddTriangle(indicies[0], indicies[1], indicies[2]);
//        pmb.AddTriangle(indicies[2], indicies[3], indicies[0]);
//    }
//    public static void GenerateCornerMesh(ProceduralMeshBuilder pmb, PlanetEdge edge, PlatonicGraphParameters parameters)
//    {
//        //if (solidity == 1f)
//        //    return;//Nothing to do if edge is solid.

//        ProceduralVertex[] verticies = new ProceduralVertex[4];
//        Vector3
//            pivotMid = edge.Cell.Position,
//            leftMid = edge.Twin.Next.Cell.Position,
//            rightMid = edge.Twin.Next.Twin.Next.Cell.Position,
//            norm = (edge.Origin + edge.Twin.Next.Origin + edge.Twin.Next.Twin.Next.Origin).normalized,
//            tan = Quaternion.FromToRotation(Vector3.up, norm) * Vector3.right;

//        Vector3
//            terrainUv = new Vector3(edge.Cell.TerrainType, edge.Twin.Next.Cell.TerrainType, edge.Twin.Next.Twin.Next.Cell.TerrainType);

//        verticies[0] = new ProceduralVertex(parameters.Radius * Vector3.Lerp(pivotMid, edge.Origin, parameters.Solidity)).SetColor(Color.red).SetUv3(terrainUv);
//        verticies[1] = new ProceduralVertex(parameters.Radius * Vector3.Lerp(leftMid, edge.Twin.Next.Origin, parameters.Solidity)).SetColor(Color.green).SetUv3(terrainUv);
//        verticies[2] = new ProceduralVertex(parameters.Radius * Vector3.Lerp(rightMid, edge.Twin.Next.Twin.Next.Origin, parameters.Solidity)).SetColor(Color.blue).SetUv3(terrainUv);

//        int[] indicies = new int[3];

//        for (int i = 0; i < 3; i++)
//        {
//            //Vector2 uv = UvFromSphereScaled(verticies[i].Position, uvScale);
//            Vector2 uv = UvFromShapeScaled(i, verticies.Length, parameters.CornerScale);
//            //Vector2 gridUv = GridUvFromShape(1, i);
//            //uv = new Vector4(uv.x * uvScale.x, uv.y * uvScale.y, gridUv.x, gridUv.y);
//            verticies[i] = verticies[i].SetNormal(norm).SetTangent(tan).SetUv(uv)/*.SetUv2(gridUv)*/;
//            indicies[i] = pmb.AddVertex(verticies[i]);
//        }

//        pmb.AddTriangle(indicies[0], indicies[1], indicies[2]);
//        //pmb.AddTriangle(indicies[2], indicies[3], indicies[0]);
//    }
//    public void GenerateMesh(PlanetGraph pGraph, ProceduralMeshBuilder pmb, PlatonicGraphParameters parameters)
//    {
//        myEdgeList.Clear();
//        myCornerList.Clear();        
//        foreach (PlanetCell cell in pGraph.Cells)
//            GenerateCellMesh(pmb, cell, parameters);
//        if (parameters.Solidity != 1f)
//        {
//            foreach (PlanetEdge edge in pGraph.EdgeLookup)
//            {
//                EdgeData e = new EdgeData(edge);
//                PolygonCorner c = new PolygonCorner(edge);
//                if (!myEdgeList.Contains(e))
//                {
//                    GenerateEdgeMesh(pmb, edge, parameters);
//                    myEdgeList.Add(e);
//                }
//                if (!myCornerList.Contains(c))
//                {
//                    GenerateCornerMesh(pmb, edge, parameters);
//                    myCornerList.Add(c);
//                }
//            }
//        }
//    }
//}