using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Graphing
{
    //public class ObsoleteGraphPoly<PolyT, EdgeT, NodeT> : IEnumerable<EdgeT>
    //    where NodeT : ObsoleteGraphNode<PolyT, EdgeT, NodeT>, new()
    //    where EdgeT : ObsoleteGraphEdge<PolyT, EdgeT, NodeT>, new()
    //    where PolyT : ObsoleteGraphPoly<PolyT, EdgeT, NodeT>, new()
    //{
    //    public ObsoleteGraphPoly()
    //    {
    //        Identity = -1;
    //        Edge = null;
    //    }
    //    public int Identity
    //    {
    //        get;
    //        internal set;
    //    }
    //    public EdgeT Edge
    //    {
    //        get;
    //        internal set;
    //    }
    //    internal virtual void LoadDual(NodeT data, EdgeT newEdge)
    //    {
    //        Identity = data.Identity;
    //        Edge = newEdge;
    //    }
    //    internal virtual void Serialize(BinaryWriter writer)
    //    {
    //        writer.Write(Edge.Identity);
    //    }
    //    internal virtual void Deserialize(BinaryReader reader, ObsoleteGraph<PolyT, EdgeT, NodeT> graph)
    //    {
    //        Edge = graph.Edges[reader.ReadInt32()];
    //    }

    //    public IEnumerator<EdgeT> GetEnumerator()
    //    {
    //        return new GraphPolyWalker(Edge);
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }

    //    class GraphPolyWalker : IEnumerator<EdgeT>
    //    {
    //        IEnumerator<EdgeT> ListEnum;
    //        public GraphPolyWalker(EdgeT edge)
    //        {
    //            var list = new List<EdgeT>();
    //            var currentEdge = edge;
    //            if (currentEdge != null)
    //            {
    //                do
    //                {
    //                    list.Add(currentEdge);
    //                    currentEdge = currentEdge.Next;
    //                } while (currentEdge != edge && currentEdge != null);
    //            }
    //            ListEnum = list.GetEnumerator();
    //        }

    //        public EdgeT Current
    //        {
    //            get
    //            {
    //                return ListEnum.Current;
    //            }
    //        }

    //        object IEnumerator.Current
    //        {
    //            get
    //            {
    //                return ListEnum.Current;
    //            }
    //        }

    //        public void Dispose()
    //        {
    //            ListEnum.Dispose();
    //        }

    //        public bool MoveNext()
    //        {
    //            return ListEnum.MoveNext();
    //        }

    //        public void Reset()
    //        {
    //            ListEnum.Reset();
    //        }
    //    }
    //}

    public class GraphPoly : IGraphPoly
    {
        public IGraphEdge Edge
        {
            get;
            protected set;
        }

        public IEnumerator<IGraphEdge> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
}