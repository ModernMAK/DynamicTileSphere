//using Positioning.Generic;
//using Positioning.Generic.Generic;
using Graphing.Generic.Generic;
using System.Collections.Generic;
using Graphing.Generic;
using System;
using System.Linq;

namespace Graphing.Position
{
    //using Generic;
    //public class ObsoletePositionPosition
    //    : PositionPosition<ObsoletePositionPoly, ObsoletePositionEdge, OsboletePositionNode>
    //{
    //    public ObsoletePositionPosition() : base()
    //    {

    //    }
    //    public ObsoletePositionPosition(int nodeCapacity, int edgeCapacity, int polyCapacity) : base(nodeCapacity, edgeCapacity, polyCapacity)
    //    {
    //    }
    //}



    namespace Generic
    {

        public class PositionGraph<PolyT, EdgeT, NodeT> : IPositionGraph<PolyT, EdgeT, NodeT>
            where NodeT : class, IPositionNode<PolyT, EdgeT, NodeT>
            where EdgeT : class, IPositionEdge<PolyT, EdgeT, NodeT>
            where PolyT : class, IPositionPoly<PolyT, EdgeT, NodeT>
        {
            public List<NodeT> NodeList { get; protected set; }
            public List<EdgeT> EdgeList { get; protected set; }
            public List<PolyT> PolygonList { get; protected set; }

            public IEnumerable<NodeT> Nodes
            {
                get
                {
                    return NodeList;
                }
            }
            public IEnumerable<EdgeT> Edges
            {
                get
                {
                    return EdgeList;
                }
            }
            public IEnumerable<PolyT> Polygons
            {
                get
                {
                    return PolygonList;
                }
            }

            IEnumerable<IPositionNode> IPositionGraph.Nodes
            {
                get
                {
                    return Nodes;
                }
            }
            IEnumerable<IGraphNode> IGraph.Nodes
            {
                get
                {
                    return Nodes;
                }
            }

            IEnumerable<IPositionEdge> IPositionGraph.Edges
            {
                get
                {
                    return Edges;
                }
            }
            IEnumerable<IGraphEdge> IGraph.Edges
            {
                get
                {
                    return Edges;
                }
            }

            IEnumerable<IPositionPoly> IPositionGraph.Polygons
            {
                get
                {
                    return Polygons;
                }
            }
            IEnumerable<IGraphPoly> IGraph.Polygons
            {
                get
                {
                    return Polygons;
                }
            }
        }

        public interface IPositionGraph<PolyT, EdgeT, NodeT> : IPositionGraph, IGraph<PolyT, EdgeT, NodeT>
            where PolyT : class, IPositionPoly<PolyT, EdgeT, NodeT>
            where EdgeT : class, IPositionEdge<PolyT, EdgeT, NodeT>
            where NodeT : class, IPositionNode<PolyT, EdgeT, NodeT>
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
    public class PositionGraph : IPositionGraph
    {
        public List<IPositionNode> NodeList { get; protected set; }
        public List<IPositionEdge> EdgeList { get; protected set; }
        public List<IPositionPoly> PolygonList { get; protected set; }

        public IEnumerable<IPositionNode> Nodes
        {
            get
            {
                return NodeList;
            }
        }
        IEnumerable<IGraphNode> IGraph.Nodes
        {
            get
            {
                return NodeList;
            }
        }

        public IEnumerable<IPositionEdge> Edges
        {
            get
            {
                return EdgeList;
            }
        }
        IEnumerable<IGraphEdge> IGraph.Edges
        {
            get
            {
                return EdgeList;
            }
        }
        public IEnumerable<IPositionPoly> Polygons
        {
            get
            {
                return PolygonList;
            }
        }
        IEnumerable<IGraphPoly> IGraph.Polygons
        {
            get
            {
                return PolygonList;
            }
        }


    }

    public interface IPositionGraph : IGraph
    {
        /// <summary>
        /// The Nodes within the graph
        /// </summary>
        new IEnumerable<IPositionNode> Nodes
        {
            get;
        }
        /// <summary>
        /// The Edges within the graph
        /// </summary>
        new IEnumerable<IPositionEdge> Edges
        {
            get;
        }
        /// <summary>
        /// The Polygons within the graph
        /// </summary>
        new IEnumerable<IPositionPoly> Polygons
        {
            get;
        }
    }
}