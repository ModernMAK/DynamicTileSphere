using System.Collections.Generic;
using System.IO;

namespace Graphing.Generic
{
    public class GenericGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>
        where GraphType : GenericGraph<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonPartitionType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodePartitionType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where PolygonType : GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where HalfEdgeType : GenericHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()
        where NodeType : GenericNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>, new()

    {
        public class GenericGraphSerializer<PolygonPartitionSerializerType, NodePartitionSerializerType, PolygonSerializerType, HalfEdgeSerializerType, NodeSerializerType>
            where PolygonPartitionSerializerType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericPolygonPartitionSerializer, new()
            where NodePartitionSerializerType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericNodePartitionSerializer, new()
            where PolygonSerializerType : GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericPolygonSerializer, new()
            where HalfEdgeSerializerType : GenericHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericHalfEdgeSerializer, new()
            where NodeSerializerType : GenericNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericNodeSerializer, new()
        {
            public GenericGraphSerializer(GraphType graph, string fName, string fDirectory = null, string fExtension = null)
            {
                Graph = graph;
                GraphPath = Path.ChangeExtension(Path.Combine(fDirectory, fName), fExtension);
            }
            public GraphType Graph
            {
                get; set;
            }
            public string GraphPath
            {
                get; set;
            }

            public void Save()
            {
                using (var stream = new FileStream(GraphPath, FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(stream))
                        SaveGraph(writer);
                }
            }
            public void Load()
            {
                using (var stream = new FileStream(GraphPath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream))
                        LoadGraph(reader);
                }
            }
            protected virtual void SaveGraph(BinaryWriter writer)
            {
                writer.Write(Graph.Nodes.Length);
                NodeSerializerType nodeSerializer = new NodeSerializerType();
                foreach (var node in Graph.Nodes)
                    nodeSerializer.Save(writer, node);

                writer.Write(Graph.Polygons.Length);
                PolygonSerializerType polygonSerializer = new PolygonSerializerType();
                foreach (var polygon in Graph.Polygons)
                    polygonSerializer.Save(writer, polygon);

                writer.Write(Graph.Edges.Length);
                HalfEdgeSerializerType edgeSerializer = new HalfEdgeSerializerType();
                foreach (var edge in Graph.Edges)
                    edgeSerializer.Save(writer, edge);

                writer.Write(Graph.PolygonPartitions.Length);
                PolygonPartitionSerializerType polyPartitionSerializer = new PolygonPartitionSerializerType();
                foreach (var partition in Graph.PolygonPartitions)
                    polyPartitionSerializer.Save(writer, partition);

                writer.Write(Graph.NodePartitions.Length);
                NodePartitionSerializerType nodePartitionSerializer = new NodePartitionSerializerType();
                foreach (var partition in Graph.NodePartitions)
                    nodePartitionSerializer.Save(writer, partition);

            }
            protected virtual void LoadGraph(BinaryReader reader)
            {
                Graph.Nodes = new NodeType[reader.ReadInt32()];
                NodeSerializerType nodeSerializer = new NodeSerializerType();
                foreach (var node in Graph.Nodes)
                    nodeSerializer.Load(reader, Graph, node);

                Graph.Polygons = new PolygonType[reader.ReadInt32()];
                PolygonSerializerType polygonSerializer = new PolygonSerializerType();
                foreach (var polygon in Graph.Polygons)
                    polygonSerializer.Load(reader, Graph, polygon);

                Graph.Edges = new HalfEdgeType[reader.ReadInt32()];
                HalfEdgeSerializerType edgeSerializer = new HalfEdgeSerializerType();
                foreach (var edge in Graph.Edges)
                    edgeSerializer.Load(reader, Graph, edge);

                Graph.PolygonPartitions = new PolygonPartitionType[reader.ReadInt32()];
                PolygonPartitionSerializerType polyPartitionSerializer = new PolygonPartitionSerializerType();
                foreach (var partition in Graph.PolygonPartitions)
                    polyPartitionSerializer.Load(reader, Graph, partition);

                Graph.NodePartitions = new NodePartitionType[reader.ReadInt32()];
                NodePartitionSerializerType nodePartitionSerializer = new NodePartitionSerializerType();
                foreach (var partition in Graph.NodePartitions)
                    nodePartitionSerializer.Load(reader, Graph, partition);
            }
        }
        public class GenericGraphBuilder<PolygonPartitionBuilderType, NodePartitionBuilderType, PolygonBuilderType, HalfEdgeBuilderType, NodeBuilderType>
            where PolygonPartitionBuilderType : GenericPolygonPartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericPolygonPartitionBuilder, new()
            where NodePartitionBuilderType : GenericNodePartition<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericNodePartitionBuilder, new()
            where PolygonBuilderType : GenericPolygon<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericPolygonBuilder, new()
            where HalfEdgeBuilderType : GenericHalfEdge<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericHalfEdgeBuilder, new()
            where NodeBuilderType : GenericNode<GraphType, PolygonPartitionType, NodePartitionType, PolygonType, HalfEdgeType, NodeType>.GenericNodeBuilder, new()
        {
            public GenericGraphBuilder()
            {
                Nodes = new List<NodeBuilderType>();
                Polygons = new List<PolygonBuilderType>();
                Edges = new List<HalfEdgeBuilderType>();
                NodePartitions = new List<NodePartitionBuilderType>();
                PolygonPartitions = new List<PolygonPartitionBuilderType>();
            }
            public GenericGraphBuilder(GenericGraphBuilder<PolygonPartitionBuilderType, NodePartitionBuilderType, PolygonBuilderType, HalfEdgeBuilderType, NodeBuilderType> builder) : this()
            {
                Load(builder);
            }
            public List<NodeBuilderType> Nodes
            {
                get; private set;
            }
            public List<PolygonBuilderType> Polygons
            {
                get; private set;
            }
            public List<HalfEdgeBuilderType> Edges
            {
                get; private set;
            }
            public List<NodePartitionBuilderType> NodePartitions
            {
                get; private set;
            }
            public List<PolygonPartitionBuilderType> PolygonPartitions
            {
                get; private set;
            }

            public virtual GraphType Build()
            {
                NodeType[] nodes = new NodeType[Nodes.Count];
                PolygonType[] polygons = new PolygonType[Polygons.Count];
                HalfEdgeType[] edges = new HalfEdgeType[Edges.Count];
                NodePartitionType[] nodePartitions = new NodePartitionType[NodePartitions.Count];
                PolygonPartitionType[] polygonPartitions = new PolygonPartitionType[PolygonPartitions.Count];
                GraphType graph = new GraphType();
                int i = 0;
                foreach (var builder in Nodes)
                    nodes[i++] = builder.Build(graph);
                i = 0;
                foreach (var builder in Polygons)
                    polygons[i++] = builder.Build(graph);
                i = 0;
                foreach (var builder in Edges)
                    edges[i++] = builder.Build(graph);
                i = 0;
                foreach (var builder in NodePartitions)
                    nodePartitions[i++] = builder.Build(graph);
                i = 0;
                foreach (var builder in PolygonPartitions)
                    polygonPartitions[i++] = builder.Build(graph);
                return graph;
            }
            public virtual void Load(GraphType graph)
            {
                Clear();
                NodePartitionBuilderType nodePartitionBuilder;
                int i = 0;
                foreach (var node in graph.NodePartitions)
                {
                    nodePartitionBuilder = new NodePartitionBuilderType()
                    {
                        Id = i++
                    };
                    nodePartitionBuilder.Load(node);
                    NodePartitions.Add(nodePartitionBuilder);
                }
                i = 0;
                PolygonPartitionBuilderType polyPartitionBuilder;
                foreach (var poly in graph.PolygonPartitions)
                {
                    polyPartitionBuilder = new PolygonPartitionBuilderType()
                    {
                        Id = i++
                    };
                    polyPartitionBuilder.Load(poly);
                    PolygonPartitions.Add(polyPartitionBuilder);
                }
                i = 0;
                NodeBuilderType nodeBuilder;
                foreach (var node in graph.Nodes)
                {
                    nodeBuilder = new NodeBuilderType()
                    {
                        Id = i++
                    };
                    nodeBuilder.Load(node);
                    Nodes.Add(nodeBuilder);
                }
                i = 0;
                PolygonBuilderType polyBuilder;
                foreach (var poly in graph.Polygons)
                {
                    polyBuilder = new PolygonBuilderType()
                    {
                        Id = i++
                    };
                    polyBuilder.Load(poly);
                    Polygons.Add(polyBuilder);
                }
                i = 0;
                HalfEdgeBuilderType edgeBuider;
                foreach (var edge in graph.Edges)
                {
                    edgeBuider = new HalfEdgeBuilderType()
                    {
                        Id = i++
                    };
                    edgeBuider.Load(edge);
                    Edges.Add(edgeBuider);
                }
            }
            public void Clear()
            {
                Nodes.Clear();
                Polygons.Clear();
                Edges.Clear();
                NodePartitions.Clear();
                PolygonPartitions.Clear();
            }
            public virtual void Load(GenericGraphBuilder<PolygonPartitionBuilderType, NodePartitionBuilderType, PolygonBuilderType, HalfEdgeBuilderType, NodeBuilderType> graph)
            {
                LoadGeneric(graph);
            }
            public void LoadGeneric(GenericGraphBuilder<PolygonPartitionBuilderType, NodePartitionBuilderType, PolygonBuilderType, HalfEdgeBuilderType, NodeBuilderType> graph)
            {
                Clear();
                NodePartitionBuilderType nodePartitionBuilder;
                foreach (var node in graph.NodePartitions)
                {
                    nodePartitionBuilder = new NodePartitionBuilderType();
                    nodePartitionBuilder.Load(node);
                    NodePartitions.Add(nodePartitionBuilder);
                }
                PolygonPartitionBuilderType polyPartitionBuilder;
                foreach (var poly in graph.PolygonPartitions)
                {
                    polyPartitionBuilder = new PolygonPartitionBuilderType();
                    polyPartitionBuilder.Load(poly);
                    PolygonPartitions.Add(polyPartitionBuilder);
                }
                NodeBuilderType nodeBuilder;
                foreach (var node in graph.Nodes)
                {
                    nodeBuilder = new NodeBuilderType();
                    nodeBuilder.Load(node);
                    Nodes.Add(nodeBuilder);
                }
                HalfEdgeBuilderType edgeBuilder;
                foreach (var edge in graph.Edges)
                {
                    edgeBuilder = new HalfEdgeBuilderType();
                    edgeBuilder.Load(edge);
                    Edges.Add(edgeBuilder);
                }
                PolygonBuilderType polyBuilder;
                foreach (var poly in graph.Polygons)
                {
                    polyBuilder = new PolygonBuilderType();
                    polyBuilder.Load(poly);
                    Polygons.Add(polyBuilder);
                }
            }
            public virtual void LoadDual(GraphType graph)
            {
                Nodes.Clear();
                Polygons.Clear();
                Edges.Clear();
                NodePartitions.Clear();
                PolygonPartitions.Clear();
                PolygonPartitionBuilderType polyPartitionBuilder;
                foreach (var node in graph.NodePartitions)
                {
                    polyPartitionBuilder = new PolygonPartitionBuilderType();
                    polyPartitionBuilder.LoadDual(node);
                    PolygonPartitions.Add(polyPartitionBuilder);
                }
                NodePartitionBuilderType nodePartitionBuilder;
                foreach (var poly in graph.PolygonPartitions)
                {
                    nodePartitionBuilder = new NodePartitionBuilderType();
                    nodePartitionBuilder.LoadDual(poly);
                    NodePartitions.Add(nodePartitionBuilder);
                }
                PolygonBuilderType polyBuilder;
                foreach (var node in graph.Nodes)
                {
                    polyBuilder = new PolygonBuilderType();
                    polyBuilder.LoadDual(node);
                    Polygons.Add(polyBuilder);
                }
                NodeBuilderType nodeBuilder;
                foreach (var poly in graph.Polygons)
                {
                    nodeBuilder = new NodeBuilderType();
                    nodeBuilder.LoadDual(poly);
                    Nodes.Add(nodeBuilder);
                }
                HalfEdgeBuilderType edgeBuider;
                foreach (var edge in graph.Edges)
                {
                    edgeBuider = new HalfEdgeBuilderType();
                    edgeBuider.LoadDual(edge);
                    Edges.Add(edgeBuider);
                }
            }
            public GraphType BuildDual(GraphType graph)
            {
                LoadDual(graph);
                return Build();
            }
        }
        public GenericGraph(NodeType[] nodes, PolygonType[] polygons, HalfEdgeType[] edges, NodePartitionType[] nodePartititons, PolygonPartitionType[] polyPartitions)
        {
            Nodes = nodes;
            Polygons = polygons;
            Edges = edges;
            PolygonPartitions = polyPartitions;
            NodePartitions = nodePartititons;
        }
        public GenericGraph() : this(new NodeType[0], new PolygonType[0], new HalfEdgeType[0], new NodePartitionType[0], new PolygonPartitionType[0])
        {
        }
        public NodeType[] Nodes
        {
            get; private set;
        }
        public PolygonType[] Polygons
        {
            get; private set;
        }
        public HalfEdgeType[] Edges
        {
            get; private set;
        }
        public PolygonPartitionType[] PolygonPartitions
        {
            get; private set;
        }
        public NodePartitionType[] NodePartitions
        {
            get; private set;
        }
    }
}