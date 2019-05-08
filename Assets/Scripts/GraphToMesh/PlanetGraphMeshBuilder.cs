using System.Collections.Generic;
using Graphing.Planet;
using ProceduralMeshFramework;

namespace GraphToMesh
{
    public class PlanetGraphMeshBuilder : GraphMeshBuilder<PlanetGraph, PlanetPoly, PlanetEdge, PlanetNode>
    {
        public PlanetGraphMeshBuilder(PlanetGraph graph, PlanetGraphMeshParameters parameters)
        {
            Partitions = new List<PlanetPartitionMeshBuilder>();
            Parameters = parameters;
            foreach (var partition in graph.Partitions)
                Partitions.Add(new PlanetPartitionMeshBuilder(this, partition));
        }

        private List<PlanetPartitionMeshBuilder> Partitions { get; set; }
        public PlanetGraphMeshParameters Parameters { get; private set; }

        internal override List<ProceduralMeshBuilder> CreateSubmeshes()
        {
            var SubMeshes = new List<ProceduralMeshBuilder>();
            foreach (var partition in Partitions)
                SubMeshes.AddRange(partition.CreateSubmeshes());
            return SubMeshes;
        }

        public override void Generate()
        {
            foreach (var partition in Partitions)
                partition.Generate();
        }

        public override void Clear()
        {
            ClearMeshBuilder();
        }

        public override void Generate(PlanetGraph graph)
        {
        }

        protected override void GeneratePolygon(PlanetPoly polygon)
        {
        }

        protected override void GenerateCorner(PlanetEdge edge)
        {
        }

        protected override void GenerateEdge(PlanetEdge edge)
        {
        }
    }
}