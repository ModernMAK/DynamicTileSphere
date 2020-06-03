using System;
using System.IO;
using ModernMAK.Graphing.Native;
using ModernMAK.Serialization;
using Unity.Collections;

namespace ModernMAK.Graphing.Planet
{
    public class PlanetPolygonData : PolygonData
    {
        public PlanetPolygonData(Allocator allocator = Allocator.Persistent) : base(allocator)
        {
            Terrain = new NativeList<int>(allocator);
            Elevation = new NativeList<int>(allocator);
            Partition = new NativeList<int>(allocator);
        }

        public PlanetPolygonData(int size, Allocator allocator = Allocator.Persistent) : base(size, allocator)
        {
            Terrain = new NativeList<int>(size, allocator);
            Elevation = new NativeList<int>(size, allocator);
            Partition = new NativeList<int>(size, allocator);
        }

        public NativeList<int> Terrain { get; private set; }
        public NativeList<int> Elevation { get; private set; }
        public NativeList<int> Partition { get; private set; }

        public override void Resize(int size, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
        {
            base.Resize(size, options);
            Terrain.Resize(size, options);
            Elevation.Resize(size, options);
            Partition.Resize(size, options);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            reader.ReadList(Terrain, Count);
            reader.ReadList(Elevation, Count);
            reader.ReadList(Partition, Count);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.WriteList(Terrain);
            writer.WriteList(Elevation);
            writer.WriteList(Partition);
        }

        public override void Assert()
        {
            base.Assert();
            var expectedLen = Count;
            Asserter.SizeMatch(Terrain, expectedLen, nameof(Terrain));
            Asserter.SizeMatch(Elevation, expectedLen, nameof(Elevation));
            Asserter.SizeMatch(Partition, expectedLen, nameof(Partition));
        }
    }
}