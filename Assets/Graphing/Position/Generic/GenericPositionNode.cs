using Graphing.Generic;
using System.IO;
using UnityEngine;
namespace Graphing.Position.Generic
{
    public class GenericPositionNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        : GenericNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        where GraphType : GenericPositionGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonPartitionType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodePartitionType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonType : GenericPositionPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where HalfEdgeType : GenericPositionHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodeType : GenericPositionNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
    {

        public class GenericPositionNodeSerializer : GenericNodeSerializer
        {
            public override void Save(BinaryWriter writer, NodeType node)
            {
                base.Save(writer, node);
                writer.Write(node.Position.x);
                writer.Write(node.Position.y);
                writer.Write(node.Position.z);
            }
            public override void Load(BinaryReader reader, GraphType graph, NodeType node)
            {
                base.Load(reader, graph, node);
                node.Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
        }
        public class GenericPositionNodeBuilder : GenericNodeBuilder
        {
            //It took me a lifetime, but I finally figured out how to encapsulate, and unencapsulate, thank god 
            public Vector3 Position;
            public override NodeType Build(GraphType graph)
            {
                NodeType node = base.Build(graph);
                node.Position = Position;
                return node;
            }
            public override void Load(NodeType node)
            {
                base.Load(node);
                node.Position = node.Position;
            }
            public override void LoadDual(PolygonType polygon)
            {
                base.LoadDual(polygon);

                float mag = 0f;
                Vector3 pos = Vector3.zero;
                int counter = 0;
                foreach (var edge in polygon)
                {
                    pos += edge.Node.Position;
                    mag += edge.Node.Position.magnitude;
                    counter++;
                }
                if (counter == 0)
                    counter = 1;
                mag /= counter;
                pos.Normalize();
                Position = pos * mag;
            }
        }
        public GenericPositionNode(GraphType graph, int id, int partitionId, Vector3 position, int edgeId = -1) : base(graph, id, partitionId, edgeId)
        {
            Position = position;
        }
        public GenericPositionNode() : base()
        {
        }
        public Vector3 Position
        {
            get; private set;
        }
    }
}