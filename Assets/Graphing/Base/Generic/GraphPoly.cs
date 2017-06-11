using System;
using System.Collections;
using System.Collections.Generic;

namespace Graphing.Generic
{
    public class GraphPoly<PolyT, EdgeT, NodeT> : IGraphPoly<PolyT, EdgeT, NodeT>, IEnumerable<EdgeT>
            where PolyT : class, IGraphPoly<PolyT, EdgeT, NodeT>
            where EdgeT : class, IGraphEdge<PolyT, EdgeT, NodeT>
            where NodeT : class, IGraphNode<PolyT, EdgeT, NodeT>
    {
        public EdgeT Edge
        {
            get;
            protected set;
        }
        IGraphEdge IGraphPoly.Edge
        {
            get
            {
                return Edge;
            }
        }

        public IEnumerator<EdgeT> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
