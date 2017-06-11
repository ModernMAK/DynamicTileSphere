using Graphing.Position.Generic;

namespace Graphing.Planet
{
    public class PlatePoly
        : ObsoletePositionPoly<PlatePoly, PlateEdge, PlateNode>
    {
        public PlatePoly() : base()
        {
            Height = 0;
        }
        public int Height { get; set; }       
        public bool Oceanic { get { return Height < 0; } }
    }
}