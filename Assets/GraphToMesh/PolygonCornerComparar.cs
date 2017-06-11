﻿using Graphing.Generic;
using System.Collections.Generic;
public class PolygonCornerComparar<PolyType, EdgeType, NodeType> : IEqualityComparer<PolygonCorner<PolyType, EdgeType, NodeType>>
    where PolyType : ObsoleteGraphPoly<PolyType, EdgeType, NodeType>, new()
    where EdgeType : ObsoleteGraphEdge<PolyType, EdgeType, NodeType>, new()
    where NodeType : ObsoleteGraphNode<PolyType, EdgeType, NodeType>, new()
{
    public bool Equals(PolygonCorner<PolyType, EdgeType, NodeType> x, PolygonCorner<PolyType, EdgeType, NodeType> y)
    {
        return
            (x.Pivot == y.Pivot && x.Left == y.Left && x.Right == y.Right) ||
            (x.Pivot == y.Left && x.Left == y.Right && x.Right == x.Pivot) ||
            (x.Pivot == y.Right && x.Left == y.Pivot && x.Right == x.Left);
    }

    public int GetHashCode(PolygonCorner<PolyType, EdgeType, NodeType> obj)
    {
        int hCode = obj.Pivot.GetHashCode() ^ obj.Left.GetHashCode() ^ obj.Right.GetHashCode();
        return hCode.GetHashCode();
    }
}