using System;
using GraphToMesh;

namespace ProceduralGraph
{
    [Serializable]
    public struct PlanetGraphParameters
    {
        public static PlanetGraphParameters Default
        {
            get
            {
                return new PlanetGraphParameters
                {
                    Seed = 0,
                    TerrainTypes = 1,
                    MaxHeight = 5,
                    MinHeight = -8,
//                    TectonicParameters = PlateParameters.Default
                };
            }
        }

        public int Seed;

        public int TerrainTypes;

//        public PlateParameters TectonicParameters;
        public int MaxHeight;
        public int MinHeight;
    }
}