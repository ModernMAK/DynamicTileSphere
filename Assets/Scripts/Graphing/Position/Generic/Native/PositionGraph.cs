using Graphing.Generic.Native;

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