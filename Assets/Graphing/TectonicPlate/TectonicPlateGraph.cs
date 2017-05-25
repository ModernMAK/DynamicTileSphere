using Graphing.Position.Generic;
namespace Graphing.TectonicPlate
{
    public class TectonicPlateGraph
        : GenericPositionGraph<TectonicPlateGraph, TectonicPlatePolygonPartition, TectonicPlateNodePartition, TectonicPlatePolygon, TectonicPlateHalfEdge, TectonicPlateNode>
    {

        //Mostly to make life easy for everyone
        public class TectonicPlateGraphSerializer : GenericPositionGraphSerializer<TectonicPlatePolygonPartition.GenericPolygonPartitionSerializer, TectonicPlateNodePartition.GenericNodePartitionSerializer, TectonicPlatePolygon.GenericPolygonSerializer, TectonicPlateHalfEdge.GenericHalfEdgeSerializer, TectonicPlateNode.GenericPositionNodeSerializer>
        {
            public TectonicPlateGraphSerializer(TectonicPlateGraph graph, string fName, string fDirectory = null, string fExtension = null) : base(graph, fName, fDirectory, fExtension)
            {
            }
        }
        //Mostly to make life easy for everyone
        public class TectonicPlateGraphBuilder : GenericPositionGraphBuilder<TectonicPlatePolygonPartition.GenericPolygonPartitionBuilder, TectonicPlateNodePartition.GenericNodePartitionBuilder, TectonicPlatePolygon.TectonicPlatePolygonBuilder, TectonicPlateHalfEdge.GenericHalfEdgeBuilder, TectonicPlateNode.GenericPositionNodeBuilder>
        {
        }
        public TectonicPlateGraph() : base()
        {
        }
        public TectonicPlateGraph(TectonicPlateNode[] nodes, TectonicPlatePolygon[] polygons, TectonicPlateHalfEdge[] edges, TectonicPlateNodePartition[] nodePartititons, TectonicPlatePolygonPartition[] polyPartititons) : base(nodes, polygons, edges, nodePartititons, polyPartititons)
        {
        }
    }
}