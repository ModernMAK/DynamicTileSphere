using System;
using Graphing.Generic;
using Graphing.Generic.Generic;

namespace Graphing.Position
{
    //using Generic;
    //public class ObsoletePositionEdge
    //    : ObsoletePositionEdge<ObsoletePositionPoly, ObsoletePositionEdge, OsboletePositionNode>
    //{
    //    public ObsoletePositionEdge() : base()
    //    {
    //    }
    //}


    namespace Generic
    {
        public class PositionEdge<PolyT, EdgeT, NodeT> : IPositionEdge<PolyT, EdgeT, NodeT>
            where PolyT : class, IPositionPoly<PolyT, EdgeT, NodeT>
            where EdgeT : class, IPositionEdge<PolyT, EdgeT, NodeT>
            where NodeT : class, IPositionNode<PolyT, EdgeT, NodeT>
        {
            public NodeT Node
            {
                get;
                protected set;
            }
            public PolyT Poly
            {
                get;
                protected set;
            }
            public EdgeT Twin
            {
                get;
                protected set;
            }
            public EdgeT Next
            {
                get;
                protected set;
            }
            public EdgeT Prev
            {
                get;
                protected set;
            }

            IPositionNode IPositionEdge.Node
            {
                get
                {
                    return Node;
                }
            }
            IGraphNode IGraphEdge.Node
            {
                get
                {
                    return Node;
                }
            }

            IPositionPoly IPositionEdge.Poly
            {
                get
                {
                    return Poly;
                }
            }
            IGraphPoly IGraphEdge.Poly
            {
                get
                {
                    return Poly;
                }
            }

            IPositionEdge IPositionEdge.Twin
            {
                get
                {
                    return Twin;
                }
            }
            IGraphEdge IGraphEdge.Twin
            {
                get
                {
                    return Twin;
                }
            }

            IPositionEdge IPositionEdge.Next
            {
                get
                {
                    return Next;
                }
            }
            IGraphEdge IGraphEdge.Next
            {
                get
                {
                    return Next;
                }
            }

            IPositionEdge IPositionEdge.Prev
            {
                get
                {
                    return Prev;
                }
            }
            IGraphEdge IGraphEdge.Prev
            {
                get
                {
                    return Prev;
                }
            }
        }

        /// <summary>
        /// TODO proper summary.
        /// </summary>
        public interface IPositionEdge<PolyT, EdgeT, NodeT> : IPositionEdge, IGraphEdge<PolyT, EdgeT, NodeT>
            where PolyT : class, IPositionPoly<PolyT, EdgeT, NodeT>
            where EdgeT : class, IPositionEdge<PolyT, EdgeT, NodeT>
            where NodeT : class, IPositionNode<PolyT, EdgeT, NodeT>
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
    public class PositionEdge : IPositionEdge
    {
        public IPositionNode Node
        {
            get;
            protected set;
        }
        IGraphNode IGraphEdge.Node
        {
            get
            {
                return Node;
            }
        }

        public IPositionPoly Poly
        {
            get;
            protected set;
        }
        IGraphPoly IGraphEdge.Poly
        {
            get
            {
                return Poly;
            }
        }

        public IPositionEdge Twin
        {
            get;
            protected set;
        }
        IGraphEdge IGraphEdge.Twin
        {
            get
            {
                return Twin;
            }
        }

        public IPositionEdge Next
        {
            get;
            protected set;
        }
        IGraphEdge IGraphEdge.Next
        {
            get
            {
                return Next;
            }
        }

        public IPositionEdge Prev
        {
            get;
            protected set;
        }
        IGraphEdge IGraphEdge.Prev
        {
            get
            {
                return Prev;
            }
        }
    }
    /// <summary>
    /// TODO proper summary.
    /// </summary>
    public interface IPositionEdge : IGraphEdge
    {
        Vector3 Position
        {
            get;
        }

        /// <summary>
        /// The origin node of this edge.
        /// </summary>
        new IPositionNode Node
        {
            get;
        }
        /// <summary>
        /// The polygon this edge faces.
        /// </summary>
        new IPositionPoly Poly
        {
            get;
        }
        /// <summary>
        /// The 'Twin' of this edge, an edge that moves from the Next's Node to this Node
        /// </summary>
        new IPositionEdge Twin
        {
            get;
        }
        /// <summary>
        /// The 'Next' of this edge.
        /// TODO some more explaining.
        /// </summary>
        new IPositionEdge Next
        {
            get;
        }
        /// <summary>
        /// The 'Prev' of this edge.
        /// TODO some more explaining.
        /// </summary>
        new IPositionEdge Prev
        {
            get;
        }
    }


}

