using UnityEngine;

public class Octree
{
    private int GetDepth(OctreeNode node)


    {
        return (int) Mathf.Log(BitScanReverse(node.LocationCode), 2f) / 3;
    }

    private static long BitScanReverse(long code)
    {
        if (code == 0)
            return -1;

        long lookupCode = 0xFFFFFFFF;
        var div = 32;
        var result = 0;
        while (lookupCode >= 1)
        {
            if (code > lookupCode)
            {
                code >>= div;
                result += div;
            }

            if (lookupCode > 1)
            {
                div /= 2;
                lookupCode /= 2;
            }
            else
            {
                lookupCode = 0;
            }
        }

        return result;
        //long result = 0;
        //if (code > 0xFFFFFFFF)
        //{
        //    code >>= 32;
        //    result = 32;
        //}
        //if (code > 0xFFFF)
        //{
        //    code >>= 16;
        //    result += 16;
        //}
        //if (code > 0xFF)
        //{
        //    code >>= 8;
        //    result += 8;
        //}
        //if (code > 0xF)
        //{
        //    code >>= 4;
        //    result += 4;
        //}
    }
}

public class OctreeNode
{
    public int LocationCode { get; private set; }
}