using Graphing.Position.Generic;
namespace Graphing.Planet
{
    public class PlanetPoly
        : PositionPoly<PlanetPoly, PlanetEdge, PlanetNode>
    {
        public PlanetPoly() : base()
        {
            myElevation = 0;
            myTerrainType = -1;
        }
        private int myElevation, myTerrainType;

        public int Elevation
        {
            get
            {
                return myElevation;
            }
            internal set
            {
                bool changed = (myElevation != value);
                myElevation = value;
                if (changed && OnChanged != null)
                    OnChanged();
            }
        }
        public int TerrainType
        {
            get
            {
                return myTerrainType;
            }
            internal set
            {
                bool changed = (myTerrainType != value);
                myTerrainType = value;
                if (changed && OnChanged != null)
                    OnChanged();
            }
        }
        public event ChangeEvent OnChanged;
        
    }
}
public delegate void ChangeEvent();

