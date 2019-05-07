[System.Serializable]
public struct PlanetGraphParameters
{
    public static PlanetGraphParameters Default
    {
        get
        {
            return new PlanetGraphParameters()
            {
                Seed = 0,
                TerrainTypes = 1,
                TectonicParameters = PlateParameters.Default
            };
        }
    }
    public int Seed;
    public int TerrainTypes;
    public PlateParameters TectonicParameters;
}