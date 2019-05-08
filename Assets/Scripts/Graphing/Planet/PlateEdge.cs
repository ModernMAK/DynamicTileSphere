using Graphing.Position.Generic;

namespace Graphing.Planet
{
    public class PlateEdge
        : PositionEdge<PlatePoly, PlateEdge, PlateNode>
    {
        public PlanetEdge DebugEdge { get; set; }
    }
}