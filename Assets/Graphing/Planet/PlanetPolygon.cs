using System.IO;
using Graphing.Position.Generic;

namespace Graphing.Planet
{
    public class PlanetPolygon
        : GenericPositionPolygon<PlanetGraph, PlanetPolygonPartition, PlanetNodePartition, PlanetPolygon, PlanetHalfEdge, PlanetNode>

    {
        public class PlanetPolygonSerializer : GenericPolygonSerializer
        {
            public override void Save(BinaryWriter writer, PlanetPolygon polygon)
            {
                base.Save(writer, polygon);
                writer.Write(polygon.Elevation);
                writer.Write(polygon.TerrainType);
            }
            public override void Load(BinaryReader reader, PlanetGraph graph, PlanetPolygon polygon)
            {
                base.Load(reader, graph, polygon);
                polygon.Elevation = reader.ReadInt32();
                polygon.TerrainType = reader.ReadInt32();
            }
        }
        public class PlanetPolygonBuilder : GenericPolygonBuilder
        {
            public int Elevation { get; set; }
            public int TerrainType { get; set; }

            public override PlanetPolygon Build(PlanetGraph graph)
            {
                PlanetPolygon polygon = base.Build(graph);
                polygon.Elevation = Elevation;
                polygon.TerrainType = TerrainType;
                return polygon;
            }
            public override void Load(PlanetPolygon poly)
            {
                base.Load(poly);
                Elevation = poly.Elevation;
                TerrainType = poly.TerrainType;
            }
            public override void LoadDual(PlanetNode node)
            {
                base.LoadDual(node);
                Elevation = node.Edge.Polygon.Elevation;
                TerrainType = node.Edge.Polygon.TerrainType;
            }
        }
        public PlanetPolygon() : base()
        {
        }
        public PlanetPolygon(PlanetGraph graph, int id, int partitionId, int edgeId) : base(graph, id, partitionId, edgeId)
        {
        }
        public int Elevation { get; private set; }
        public int TerrainType { get; private set; }

    }
}

