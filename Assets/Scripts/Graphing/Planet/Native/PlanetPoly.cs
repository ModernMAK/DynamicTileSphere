using Graphing.Position.Generic.Native;
using Unity.Mathematics;

namespace Graphing.Planet.Native
{
    public struct PlanetPolyData : IPositionData
    {
        private int myElevation;
        private int myTerrainType;


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

		public float3 Position { get; set; }

		public event ChangeEvent OnChanged;
    }

    public delegate void ChangeEvent();
}