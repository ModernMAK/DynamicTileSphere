using System;
using Random = UnityEngine.Random;

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