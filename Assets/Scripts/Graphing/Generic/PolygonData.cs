using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ModernMAK.Serialization;
using Unity.Collections;
using UnityEngine;

namespace ModernMAK.Graphing.Native
{
    public class PolygonData : IDisposable, IBinarySerializable
    {
        //ID = INDEX
        //Still, we keep a Count which represents the # of nodes
        public int Count { get; set; }
        public NativeList<int> Edge { get; }

        public IEnumerable<int> WalkEdge<TEdge>(int index, TEdge edgeData) where TEdge : EdgeData
        {
            return edgeData.WalkPolygon(Edge[index]);
        }
        
        public IEnumerable<int> WalkNodes<TEdge>(int index, TEdge edgeData) where TEdge : EdgeData
        {
            foreach (var edge in WalkEdge(index,edgeData))
            {
                yield return edgeData.Node[edge];
            }
        }
        
        
            
        //SHOULD ONLY BE USED FOR TESTING
        public virtual bool Validate()
        {
            try
            {
                Assert();
            }
            catch (Exception exception)
            {
                //We want a pause, but not a crash
                Debug.LogError(exception);
                return false;
            }

            return true;
        }

        //SHOULD ONLY BE USED FOR TESTING
        public virtual void Assert()
        {
            var expectedLen = Count;
            if(expectedLen != Edge.Length)
                throw new Exception($"{nameof(Edge.Length)} size mismatch! ({Edge.Length} != {Count})");
        }
        public virtual void Resize(int size, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
        {
            Count = size;
            Edge.Resize(size,options);
        }

        public void Dispose()
        {
            Edge.Dispose();
        }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write(Count);
            writer.WriteList(Edge);
        }

        public virtual void Read(BinaryReader reader)
        {
            var size =  reader.ReadInt32();
            Resize(size);
            reader.ReadList(Edge, Count);
        }
    }
}