using System;
using System.Collections.Generic;
using System.IO;
using ModernMAK.Serialization;
using Unity.Collections;
using UnityEngine;

namespace ModernMAK.Graphing.Native
{
    public class EdgeData : IDisposable, IBinarySerializable
    {
        public EdgeData(Allocator allocator = Allocator.Persistent)
        {
            Count = 0;
            Node = new NativeList<int>(allocator);
            Poly = new NativeList<int>(allocator);
            Twin = new NativeList<int>(allocator);
            Next = new NativeList<int>(allocator);
            Prev = new NativeList<int>(allocator);
        }

        public EdgeData(int size, Allocator allocator = Allocator.Persistent)
        {
            Node = new NativeList<int>(size, allocator);
            Poly = new NativeList<int>(size, allocator);
            Twin = new NativeList<int>(size, allocator);
            Next = new NativeList<int>(size, allocator);
            Prev = new NativeList<int>(size, allocator);
        }

        public NativeList<int> Node { get; }
        public NativeList<int> Poly { get; }
        public NativeList<int> Twin { get; }
        public NativeList<int> Next { get; }
        public NativeList<int> Prev { get; }

        public int Count { get; set; }

        public virtual void Resize(int size, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
        {
            Count = size;
            Node.Resize(size, options);
            Poly.Resize(size, options);
            Twin.Resize(size, options);
            Next.Resize(size, options);
            Prev.Resize(size, options);
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
            if (expectedLen != Node.Length)
                throw new Exception($"{nameof(Node.Length)} size mismatch! ({Node.Length} != {Count})");
            if (expectedLen != Poly.Length)
                throw new Exception($"{nameof(Poly.Length)} size mismatch! ({Poly.Length} != {Count})");
            if (expectedLen != Twin.Length)
                throw new Exception($"{nameof(Twin.Length)} size mismatch! ({Twin.Length} != {Count})");
            if (expectedLen != Next.Length)
                throw new Exception($"{nameof(Next.Length)} size mismatch! ({Next.Length} != {Count})");
            if (expectedLen != Prev.Length)
                throw new Exception($"{nameof(Prev.Length)} size mismatch! ({Prev.Length} != {Count})");
        }

        public void Dispose()
        {
            Node.Dispose();
            Poly.Dispose();
            Twin.Dispose();
            Next.Dispose();
            Prev.Dispose();
        }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write(Count);
            writer.WriteList(Node);
            writer.WriteList(Poly);
            writer.WriteList(Twin);
            writer.WriteList(Next);
            writer.WriteList(Prev);
        }

        public virtual void Read(BinaryReader reader)
        {
            var size = reader.ReadInt32();
            Resize(size);
            reader.ReadList(Node, Count);
            reader.ReadList(Poly, Count);
            reader.ReadList(Twin, Count);
            reader.ReadList(Next, Count);
            reader.ReadList(Prev, Count);
        }


        public IEnumerable<int> WalkPolygon(int index)
        {
            var start = index;
            var current = start;
            yield return current;
            do
            {
                //Traverse across all edges (keeping polygon same) until we reach start
                current = Next[current];
                yield return current;
            } while (start != current);
        }

        public IEnumerable<int> WalkNode(int index)
        {
            var start = index;
            var current = start;
            yield return current;
            do
            {
                //Traverse across all edges (keeping node same) until we reach start
                current = Twin[current];
                current = Next[current];
                yield return current;
            } while (start != current);
        }
    }
}