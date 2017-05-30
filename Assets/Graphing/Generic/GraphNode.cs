using System.IO;

namespace Graphing.Generic
{
    public class GraphNode<PolyT, EdgeT, NodeT>
        where NodeT : GraphNode<PolyT, EdgeT, NodeT>, new()
        where EdgeT : GraphEdge<PolyT, EdgeT, NodeT>, new()
        where PolyT : GraphPoly<PolyT, EdgeT, NodeT>, new()
    {
        public GraphNode()
        {
            Identity = -1;
            Edge = null;
        }
        public int Identity
        {
            get;
            internal set;
        }
        public EdgeT Edge
        {
            get;
            internal set;
        }
        internal virtual void LoadDual(PolyT data, EdgeT newEdge)
        {
            Identity = data.Identity;
            Edge = newEdge;
        }

        internal virtual void Serialize(BinaryWriter writer)
        {
            writer.Write(Edge.Identity);
        }
        internal virtual void Deserialize(BinaryReader reader, Graph<PolyT, EdgeT, NodeT> graph)
        {
            Edge = graph.Edges[reader.ReadInt32()];
        }
    }
    ////namespace Revision
    ////{
    ////    //It was today that I finally gave up on inheriting the graph, which in hindsight, is never neccessary, and makes it much more difficult, and instead impliment Data, like LinkedList, where I have a Data class do the Non-Graph work
    ////    public class GenericGraph<GraphData, PolyData, HalfEdgeData, NodeData>
    ////    {
    ////        public GenericNode<GraphData, PolyData, HalfEdgeData, NodeData>[] Nodes
    ////        {
    ////            get; private set;
    ////        }
    ////        public GenericPolygon<GraphData, PolyData, HalfEdgeData, NodeData>[] Polygons
    ////        {
    ////            get; private set;
    ////        }
    ////        public GenericHalfEdge<GraphData, PolyData, HalfEdgeData, NodeData>[] Edges
    ////        {
    ////            get; private set;
    ////        }
    ////    }
    ////    public class GenericPolygon<GraphData, PolyData, HalfEdgeData, NodeData>
    ////    {

    ////    }
    ////    public class GenericNode<GraphData, PolyData,HalfEdgeData,NodeData> : IEnumerable<>
    ////    {

    ////        public GenericNode(GenericGraph<GraphData, PolyData, HalfEdgeData, NodeData> graph, int id, int edgeId = -1)
    ////        {
    ////            Graph = graph;
    ////            Id = id;
    ////            EdgeId = edgeId;
    ////        }
    ////        public GenericNode() : this(null, -1)
    ////        {
    ////        }
    ////        private GenericGraph<GraphData, PolyData, HalfEdgeData, NodeData> Graph
    ////        {
    ////            get; set;
    ////        }
    ////        public int Id
    ////        {
    ////            get; private set;
    ////        }
    ////        private int EdgeId
    ////        {
    ////            get; set;
    ////        }
    ////        // An edge may not be present, an isolated 
    ////        public GenericHalfEdge<GraphData, PolyData, HalfEdgeData, NodeData> Edge
    ////        {
    ////            get
    ////            {
    ////                return EdgeId >= 0 ? Graph.Edges[EdgeId] : null;
    ////            }
    ////        }

    ////        public IEnumerator<GenericHalfEdge<GraphData, PolyData, HalfEdgeData, NodeData>> GetEnumerator()
    ////        {
    ////            //return new GenericNodeEnumerator(this);
    ////            return null;
    ////        }

    ////        IEnumerator IEnumerable.GetEnumerator()
    ////        {
    ////            return GetEnumerator();
    ////        }
    ////    }
    ////    public class GenericHalfEdge<GraphData, PolyData, HalfEdgeData, NodeData>
    ////    {
    ////        public GenericHalfEdge(GenericGraph<GraphData, PolyData, HalfEdgeData, NodeData> graph, int id, int nodeId, int polyId = -1, int nextId = -1, int prevId = -1, int twinId = -1)
    ////        {
    ////            Graph = graph;
    ////            Id = id;
    ////            NodeId = nodeId;
    ////            PolygonId = polyId;
    ////            NextId = nextId;
    ////            PreviousId = prevId;
    ////            TwinId = twinId;
    ////        }
    ////        public GenericHalfEdge() : this(null, -1, -1)
    ////        {
    ////        }
    ////        private GenericGraph<GraphData, PolyData, HalfEdgeData, NodeData> Graph
    ////        {
    ////            get; set;
    ////        }
    ////        public int Id
    ////        {
    ////            get; private set;
    ////        }
    ////        private int NodeId
    ////        {
    ////            get; set;
    ////        }
    ////        private int PolygonId
    ////        {
    ////            get; set;
    ////        }
    ////        private int NextId
    ////        {
    ////            get; set;
    ////        }
    ////        private int PreviousId
    ////        {
    ////            get; set;
    ////        }
    ////        private int TwinId
    ////        {
    ////            get; set;
    ////        }
    ////        //Each edge needs an origin. if one is not present there is a problem
    ////        public GenericNode<GraphData, PolyData, HalfEdgeData, NodeData> Node
    ////        {
    ////            get
    ////            {
    ////                return Graph.Nodes[NodeId];
    ////            }
    ////        }
    ////        //Each edge needs a polygon. if one is not present there is a problem
    ////        public GenericPolygon<GraphData, PolyData, HalfEdgeData, NodeData> Polygon
    ////        {
    ////            get
    ////            {
    ////                return PolygonId >= 0 ? Graph.Polygons[PolygonId] : null;
    ////            }
    ////        }
    ////        // A next may not be present,
    ////        public GenericHalfEdge<GraphData, PolyData, HalfEdgeData, NodeData> Next
    ////        {
    ////            get
    ////            {
    ////                return NextId >= 0 ? Graph.Edges[NextId] : null;
    ////            }
    ////        }
    ////        // A previous may not be present, say 
    ////        public GenericHalfEdge<GraphData, PolyData, HalfEdgeData, NodeData> Previous
    ////        {
    ////            get
    ////            {
    ////                return PreviousId >= 0 ? Graph.Edges[PreviousId] : null;
    ////            }
    ////        }// A twin may not be present, say at a border
    ////        public GenericHalfEdge<GraphData, PolyData, HalfEdgeData, NodeData> Twin
    ////        {
    ////            get { return TwinId >= 0 ? Graph.Edges[TwinId] : null; }
    ////        }
    ////    }
    ////}
    //public partial class GenericGraph<GraphT, PolyT, HalfEdgeT, NodeT>
    //    where GraphT : GenericGraph<GraphT, PolyT, HalfEdgeT, NodeT>, new()
    //    where PolyT : GenericPolygon<GraphT, PolyT, HalfEdgeT, NodeT>, new()
    //    where HalfEdgeT : GenericHalfEdge<GraphT, PolyT, HalfEdgeT, NodeT>, new()
    //    where NodeT : GenericNode<GraphT, PolyT, HalfEdgeT, NodeT>, new()

    //{
    //    public GenericGraph(NodeT[] nodes, PolyT[] polygons, HalfEdgeT[] edges)
    //    {
    //        Nodes = nodes;
    //        Polygons = polygons;
    //        Edges = edges;
    //    }
    //    public GenericGraph() : this(new NodeT[0], new PolyT[0], new HalfEdgeT[0])
    //    {
    //    }
    //    public NodeT[] Nodes
    //    {
    //        get; private set;
    //    }
    //    public PolyT[] Polygons
    //    {
    //        get; private set;
    //    }
    //    public HalfEdgeT[] Edges
    //    {
    //        get; private set;
    //    }
    //}
}