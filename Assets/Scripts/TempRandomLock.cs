using System;
using Random = UnityEngine.Random;

public class TempRandomLock : IDisposable
{
    private readonly Random.State _mPrevState;

    public TempRandomLock(int seed)
    {
        _mPrevState = Random.state;
        Random.InitState(seed);
    }

    public TempRandomLock(Random.State state)
    {
        _mPrevState = Random.state;
        Random.state = state;
    }


    void IDisposable.Dispose()
    {
        Random.state = _mPrevState;
    }
}