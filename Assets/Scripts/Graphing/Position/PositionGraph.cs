using Graphing.Position.Generic;

namespace Graphing.Position
{
    public class PositionGraph
        : PositionGraph<PositionPoly, PositionEdge, PositionNode>
    {
        public PositionGraph()
        {
        }

        public PositionGraph(int nodeCapacity, int edgeCapacity, int polyCapacity) : base(nodeCapacity, edgeCapacity,
            polyCapacity)
        {
        }
    }
}