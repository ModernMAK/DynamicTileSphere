using Graphing.Planet;
using GraphToMesh;
using ProceduralMeshFramework;

namespace ProceduralGraph
{
    public class ProceduralPlatonicMesh : ProceduralMeshBehaviour
    {
        public ProceduralPlanetMeshParameters GraphParameters = ProceduralPlanetMeshParameters.Default;
        public PlanetGraphMeshParameters MeshParameters = PlanetGraphMeshParameters.Default;
        //private PlanetGraph myPlanetGraph;

        //private PlanetGraphMeshBuilder myPlanetMeshBuilder;


        public PlanetGraphParameters PlanetParameters = PlanetGraphParameters.Default;

        protected override void Initialize()
        {
            Regenerate = true;
            //PMB = myPlanetMeshBuilder = new PlanetGraphMeshBuilder(myPlanetGraph, MeshParameters);
        }


        protected void Setup()
        {
            //myPlanetMeshBuilder.Clear();

            //myPlanetGraph = MeshGraphConverter.FetchEmptyPlanetGraph(GraphParameters.Subdivisions,
            //    GraphParameters.Shape, GraphParameters.Slerp, GraphParameters.PartitionSize);

            //PlanetGraphPopulator.Populate(myPlanetGraph, PlanetParameters);
            //PMB = myPlanetMeshBuilder = new PlanetGraphMeshBuilder(myPlanetGraph, MeshParameters);
        }


        protected override void GenerateMesh()
        {
            Setup();

            PMB = ProceduralPlatonicSolidGenerator.CreateBuilder(GraphParameters.Shape, RadiusType.Normalized, MeshParameters.Radius);
        }
    }
}