using Graphing.Generic;
namespace Graphing.Planet
{
    public sealed class PlanetPolygonPartition : GenericPolygonPartition<PlanetGraph, PlanetPolygonPartition, PlanetNodePartition, PlanetPolygon, PlanetHalfEdge, PlanetNode>
    {
        public PlanetPolygonPartition() : base()
        {

        }
        public PlanetPolygonPartition(PlanetGraph graph, int id, params int[] nodes) : base(graph, id, nodes)
        {
        }
    }
}