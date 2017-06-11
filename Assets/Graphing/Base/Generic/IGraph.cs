using System.Collections.Generic;


namespace Graphing.Generic
{
    public interface IGraph<PolyT, EdgeT, NodeT> : IGraph
        where PolyT : class, IGraphPoly<PolyT, EdgeT, NodeT>
        where EdgeT : class, IGraphEdge<PolyT, EdgeT, NodeT>
        where NodeT : class, IGraphNode<PolyT, EdgeT, NodeT>
    {
        /// <summary>
        /// The Nodes within the graph
        /// </summary>
        new IEnumerable<NodeT> Nodes
        {
            get;
        }
        /// <summary>
        /// The Edges within the graph
        /// </summary>
        new IEnumerable<EdgeT> Edges
        {
            get;
        }
        /// <summary>
        /// The Polygons within the graph
        /// </summary>
        new IEnumerable<PolyT> Polygons
        {
            get;
        }
    }
}

//public class ObsoleteGraph<PolyT, EdgeT, NodeT>
//    where NodeT : ObsoleteGraphNode<PolyT, EdgeT, NodeT>, new()
//    where EdgeT : ObsoleteGraphEdge<PolyT, EdgeT, NodeT>, new()
//    where PolyT : ObsoleteGraphPoly<PolyT, EdgeT, NodeT>, new()
//{
//    public ObsoleteGraph(int nodeCapacity = 0, int edgeCapacity = 0, int polyCapacity = 0)
//    {
//        Nodes = new List<NodeT>(nodeCapacity);
//        Edges = new List<EdgeT>(edgeCapacity);
//        Polys = new List<PolyT>(polyCapacity);
//    }
//    public List<NodeT> Nodes
//    {
//        get;
//        internal set;
//    }
//    public List<EdgeT> Edges
//    {
//        get;
//        internal set;
//    }
//    public List<PolyT> Polys
//    {
//        get;
//        internal set;
//    }
//    public GraphT Dual<GraphT>() where GraphT : ObsoleteGraph<PolyT, EdgeT, NodeT>, new()
//    {
//        GraphT dual = new GraphT();
//        dual.LoadDual(this);
//        return dual;
//    }
//    internal virtual void Serialize(BinaryWriter writer)
//    {
//        writer.Write(Nodes.Count);
//        writer.Write(Edges.Count);
//        writer.Write(Polys.Count);

//        foreach (var node in Nodes)
//            node.Serialize(writer);

//        foreach (var edge in Edges)
//            edge.Serialize(writer);

//        foreach (var poly in Polys)
//            poly.Serialize(writer);
//    }
//    internal virtual void Deserialize(BinaryReader reader)
//    {
//        Nodes.Clear();
//        int nodes = reader.ReadInt32();
//        int edges = reader.ReadInt32();
//        int polys = reader.ReadInt32();

//        for (int i = 0; i < nodes; i++)
//            Nodes.Add(new NodeT() { Identity = i });
//        for (int i = 0; i < edges; i++)
//            Edges.Add(new EdgeT() { Identity = i });
//        for (int i = 0; i < polys; i++)
//            Polys.Add(new PolyT() { Identity = i });

//        foreach (var node in Nodes)
//            node.Deserialize(reader, this);
//        foreach (var edge in Edges)
//            edge.Deserialize(reader, this);
//        foreach (var poly in Polys)
//            poly.Deserialize(reader, this);
//    }
//    internal virtual void LoadDual<GraphT>(GraphT data) where GraphT : ObsoleteGraph<PolyT, EdgeT, NodeT>
//    {
//        Dictionary<PolyT, NodeT> PolyDualLookup = new Dictionary<PolyT, NodeT>();
//        Dictionary<EdgeT, EdgeT> EdgeDualLookup = new Dictionary<EdgeT, EdgeT>();
//        Dictionary<NodeT, PolyT> NodeDualLookup = new Dictionary<NodeT, PolyT>();

//        Nodes.Clear();
//        Edges.Clear();
//        Polys.Clear();

//        foreach (PolyT poly in data.Polys)
//        {
//            Nodes.Add(PolyDualLookup[poly] = new NodeT());
//        }
//        foreach (EdgeT edge in data.Edges)
//        {
//            Edges.Add(EdgeDualLookup[edge] = new EdgeT());
//        }
//        foreach (NodeT node in data.Nodes)
//        {
//            Polys.Add(NodeDualLookup[node] = new PolyT());
//        }


//        foreach (PolyT poly in data.Polys)
//            PolyDualLookup[poly].LoadDual(poly, EdgeDualLookup[poly.Edge]);

//        foreach (NodeT node in data.Nodes)
//            NodeDualLookup[node].LoadDual(node, EdgeDualLookup[node.Edge]);

//        foreach (EdgeT edge in data.Edges)
//        {
//            EdgeT dualEdge = EdgeDualLookup[edge];// EdgeDualLookup[edge];

//            //From my VD_DEdge (Which is saved under Assets/ProceduralGraph/Graph in my first commit of github (since I will have gotten rid of it eventually))
//            /*
//            Dual_Edge = Dual(Edge)
//            Dual_Next = Dual(Edge.Twin.Next)
//            Dual_Previous = Dual(Edge.Previous.Twin)
//            Dual_Twin  = Dual(Edge.Twin)
//            */
//            dualEdge.LoadDual(
//            edge,
//            PolyDualLookup[edge.Poly],
//            NodeDualLookup[edge.Node],
//            edge.Twin != null ? EdgeDualLookup[edge.Twin] : null,
//            (edge.Twin != null && edge.Twin.Next != null) ? EdgeDualLookup[edge.Twin.Next] : null
//            //(edge.Prev != null && edge.Prev.Twin != null) ? EdgeDualLookup[edge.Prev.Twin] : null
//            );
//        }
//        Nodes.Sort((x, y) => { return x.Identity.CompareTo(y.Identity); });
//        Edges.Sort((x, y) => { return x.Identity.CompareTo(y.Identity); });
//        Polys.Sort((x, y) => { return x.Identity.CompareTo(y.Identity); });
//    }
//}

