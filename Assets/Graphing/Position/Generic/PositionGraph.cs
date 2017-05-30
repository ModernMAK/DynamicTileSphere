using Graphing.Generic;
using System.Collections.Generic;

namespace Graphing.Position.Generic
{
    public class PositionGraph<PolyT, EdgeT, NodeT>
       : Graph<PolyT, EdgeT, NodeT>
        where PolyT : PositionPoly<PolyT, EdgeT, NodeT>, new()
        where EdgeT : PositionEdge<PolyT, EdgeT, NodeT>, new()
        where NodeT : PositionNode<PolyT, EdgeT, NodeT>, new()
    {
        public PositionGraph(int nodeCapacity = 0, int edgeCapacity = 0, int polyCapacity = 0) : base(nodeCapacity, edgeCapacity, polyCapacity)
        {
        }
    }
}