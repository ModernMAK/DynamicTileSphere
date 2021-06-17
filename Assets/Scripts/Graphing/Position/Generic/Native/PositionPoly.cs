using Graphing.Generic.Native;
using Unity.Mathematics;
using UnityEngine;

namespace Graphing.Position.Generic.Native
{
    public static class PositionPoly
	{
        public static float3 CalculateCenter<TPoly, TEdge, TVertex, NodeDataT>(Graph<TPoly, TEdge, TVertex> graph, IPolygon poly, bool slerp = false)
            where TPoly : struct, IPolygon
            where TEdge : struct, IEdge
            where TVertex : struct, IVertexData<NodeDataT>
            where NodeDataT : struct, IPositionData
        {

            var center = float3.zero;
            var counter = 0;
            var mag = 0f;

            foreach (var v in graph.WalkPolygonVertexes(poly))
			{
                center += v.Data.Position;
                counter++;

                if (slerp)
                    mag += math.length(v.Data.Position);
		    }

            if (counter == 0)
                return float3.zero;
            
            if (!slerp) 
                return center / counter;

            // Slerp = True
            center /= counter; // May be irrelevent since we normalize the vector
            mag /= counter;
            center = math.normalize(center);
            return center * mag;

        }
    }

    //public class PositionPoly<PolyT, EdgeT, NodeT>
    //    : GraphPoly<PolyT, EdgeT, NodeT>
    //    where PolyT : PositionPoly<PolyT, EdgeT, NodeT>, new()
    //    where EdgeT : PositionEdge<PolyT, EdgeT, NodeT>, new()
    //    where NodeT : PositionNode<PolyT, EdgeT, NodeT>, new()
    //{
    //    public Vector3 CenterLerp
    //    {
    //        get
    //        {
    //            var dir = Vector3.zero;
    //            var counter = 0;
    //            foreach (var edge in this)
    //            {
    //                if (edge == null)
    //                    Debug.Log("EdgeNull");
    //                if (edge.Node == null)
    //                    Debug.Log("EdgeNodeNull");
    //                dir += edge.Node.Position;
    //                counter++;
    //            }

    //            return dir / (counter != 0 ? counter : 1);
    //        }
    //    }

    //    public Vector3 CenterSlerp
    //    {
    //        get
    //        {
    //            var dir = Vector3.zero;
    //            var mag = 0f;
    //            var counter = 0;
    //            foreach (var edge in this)
    //            {
    //                dir += edge.Node.Position;
    //                mag += edge.Node.Position.magnitude;
    //                counter++;
    //            }

    //            return dir.normalized * mag / (counter != 0 ? counter : 1);
    //        }
    //    }
    //}
}