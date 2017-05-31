//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SpaceGraphProto : MonoBehaviour
{
    public bool Regenerate = true;
    public int Seed = 0, pSeed = 0;
    public SpaceGraphParameters Params;

    public float DrawRadius = 1f;
    private SpaceGraph graph;
    // Use this for initialization
    void Generate()
    {
        pSeed = Seed;
        graph = SpaceGraphGenerator.Generate(Seed, Params);// MinPlanets, MaxPlanets,arms:Arms, armScale:ArmScale, scale: SpaceScale, up: SpaceUp, right: SpaceRight, tangentalVelocity:SpaceVelocity);
    }
    private void OnDrawGizmos()
    {
        if (graph == null)
            return;
        using (TempRandomLock rLock = new TempRandomLock(pSeed))
        {
            for (int v = 0; v < graph.Points.Length; v++)
            {
                Gizmos.color = Random.ColorHSV();
                Gizmos.DrawSphere(graph.Points[v], DrawRadius);
            }
            //Gizmos.color = Color.white;
            for (int e = 0; e < graph.Edges.Length; e++)
            {
                Gizmos.color = Random.ColorHSV();
                SpaceEdge edge = graph.Edges[e];
                Gizmos.DrawLine(graph.Points[edge.From], graph.Points[edge.To]);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Regenerate)
        {
            Regenerate = !Regenerate;
            Generate();
        }
    }
}
//Not disposing
public class TempRandomLock : System.IDisposable
{
    private Random.State mPrevState;
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



    void System.IDisposable.Dispose()
    {
        Random.state = mPrevState;
    }
}

public class SpaceGraph
{
    public SpaceGraph(IEnumerable<Vector3> v, IEnumerable<SpaceEdge> e)
    {
        Points = System.Linq.Enumerable.ToArray(v);
        Edges = System.Linq.Enumerable.ToArray(e);
    }

    public Vector3[] Points { get; private set; }
    public SpaceEdge[] Edges { get; private set; }
}
class SpaceEdgeComparar : IEqualityComparer<SpaceEdge>
{
    public bool Equals(SpaceEdge x, SpaceEdge y)
    {
        return (x.To == y.To && x.From == y.From) || (x.To == y.From && x.From == y.To);
    }

    public int GetHashCode(SpaceEdge obj)
    {
        return obj.GetHashCode();
    }
}
public struct SpaceEdge { public int To, From; }
