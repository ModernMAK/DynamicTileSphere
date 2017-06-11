using Graphing.Position.Generic;
using System.Collections.Generic;
public class GraphMeshBuilder<GraphT, PolyT, EdgeT, NodeT> : ProceduralMeshBuilder
    where GraphT : PositionGraph<PolyT, EdgeT, NodeT>
    where PolyT : ObsoletePositionPoly<PolyT, EdgeT, NodeT>, new()
    where EdgeT : ObsoletePositionEdge<PolyT, EdgeT, NodeT>, new()
    where NodeT : ObsoletePositionNode<PolyT, EdgeT, NodeT>, new()
{
    public GraphMeshBuilder()
    {
        Polygons = new HashSet<PolyT>();
        Edges = new HashSet<PolygonEdge<PolyT, EdgeT, NodeT>>(new PolygonEdgeComparar<PolyT, EdgeT, NodeT>());
        Corners = new HashSet<PolygonCorner<PolyT, EdgeT, NodeT>>(new PolygonCornerComparar<PolyT, EdgeT, NodeT>());
    }
    public virtual void Generate()
    {
        //Cant do anything in the generic
    }
    public virtual void Generate(GraphT graph)
    {
        foreach (var poly in graph.Polys)
            Render(poly);
        foreach (var edge in graph.Edges)
            Render(edge);
    }
    public void Render(PolyT polygon)
    {
        if (!ContainsPolygon(polygon))
        {
            GeneratePolygon(polygon);
            AddPolygon(polygon);
        }
    }
    protected virtual void GeneratePolygon(PolyT polygon)
    {
        List<int> Nodes = new List<int>();
        //foreach (var edge in polygon)
        //{
        //    Nodes.Add(AddVertex(new ProceduralVertex(edge.Node.Position)));
        //}
        var edge = polygon.Edge;
        var startEdge = edge;
        do
        {
            Nodes.Add(AddVertex(new ProceduralVertex(edge.Node.Position)));
            edge = edge.Next;
        } while (edge != startEdge && edge != null);

        for (int i = 1; i < Nodes.Count - 1; i++)
        {
            AddTriangle(Nodes[0], Nodes[i], Nodes[i + 1]);
        }
    }
    public void Render(EdgeT edge)
    {
        PolygonEdge<PolyT, EdgeT, NodeT> polyEdge = new PolygonEdge<PolyT, EdgeT, NodeT>(edge);
        PolygonCorner<PolyT, EdgeT, NodeT> polyCorner = new PolygonCorner<PolyT, EdgeT, NodeT>(edge);
        if (!ContainsEdge(polyEdge))
        {
            GenerateEdge(edge);
            AddEdge(polyEdge);
        }
        if (!ContainsCorner(polyCorner))
        {
            GenerateCorner(edge);
            AddCorner(polyCorner);
        }
    }
    protected virtual void GenerateEdge(EdgeT edge)
    {

    }
    protected virtual void GenerateCorner(EdgeT edge)
    {

    }


    protected virtual void ClearGraphMeshBuilder()
    {
        Polygons.Clear();
        Edges.Clear();
        Corners.Clear();
    }
    public override void Clear()
    {
        base.Clear();
        ClearGraphMeshBuilder();
    }
    public HashSet<PolyT> Polygons { get; internal set; }
    public HashSet<PolygonEdge<PolyT, EdgeT, NodeT>> Edges { get; internal set; }
    public HashSet<PolygonCorner<PolyT, EdgeT, NodeT>> Corners { get; internal set; }

    public virtual void RemoveEdge(PolygonEdge<PolyT, EdgeT, NodeT> edge)
    {
        Edges.Remove(edge);
    }
    public virtual bool ContainsEdge(PolygonEdge<PolyT, EdgeT, NodeT> edge)
    {
        return Edges.Contains(edge);
    }
    public virtual void AddEdge(PolygonEdge<PolyT, EdgeT, NodeT> edge)
    {
        Edges.Add(edge);
    }

    public virtual void RemoveCorner(PolygonCorner<PolyT, EdgeT, NodeT> corner)
    {
        Corners.Remove(corner);
    }
    public virtual bool ContainsCorner(PolygonCorner<PolyT, EdgeT, NodeT> corner)
    {
        return Corners.Contains(corner);
    }
    public virtual void AddCorner(PolygonCorner<PolyT, EdgeT, NodeT> corner)
    {
        Corners.Add(corner);
    }


    public virtual void RemovePolygon(PolyT poly)
    {
        Polygons.Remove(poly);
    }
    public virtual bool ContainsPolygon(PolyT poly)
    {
        return Polygons.Contains(poly);
    }
    public virtual void AddPolygon(PolyT poly)
    {
        Polygons.Add(poly);
    }
}
