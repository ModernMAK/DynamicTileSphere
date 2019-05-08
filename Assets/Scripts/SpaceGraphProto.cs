//using System;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class SpaceGraphProto : MonoBehaviour
{
    public float DrawRadius = 1f;
    private SpaceGraph graph;
    public SpaceGraphParameters Params;
    public bool Regenerate = true;

    public int Seed, pSeed;

    // Use this for initialization
    private void Generate()
    {
        pSeed = Seed;
        graph = SpaceGraphGenerator.Generate(Seed,
            Params); // MinPlanets, MaxPlanets,arms:Arms, armScale:ArmScale, scale: SpaceScale, up: SpaceUp, right: SpaceRight, tangentalVelocity:SpaceVelocity);
    }

    private void OnDrawGizmos()
    {
        if (graph == null)
            return;
        using (var rLock = new TempRandomLock(pSeed))
        {
            for (var v = 0; v < graph.Points.Length; v++)
            {
                Gizmos.color = Random.ColorHSV();
                Gizmos.DrawSphere(graph.Points[v], DrawRadius);
            }

            //Gizmos.color = Color.white;
            for (var e = 0; e < graph.Edges.Length; e++)
            {
                Gizmos.color = Random.ColorHSV();
                var edge = graph.Edges[e];
                Gizmos.DrawLine(graph.Points[edge.From], graph.Points[edge.To]);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Regenerate)
        {
            Regenerate = !Regenerate;
            Generate();
        }
    }
}

//Not disposing
public class TempRandomLock : IDisposable
{
    private readonly Random.State mPrevState;

    public TempRandomLock(int seed)
    {
        mPrevState = Random.state;
        Random.InitState(seed);
    }

    public TempRandomLock(Random.State state)
    {
        mPrevState = Random.state;
        Random.state = state;
    }


    void IDisposable.Dispose()
    {
        Random.state = mPrevState;
    }
}

public class SpaceGraph
{
    public SpaceGraph(IEnumerable<Vector3> v, IEnumerable<SpaceEdge> e)
    {
        Points = Enumerable.ToArray(v);
        Edges = Enumerable.ToArray(e);
    }

    public Vector3[] Points { get; private set; }
    public SpaceEdge[] Edges { get; private set; }
}

internal class SpaceEdgeComparar : IEqualityComparer<SpaceEdge>
{
    public bool Equals(SpaceEdge x, SpaceEdge y)
    {
        return x.To == y.To && x.From == y.From || x.To == y.From && x.From == y.To;
    }

    public int GetHashCode(SpaceEdge obj)
    {
        return obj.GetHashCode();
    }
}

public struct SpaceEdge
{
    public int To, From;
}