
//using UnityEngine;
//using System.Collections.Generic;
//using System;
//using System.IO;
////using ProceduralMesh;
//namespace PlanetGrapher
//{

//    public static class CellGraphGenerator
//    {
//        public static CellGraph Generate(int division, bool slerp)
//        {
//            CellGraph c = Load(division, slerp);
//            if (c == null)
//                c = CreateAndSave(division, slerp);
//            return c;
//        }
//        private static string GetFileName(int divisions, bool slerp)
//        {
//            return string.Format("Cell_{0}{1}", divisions, (slerp ? "S" : "L"));
//        }
//        //Null on Failure
//        private static CellGraph Load(int division, bool slerp)
//        {
//            string fName = GetFileName(division, slerp);
//            if (File.Exists(fName))
//            {
//                CellGraph c = new CellGraph();
//                c.Load(fName);
//                return c;
//            }
//            return null;
//        }
//        //Never returns Null (On expected conditions)
//        private static CellGraph CreateAndSave(int division, bool slerp)
//        {
//            string fName = GetFileName(division, slerp);
//            CellGraph c = CellGraphBuilder.CreateGraph(VD_GraphGenerator.Generate(division,slerp));
//            c.Save(fName);
//            return c;
//        }
//    }
//    public static class CellGraphSerizlizer
//    {
//        public static void Save(CellGraph graph, string fName)
//        {
//            graph.Save(fName);
//        }
//        public static CellGraph Load(string fName)
//        {
//            CellGraph graph = new CellGraph();
//            graph.Load(fName);
//            return graph;
//        }
//    }
//    public static class CellGraphBuilder
//    {
//        public static CellGraph CreateGraph(VD_Graph vdGraph)
//        {
//            Cell[] cells;
//            CellEdge[] cellEdges;

//            int
//                cCount = vdGraph.VPolyLookup.Length,
//                eCount = vdGraph.VEdgeLookup.Length;
//            CellGraph cGraph = new CellGraph()
//            {
//                CellLookup = cells = new Cell[cCount],
//                EdgeLookup = cellEdges = new CellEdge[eCount],
//            };

//            VD_VPoly vPoly;
//            //VD_VEdge
//            //    start,
//            //    current;
//            for (int i = 0; i < cCount; i++)
//            {
//                vPoly = vdGraph.VPolyLookup[i];
//                cells[i] = new Cell()
//                {
//                    Id = i,
//                    EdgeId = vPoly.Edge.Id,
//                    Position = vPoly.Position,
//                    Graph = cGraph
//                };
//                //start = vPoly.Edge;
//                //current = start;
//                //int count = 0;
//                //do
//                //{
//                //    count++;
//                //    current = current.Next;
//                //}
//                //while (start != current);
//                ////cells[i].IsHex = (count == 6);
//            }
//            VD_VEdge vEdge;
//            for (int i = 0; i < eCount; i++)
//            {
//                vEdge = vdGraph.VEdgeLookup[i];
//                cellEdges[i] = new CellEdge()
//                {
//                    Id = i,
//                    TwinIndex = vEdge.Twin.Id,
//                    NextIndex = vEdge.Next.Id,
//                    PrevIndex = vEdge.Previous.Id,
//                    CellIndex = vEdge.Polygon.Id,
//                    Origin = vEdge.Origin.Position,
//                    Graph = cGraph,
//                };
//                if (cellEdges[i].Id == cellEdges[i].TwinIndex)
//                    throw new Exception(string.Format("Edge {0} is its own twin!", i));
//            }

//            //QuickSortExtensions.Quicksort(dEdgeLookup, new KeyCompare<long, int>());

//            //for (int i = 0; i < tris.Length * 3; i++)
//            //{
//            //    int twinId = BinarySearch.BinaryKeyValue(dEdgeLookup, dTwin[i]);
//            //    graph.DEdgeLookup[i].TwinId = twinId;
//            //}

//            return cGraph;
//        }

//    }

//    public class CellGraph : IFileSerializable, IBinarySerializable
//    {
//        internal CellEdge[] EdgeLookup { get; set; }
//        internal Cell[] CellLookup { get; set; }

//        public IEnumerable<Cell> Cells { get { return CellLookup; } }

//        public const string ACTIVE_EXT = ".cg";
//        public const int ACTIVE_VERSION = 0;

//        public void Save(BinaryWriter writer)
//        {
//            writer.Write(ACTIVE_VERSION);
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
//                    throw new Exception(string.Format("Invalid Version : {0}! Active Version : {1}.", version, ACTIVE_VERSION));
//            }
//        }
//        public void LoadActive(BinaryReader reader)
//        {
//            int l = reader.ReadInt32();
//            CellLookup = new Cell[l];
//            for (int i = 0; i < l; i++)
//            {
//                CellLookup[i] = new Cell() { Id = i, Graph = this };
//                CellLookup[i].Load(reader);
//            }
//            l = reader.ReadInt32();
//            EdgeLookup = new CellEdge[l];
//            for (int i = 0; i < l; i++)
//            {
//                EdgeLookup[i] = new CellEdge() { Id = i, Graph = this };
//                EdgeLookup[i].Load(reader);
//                if (EdgeLookup[i].Id == EdgeLookup[i].TwinIndex)
//                    throw new Exception(string.Format("Edge {0} is its own twin!", i));
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

//    public class CellEdge : IBinarySerializable
//    {
//        public CellEdge(int id = -1, int twinId = -1, int nextId = -1, int prevId = -1, int cellIndex = -1, CellGraph graph = null)
//        {
//            Id = id;
//            CellIndex = cellIndex;
//            TwinIndex = twinId;
//            NextIndex = nextId;
//            PrevIndex = prevId;
//        }
//        internal CellGraph Graph { get; set; }
//        public int Id { get; internal set; }
//        internal int TwinIndex { get; set; }
//        internal int NextIndex { get; set; }
//        internal int PrevIndex { get; set; }
//        internal int CellIndex { get; set; }

//        public Vector3 Origin { get; internal set; }
//        public Cell Cell { get { return Graph.CellLookup[CellIndex]; } }
//        public CellEdge Twin { get { return Graph.EdgeLookup[TwinIndex]; } }
//        public CellEdge Next { get { return Graph.EdgeLookup[NextIndex]; } }
//        public CellEdge Prev { get { return Graph.EdgeLookup[PrevIndex]; } }

//        public void Save(BinaryWriter writer)
//        {
//            writer.Write(CellIndex);
//            writer.Write(NextIndex);
//            writer.Write(TwinIndex);
//            writer.Write(PrevIndex);
//            writer.Write(Origin.x);
//            writer.Write(Origin.y);
//            writer.Write(Origin.z);
//        }

//        public void Load(BinaryReader reader)
//        {
//            CellIndex = reader.ReadInt32();//Write(CellIndex);
//            NextIndex = reader.ReadInt32();//Write(NextIndex);
//            TwinIndex = reader.ReadInt32();//Write(TwinIndex);
//            PrevIndex = reader.ReadInt32();// (PrevIndex);
//            Origin = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

//        }
//    }
//    public class Cell : IBinarySerializable
//    {
//        public Cell(int id = -1, int edgeId = -1, Vector3 position = default(Vector3),/* bool isHex = false,*//*[] nodeNeighbors = null, int[] nodeTwinNeighbors = null, Vector3[] polyVerts = null,*/ CellGraph graph = null)
//        {
//            Id = id;
//            EdgeId = edgeId;
//            Position = position;
//            //IsHex = isHex;
//            //NeighborIdentities = nodeNeighbors;
//            //NeighborTwinIdentities = nodeTwinNeighbors;
//            //PolygonVerticies = polyVerts;
//            Graph = graph;
//        }

//        internal CellGraph Graph { get; set; }
//        public int Id { get; internal set; }
//        public int EdgeId { get; internal set; }
//        public Vector3 Position { get; internal set; }
//        //public bool IsHex { get; internal set; }
//        //public int[] NeighborIdentities { get; internal set; }
//        //public int[] NeighborTwinIdentities { get; internal set; }
//        //public Vector3[] PolygonVerticies { get; internal set; }

//        public CellEdge Edge { get { return Graph.EdgeLookup[EdgeId]; } }

//        public void Save(BinaryWriter writer)
//        {
//            //writer.Write(IsHex);
//            writer.Write(EdgeId);
//            writer.Write(Position.x);
//            writer.Write(Position.y);
//            writer.Write(Position.z);
//            //for (int i = 0; i < (IsHex ? 6 : 5); i++)
//            //{
//            //    writer.Write(NeighborIdentities[i]);
//            //    writer.Write(NeighborTwinIdentities[i]);

//            //    writer.Write(PolygonVerticies[i].x);
//            //    writer.Write(PolygonVerticies[i].y);
//            //    writer.Write(PolygonVerticies[i].z);
//            //}
//        }

//        public void Load(BinaryReader reader)
//        {

//            //IsHex = reader.ReadBoolean();
//            //int l = (IsHex ? 6 : 5);
//            //NeighborIdentities = new int[l];
//            //NeighborTwinIdentities = new int[l];
//            //PolygonVerticies = new Vector3[l];
//            //for (int i = 0; i < l; i++)
//            //{
//            //    NeighborIdentities[i] = reader.ReadInt32();
//            //    NeighborTwinIdentities[i] = reader.ReadInt32();
//            //    PolygonVerticies[i] = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
//            //}
//            EdgeId = reader.ReadInt32();
//            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
//        }
//    }
//}