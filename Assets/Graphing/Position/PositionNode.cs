namespace Graphing.Position
{
    //using System;
    //using Generic;
    using Graphing.Generic;
    using Graphing.Generic.Generic;
    using UnityEngineSub;

    //public class OsboletePositionNode
    //    : PositionNode<ObsoletePositionPoly, ObsoletePositionEdge, OsboletePositionNode>
    //{
    //    public OsboletePositionNode() : base()
    //    {
    //    }
    //}
    public class PositionNode : IPositionNode
    {
        public IPositionEdge Edge
        {
            get;
            protected set;
        }

        IGraphEdge IGraphNode.Edge
        {
            get
            {
                return Edge;
            }
        }
        public Vector3 Position
        {
            get;
            protected set;
        }
    }

    /// <summary>
    /// A node within a graph.
    /// </summary>
    public interface IPositionNode : IGraphNode
    {
        /// <summary>
        /// An edge pointing away from this node (the edge's Node is this node).
        /// TODO Explain some more
        /// </summary>
        new IPositionEdge Edge
        {
            get;
        }
        Vector3 Position
        {
            get;
        }
    }

    namespace Generic
    {
        public class PositionNode<PolyT, EdgeT, NodeT> : IPositionNode<PolyT, EdgeT, NodeT>
            where PolyT : class, IPositionPoly<PolyT, EdgeT, NodeT>
            where EdgeT : class, IPositionEdge<PolyT, EdgeT, NodeT>
            where NodeT : class, IPositionNode<PolyT, EdgeT, NodeT>
        {
            public EdgeT Edge
            {
                get;
                protected set;
            }

            public Vector3 Position
            {
                get;
                protected set;
            }

            IPositionEdge IPositionNode.Edge
            {
                get
                {
                    return Edge;
                }
            }
            IGraphEdge IGraphNode.Edge
            {
                get
                {
                    return Edge;
                }
            }
        }


        /// <summary>
        /// A node within a graph.
        /// </summary>
        /// <typeparam name="PolyT">The Typesafe Poly</typeparam>
        /// <typeparam name="EdgeT">The Typesafe Edge</typeparam>
        /// <typeparam name="NodeT">This</typeparam>
        public interface IPositionNode<PolyT, EdgeT, NodeT> : IPositionNode, IGraphNode<PolyT, EdgeT, NodeT>
            where PolyT : class, IPositionPoly<PolyT, EdgeT, NodeT>
            where EdgeT : class, IPositionEdge<PolyT, EdgeT, NodeT>
            where NodeT : class, IPositionNode<PolyT, EdgeT, NodeT>
        {
            /// <summary>
            /// An edge pointing away from this node (the edge's Node is this node).
            /// TODO Explain some more
            /// </summary>
            new EdgeT Edge
            {
                get;
            }

        }
    }
}