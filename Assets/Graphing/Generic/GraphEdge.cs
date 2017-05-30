using System.IO;

namespace Graphing.Generic
{
    //Technically a Half Edge, which is a Directed Edge, but whatever.
    public class GraphEdge<PolyT, EdgeT, NodeT>
        where NodeT : GraphNode<PolyT, EdgeT, NodeT>, new()
        where EdgeT : GraphEdge<PolyT, EdgeT, NodeT>, new()
        where PolyT : GraphPoly<PolyT, EdgeT, NodeT>, new()
    {
        public GraphEdge()
        {
            Identity = -1;
            Node = null;
            Poly = null;
            Twin = null;
            Next = null;
            Prev = null;
        }

        public int Identity
        {
            get;
            internal set;
        }
        public NodeT Node
        {
            get;
            internal set;
        }
        public PolyT Poly
        {
            get;
            internal set;
        }
        public EdgeT Twin
        {
            get;
            internal set;
        }
        public EdgeT Next
        {
            get;
            internal set;
        }
        public EdgeT Prev
        {
            get;
            internal set;
        }
        internal virtual void LoadDual(EdgeT data, NodeT newNode, PolyT newPoly, EdgeT newTwin, EdgeT newPrev)
        {
            Identity = data.Identity;
            Node = newNode;
            Poly = newPoly;
            Twin = newTwin;
            Prev = newPrev;
            newPrev.Next = (EdgeT)this;
        }
        internal virtual void Serialize(BinaryWriter writer)
        {
            writer.Write(Node.Identity);
            writer.Write(Poly.Identity);
            writer.Write(Twin != null ? Twin.Identity : -1);
            writer.Write(Next != null ? Next.Identity : -1);
            writer.Write(Prev != null ? Prev.Identity : -1);
        }
        internal virtual void Deserialize(BinaryReader reader, Graph<PolyT,EdgeT,NodeT> graph)
        {
            Node = graph.Nodes[reader.ReadInt32()];
            Poly = graph.Polys[reader.ReadInt32()];
            int index = reader.ReadInt32();
            Twin = index >= 0 ? graph.Edges[index] : null;
            index = reader.ReadInt32();
            Next = index >= 0 ? graph.Edges[index] : null;
            index = reader.ReadInt32();
            Prev = index >= 0 ? graph.Edges[index] : null;
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