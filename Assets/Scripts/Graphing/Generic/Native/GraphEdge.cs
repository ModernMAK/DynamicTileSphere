namespace Graphing.Generic.Native
{
	public struct Edge : IEdge
	{
		public int Node { get; set; }
		public int Poly { get; set; }
        public int Twin { get; set; }
        public int Next { get; set; }
        public int Prev { get; set; }
    }
    public interface IEdge
    {
        public int Node { get; set; }
        public int Poly { get; set; }

        public int Twin { get; set; }

        public int Next { get; set; }

        public int Prev { get; set; }
    }

    ////Technically a Half Edge, which is a Directed Edge, but whatever.
    //public class GraphEdge<PolyT, EdgeT, NodeT>
    //    where NodeT : GraphNode<PolyT, EdgeT, NodeT>, new()
    //    where EdgeT : GraphEdge<PolyT, EdgeT, NodeT>, new()
    //    where PolyT : GraphPoly<PolyT, EdgeT, NodeT>, new()
    //{
    //    public GraphEdge()
    //    {
    //        Identity = -1;
    //        Node = null;
    //        Poly = null;
    //        Twin = null;
    //        Next = null;
    //        Prev = null;
    //    }

    //    public int Identity { get; internal set; }

    //    public NodeT Node { get; internal set; }

    //    public PolyT Poly { get; internal set; }

    //    public EdgeT Twin { get; internal set; }

    //    public EdgeT Next { get; internal set; }

    //    public EdgeT Prev { get; internal set; }

    //    internal virtual void LoadDual(EdgeT data, NodeT newNode, PolyT newPoly, EdgeT newTwin, EdgeT newPrev)
    //    {
    //        Identity = data.Identity;
    //        Node = newNode;
    //        Poly = newPoly;
    //        Twin = newTwin;
    //        Prev = newPrev;
    //        newPrev.Next = (EdgeT) this;
    //    }

    //    internal virtual void Serialize(BinaryWriter writer)
    //    {
    //        writer.Write(Node.Identity);
    //        writer.Write(Poly.Identity);
    //        writer.Write(Twin != null ? Twin.Identity : -1);
    //        writer.Write(Next != null ? Next.Identity : -1);
    //        writer.Write(Prev != null ? Prev.Identity : -1);
    //    }

    //    internal virtual void Deserialize(BinaryReader reader, Graph<PolyT, EdgeT, NodeT> graph)
    //    {
    //        Node = graph.Nodes[reader.ReadInt32()];
    //        Poly = graph.Polys[reader.ReadInt32()];
    //        var index = reader.ReadInt32();
    //        Twin = index >= 0 ? graph.Edges[index] : null;
    //        index = reader.ReadInt32();
    //        Next = index >= 0 ? graph.Edges[index] : null;
    //        index = reader.ReadInt32();
    //        Prev = index >= 0 ? graph.Edges[index] : null;
    //    }
    //}
}