using System.Collections.Generic;
using System.IO;
using Unity.Collections;

namespace Graphing.Generic.Native
{
    public interface IData<T>
    {
        T Data { get; set; }
    }
    public struct DataPolygon<TData> : IPolygon, IData<TData>
    {
        public int Edge { get; set; }
        public TData Data { get; set; }
    }
    public struct DataEdge<TData> : IEdge, IData<TData>
    {
        public int Node { get; set; }
        public int Poly { get; set; }
        public int Twin { get; set; }
        public int Next { get; set; }
        public int Prev { get; set; }

        public TData Data { get; set; }
    }
    public struct DataVertex<TData> : IVertex, IData<TData>
    {
        public int Edge { get; set; }
        public TData Data { get; set; }

    }

    public class DataGraph<PolyDataT, EdgeDataT, VertexDataT> : Graph<DataPolygon<PolyDataT>, DataEdge<EdgeDataT>, DataVertex<VertexDataT>>
    {

        public DataGraph(int nodeCapacity = 0, int edgeCapacity = 0, int polyCapacity = 0) : base(nodeCapacity, edgeCapacity, polyCapacity)
        {
        }
    }
    public class Graph<PolyT, EdgeT, VertexT>
        where VertexT : struct, IVertex
        where EdgeT : struct, IEdge
        where PolyT : struct, IPolygon
    {
        public Graph(int nodeCapacity = 0, int edgeCapacity = 0, int polyCapacity = 0)
        {
            Nodes = new NativeList<VertexT>(nodeCapacity, Allocator.Persistent);
            Edges = new NativeList<EdgeT>(edgeCapacity, Allocator.Persistent);
            Polygons = new NativeList<PolyT>(polyCapacity, Allocator.Persistent);
        }

        public NativeList<VertexT> Nodes;

        public NativeList<EdgeT> Edges;
        public NativeList<PolyT> Polygons;

        public IEnumerable<EdgeT> WalkPolygonEdges(IPolygon poly)
        {

            var start = poly.Edge;
            var current = start;
            do
            {
                var temp = Edges[current];
                yield return temp;
                current = temp.Next;
            } while (current != start);
        }
        public IEnumerable<VertexT> WalkPolygonVertexes(IPolygon poly)
		{
            foreach (var edge in WalkPolygonEdges(poly))
                yield return Nodes[edge.Node];
		}

        public IEnumerable<EdgeT> WalkVertexEdges(IVertex vertex)
        {

            var start = vertex.Edge;
            var current = start;
            do
            {
                var temp = Edges[current];
                yield return temp;
                current = Edges[temp.Twin].Next;
            } while (current != start);
        }
        public IEnumerable<PolyT> WalkVertexPolygons(IVertex vertex)
        {
            foreach (var edge in WalkVertexEdges(vertex))
                yield return Polygons[edge.Poly];
        }

        //public GraphT Dual<GraphT>() where GraphT : Graph<PolyT, EdgeT, VertexT>, new()
        //{
        //    var dual = new GraphT();
        //    dual.LoadDual(this);
        //    return dual;
        //}

        //internal virtual void Serialize(BinaryWriter writer)
        //{
        //    writer.Write(Nodes.Count);
        //    writer.Write(Edges.Count);
        //    writer.Write(Polygons.Count);

        //    foreach (var node in Nodes)
        //        node.Serialize(writer);

        //    foreach (var edge in Edges)
        //        edge.Serialize(writer);

        //    foreach (var poly in Polygons)
        //        poly.Serialize(writer);
        //}

        //internal virtual void Deserialize(BinaryReader reader)
        //{
        //    Nodes.Clear();
        //    var nodes = reader.ReadInt32();
        //    var edges = reader.ReadInt32();
        //    var polys = reader.ReadInt32();

        //    for (var i = 0; i < nodes; i++)
        //        Nodes.Add(new VertexT {Identity = i});
        //    for (var i = 0; i < edges; i++)
        //        Edges.Add(new EdgeT {Identity = i});
        //    for (var i = 0; i < polys; i++)
        //        Polygons.Add(new PolyT {Identity = i});

        //    foreach (var node in Nodes)
        //        node.Deserialize(reader, this);
        //    foreach (var edge in Edges)
        //        edge.Deserialize(reader, this);
        //    foreach (var poly in Polygons)
        //        poly.Deserialize(reader, this);
        //}

        //internal virtual void LoadDual<GraphT>(GraphT data) where GraphT : Graph<PolyT, EdgeT, VertexT>
        //{
        //    var PolyDualLookup = new Dictionary<PolyT, VertexT>();
        //    var EdgeDualLookup = new Dictionary<EdgeT, EdgeT>();
        //    var NodeDualLookup = new Dictionary<VertexT, PolyT>();

        //    Nodes.Clear();
        //    Edges.Clear();
        //    Polygons.Clear();

        //    foreach (var poly in data.Polygons) Nodes.Add(PolyDualLookup[poly] = new VertexT());
        //    foreach (var edge in data.Edges) Edges.Add(EdgeDualLookup[edge] = new EdgeT());
        //    foreach (var node in data.Nodes) Polygons.Add(NodeDualLookup[node] = new PolyT());


        //    foreach (var poly in data.Polygons)
        //        PolyDualLookup[poly].LoadDual(poly, EdgeDualLookup[poly.Edge]);

        //    foreach (var node in data.Nodes)
        //        NodeDualLookup[node].LoadDual(node, EdgeDualLookup[node.Edge]);

        //    foreach (var edge in data.Edges)
        //    {
        //        var dualEdge = EdgeDualLookup[edge]; // EdgeDualLookup[edge];

        //        //From my VD_DEdge (Which is saved under Assets/ProceduralGraph/Graph in my first commit of github (since I will have gotten rid of it eventually))
        //        /*
        //        Dual_Edge = Dual(Edge)
        //        Dual_Next = Dual(Edge.Twin.Next)
        //        Dual_Previous = Dual(Edge.Previous.Twin)
        //        Dual_Twin  = Dual(Edge.Twin)
        //        */
        //        dualEdge.LoadDual(
        //            edge,
        //            PolyDualLookup[edge.Poly],
        //            NodeDualLookup[edge.Node],
        //            edge.Twin != null ? EdgeDualLookup[edge.Twin] : null,
        //            edge.Twin != null && edge.Twin.Next != null ? EdgeDualLookup[edge.Twin.Next] : null
        //            //(edge.Prev != null && edge.Prev.Twin != null) ? EdgeDualLookup[edge.Prev.Twin] : null
        //        );
        //    }

        //    Nodes.Sort((x, y) => { return x.Identity.CompareTo(y.Identity); });
        //    Edges.Sort((x, y) => { return x.Identity.CompareTo(y.Identity); });
        //    Polygons.Sort((x, y) => { return x.Identity.CompareTo(y.Identity); });
        //}
    }
}
