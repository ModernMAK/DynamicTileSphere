
using Graphing.Planet;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlanetGraphMeshParameters
{
    public float Radius;
    public Vector3 DistortionScale;
    [Range(0f, 1f)]
    public float Solidity;
    public float ElevationOffset;

    public static PlanetGraphMeshParameters Default
    {
        get
        {
            return new PlanetGraphMeshParameters()
            {
                DistortionScale = Vector3.one,
                Solidity = 1f,
                Radius = 1f,
                ElevationOffset = 0.01f
            };
        }
    }
    public Vector3 Scale(int elevation)
    {
        return (1f + (elevation * ElevationOffset)) * Radius * DistortionScale;

    }

}
public class PlanetGraphMeshBuilder : GraphMeshBuilder<PlanetGraph, PlanetPoly, PlanetEdge, PlanetNode>
{


    public PlanetGraphMeshBuilder(PlanetGraph graph, PlanetGraphMeshParameters parameters) : base()
    {
        Partitions = new List<PlanetPartitionMeshBuilder>();
        Parameters = parameters;
        foreach (var partition in graph.Partitions)
            Partitions.Add(new PlanetPartitionMeshBuilder(this, partition));
    }
    private List<PlanetPartitionMeshBuilder> Partitions { get; set; }
    public PlanetGraphMeshParameters Parameters { get; private set; }

    internal override List<ProceduralMeshBuilder> CreateSubmeshes()
    {
        List<ProceduralMeshBuilder> SubMeshes = new List<ProceduralMeshBuilder>();
        foreach (var partition in Partitions)
            SubMeshes.AddRange(partition.CreateSubmeshes());
        return SubMeshes;
    }
    public override void Generate()
    {
        foreach (var partition in Partitions)
            partition.Generate();
    }
    public override void Clear()
    {
        ClearMeshBuilder();
    }
    public override void Generate(PlanetGraph graph)
    {
    }
    protected override void GeneratePolygon(PlanetPoly polygon)
    {
    }
    protected override void GenerateCorner(PlanetEdge edge)
    {
    }
    protected override void GenerateEdge(PlanetEdge edge)
    {
    }
}
