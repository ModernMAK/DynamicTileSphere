using UnityEngine;
using Graphing.Position.Generic;

namespace Graphing.Planet
{
    public class PlanetHalfEdge
        : GenericPositionHalfEdge<PlanetGraph, PlanetPolygonPartition, PlanetNodePartition, PlanetPolygon, PlanetHalfEdge, PlanetNode>
    {
        public PlanetHalfEdge() : base()
        {
        }
        public PlanetHalfEdge(PlanetGraph graph, int id, int nodeId, int polyId = -1, int nextId = -1, int prevId = -1, int twinId = -1) : base(graph, id, nodeId, polyId, nextId, prevId, twinId)
        {
        }
    }
}

