using System;
using Graphing.Generic;
using Graphing.Generic;
using UnityEngineSub;

namespace Graphing.Position
{
    //using Graphing.Generic.Generic;

    //public class ObsoletePositionPoly
    //    : GraphPoly<ObsoletePositionPoly, ObsoletePositionEdge, PositionNode>
    //{
    //    public ObsoletePositionPoly() : base()
    //    {
    //    }
    //}


    public class PositionPoly : IPositionPoly
    {
        public IPositionEdge Edge
        {
            get;
            protected set;
        }

        public Vector3 CenterLerped
        {
            get
            {
                Vector3 center = default(Vector3);//#Todo In UnityEngineSub impliment Vector3.zero
                int count = 0;
                foreach (var edge in this)
                {
                    center += edge.Position;
                }

            }
        }

        public Vector3 CenterSlerped => throw new NotImplementedException();

        IGraphEdge IGraphPoly.Edge
        {
            get { return Edge; }
        }
    }

    /// <summary>
    /// A polygon within a Position. Does not have to be closed, but the polygon's edge must be the start edge (previous is null) and the end edge must have a twin (to include the last node).
    /// </summary>
    public interface IPositionPoly : IGraphPoly
    {
        /// <summary>
        /// An interior Half edge of the polygon.
        /// This should be the first Half edge if the polygon is not closed.
        /// </summary>
        new IPositionEdge Edge
        {
            get;
        }
        Vector3 CenterLerped
        {
            get;
        }
        Vector3 CenterSlerped
        {
            get;
        }
    }
    namespace Generic
    {
        public class PositionPoly<PolyT, EdgeT, NodeT> : GraphPoly<PolyT,EdgeT,NodeT>, IPositionPoly<PolyT, EdgeT, NodeT>
            where PolyT : class, IPositionPoly<PolyT, EdgeT, NodeT>
            where EdgeT : class, IPositionEdge<PolyT, EdgeT, NodeT>
            where NodeT : class, IPositionNode<PolyT, EdgeT, NodeT>
        {
            
        }

        /// <summary>
        /// A polygon within a Position. Does not have to be closed, but the polygon's edge must be the start edge (previous is null) and the end edge must have a twin (to include the last node).
        /// </summary>
        /// <typeparam name="PolyT">This</typeparam>
        /// <typeparam name="EdgeT">The Typesafe Edge</typeparam>
        /// <typeparam name="NodeT">The Typesafe Node</typeparam>
        public interface IPositionPoly<PolyT, EdgeT, NodeT> : IGraphPoly<PolyT, EdgeT, NodeT>
            where PolyT : class, IPositionPoly<PolyT, EdgeT, NodeT>
            where EdgeT : class, IPositionEdge<PolyT, EdgeT, NodeT>
            where NodeT : class, IPositionNode<PolyT, EdgeT, NodeT>
        {
        }
    }
}