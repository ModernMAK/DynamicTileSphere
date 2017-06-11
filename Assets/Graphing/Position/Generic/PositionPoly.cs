//using Graphing.Generic;
//using UnityEngine;
//namespace Graphing.Position.Generic
//{
//    public class ObsoletePositionPoly<PolyT, EdgeT, NodeT>
//        : ObsoleteGraphPoly<PolyT, EdgeT, NodeT>
//        where PolyT : ObsoletePositionPoly<PolyT, EdgeT, NodeT>, new()
//        where EdgeT : ObsoletePositionEdge<PolyT, EdgeT, NodeT>, new()
//        where NodeT : PositionNode<PolyT, EdgeT, NodeT>, new()
//    {
//        public ObsoletePositionPoly() : base()
//        {
//        }
//        public Vector3 CenterLerp
//        {
//            get
//            {
//                Vector3 dir = Vector3.zero;
//                int counter = 0;
//                foreach (EdgeT edge in this)
//                {
//                    if (edge == null)
//                        Debug.Log("EdgeNull");
//                    if (edge.Node == null)
//                        Debug.Log("EdgeNodeNull");
//                    dir += edge.Node.Position;
//                    counter++;
//                }
//                return dir / (counter != 0 ? counter : 1);
//            }
//        }
//        public Vector3 CenterSlerp
//        {
//            get
//            {
//                Vector3 dir = Vector3.zero;
//                float mag = 0f;
//                int counter = 0;
//                foreach (EdgeT edge in this)
//                {
//                    dir += edge.Node.Position;
//                    mag += edge.Node.Position.magnitude;
//                    counter++;
//                }
//                return dir.normalized * mag / (counter != 0 ? counter : 1);
//            }
//        }
//    }
//}