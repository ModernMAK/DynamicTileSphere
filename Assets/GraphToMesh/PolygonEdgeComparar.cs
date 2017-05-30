using Graphing.Generic;
using System.Collections.Generic;
public class PolygonEdgeComparar<PolyType, EdgeType, NodeType> : IEqualityComparer<PolygonEdge<PolyType, EdgeType, NodeType>>
    where PolyType : GraphPoly<PolyType, EdgeType, NodeType>, new()
    where EdgeType : GraphEdge<PolyType, EdgeType, NodeType>, new()
    where NodeType : GraphNode<PolyType, EdgeType, NodeType>, new()
{
    public bool Equals(PolygonEdge<PolyType, EdgeType, NodeType> x, PolygonEdge<PolyType, EdgeType, NodeType> y)
    {
        return
            (x.Edge == y.Edge && x.Twin == y.Twin) ||
            (x.Edge == y.Twin && x.Twin == y.Edge);
    }

    public int GetHashCode(PolygonEdge<PolyType, EdgeType, NodeType> obj)
    {
        int hCode = obj.Edge.GetHashCode() ^ obj.Twin.GetHashCode();
        return hCode.GetHashCode();
    }
}
