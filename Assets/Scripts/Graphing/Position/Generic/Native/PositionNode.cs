//using System.IO;
//using Graphing.Generic;
//using UnityEngine;

//namespace Graphing.Position.Generic.Native
//{
//	public class PositionNode<PolyT, EdgeT, NodeT>
//        : GraphNode<PolyT, EdgeT, NodeT>
//        where PolyT : PositionPoly<PolyT, EdgeT, NodeT>, new()
//        where EdgeT : PositionEdge<PolyT, EdgeT, NodeT>, new()
//        where NodeT : PositionNode<PolyT, EdgeT, NodeT>, new()
//    {
//        public Vector3 Position { get; internal set; }

//        internal override void Serialize(BinaryWriter writer)
//        {
//            base.Serialize(writer);
//            writer.Write(Position.x);
//            writer.Write(Position.y);
//            writer.Write(Position.z);
//        }

//        internal override void Deserialize(BinaryReader reader, Graph<PolyT, EdgeT, NodeT> graph)
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