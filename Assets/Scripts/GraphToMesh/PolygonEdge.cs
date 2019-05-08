using Graphing.Generic;

namespace GraphToMesh
{
    public class PolygonEdge<PolyType, EdgeType, NodeType>
        where PolyType : GraphPoly<PolyType, EdgeType, NodeType>, new()
        where EdgeType : GraphEdge<PolyType, EdgeType, NodeType>, new()
        where NodeType : GraphNode<PolyType, EdgeType, NodeType>, new()
    {
        public PolygonEdge(EdgeType edge)
        {
            Edge = edge.Poly;
            Twin = edge.Twin.Poly;
        }

        public PolyType Edge { get; private set; }
        public PolyType Twin { get; private set; }
    }
}