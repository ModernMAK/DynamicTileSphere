using Graphing.Position.Generic;

namespace Graphing.Planet
{
    public class PlanetPoly
        : PositionPoly<PlanetPoly, PlanetEdge, PlanetNode>
    {
        private int myElevation, myTerrainType;

        public PlanetPoly()
        {
            myElevation = 0;
            myTerrainType = -1;
        }

        public int Elevation
        {
            get { return myElevation; }
            internal set
            {
                var changed = myElevation != value;
                myElevation = value;
                if (changed && OnChanged != null)
                    OnChanged();
            }
        }

        public int TerrainType
        {
            get { return myTerrainType; }
            internal set
            {
                var changed = myTerrainType != value;
                myTerrainType = value;
                if (changed && OnChanged != null)
                    OnChanged();
            }
        }

        public event ChangeEvent OnChanged;
    }

    public delegate void ChangeEvent();
}