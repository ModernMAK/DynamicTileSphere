using UnityEngine;
using Graphing.Position.Generic;

namespace Graphing.Position
{
    public class PositionHalfEdge
        : GenericPositionHalfEdge<PositionGraph, PositionPolygonPartition, PositionNodePartition, PositionPolygon, PositionHalfEdge, PositionNode>
    {
        public PositionHalfEdge() : base()
        {
        }
        public PositionHalfEdge(PositionGraph graph, int id, int nodeId, int polyId = -1, int nextId = -1, int prevId = -1, int twinId = -1) : base(graph, id, nodeId, polyId, nextId, prevId, twinId)
        {
        }
    }
}

