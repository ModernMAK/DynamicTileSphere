using ModernMAK.Graphing.Planet;

namespace ModernMAK.Graphing.Native
{
    public class PositionGraph<TPos> : Generic.Graph<PolygonData,EdgeData,PositionNodeData<TPos>> where TPos : struct
    {

        public PositionGraph() : base(new PolygonData(), new EdgeData(), new PositionNodeData<TPos>())
        {
            
        }
        public PositionGraph(PolygonData polyData, EdgeData edgeData, PositionNodeData<TPos> nodeData) : base(polyData, edgeData, nodeData)
        {
        }
    }
    public class PlanetGraph<TPos> : Generic.Graph<PlanetPolygonData,EdgeData,PositionNodeData<TPos>> where TPos : struct
    {
        public PlanetGraph(PlanetPolygonData polyData, EdgeData edgeData, PositionNodeData<TPos> nodeData) : base(polyData, edgeData, nodeData)
        {
        }

        public PlanetGraph() : base(new PlanetPolygonData(), new EdgeData(), new PositionNodeData<TPos>())
        {
            
        }
    }
}