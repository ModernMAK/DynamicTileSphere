namespace Graphing.Generic.Native
{
	public interface IPolygonData<TData>        
    {
        TData Data { get; set; }
    }
    public struct PolygonData<TData> : IPolygonData<TData>
    {
        public int Edge { get; set; }
        public TData Data { get; set; }
    }
    public interface IPolygon
	{
        int Edge { get; set; }
	}
    public struct Polygon : IPolygon
    {

        public int Edge { get; set; }
    }
        //public IEnumerator<EdgeT> GetEnumerator()
        //{
        //    return new GraphPolyWalker(Edge);
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //internal virtual void LoadDual(NodeT data, EdgeT newEdge)
        //{
        //    Identity = data.Identity;
        //    Edge = newEdge;
        //}

        //internal virtual void Serialize(BinaryWriter writer)
        //{
        //    writer.Write(Edge.Identity);
        //}

        //internal virtual void Deserialize(BinaryReader reader, Graph<PolyT, EdgeT, NodeT> graph)
        //{
        //    Edge = graph.Edges[reader.ReadInt32()];
        //}


        //private class GraphPolyWalker : IEnumerator<EdgeT>
        //{
        //    private readonly IEnumerator<EdgeT> ListEnum;

        //    public GraphPolyWalker(EdgeT edge)
        //    {
        //        var list = new List<EdgeT>();
        //        var currentEdge = edge;
        //        if (currentEdge != null)
        //            do
        //            {
        //                list.Add(currentEdge);
        //                currentEdge = currentEdge.Next;
        //            } while (currentEdge != edge && currentEdge != null);

        //        ListEnum = list.GetEnumerator();
        //    }

        //    public EdgeT Current
        //    {
        //        get { return ListEnum.Current; }
        //    }

        //    object IEnumerator.Current
        //    {
        //        get { return ListEnum.Current; }
        //    }

        //    public void Dispose()
        //    {
        //        ListEnum.Dispose();
        //    }

        //    public bool MoveNext()
        //    {
        //        return ListEnum.MoveNext();
        //    }

        //    public void Reset()
        //    {
        //        ListEnum.Reset();
        //    }
        //}
    //}
}