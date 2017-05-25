using Graphing.Position.Generic;
namespace Graphing.Planet
{
    public class PlanetGraph
        : GenericPositionGraph<PlanetGraph, PlanetPolygonPartition, PlanetNodePartition, PlanetPolygon, PlanetHalfEdge, PlanetNode>
    {

        //Mostly to make life easy for everyone
        public class PlanetGraphSerializer : GenericPositionGraphSerializer<PlanetPolygonPartition.GenericPolygonPartitionSerializer, PlanetNodePartition.GenericNodePartitionSerializer, PlanetPolygon.PlanetPolygonSerializer, PlanetHalfEdge.GenericHalfEdgeSerializer, PlanetNode.GenericPositionNodeSerializer>
        {
            public PlanetGraphSerializer(PlanetGraph graph, string fName, string fDirectory = null, string fExtension = null) : base(graph, fName, fDirectory, fExtension)
            {
            }
        }
        //Mostly to make life easy for everyone
        public class PlanetGraphBuilder : GenericPositionGraphBuilder<PlanetPolygonPartition.GenericPolygonPartitionBuilder, PlanetNodePartition.GenericNodePartitionBuilder, PlanetPolygon.PlanetPolygonBuilder, PlanetHalfEdge.GenericHalfEdgeBuilder, PlanetNode.GenericPositionNodeBuilder>
        {
        }
        public PlanetGraph() : base()
        {
        }
        public PlanetGraph(PlanetNode[] nodes, PlanetPolygon[] polygons, PlanetHalfEdge[] edges, PlanetNodePartition[] nodePartititons, PlanetPolygonPartition[] polyPartititons) : base(nodes, polygons, edges, nodePartititons, polyPartititons)
        {
        }
    }
}