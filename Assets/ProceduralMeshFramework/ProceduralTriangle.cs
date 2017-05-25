using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public struct ProceduralTriangle : IEnumerable<int>
{
    public ProceduralTriangle(int pivot, int left, int right)
    {
        Indicies = new int[3] { pivot, left, right };
    }
    private int[] Indicies { get; set; }
    public int Pivot { get { return Indicies[0]; } set { Indicies[0] = value; } }
    public int Left { get { return Indicies[1]; } set { Indicies[1] = value; } }
    public int Right { get { return Indicies[2]; } set { Indicies[2] = value; } }

    public int this[int i] { get { return Indicies[i]; } set { Indicies[i] = value; } }


    public IEnumerator<int> GetEnumerator()
    {
        return ((IEnumerable<int>)Indicies).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<int>)Indicies).GetEnumerator();
    }
}