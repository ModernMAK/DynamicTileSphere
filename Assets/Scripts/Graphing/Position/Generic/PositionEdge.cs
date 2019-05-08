using Graphing.Generic;
using UnityEngine;

namespace Graphing.Position.Generic
{
    public class PositionEdge<PolyT, EdgeT, NodeT>
        : GraphEdge<PolyT, EdgeT, NodeT>
        where PolyT : PositionPoly<PolyT, EdgeT, NodeT>, new()
        where EdgeT : PositionEdge<PolyT, EdgeT, NodeT>, new()
        where NodeT : PositionNode<PolyT, EdgeT, NodeT>, new()

    {
        public Vector3 CenterLerp
        {
            get
            {
                var center = Node.Position;
                if (Next != null)
                {
                    center += Next.Node.Position;
                    center /= 2f;
                }

                return center;
            }
        }

        public Vector3 CenterSlerp
        {
            get
            {
                var center = Node.Position;
                var mag = Node.Position.magnitude;
                if (Next != null)
                {
                    center += Next.Node.Position;
                    mag += Next.Node.Position.magnitude;
                    mag /= 2f;
                }

                return center.normalized * mag;
            }
        }
    }
}