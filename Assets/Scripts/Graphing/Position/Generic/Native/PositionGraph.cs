using Graphing.Generic.Native;
using Graphing.Position.Generic.Native;

namespace Graphing.Position.Generic.Native
{
    public class PositionGraph<PolyDataT, EdgeDataT, NodeDataT>
        : DataGraph<PolyDataT, EdgeDataT, NodeDataT>
        where PolyDataT : struct
        where EdgeDataT : struct
        where NodeDataT : struct, IPositionData
    {
        public PositionGraph(int nodeCapacity = 0, int edgeCapacity = 0, int polyCapacity = 0) : base(nodeCapacity,
            edgeCapacity, polyCapacity)
        {
        }
    }
}

namespace Graphing.Position.Native
{
    public class PositionGraph : Graph<Polygon, Edge, DataVertex<PositionData>>
	{
        public PositionGraph(int nodeCapacity = 0, int edgeCapacity = 0, int polyCapacity = 0) : base(nodeCapacity,
            edgeCapacity, polyCapacity)
        {
        }
    }
}