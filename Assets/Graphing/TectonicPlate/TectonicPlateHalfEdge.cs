using Graphing.Position.Generic;

namespace Graphing.TectonicPlate
{
    public class TectonicPlateHalfEdge
        : GenericPositionHalfEdge<TectonicPlateGraph, TectonicPlatePolygonPartition, TectonicPlateNodePartition, TectonicPlatePolygon, TectonicPlateHalfEdge, TectonicPlateNode>
    {
        public TectonicPlateHalfEdge() : base()
        {
        }
        public TectonicPlateHalfEdge(TectonicPlateGraph graph, int id, int nodeId, int polyId = -1, int nextId = -1, int prevId = -1, int twinId = -1) : base(graph, id, nodeId, polyId, nextId, prevId, twinId)
        {
        }
    }
}

