using Graphing.Generic.Native;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Graphing.Position.Generic.Native
{
    public interface IPositionData
	{
        float3 Position { get; set; }
	}
    public struct PositionData : IPositionData
	{
        public float3 Position { get; set; }
	}
    public static class PositionEdge
    {
        private static readonly float3 up = new float3(0, 1, 0);
        private const float midpoint = 0.5f;
        public static float3 CalculateCenter(IPositionData self, IPositionData next, bool slerp) 
        {
   //         var selfNull = self == null;
   //         var nextNull = next == null;


   //         if (selfNull && nextNull)
   //             throw new NotImplementedException();
   //         else if(selfNull ^ nextNull)
			//{
   //             if (!selfNull)
   //                 return self.Position;
   //             else
   //                 return next.Position;
			//}
			//else 
   //         {
            if (slerp)
            {
                throw new NotImplementedException();
                //var magLerp = math.lerp(math.length(self.Position), math.length(next.Position), midpoint);
                //var start = quaternion.LookRotation(self.Position, up);
                //var end = quaternion.LookRotation(next.Position, up);
                //var angleLerp = math.slerp(start, end, midpoint);
                //var result = math.slerp(selfQ, nextQ, midpoint);
                //result
            }
            else
                return math.lerp(self.Position, next.Position, midpoint);
            //}
        }

		public static float3 CalculateCenter<TPoly,TEdge,TVertex, NodeDataT>(Graph<TPoly, TEdge, TVertex> graph, IEdge edge, bool slerp = false)
            where TPoly : struct, IPolygon
            where TEdge : struct, IEdge
            where TVertex : struct, IVertexData<NodeDataT>
            where NodeDataT : struct, IPositionData
        {
            var next = graph.Edges[edge.Next];
            var edgeNode = graph.Nodes[edge.Node];
            var nextNode = graph.Nodes[next.Node];
            return CalculateCenter(edgeNode.Data, nextNode.Data, slerp);
        }
    }
}