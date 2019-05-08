using System.Collections.Generic;
using Graphing.Planet;
using ProceduralGraph;
using Random = UnityEngine.Random;

namespace GraphToMesh
{
    public class PlanetGraphPopulator
    {
        public static void SetRandomTerrain(PlanetGraph graph, PlanetGraphParameters parameters)
        {
            foreach (var poly in graph.Polys)
                poly.TerrainType = Random.Range(0, parameters.TerrainTypes);
        }


        public static void Populate(PlanetGraph graph, PlanetGraphParameters parameters)
        {
            using (var randLock = new TempRandomLock(parameters.Seed))
            {
                SetRandomTerrain(graph, parameters);
                CalculateRandomHeights(graph, parameters);
            }
        }

        private static void CalculateRandomHeights(PlanetGraph graph, PlanetGraphParameters parameters)
        {
            foreach (var poly in graph.Polys)
                poly.Elevation = Random.Range(parameters.MinHeight, parameters.MaxHeight);
        }
    }
}