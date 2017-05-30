using Graphing.Generic;
public class PolygonCorner<PolyType, EdgeType, NodeType>
    where PolyType : GraphPoly<PolyType, EdgeType, NodeType>, new()
    where EdgeType : GraphEdge<PolyType, EdgeType, NodeType>, new()
    where NodeType : GraphNode<PolyType, EdgeType, NodeType>, new()
{
    public PolygonCorner(EdgeType edge)
    {
        Pivot = edge.Poly;
        Left = edge.Next.Twin.Poly;
        Right = edge.Next.Twin.Next.Twin.Poly;
    }
    public PolyType Pivot { get; private set; }
    public PolyType Left { get; private set; }
    public PolyType Right { get; private set; }
}
