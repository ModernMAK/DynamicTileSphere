using System;
using System.Collections.Generic;
using System.IO;
using ModernMAK.Serialization;
using Unity.Collections;
using UnityEngine;

namespace ModernMAK.Graphing.Native
{
    public class NodeData : IDisposable, IBinarySerializable
    {
        protected NodeData(int initialSize, Allocator allocator)
        {
            Count = initialSize;
            Edge = new NativeList<int>(initialSize, allocator);
        }

        //ID is index so we can optimize that away!
//            public NativeList<int> Id { get; }
        //Still, we keep a Count which represents the # of nodes
        //This way we can avoid relying on an arbitrary list for consistancies.
        public int Count { get; set; }
        public NativeList<int> Edge { get; }

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
            if (expectedLen != Edge.Length)
                throw new Exception($"{nameof(Edge.Length)} size mismatch! ({Edge.Length} != {Count})");
        }

        public virtual void Resize(int size, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
        {
            Count = size;
            Edge.Resize(size, options);
        }

        public virtual void Dispose()
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
            var size = reader.ReadInt32();
            Resize(size);
            reader.ReadList(Edge, Count);
        }


        public IEnumerable<int> WalkEdge<TEdge>(int index, TEdge edgeData) where TEdge : EdgeData
        {
            return edgeData.WalkNode(Edge[index]);
        }

        public IEnumerable<int> WalkPolygons<TEdge>(int index, TEdge edgeData) where TEdge : EdgeData
        {
            foreach (var edge in WalkEdge(index, edgeData))
            {
                yield return edgeData.Poly[edge];
            }
        }
    }

    //For this project i only need float3, but by abstracting it, position could be any arbitrary type and dimension
    public class PositionNodeData<TPos> : NodeData where TPos : struct
    {
        public PositionNodeData(int initialSize, Allocator allocator = Allocator.Persistent) :
            base(initialSize, allocator)
        {
            Position = new NativeList<TPos>(initialSize, allocator);
        }

        public NativeList<TPos> Position { get; }

        public override void Resize(int size, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
        {
            base.Resize(size, options);
            Position.Resize(size, options);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.WriteList(Position);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            reader.ReadList(Position, Count);
        }

        public override void Dispose()
        {
            base.Dispose();
            Position.Dispose();
        }
    }
}