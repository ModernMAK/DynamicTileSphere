using System;
using Unity.Collections;
using UnityEngine;

namespace ModernMAK
{
    public static class Asserter
    {
        public static void SizeMatch<T>(NativeList<T> list, int size) where T : struct
        {
            if(list.Length != size)
                throw new Exception($"Size mismatch! ({list.Length} != {size})");
        }
        public static void SizeMatch<T>(NativeList<T> list, int size, string name) where T : struct
        {
            if(list.Length != size)
                throw new Exception($"{name} size mismatch! ({list.Length} != {size})");
        }
    }
}