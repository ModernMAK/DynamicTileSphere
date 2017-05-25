using Graphing.Generic;
namespace Graphing.TectonicPlate
{
    public sealed class TectonicPlateNodePartition : GenericNodePartition<TectonicPlateGraph, TectonicPlatePolygonPartition, TectonicPlateNodePartition, TectonicPlatePolygon, TectonicPlateHalfEdge, TectonicPlateNode>
    {
        public TectonicPlateNodePartition() : base()
        {

        }
        public TectonicPlateNodePartition(TectonicPlateGraph graph, int id, params int[] nodes) : base(graph, id, nodes)
        {
        }
    }
}