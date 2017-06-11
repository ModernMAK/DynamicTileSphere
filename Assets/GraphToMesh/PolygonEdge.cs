using Graphing.Generic;
public class PolygonEdge<PolyType, EdgeType, NodeType>
    where PolyType : ObsoleteGraphPoly<PolyType, EdgeType, NodeType>, new()
    where EdgeType : ObsoleteGraphEdge<PolyType, EdgeType, NodeType>, new()
    where NodeType : ObsoleteGraphNode<PolyType, EdgeType, NodeType>, new()
{
    public PolygonEdge(EdgeType edge)
    {
        Edge = edge.Poly;
        Twin = edge.Twin.Poly;
    }
    public PolyType Edge { get; private set; }
    public PolyType Twin { get; private set; }
}
