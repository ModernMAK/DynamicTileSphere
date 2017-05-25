using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace Graphing.Generic
{
    public class GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        : IEnumerable<NodeType>
        where GraphType : GenericGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonPartitionType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodePartitionType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonType : GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where HalfEdgeType : GenericHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodeType : GenericNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
    {
        public class GenericNodePartitionSerializer
        {
            public virtual void Save(BinaryWriter writer, NodePartitionType partition)
            {
                writer.Write(partition.NodeIds.Length);
                foreach (int id in partition.NodeIds)
                    writer.Write(id);
            }
            public virtual void Load(BinaryReader reader, GraphType graph, NodePartitionType partition)
            {
                partition.Graph = graph;
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    partition.NodeIds[i] = reader.ReadInt32();
            }
        }
        public class GenericNodePartitionBuilder
        {
            public GenericNodePartitionBuilder()
            {
                Id = -1;
                NodeIds = new List<int>();
            }
            public int Id;
            public List<int> NodeIds
            {
                get;
                private set;
            }
            public virtual NodePartitionType Build(GraphType graph)
            {
                return new NodePartitionType() { Graph = graph, NodeIds = NodeIds.ToArray() };
            }
            public virtual void LoadDual(PolygonPartitionType partition)
            {
                NodeIds.Clear();
                Id = partition.Id;
                foreach (PolygonType poly in partition)
                    NodeIds.Add(poly.Id);
            }
            public virtual void Load(NodePartitionType partition)
            {
                Id = partition.Id;
                NodeIds.Clear();
                foreach (NodeType node in partition)
                    NodeIds.Add(node.Id);
            }
            public virtual void Load(GenericNodePartitionBuilder partition)
            {
                Id = partition.Id;
                NodeIds.Clear();
                foreach (int node in partition.NodeIds)
                    NodeIds.Add(node);
            }
            public NodePartitionType BuildDual(GraphType graph, PolygonPartitionType partition)
            {
                LoadDual(partition);
                return Build(graph);
            }

        }
        class GenericNodePartitionEnumerator : IEnumerator<NodeType>
        {
            public GenericNodePartitionEnumerator(GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType> partition)
            {
                myPartition = partition;
                currentIndex = -1;
            }
            private GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType> myPartition;
            private int currentIndex;
            public NodeType Current
            {
                get
                {
                    return myPartition.Graph.Nodes[myPartition.NodeIds[currentIndex]];
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
                currentIndex++;
                if (currentIndex >= myPartition.NodeIds.Length)
                    return false;
                return true;
            }
            public void Reset()
            {
                currentIndex = -1;
            }
        }
        public GenericNodePartition(GraphType graph, int id, params int[] nodes)
        {
            Graph = graph;
            Id = id;
            NodeIds = nodes;
        }
        public GenericNodePartition() : this(null, -1, new int[0])
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
        private int[] NodeIds
        {
            get; set;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<NodeType> GetEnumerator()
        {
            return new GenericNodePartitionEnumerator(this);
        }
    }

}

