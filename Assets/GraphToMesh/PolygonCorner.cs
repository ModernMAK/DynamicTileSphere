using Graphing.Generic;
public class PolygonCorner<PolyType, EdgeType, NodeType>
    where PolyType : ObsoleteGraphPoly<PolyType, EdgeType, NodeType>, new()
    where EdgeType : ObsoleteGraphEdge<PolyType, EdgeType, NodeType>, new()
    where NodeType : ObsoleteGraphNode<PolyType, EdgeType, NodeType>, new()
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
