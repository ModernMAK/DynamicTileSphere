
using System.IO;
using UnityEngine;
namespace PlanetGrapher
{

    public class PlanetEdge : IBinarySerializable
    {
        public PlanetEdge(int id = -1, int twinId = -1, int nextId = -1, int prevI = -1, int cellIndex = -1, PlanetGraph graph = null)
        {
            Id = id;
            CellIndex = cellIndex;
        }
        internal PlanetGraph Graph { get; set; }
        public int Id { get; internal set; }
        internal int TwinIndex { get; set; }
        internal int NextIndex { get; set; }
        internal int PrevIndex { get; set; }
        internal int CellIndex { get; set; }

        public Vector3 Origin { get; internal set; }
        public PlanetCell Cell { get { return Graph.CellLookup[CellIndex]; } }
        public PlanetEdge Twin { get { return Graph.EdgeLookup[TwinIndex]; } }
        public PlanetEdge Next { get { return Graph.EdgeLookup[NextIndex]; } }
        public PlanetEdge Prev { get { return Graph.EdgeLookup[PrevIndex]; } }

        public bool IsPlateBoundary { get { return Cell.TectonicPlate != Twin.Cell.TectonicPlate; } }
        public Vector2 BoundaryPressure { get; internal set; }
        
        public int GetDirection(Vector3 a, Vector3 b)
        {
            float dot = Vector3.Dot(a, b);
            return (dot < 0f ? -1 : (dot > 0f ? 1 : 0));
        }


        public void Save(BinaryWriter writer)
        {
            writer.Write(CellIndex);
            writer.Write(NextIndex);
            writer.Write(TwinIndex);
            writer.Write(PrevIndex);
            writer.Write(Origin.x);
            writer.Write(Origin.y);
            writer.Write(Origin.z);
        }

        public void Load(BinaryReader reader)
        {
            CellIndex = reader.ReadInt32();//Write(PlanetIndex);
            NextIndex = reader.ReadInt32();//Write(NextIndex);
            TwinIndex = reader.ReadInt32();//Write(TwinIndex);
            PrevIndex = reader.ReadInt32();// (PrevIndex);
            Origin = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

        }
    }
}