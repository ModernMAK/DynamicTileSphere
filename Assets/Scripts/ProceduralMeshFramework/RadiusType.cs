namespace ProceduralMeshFramework
{
    /// <summary>
    ///     The radius to use as reference;
    ///     Normalized will produce shapes based upon a circumradius of 1f
    ///     Circumradius will produce shapes based where the vertex-to-center distance is the radius
    ///     Inradius will produce shapes based where the polygon-to-center distance is the radius
    ///     Midradius will produce shapes based where the edge-to-center distance is the radius
    /// </summary>
    public enum RadiusType
    {
        Circumradius,
        Midradius,
        Inradius,
        Normalized
    }
}