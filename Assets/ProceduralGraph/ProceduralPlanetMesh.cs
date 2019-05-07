//using 
//using NodeGraphOld;
using Graphing.Planet;
using UnityEngine;

//[System.Serializable]
//public struct ProceduralPlanetMeshParameters
//{
//    public static ProceduralPlanetMeshParameters Default { get { return new ProceduralPlanetMeshParameters() { Seed = 0, /*TectonicPlateParameters = PlanetTectonicPlateParameters.Default,*/ NeighborWeight = 10, CellScale = Vector2.one, EdgeScale = Vector2.one, CornerScale = Vector2.one, Divisions = 0, Slerp = false, Radius = 1f, Solidity = 1f }; } }

//    public float ElevationScale;
//    public int Seed;
//    public Vector2 CellScale;
//    public Vector2 EdgeScale;
//    public Vector2 CornerScale;
//    public int NeighborWeight;
//    public int Divisions;
//    public bool Slerp;
//    public float Radius;
//    [Range(0, 1)]
//    public float Solidity;
//    //public PlanetTectonicPlateParameters TectonicPlateParameters;
//}
public class ProceduralPlanetMesh : ProceduralMeshBehaviour
{
    public bool RebuildGraph;
    protected override void Initialize()
    {
        myPlanetGraph = MeshGraphConverter.FetchEmptyPlanetGraph(GraphParameters.Subdivisions, GraphParameters.Shape, GraphParameters.Slerp, GraphParameters.Dual, GraphParameters.PartitionSize);
        PMB = myPlanetMeshBuilder = new PlanetGraphMeshBuilder(myPlanetGraph, MeshParameters);
        //PMB = myTempPlanetBuilder = new GraphMeshBuilder<PlanetGraph, PlanetPoly, PlanetEdge, PlanetNode>();
        //PMB = myTempPositionBuilder = new GraphMeshBuilder<PositionGraph, PositionPoly, PositionEdge, PositionNode>();
        //AsyncGizmoData = new List<KeyValuePair<PlanetEdge, Color>>();
        //MeshGraphConverter.AssertGraph();
    }

    private PlanetGraphMeshBuilder myPlanetMeshBuilder;
//    private PlateGraph myTectonicGraph;
    public PlanetGraphParameters PlanetParameters = PlanetGraphParameters.Default;
    public ProceduralPlanetMeshParameters GraphParameters = ProceduralPlanetMeshParameters.Default;
    public PlanetGraphMeshParameters MeshParameters = PlanetGraphMeshParameters.Default;
    private PlanetGraph myPlanetGraph;
    //private PositionGraph posGraph;
    //public bool DrawPressure, DrawBorder, DrawMovement;
    protected void Setup()
    {
        if (RebuildGraph)
        {
            myPlanetMeshBuilder.Clear();
            //RebuildGraph = false;
            myPlanetGraph = MeshGraphConverter.FetchEmptyPlanetGraph(GraphParameters.Subdivisions, GraphParameters.Shape, GraphParameters.Slerp, GraphParameters.Dual, GraphParameters.PartitionSize);
//            PlanetGraphPopulator.Populate(myPlanetGraph, PlanetParameters, out myTectonicGraph);
            PlanetGraphPopulator.Populate(myPlanetGraph, PlanetParameters);
            PMB = myPlanetMeshBuilder = new PlanetGraphMeshBuilder(myPlanetGraph, MeshParameters);
        }
        //graph = PlanetGraphMeshBuilder.CreatePlanetGraph(GraphParameters.Divisions, GraphParameters.Slerp);
        //Random.InitState(GraphParameters.Seed);
        //PlanetGraphMethods.TectonicPlateGenerator(graph, GraphParameters.TectonicPlateParameters);
        //PlanetGraphMeshBuilder.SetTerrains(graph, GraphParameters.NeighborWeight);
    }
    //private BiDictionaryOneToOne<PlateEdge, PlanetEdge> EdgeLookup;
    //private void BuildLookup()
    //{
    //    EdgeLookup = new BiDictionaryOneToOne<PlateEdge, PlanetEdge>();
    //    foreach (var plateEdge in myTectonicGraph.Edges)
    //    {
    //        foreach(var planetEdge in myPlanetGraph.Edges)
    //        {
    //            if (plateEdge.CenterLerp == planetEdge.CenterLerp)
    //            {
    //                EdgeLookup.Add(plateEdge, planetEdge);
    //                break;
    //            }
    //        }
    //    }
    //}
    protected override void GenerateMesh()
    {
        //PMB.Clear();
        Setup();
        //StopAllCoroutines();
        //posGraph = MeshGraphConverter.FetchGraph(GraphParameters.Subdivisions, GraphParameters.Shape, GraphParameters.Slerp, GraphParameters.Dual, true);
        //AsyncGizmoData.Clear();
        //StartCoroutine(GizmoRoutine());
        //PMB = myTempPositionBuilder;
        PMB.Clear();
        //myTempPositionBuilder.Generate(posGraph);// MeshGraphConverter.FetchGraph(GraphParameters.Subdivisions, GraphParameters.Shape, GraphParameters.Slerp, GraphParameters.Dual));
        myPlanetMeshBuilder.Generate();//.GenerateMesh(graph, PMB, GraphParameters);
    }
    
//    public bool drawFullLine;
//    private void OnDrawGizmos()
//    {
//        if (myTectonicGraph == null)
//            return;
//
//        int i = 0, l = myTectonicGraph.Polys.Count;
//        foreach (var poly in myTectonicGraph.Polys)
//        {
//            Gizmos.color = Color.HSVToRGB(((float)i) / l, 1f, 1f);
//            if (poly.Oceanic)
//                Gizmos.DrawSphere(Vector3.Scale(poly.CenterSlerp, MeshParameters.Scale(PlanetParameters.TectonicParameters.MaxHeight)), 0.01f);
//            else
//                Gizmos.DrawCube(Vector3.Scale(poly.CenterSlerp, MeshParameters.Scale(PlanetParameters.TectonicParameters.MaxHeight)), Vector3.one * 0.02f);
//
//            foreach (var edge in poly)
//            {
//                if(!drawFullLine)
//                    Gizmos.DrawLine(Vector3.Scale(edge.Node.Position, MeshParameters.Scale(PlanetParameters.TectonicParameters.MaxHeight)), Vector3.Scale(edge.CenterLerp, MeshParameters.Scale(PlanetParameters.TectonicParameters.MaxHeight)));
//                else
//                    Gizmos.DrawLine(Vector3.Scale(edge.Node.Position, MeshParameters.Scale(PlanetParameters.TectonicParameters.MaxHeight)), Vector3.Scale(edge.Next.Node.Position, MeshParameters.Scale(PlanetParameters.TectonicParameters.MaxHeight)));
//
//                Gizmos.DrawWireSphere(edge.DebugEdge.Poly.CenterLerp, 0.01f);
//
//            }
//            i++;
//        }
//    }
    //IEnumerator GizmoRoutine()
    //{
    //    int i = -1;
    //    const int H_D = 6;
    //    foreach (PositionPoly poly in posGraph.Polys)
    //    {
    //        i++;
    //        i %= H_D;
    //        //int j = -1;
    //        foreach (PositionEdge edge in poly)
    //        {
    //            //j++;
    //            //j %= 8;
    //            KeyValuePair<PositionEdge, Color> kvp = new KeyValuePair<PositionEdge, Color>(edge, Color.HSVToRGB(1f * i / H_D, 1f, 1f));
    //            AsyncGizmoData.Add(kvp);

    //            yield return null;
    //        }
    //    }
    //}

    //private List<KeyValuePair<PositionEdge, Color>> AsyncGizmoData;
    //private GraphMeshBuilder<PlanetGraph, PlanetPoly, PlanetEdge, PlanetNode> myTempPlanetBuilder;
    //private GraphMeshBuilder<PositionGraph, PositionPoly, PositionEdge, PositionNode> myTempPositionBuilder;

    //private void OnDrawGizmos()
    //{
    //    if (AsyncGizmoData == null)
    //        return;
    //    Gizmos.color = Color.black;
    //    foreach (PlanetNode node in graph.Nodes)
    //        Gizmos.DrawCube(node.Position, Vector3.one * 0.1f);


    //    foreach (var kvp in AsyncGizmoData)
    //    {
    //        Gizmos.color = kvp.Value;
    //        Gizmos.DrawSphere(kvp.Key.Node.Position, 0.01f);
    //        Gizmos.DrawLine(kvp.Key.Node.Position, kvp.Key.CenterLerp);
    //    }

    //}
    //private void OnDrawGizmosSelected()
    //{
    //    if (!(DrawPressure || DrawBorder || DrawMovement) || graph == null)
    //        return;
    //    float CenterScale = 1f / (10f * (1f + Paramaters.Divisions));
    //    float DrawSolidity = 0.999f * Paramaters.Solidity;
    //    float MovementScale = (1f / (4f * (1f + Paramaters.Divisions)));
    //    float PressureScale = MovementScale * 5f / 2f;
    //    if (DrawBorder)
    //        foreach (PlanetTectonicPlate plate in graph.PlateLookup)
    //        {
    //            Gizmos.color = Color.HSVToRGB(1f / graph.PlateLookup.Length * plate.Id, 1f, 1f);
    //            if (plate.IsOcean)
    //                Gizmos.DrawSphere(plate.Center.Position, CenterScale);
    //            else
    //                Gizmos.DrawCube(plate.Center.Position, CenterScale * Vector3.one);
    //        }
    //    if (DrawMovement)
    //        foreach (PlanetCell cell in graph.CellLookup)
    //        {
    //            Gizmos.color = Color.HSVToRGB(1f / graph.PlateLookup.Length * cell.TectonicPlateId, 1f, 1f);
    //            Gizmos.DrawRay(cell.Position, cell.TectonicPlateVector * MovementScale);
    //        }
    //    if (DrawBorder || DrawPressure)
    //        foreach (PlanetEdge edge in graph.EdgeLookup)
    //        {
    //            if (edge.IsPlateBoundary)
    //            {
    //                Vector3 right = (edge.Origin - edge.Twin.Origin);
    //                Vector3 up = ((edge.Origin + edge.Twin.Origin) / 2f);//Average of two ups form the correct up
    //                Vector3 forward = ((edge.Cell.Position - edge.Twin.Cell.Position));

    //                if (DrawBorder)
    //                {
    //                    Gizmos.color = Color.HSVToRGB(1f / graph.PlateLookup.Length * edge.Cell.TectonicPlateId, 1f, 1f);
    //                    Gizmos.DrawLine(Vector3.Lerp(edge.Cell.Position, edge.Origin, DrawSolidity), Vector3.Lerp(edge.Cell.Position, edge.Twin.Origin, DrawSolidity));
    //                }
    //                if (DrawPressure)
    //                {
    //                    Vector2 desired = edge.BoundaryPressure;
    //                    //Black To White for seperation
    //                    //Green To Red for Collision
    //                    //Blue for Shearing
    //                    Gizmos.color = (desired.y > 0f ? Color.Lerp(Color.green, Color.red, desired.magnitude) : (desired.y < 0f ? Color.Lerp(Color.black, Color.white, desired.magnitude) : Color.blue));
    //                    Gizmos.DrawRay(Vector3.Lerp(edge.Cell.Position, up, DrawSolidity), PressureScale * (forward.normalized * edge.BoundaryPressure.y + right.normalized * edge.BoundaryPressure.x));
    //                }

    //            }

    //        }

    //}
}
