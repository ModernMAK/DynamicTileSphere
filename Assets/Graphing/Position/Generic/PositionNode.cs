//using System.IO;
//using Graphing.Generic;
//using UnityEngine;

//namespace Graphing.Position.Generic
//{
//    public class ObsoletePositionNode<PolyT, EdgeT, NodeT>
//        : ObsoleteGraphNode<PolyT, EdgeT, NodeT>
//        where PolyT : ObsoletePositionPoly<PolyT, EdgeT, NodeT>, new()
//        where EdgeT : ObsoletePositionEdge<PolyT, EdgeT, NodeT>, new()
//        where NodeT : ObsoletePositionNode<PolyT, EdgeT, NodeT>, new()
//    {

//        public ObsoletePositionNode() : base()
//        {
//        }
//        public Vector3 Position
//        {
//            get; internal set;
//        }
//        internal override void Serialize(BinaryWriter writer)
//        {
//            base.Serialize(writer);
//            writer.Write(Position.x);
//            writer.Write(Position.y);
//            writer.Write(Position.z);
//        }
//        internal override void Deserialize(BinaryReader reader, ObsoleteGraph<PolyT, EdgeT, NodeT> graph)
//        {
//            base.Deserialize(reader, graph);
//            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

//        }
//        internal override void LoadDual(PolyT data, EdgeT newEdge)
//        {
//            base.LoadDual(data, newEdge);
//            Position = data.CenterSlerp;
//        }
//    }
//}