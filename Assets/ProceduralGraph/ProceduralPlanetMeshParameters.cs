[System.Serializable]
public struct ProceduralPlanetMeshParameters
{
    public static ProceduralPlanetMeshParameters Default
    {
        get
        {
            return new ProceduralPlanetMeshParameters()
            {
                Subdivisions = 0,
                Shape = ShapeType.Icosahedron,
                Slerp = false,
                Dual = true,
                PartitionSize = 32,
            };
        }
    }
    public int Subdivisions;
    public ShapeType Shape;
    public bool Slerp;
    public bool Dual;
    public int PartitionSize;
}