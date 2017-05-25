using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace Graphing.Generic
{
    public class GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        : IEnumerable<PolygonType>
        where GraphType : GenericGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonPartitionType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodePartitionType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonType : GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where HalfEdgeType : GenericHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodeType : GenericNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
    {
        public class GenericPolygonPartitionSerializer
        {
            public virtual void Save(BinaryWriter writer, PolygonPartitionType partition)
            {
                writer.Write(partition.PolygonIds.Length);
                foreach (int id in partition.PolygonIds)
                    writer.Write(id);
            }
            public virtual void Load(BinaryReader reader, GraphType graph, PolygonPartitionType partition)
            {
                partition.Graph = graph;
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    partition.PolygonIds[i] = reader.ReadInt32();
            }
        }
        public class GenericPolygonPartitionBuilder
        {
            public GenericPolygonPartitionBuilder()
            {
                PolygonIds = new List<int>();
                Id = -1;
            }
            public int Id;
            public List<int> PolygonIds
            {
                get;
                private set;
            }
            public virtual PolygonPartitionType Build(GraphType graph)
            {
                return new PolygonPartitionType()
                {
                    Id = Id,
                    Graph = graph,
                    PolygonIds = PolygonIds.ToArray()
                };
            }
            public virtual void LoadDual(NodePartitionType partition)
            {
                PolygonIds.Clear();
                Id = partition.Id;
                foreach (NodeType node in partition)
                    PolygonIds.Add(node.Id);
            }
            public virtual void Load(PolygonPartitionType partition)
            {
                PolygonIds.Clear();
                Id = partition.Id;
                foreach (PolygonType poly in partition)
                    PolygonIds.Add(poly.Id);
            }
            public virtual void Load(GenericPolygonPartitionBuilder builder)
            {
                LoadGeneric(builder);
            }
            public void LoadGeneric(GenericPolygonPartitionBuilder builder)
            {
                Id = builder.Id;
                PolygonIds.Clear();
                foreach (int poly in builder.PolygonIds)
                    PolygonIds.Add(poly);
            }
            public PolygonPartitionType BuildDual(GraphType graph, NodePartitionType partition)
            {
                LoadDual(partition);
                return Build(graph);
            }
        }
        class GenericPolygonPartitionEnumerator : IEnumerator<PolygonType>
        {

            public GenericPolygonPartitionEnumerator(GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType> partition)
            {
                myPartition = partition;
                currentIndex = -1;
            }
            private GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType> myPartition;
            private int currentIndex;
            public PolygonType Current
            {
                get
                {
                    return myPartition.Graph.Polygons[myPartition.PolygonIds[currentIndex]];
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
                if (currentIndex >= myPartition.PolygonIds.Length)
                    return false;
                return true;
            }
            public void Reset()
            {
                currentIndex = -1;
            }
        }
        public GenericPolygonPartition(GraphType graph, int id, params int[] nodes)
        {
            Graph = graph;
            Id = id;
            PolygonIds = nodes;
        }
        public GenericPolygonPartition() : this(null, -1, new int[0])
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
        private int[] PolygonIds
        {
            get; set;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<PolygonType> GetEnumerator()
        {
            return new GenericPolygonPartitionEnumerator(this);
        }
    }
}