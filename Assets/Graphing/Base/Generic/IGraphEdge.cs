namespace Graphing.Generic
{
    ////Technically a Half Edge, which is a Directed Edge, but whatever.
    //public class ObsoleteGraphEdge<PolyT, EdgeT, NodeT>
    //    where NodeT : ObsoleteGraphNode<PolyT, EdgeT, NodeT>, new()
    //    where EdgeT : ObsoleteGraphEdge<PolyT, EdgeT, NodeT>, new()
    //    where PolyT : ObsoleteGraphPoly<PolyT, EdgeT, NodeT>, new()
    //{
    //    public ObsoleteGraphEdge()
    //    {
    //        Identity = -1;
    //        Node = null;
    //        Poly = null;
    //        Twin = null;
    //        Next = null;
    //        Prev = null;
    //    }

    //    public int Identity
    //    {
    //        get;
    //        internal set;
    //    }
    //    public NodeT Node
    //    {
    //        get;
    //        internal set;
    //    }
    //    public PolyT Poly
    //    {
    //        get;
    //        internal set;
    //    }
    //    public EdgeT Twin
    //    {
    //        get;
    //        internal set;
    //    }
    //    public EdgeT Next
    //    {
    //        get;
    //        internal set;
    //    }
    //    public EdgeT Prev
    //    {
    //        get;
    //        internal set;
    //    }
    //    internal virtual void LoadDual(EdgeT data, NodeT newNode, PolyT newPoly, EdgeT newTwin, EdgeT newPrev)
    //    {
    //        Identity = data.Identity;
    //        Node = newNode;
    //        Poly = newPoly;
    //        Twin = newTwin;
    //        Prev = newPrev;
    //        newPrev.Next = (EdgeT)this;
    //    }
    //    internal virtual void Serialize(BinaryWriter writer)
    //    {
    //        writer.Write(Node.Identity);
    //        writer.Write(Poly.Identity);
    //        writer.Write(Twin != null ? Twin.Identity : -1);
    //        writer.Write(Next != null ? Next.Identity : -1);
    //        writer.Write(Prev != null ? Prev.Identity : -1);
    //    }
    //    internal virtual void Deserialize(BinaryReader reader, ObsoleteGraph<PolyT, EdgeT, NodeT> graph)
    //    {
    //        Node = graph.Nodes[reader.ReadInt32()];
    //        Poly = graph.Polys[reader.ReadInt32()];
    //        int index = reader.ReadInt32();
    //        Twin = index >= 0 ? graph.Edges[index] : null;
    //        index = reader.ReadInt32();
    //        Next = index >= 0 ? graph.Edges[index] : null;
    //        index = reader.ReadInt32();
    //        Prev = index >= 0 ? graph.Edges[index] : null;
    //    }
    //}


    /// <summary>
    /// TODO proper summary.
    /// </summary>
    public interface IGraphEdge<PolyT, EdgeT, NodeT> : IGraphEdge
    where PolyT : class, IGraphPoly<PolyT, EdgeT, NodeT>
    where EdgeT : class, IGraphEdge<PolyT, EdgeT, NodeT>
    where NodeT : class, IGraphNode<PolyT, EdgeT, NodeT>
    {
        /// <summary>
        /// The origin node of this edge.
        /// </summary>
        new NodeT Node
        {
            get;
        }
        /// <summary>
        /// The polygon this edge faces.
        /// </summary>
        new PolyT Poly
        {
            get;
        }
        /// <summary>
        /// The 'Twin' of this edge, an edge that moves from the Next's Node to this Node
        /// </summary>
        new EdgeT Twin
        {
            get;
        }
        /// <summary>
        /// The 'Next' of this edge.
        /// TODO some more explaining.
        /// </summary>
        new EdgeT Next
        {
            get;
        }
        /// <summary>
        /// The 'Prev' of this edge.
        /// TODO some more explaining.
        /// </summary>
        new EdgeT Prev
        {
            get;
        }
    }
}

