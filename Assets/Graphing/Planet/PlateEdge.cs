using Graphing.Position.Generic;
using Graphing.Planet;
namespace Graphing.Planet
{
    public class PlateEdge
        : ObsoletePositionEdge<PlatePoly, PlateEdge, PlateNode>
    {
        public PlanetEdge DebugEdge { get; set; }
    }
}

