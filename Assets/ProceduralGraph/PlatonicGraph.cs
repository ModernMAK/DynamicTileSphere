//using 
//using NodeGraphOld;
using PlanetGrapher;
using UnityEngine;

[System.Serializable]
public struct PlatonicGraphParameters
{
    public static PlatonicGraphParameters Default { get { return new PlatonicGraphParameters() { Seed = 0, TectonicPlateParameters = PlanetTectonicPlateParameters.Default, NeighborWeight = 10, CellScale = Vector2.one, EdgeScale = Vector2.one, CornerScale = Vector2.one, Divisions = 0, Slerp = false, Radius = 1f, Solidity = 1f }; } }

    public float ElevationScale;
    public int Seed;
    public Vector2 CellScale;
    public Vector2 EdgeScale;
    public Vector2 CornerScale;
    public int NeighborWeight;
    public int Divisions;
    public bool Slerp;
    public float Radius;
    [Range(0, 1)]
    public float Solidity;
    public PlanetTectonicPlateParameters TectonicPlateParameters;
}
public class PlatonicGraph : ProceduralMeshBehaviour
{


    protected override void Initialize()
    {
        myPlanetMeshBuilder = new PlanetGraphMeshBuilder();
    }

    public PlanetGraphMeshBuilder myPlanetMeshBuilder;
    public PlatonicGraphParameters Paramaters = PlatonicGraphParameters.Default;
    private PlanetGraph graph;
    public bool DrawPressure, DrawBorder, DrawMovement;
    protected void Setup()
    {
        graph = PlanetGraphMeshBuilder.CreatePlanetGraph(Paramaters.Divisions, Paramaters.Slerp);
        Random.InitState(Paramaters.Seed);
        PlanetGraphMethods.TectonicPlateGenerator(graph, Paramaters.TectonicPlateParameters);
        PlanetGraphMeshBuilder.SetTerrains(graph, Paramaters.NeighborWeight);
    }
    protected override void GenerateMesh()
    {
        PMB.Clear();
        Setup();
        myPlanetMeshBuilder.GenerateMesh(graph, PMB, Paramaters);
    }
    private void OnDrawGizmosSelected()
    {
        if (!(DrawPressure || DrawBorder || DrawMovement) || graph == null)
            return;
        float CenterScale = 1f / (10f * (1f + Paramaters.Divisions));
        float DrawSolidity = 0.999f * Paramaters.Solidity;
        float MovementScale = (1f / (4f * (1f + Paramaters.Divisions)));
        float PressureScale = MovementScale * 5f / 2f;
        if (DrawBorder)
            foreach (PlanetTectonicPlate plate in graph.PlateLookup)
            {
                Gizmos.color = Color.HSVToRGB(1f / graph.PlateLookup.Length * plate.Id, 1f, 1f);
                if (plate.IsOcean)
                    Gizmos.DrawSphere(plate.Center.Position, CenterScale);
                else
                    Gizmos.DrawCube(plate.Center.Position, CenterScale * Vector3.one);
            }
        if (DrawMovement)
            foreach (PlanetCell cell in graph.CellLookup)
            {
                Gizmos.color = Color.HSVToRGB(1f / graph.PlateLookup.Length * cell.TectonicPlateId, 1f, 1f);
                Gizmos.DrawRay(cell.Position, cell.TectonicPlateVector * MovementScale);
            }
        if (DrawBorder || DrawPressure)
            foreach (PlanetEdge edge in graph.EdgeLookup)
            {
                if (edge.IsPlateBoundary)
                {
                    Vector3 right = (edge.Origin - edge.Twin.Origin);
                    Vector3 up = ((edge.Origin + edge.Twin.Origin) / 2f);//Average of two ups form the correct up
                    Vector3 forward = ((edge.Cell.Position - edge.Twin.Cell.Position));

                    if (DrawBorder)
                    {
                        Gizmos.color = Color.HSVToRGB(1f / graph.PlateLookup.Length * edge.Cell.TectonicPlateId, 1f, 1f);
                        Gizmos.DrawLine(Vector3.Lerp(edge.Cell.Position, edge.Origin, DrawSolidity), Vector3.Lerp(edge.Cell.Position, edge.Twin.Origin, DrawSolidity));
                    }
                    if (DrawPressure)
                    {
                        Vector2 desired = edge.BoundaryPressure;
                        //Black To White for seperation
                        //Green To Red for Collision
                        //Blue for Shearing
                        Gizmos.color = (desired.y > 0f ? Color.Lerp(Color.green, Color.red, desired.magnitude) : (desired.y < 0f ? Color.Lerp(Color.black,Color.white,desired.magnitude) : Color.blue));
                        Gizmos.DrawRay(Vector3.Lerp(edge.Cell.Position, up, DrawSolidity), PressureScale * (forward.normalized * edge.BoundaryPressure.y + right.normalized * edge.BoundaryPressure.x));
                    }

                }

            }

    }
}
