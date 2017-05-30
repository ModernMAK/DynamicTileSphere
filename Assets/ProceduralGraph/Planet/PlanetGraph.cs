
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//namespace PlanetGrapher
//{

//    public class PlanetGraph : IFileSerializable, IBinarySerializable
//    {
//        public PlanetGraph() : this(1)
//        {
//        }
//        public PlanetGraph(int chunkSize)
//        {
//            ChunkSize = chunkSize;
//        }

//        internal PlanetTectonicPlate[] PlateLookup { get; set; }
//        internal PlanetEdge[] EdgeLookup { get; set; }
//        internal PlanetCell[] CellLookup { get; set; }
//        internal PlanetChunk[] ChunkLookup { get; set; }
//        public int ChunkCount { get { return Mathf.CeilToInt(CellLookup.Length / (float)ChunkSize); } }

//        public IEnumerable<PlanetCell> Cells { get { return CellLookup; } }
//        public IEnumerable<PlanetChunk> Chunks { get { return ChunkLookup; } }

//        public int ChunkSize { get; internal set; }

//        public const string ACTIVE_EXT = ".p";
//        public const int ACTIVE_VERSION = 1;

//        public void Save(BinaryWriter writer)
//        {
//            writer.Write(ACTIVE_VERSION);

//            writer.Write(ChunkSize);

//            int l = CellLookup.Length;
//            writer.Write(l);
//            for (int i = 0; i < l; i++)
//            {
//                CellLookup[i].Save(writer);
//            }
//            l = EdgeLookup.Length;
//            writer.Write(l);
//            for (int i = 0; i < l; i++)
//            {
//                EdgeLookup[i].Save(writer);
//            }
//        }

//        public void Load(BinaryReader reader)
//        {
//            int version = reader.ReadInt32();
//            switch (version)
//            {
//                case ACTIVE_VERSION:
//                    LoadActive(reader);
//                    break;
//                default:
//                    throw new System.Exception(string.Format("Invalid Version : {0}! Active Version : {1}.", version, ACTIVE_VERSION));
//            }
//        }
//        public void LoadActive(BinaryReader reader)
//        {
//            ChunkSize = reader.ReadInt32();

//            int l = reader.ReadInt32();
//            int l2 = l / ChunkSize + (l % ChunkSize > 0 ? 1 : 0);
//            CellLookup = new PlanetCell[l];
//            ChunkLookup = new PlanetChunk[l2];
//            int i2 = 0;
//            for (int i = 0; i < l; i++)
//            {
//                if (i2 < l2 && i % ChunkSize == 0)
//                {
//                    ChunkLookup[i2] = new PlanetChunk(i2, this);
//                    i2++;
//                }
//                CellLookup[i] = new PlanetCell() { Id = i, Graph = this, ChunkId = (i2 - 1) };
//                CellLookup[i].Load(reader);
//            }

//            l = reader.ReadInt32();
//            EdgeLookup = new PlanetEdge[l];
//            for (int i = 0; i < l; i++)
//            {
//                EdgeLookup[i] = new PlanetEdge() { Id = i, Graph = this };
//                EdgeLookup[i].Load(reader);
//            }
//        }

//        public void Save(string fName, string fPath = null)
//        {
//            if (fPath == null)
//                fPath = Application.persistentDataPath;
//            string path = Path.Combine(fPath, Path.ChangeExtension(fName, ACTIVE_EXT));
//            using (FileStream writer = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
//            {
//                Save(writer);
//            }
//        }

//        public void Load(string fName, string fPath = null)
//        {
//            if (fPath == null)
//                fPath = Application.persistentDataPath;
//            string path = Path.Combine(fPath, Path.ChangeExtension(fName, ACTIVE_EXT));
//            using (FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read))
//            {
//                Load(reader);
//            }
//        }

//        public void Save(FileStream writer)
//        {
//            using (BinaryWriter bWriter = new BinaryWriter(writer))
//            {
//                Save(bWriter);
//            }
//        }

//        public void Load(FileStream reader)
//        {
//            using (BinaryReader bReader = new BinaryReader(reader))
//            {
//                Load(bReader);
//            }
//        }
//    }
//}