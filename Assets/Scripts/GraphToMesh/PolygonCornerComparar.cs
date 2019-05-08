using System.Collections.Generic;
using Graphing.Generic;

namespace GraphToMesh
{
    public class
        PolygonCornerComparar<PolyType, EdgeType, NodeType> : IEqualityComparer<PolygonCorner<PolyType, EdgeType, NodeType>>
        where PolyType : GraphPoly<PolyType, EdgeType, NodeType>, new()
        where EdgeType : GraphEdge<PolyType, EdgeType, NodeType>, new()
        where NodeType : GraphNode<PolyType, EdgeType, NodeType>, new()
    {
        public bool Equals(PolygonCorner<PolyType, EdgeType, NodeType> x, PolygonCorner<PolyType, EdgeType, NodeType> y)
        {
            return
                x.Pivot == y.Pivot && x.Left == y.Left && x.Right == y.Right ||
                x.Pivot == y.Left && x.Left == y.Right && x.Right == x.Pivot ||
                x.Pivot == y.Right && x.Left == y.Pivot && x.Right == x.Left;
        }

        public int GetHashCode(PolygonCorner<PolyType, EdgeType, NodeType> obj)
        {
            var hCode = obj.Pivot.GetHashCode() ^ obj.Left.GetHashCode() ^ obj.Right.GetHashCode();
            return hCode.GetHashCode();
        }
    }
}