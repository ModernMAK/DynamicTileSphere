using System;
using ProceduralMeshFramework;

namespace ProceduralGraph
{
    [Serializable]
    public struct ProceduralPlanetMeshParameters
    {
        public static ProceduralPlanetMeshParameters Default
        {
            get
            {
                return new ProceduralPlanetMeshParameters
                {
                    Subdivisions = 0,
                    Shape = ShapeType.Icosahedron,
                    Slerp = false,
                    PartitionSize = 32
                };
            }
        }

        public int Subdivisions;
        public ShapeType Shape;
        public bool Slerp;
        public int PartitionSize;
    }
}