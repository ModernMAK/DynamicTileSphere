using System.IO;

namespace Graphing
{
    //public class ObsoleteGraphNode<PolyT, EdgeT, NodeT>
    //    where NodeT : ObsoleteGraphNode<PolyT, EdgeT, NodeT>, new()
    //    where EdgeT : ObsoleteGraphEdge<PolyT, EdgeT, NodeT>, new()
    //    where PolyT : ObsoleteGraphPoly<PolyT, EdgeT, NodeT>, new()
    //{
    //    public ObsoleteGraphNode()
    //    {
    //        Identity = -1;
    //        Edge = null;
    //    }
    //    public int Identity
    //    {
    //        get;
    //        internal set;
    //    }
    //    public EdgeT Edge
    //    {
    //        get;
    //        internal set;
    //    }
    //    internal virtual void LoadDual(PolyT data, EdgeT newEdge)
    //    {
    //        Identity = data.Identity;
    //        Edge = newEdge;
    //    }
    //    internal virtual void Serialize(BinaryWriter writer)
    //    {
    //        writer.Write(Edge.Identity);
    //    }
    //    internal virtual void Deserialize(BinaryReader reader, ObsoleteGraph<PolyT, EdgeT, NodeT> graph)
    //    {
    //        Edge = graph.Edges[reader.ReadInt32()];
    //    }
    //}

    public class GraphNode : IGraphNode
    {
        public IGraphEdge Edge
        {
            get;
            protected set;
        }
    }

    /// <summary>
    /// A node within a graph.
    /// </summary>
    public interface IGraphNode
    {
        /// <summary>
        /// An edge pointing away from this node (the edge's Node is this node).
        /// TODO Explain some more
        /// </summary>
        IGraphEdge Edge
        {
            get;
        }
    }

    namespace Generic
    {
        public class GraphNode<PolyT, EdgeT, NodeT> : IGraphNode<PolyT, EdgeT, NodeT>
            where PolyT : class, IGraphPoly<PolyT, EdgeT, NodeT>
            where EdgeT : class, IGraphEdge<PolyT, EdgeT, NodeT>
            where NodeT : class, IGraphNode<PolyT, EdgeT, NodeT>
        {
            public EdgeT Edge
            {
                get;
                protected set;
            }
        }


        /// <summary>
        /// A node within a graph.
        /// </summary>
        /// <typeparam name="PolyT">The Typesafe Poly</typeparam>
        /// <typeparam name="EdgeT">The Typesafe Edge</typeparam>
        /// <typeparam name="NodeT">This</typeparam>
        public interface IGraphNode<PolyT, EdgeT, NodeT>
            where PolyT : class, IGraphPoly<PolyT, EdgeT, NodeT>
            where EdgeT : class, IGraphEdge<PolyT, EdgeT, NodeT>
            where NodeT : class, IGraphNode<PolyT, EdgeT, NodeT>
        {
            /// <summary>
            /// An edge pointing away from this node (the edge's Node is this node).
            /// TODO Explain some more
            /// </summary>
            EdgeT Edge
            {
                get;
            }
        }
    }
}