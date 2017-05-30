namespace Graphing.Position
{
    using Generic;
    public class PositionGraph
        : PositionGraph<PositionPoly, PositionEdge, PositionNode>
    {
        public PositionGraph() : base()
        {

        }
        public PositionGraph(int nodeCapacity, int edgeCapacity, int polyCapacity) : base(nodeCapacity, edgeCapacity, polyCapacity)
        {
        }
    }
}