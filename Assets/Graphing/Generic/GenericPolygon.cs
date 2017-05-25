using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace Graphing.Generic
{
    public class GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        : IEnumerable<HalfEdgeType>
        where GraphType : GenericGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonPartitionType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodePartitionType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonType : GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where HalfEdgeType : GenericHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodeType : GenericNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
    {
        public class GenericPolygonSerializer
        {
            public virtual void Save(BinaryWriter writer, PolygonType edge)
            {
                writer.Write(edge.EdgeId);
                writer.Write(edge.PartitionId);
            }
            public virtual void Load(BinaryReader reader, GraphType graph, PolygonType edge)
            {
                edge.Graph = graph;
                edge.EdgeId = reader.ReadInt32();
                edge.PartitionId = reader.ReadInt32();
            }
        }
        public class GenericPolygonBuilder
        {
            public int Id;
            public int EdgeId;
            public int PartitionId;
            public virtual PolygonType Build(GraphType graph)
            {
                return new PolygonType()
                {
                    Id = Id,
                    Graph = graph,
                    EdgeId = EdgeId,
                    PartitionId = PartitionId
                };
            }
            public virtual void LoadDual(NodeType node)
            {
                //EdgeId = polygon.Edge.Id;
                Id = node.Id;
                EdgeId = node.Edge.Id;
                PartitionId = node.Partition.Id;//Duals do not copy their partition, an unfortunate artifact od only keeping polygon partitions and not node partitions.
            }
            public virtual void Load(PolygonType poly)
            {
                //EdgeId = polygon.Edge.Id;
                Id = poly.Id;
                EdgeId = poly.EdgeId;
                PartitionId = poly.PartitionId;//Duals do not copy their partition, an unfortunate artifact od only keeping polygon partitions and not node partitions.
            }
            public virtual void Load(GenericPolygonBuilder builder)
            {
                //EdgeId = polygon.Edge.Id;
                Id = builder.Id;
                EdgeId = builder.EdgeId;
                PartitionId = builder.PartitionId;//Duals do not copy their partition, an unfortunate artifact od only keeping polygon partitions and not node partitions.
            }
            public PolygonType BuildDual(GraphType graph, NodeType node)
            {
                LoadDual(node);
                return Build(graph);
            }
        }
        class GenericPolygonEnumerator : IEnumerator<HalfEdgeType>
        {
            public GenericPolygonEnumerator(GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType> polygon)
            {
                myPolygon = polygon;
                CurrentEdge = null;
            }
            private GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType> myPolygon;
            private HalfEdgeType CurrentEdge;
            public HalfEdgeType Current
            {
                get
                {
                    return CurrentEdge;
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }
            public void Dispose()
            {
                //Do nothing
            }
            public bool MoveNext()
            {
                if (CurrentEdge == null)
                {
                    CurrentEdge = myPolygon.Edge;
                    return true;
                }
                else
                {
                    CurrentEdge = CurrentEdge.Next;
                    //Not Null (Broken Polygon) and Not Back at the start
                    return (CurrentEdge != null && CurrentEdge != myPolygon.Edge);
                }

            }
            public void Reset()
            {
                CurrentEdge = null;
            }
        }
        public GenericPolygon() : this(null, -1, -1, -1)
        {
        }
        public GenericPolygon(GraphType graph, int id, int partitionId, int edgeId)
        {
            Graph = graph;
            Id = id;
            PartitionId = partitionId;
            EdgeId = edgeId;
        }
        private GraphType Graph
        {
            get; set;
        }
        public int Id
        {
            get; private set;
        }
        private int EdgeId
        {
            get; set;
        }
        private int PartitionId
        {
            get; set;
        }
        public HalfEdgeType Edge
        {
            get
            {
                return Graph.Edges[EdgeId];
            }
        }
        public PolygonPartitionType Partition
        {
            get
            {
                return Graph.PolygonPartitions[PartitionId];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<HalfEdgeType> GetEnumerator()
        {
            return new GenericPolygonEnumerator(this);
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(EdgeId);
            writer.Write(PartitionId);
        }
        public virtual void Load(GraphType graph, BinaryReader reader)
        {
            Graph = graph;
            EdgeId = reader.ReadInt32();
            PartitionId = reader.ReadInt32();
        }
    }
}