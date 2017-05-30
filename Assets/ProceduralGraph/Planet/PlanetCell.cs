//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//namespace PlanetGrapher
//{
//    public class PlanetEdgeWalker : IEnumerator<PlanetEdge>
//    {
//        public PlanetEdgeWalker (PlanetEdge edge)
//        {
//            StartEdge = edge;
//            PrevEdge = null;
//            CurrentEdge = null;
//        }
//        //PrevEdge is the previously held current edge, not the edge in prev of current edge
//        private PlanetEdge StartEdge, PrevEdge, CurrentEdge;


//        public PlanetEdge Current
//        {
//            get
//            {
//                return CurrentEdge;
//            }
//        }

//        object IEnumerator.Current { get { return Current; } }

//        public void Dispose()
//        {
//            //Do nothing
//        }

//        public bool MoveNext()
//        {
//            PrevEdge = CurrentEdge;
//            if (CurrentEdge == null)
//            {
//                if (PrevEdge == null)
//                    CurrentEdge = StartEdge;
//                else
//                    return false;
//            }
//            if (CurrentEdge.Next == StartEdge)
//                return false;
//            CurrentEdge = CurrentEdge.Next;
//            return true;
//        }

//        public void Reset()
//        {
//            CurrentEdge = null;
//            PrevEdge = null;
//        }
//    }
//    public class PlanetCell : IBinarySerializable, IEnumerable<PlanetEdge>
//    {
//        public const int DEFAULT_PLATE_ID = -1;
//        public PlanetCell(int id = -1, int edgeId = -1, int chunkId = -1, Vector3 position = default(Vector3), bool isHex = false, PlanetGraph graph = null)
//        {
//            Id = id;
//            EdgeId = edgeId;
//            ChunkId = chunkId;
//            Position = position;
//            IsHex = isHex;
//            Graph = graph;
//            Elevation = 0;
//            TerrainType = 0;
//            Owner = -1;
//            TectonicPlateId = DEFAULT_PLATE_ID;
//        }

//        internal PlanetGraph Graph { get; set; }

//        public int TectonicPlateId { get; internal set; }
//        public int Id { get; internal set; }
//        public int ChunkId { get; internal set; }
//        internal int EdgeId { get; set; }
//        public Vector3 Position { get; internal set; }
//        public bool IsHex { get; internal set; }
//        public int Elevation { get; set; }
//        public int TerrainType { get; set; }
//        public int Owner { get; set; }

//        public PlanetChunk Chunk { get { return Graph.ChunkLookup[ChunkId]; } }
//        public PlanetEdge Edge { get { return Graph.EdgeLookup[EdgeId]; } }

//        public PlanetTectonicPlate TectonicPlate { get { return Graph.PlateLookup[TectonicPlateId]; } }

//        public Vector3 TectonicPlateVector { get; internal set; }

//        public void Save(BinaryWriter writer)
//        {
//            writer.Write(IsHex);
//            writer.Write(EdgeId);
//            writer.Write(Position.x);
//            writer.Write(Position.y);
//            writer.Write(Position.z);
//        }

//        public void Load(BinaryReader reader)
//        {

//            IsHex = reader.ReadBoolean();
//            EdgeId = reader.ReadInt32();
//            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
//        }

//        public IEnumerator<PlanetEdge> GetEnumerator()
//        {
//            return new PlanetEdgeWalker(Edge);
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }
//    }
//}