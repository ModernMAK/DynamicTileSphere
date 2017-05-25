using Graphing.Position.Generic;

namespace Graphing.TectonicPlate
{
    public class TectonicPlatePolygon
        : GenericPositionPolygon<TectonicPlateGraph, TectonicPlatePolygonPartition, TectonicPlateNodePartition, TectonicPlatePolygon, TectonicPlateHalfEdge, TectonicPlateNode>
    {
        public class TectonicPlatePolygonBuilder : GenericPolygonBuilder
        {
        }
        public TectonicPlatePolygon() : base()
        {
        }
        public TectonicPlatePolygon(TectonicPlateGraph graph, int id, int partitionId, int edgeId) : base(graph, id, partitionId, edgeId)
        {
        }
    }
}