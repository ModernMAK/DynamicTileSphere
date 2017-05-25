using Graphing.Generic;
namespace Graphing.Planet
{
    public sealed class PlanetNodePartition : GenericNodePartition<PlanetGraph, PlanetPolygonPartition, PlanetNodePartition, PlanetPolygon, PlanetHalfEdge, PlanetNode>
    {
        public PlanetNodePartition() : base()
        {
        }
        public PlanetNodePartition(PlanetGraph graph, int id, params int[] nodes) : base(graph, id, nodes)
        {
        }
    }
}

