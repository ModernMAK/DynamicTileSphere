using Graphing.Generic;
namespace Graphing.TectonicPlate
{
    public sealed class TectonicPlatePolygonPartition : GenericPolygonPartition<TectonicPlateGraph, TectonicPlatePolygonPartition, TectonicPlateNodePartition, TectonicPlatePolygon, TectonicPlateHalfEdge, TectonicPlateNode>
    {
        public TectonicPlatePolygonPartition() : base()
        {

        }
        public TectonicPlatePolygonPartition(TectonicPlateGraph graph, int id, params int[] nodes) : base(graph, id, nodes)
        {
        } 
    }
}