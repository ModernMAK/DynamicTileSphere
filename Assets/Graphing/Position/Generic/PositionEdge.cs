//using Graphing.Generic;
//#if UNITY
//using UnityEngine;
//#else
//using UnityEngineSub;
//#endif

//namespace Graphing.Position.Generic
//{
//    public class ObsoletePositionEdge<PolyT, EdgeT, NodeT>
//        : ObsoleteGraphEdge<PolyT, EdgeT, NodeT>
//        where PolyT : ObsoletePositionPoly<PolyT, EdgeT, NodeT>, new()
//        where EdgeT : ObsoletePositionEdge<PolyT, EdgeT, NodeT>, new()
//        where NodeT : ObsoletePositionNode<PolyT, EdgeT, NodeT>, new()

//    {
//        public ObsoletePositionEdge() : base()
//        {
//        }
//        public Vector3 CenterLerp
//        {
//            get
//            {
//                Vector3 center = Node.Position;
//                if (Next != null)
//                {
//                    center += Next.Node.Position;
//                    center /= 2f;
//                }
//                return center;
//            }
//        }
//        public Vector3 CenterSlerp
//        {
//            get
//            {
//                Vector3 center = Node.Position;
//                float mag = Node.Position.magnitude;
//                if (Next != null)
//                {
//                    center += Next.Node.Position;
//                    mag += Next.Node.Position.magnitude;
//                    mag /= 2f;
//                }
//                return center.normalized * mag;
//            }
//        }
//    }
//}