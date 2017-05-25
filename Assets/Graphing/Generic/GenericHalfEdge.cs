using System.IO;
namespace Graphing.Generic
{
    public class GenericHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        where GraphType : GenericGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonPartitionType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodePartitionType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonType : GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where HalfEdgeType : GenericHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodeType : GenericNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
    {
        public class GenericHalfEdgeSerializer
        {
            public virtual void Save(BinaryWriter writer, HalfEdgeType edge)
            {
                writer.Write(edge.NodeId);
                writer.Write(edge.PolygonId);
                writer.Write(edge.NextId);
                writer.Write(edge.PreviousId);
                writer.Write(edge.TwinId);
            }
            public virtual void Load(BinaryReader reader, GraphType graph, HalfEdgeType edge)
            {
                edge.Graph = graph;
                edge.NodeId = reader.ReadInt32();
                edge.PolygonId = reader.ReadInt32();
                edge.NextId = reader.ReadInt32();
                edge.PreviousId = reader.ReadInt32();
                edge.TwinId = reader.ReadInt32();
            }
        }
        public class GenericHalfEdgeBuilder
        {
            public int Id;
            public int NodeId;
            public int PolygonId;
            public int NextId;
            public int PreviousId;
            public int TwinId;
            public virtual HalfEdgeType Build(GraphType graph)
            {
                return new HalfEdgeType()
                {
                    Graph = graph,
                    Id = Id,
                    NodeId = NodeId,
                    PolygonId = PolygonId,
                    NextId = NextId,
                    PreviousId = PreviousId,
                    TwinId = TwinId
                };
            }
            public virtual void LoadDual(HalfEdgeType edge)
            {
                //Switched
                PolygonId = edge.NodeId;
                NodeId = edge.PolygonId;
                //Preserved
                Id = edge.Id;
                NextId = edge.NextId;
                PreviousId = edge.PreviousId;
                TwinId = edge.TwinId;
            }
            public virtual void Load(HalfEdgeType edge)
            {
                //Switched
                PolygonId = edge.PolygonId;
                NodeId = edge.NodeId;
                //Preserved
                Id = edge.Id;
                NextId = edge.NextId;
                PreviousId = edge.PreviousId;
                TwinId = edge.TwinId;
            }
            public virtual void Load(GenericHalfEdgeBuilder builder)
            {
                LoadGeneric(builder);
            }
            public void LoadGeneric(GenericHalfEdgeBuilder builder)
            {
                PolygonId = builder.PolygonId;
                NodeId = builder.NodeId;
                Id = builder.Id;
                NextId = builder.NextId;
                PreviousId = builder.PreviousId;
                TwinId = builder.TwinId;
            }
            public HalfEdgeType BuildDual(GraphType graph, HalfEdgeType edge)
            {
                LoadDual(edge);
                return Build(graph);
            }
        }
        public GenericHalfEdge(GraphType graph, int id, int nodeId, int polyId = -1, int nextId = -1, int prevId = -1, int twinId = -1)
        {
            Graph = graph;
            Id = id;
            NodeId = nodeId;
            PolygonId = polyId;
            NextId = nextId;
            PreviousId = prevId;
            TwinId = twinId;
        }
        public GenericHalfEdge() : this(null, -1, -1)
        {
        }
        private GraphType Graph
        {
            get; set;
        }
        public int Id
        {
            get; private set;
        }
        private int NodeId
        {
            get; set;
        }
        private int PolygonId
        {
            get; set;
        }
        private int NextId
        {
            get; set;
        }
        private int PreviousId
        {
            get; set;
        }
        private int TwinId
        {
            get; set;
        }
        //Each edge needs an origin. if one is not present there is a problem
        public NodeType Node
        {
            get
            {
                return Graph.Nodes[NodeId];
            }
        }
        //Each edge needs a polygon. if one is not present there is a problem
        public PolygonType Polygon
        {
            get
            {
                return PolygonId >= 0 ? Graph.Polygons[PolygonId] : null;
            }
        }
        // A next may not be present,
        public HalfEdgeType Next
        {
            get
            {
                return NextId >= 0 ? Graph.Edges[NextId] : null;
            }
        }
        // A previous may not be present, say 
        public HalfEdgeType Previous
        {
            get
            {
                return PreviousId >= 0 ? Graph.Edges[PreviousId] : null;
            }
        }// A twin may not be present, say at a border
        public HalfEdgeType Twin
        {
            get { return TwinId >= 0 ? Graph.Edges[TwinId] : null; }
        }
    }
}

