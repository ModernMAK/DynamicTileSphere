using Graphing.Generic;
using UnityEngine;

namespace Graphing.Position.Generic
{
    public class PositionPoly<PolyT, EdgeT, NodeT>
        : GraphPoly<PolyT, EdgeT, NodeT>
        where PolyT : PositionPoly<PolyT, EdgeT, NodeT>, new()
        where EdgeT : PositionEdge<PolyT, EdgeT, NodeT>, new()
        where NodeT : PositionNode<PolyT, EdgeT, NodeT>, new()
    {
        public Vector3 CenterLerp
        {
            get
            {
                var dir = Vector3.zero;
                var counter = 0;
                foreach (var edge in this)
                {
                    if (edge == null)
                        Debug.Log("EdgeNull");
                    if (edge.Node == null)
                        Debug.Log("EdgeNodeNull");
                    dir += edge.Node.Position;
                    counter++;
                }

                return dir / (counter != 0 ? counter : 1);
            }
        }

        public Vector3 CenterSlerp
        {
            get
            {
                var dir = Vector3.zero;
                var mag = 0f;
                var counter = 0;
                foreach (var edge in this)
                {
                    dir += edge.Node.Position;
                    mag += edge.Node.Position.magnitude;
                    counter++;
                }

                return dir.normalized * mag / (counter != 0 ? counter : 1);
            }
        }
    }
}