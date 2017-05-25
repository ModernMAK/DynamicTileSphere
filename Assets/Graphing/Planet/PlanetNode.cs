using UnityEngine;
namespace Graphing.Planet
{
    using Position.Generic;
    public class PlanetNode
        : GenericPositionNode<PlanetGraph, PlanetPolygonPartition, PlanetNodePartition, PlanetPolygon, PlanetHalfEdge, PlanetNode>
    {
        public PlanetNode() : base()
        {
        }
        public PlanetNode(PlanetGraph graph, int id, int partitionId, Vector3 position, int edgeId = -1) : base(graph, id, partitionId, position, edgeId)
        {
        }

    }

}

