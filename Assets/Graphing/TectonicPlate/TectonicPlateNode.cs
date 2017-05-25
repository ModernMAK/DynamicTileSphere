using UnityEngine;
namespace Graphing.TectonicPlate
{
    using Position.Generic;
    public class TectonicPlateNode
        : GenericPositionNode<TectonicPlateGraph, TectonicPlatePolygonPartition, TectonicPlateNodePartition, TectonicPlatePolygon, TectonicPlateHalfEdge, TectonicPlateNode>
    {
        public TectonicPlateNode() : base()
        {
        }
        public TectonicPlateNode(TectonicPlateGraph graph, int id, int partitionId, Vector3 position, int edgeId = -1) : base(graph, id, partitionId, position, edgeId)
        {
        }
    }
}