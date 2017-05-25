//using UnityEngine;
////using UnityEditor;
//using System.Collections.Generic;
//using PlanetGrapher;
//using System.IO;
//using System;
////using System;
////using System.IO;
////using ProceduralMesh;


////namespace NodeGraphOld
////{
////    using Generic;
////    using System.Linq;

////    namespace Generic
////    {

////        public interface IGraph<TNode, THalfEdge, TPolygon> : IGraph
////            where TNode : INode<TNode, THalfEdge, TPolygon>
////            where THalfEdge : IHalfEdge<TNode, THalfEdge, TPolygon>
////            where TPolygon : IPolygon<TNode, THalfEdge, TPolygon>
////        {
////            new IEnumerable<TNode> Nodes { get; }
////            new IEnumerable<TPolygon> Polygons { get; }
////        }

////        //Way too ugly
////        //But its typesafe!
////        //Look for other patterns or optomize?
////        //Nah, just restructure
////        public interface INode<TNode, THalfEdge, TPolygon> : INode
////            where TNode : INode<TNode, THalfEdge, TPolygon>
////            where THalfEdge : IHalfEdge<TNode, THalfEdge, TPolygon>
////            where TPolygon : IPolygon<TNode, THalfEdge, TPolygon>

////        {
////            new THalfEdge Edge { get; }
////        }

////        public interface IHalfEdge<TNode, THalfEdge, TPolygon> : IHalfEdge
////            where TNode : INode<TNode, THalfEdge, TPolygon>
////            where THalfEdge : IHalfEdge<TNode, THalfEdge, TPolygon>
////            where TPolygon : IPolygon<TNode, THalfEdge, TPolygon>
////        {
////            new TNode Node { get; }
////            new TPolygon Polygon { get; }
////            new THalfEdge Next { get; }
////            new THalfEdge Twin { get; }
////            new THalfEdge Prev { get; }
////        }

////        public interface IPolygon<TNode, THalfEdge, TPolygon> : IPolygon
////            where TNode : INode<TNode, THalfEdge, TPolygon>
////            where THalfEdge : IHalfEdge<TNode, THalfEdge, TPolygon>
////            where TPolygon : IPolygon<TNode, THalfEdge, TPolygon>
////        {
////            new THalfEdge Edge { get; }
////        }
////    }

////    public interface IPositional
////    {
////        Vector3 Position { get; }
////    }
////    public interface INode : IPositional
////    {
////        //Edges pointing  away from 
////        IHalfEdge Edge { get; }
////    }
////    public interface IHalfEdge
////    {
////        INode Node { get; }
////        IPolygon Polygon { get; }
////        IHalfEdge Next { get; }
////        IHalfEdge Twin { get; }
////        IHalfEdge Prev { get; }
////    }
////    public interface IPolygon : IPositional
////    {
////        IHalfEdge Edge { get; }
////    }
////    public interface IGraph
////    {
////        IEnumerable<INode> Nodes { get; }
////        IEnumerable<IPolygon> Polygons { get; }
////    }

////    public static class GenericGraphBuilder
////    {
////        public static GenericGraph CreateDual(GenericGraph graph)
////        {
////            //Create Nodes from Polygons
////            GenericNode[] nodes = new GenericNode[graph.Polygons.Count()];
////            GenericPolygon[] polygons = new GenericPolygon[graph.Nodes.Count()];
////            GenericHalfEdge[] halfEdges = new GenericHalfEdge[graph.Nodes.Count()*3];

////            for (int n = 0; n < nodes.Length; n++)
////            {
////                GenericPolygon poly = polygons[n];
////                Vector3 position = Vector3.zero;
////                float mag = 0f;
////                int count = 0;
////                IHalfEdge startEdge = poly.Edge;
////                IHalfEdge edge = startEdge;
////                do
////                {
////                    position += edge.Node.Position;
////                    mag += edge.Node.Position.magnitude;
////                    count++;
////                    edge = edge.Next;
////                } while (edge != startEdge);
////                position = position.normalized * (mag / count);
////                nodes[n] = new GenericNode { Position = position };
////            }
////            for(int p = 0; p < polygons.Length; p++)



////            return new GenericGraph
////            {
////                Nodes = nodes,
////                Polygons = polygons
////            };
////        }
////        public static GenericGraph CreateFromMeshBuilder(ProceduralMeshBuilder builder)
////        {
////            return CreateGraphFromTriangles(builder.Verticies.ToArray(), builder.Triangles.ToArray());
////        }
////        public static GenericGraph CreateGraphFromTriangles(ProceduralVertex[] vertexArr, ProceduralTriangle[] triangleArr)
////        {

////            //We have triangles -> (P)olygons
////            //We have triangles -> (E)dges or (A)rcs
////            //We have verticies -> (N)odes
////            GenericNode[] Verticies = new GenericNode[vertexArr.Length];
////            GenericHalfEdge[] HalfEdges = new GenericHalfEdge[triangleArr.Length * 3];
////            GenericPolygon[] Polygons = new GenericPolygon[triangleArr.Length];
////            //long[] twinArr = new long[triangleArr.Length * 3];
////            //int[] nextArr = new int[triangleArr.Length * 3];
////            //int[] prevArr = new int[triangleArr.Length * 3];

////            for (int i = 0; i < vertexArr.Length; i++)
////            {
////                Verticies[i] = new GenericNode()
////                {
////                    Position = vertexArr[i].Position
////                };
////            }


////            //We want it to be sorted... We could sort the edges we get, or more easily, just start with an arbitrary ege, and use next and twin to find them, and stop when we reach our start
////            //Dictionary<int, int> VertLookup = new Dictionary<int, int>();
////            //Vertex and an index to an edge pointint away from it 
////            Dictionary<int, int> VertLookup = new Dictionary<int, int>();

////            //Edge to Edge,
////            //Stores Triangle, and Index of To Vertex in Triangle
////            Dictionary<KeyValuePair<int, int>, int> EdgeLookup = new Dictionary<KeyValuePair<int, int>, int>();
////            //Second pass creates tables of Unique TriangleEdges
////            //Essentially solves the clusterfuck above
////            //After we have this, we can build our half edges
////            //We have to do this instead of doing it in the
////            //We also build our polygons, first stage
////            for (int i = 0; i < triangleArr.Length; i++)
////            {
////                ProceduralTriangle triangle = triangleArr[i];
////                GenericPolygon polygon = new GenericPolygon()
////                {
////                    //Verticies = new VoronoiVertex[3],
////                    //Edges = new GenericHalfEdge[3]
////                };
////                Vector3 position = Vector3.zero;

////                for (int j = 0; j < 3; j++)
////                {
////                    int k = (j + 1) % 3;
////                    int vertJ = triangle[j];
////                    int vertK = triangle[k];
////                    KeyValuePair<int, int> key = new KeyValuePair<int, int>(vertJ, vertK);

////                    VertLookup[vertJ] = vertK;
////                    VertLookup[vertK] = vertJ;

////                    //KeyValuePair<int, int>
////                    //    kvpKey = new KeyValuePair<int, int>(triangle.Indicies[j], triangle.Indicies[k]);

////                    int edgeId = i * 3 + j;
////                    EdgeLookup[key] = edgeId;

////                    GenericNode vertex = Verticies[vertK];
////                    position += vertex.Position;

////                    GenericHalfEdge halfEdge = new GenericHalfEdge()
////                    {
////                        Node = vertex,
////                        Polygon = polygon
////                    };
////                    HalfEdges[edgeId] = halfEdge;
////                    polygon.Edge = halfEdge;
////                    //polygon
////                }
////                polygon.Position = position / 3f;
////                Polygons[i] = polygon;

////            }
////            //for (int i = 0; i < triangleArr.Length * 3; i++)
////            //{
////            //    HalfEdge edge = HalfEdges[i];


////            //    //edge.Twin = HalfEdges[twinArr[i]];
////            //    //edge.Next = HalfEdges[nextArr[i]];
////            //    //edge.Previous = HalfEdges[prevArr[i]];
////            //}
////            for (int i = 0; i < triangleArr.Length; i++)
////            {
////                ProceduralTriangle triangle = triangleArr[i];
////                for (int j = 0; j < 3; j++)
////                {
////                    int k = (j + 1) % 3;
////                    int vertJ = triangle[j];
////                    int vertK = triangle[k];
////                    KeyValuePair<int, int> key = new KeyValuePair<int, int>(vertK, vertJ);
////                    //KeyValuePair<int, int>
////                    //    twinKey = new KeyValuePair<int, int>(triangle.Indicies[j], triangle.Indicies[k]);


////                    int
////                        edgeId = i * 3 + j,
////                        nextId = i * 3 + (j + 1) % 3,
////                        prevId = i * 3 + (j - 1 + 3) % 3,
////                        twinId = EdgeLookup[key];

////                    GenericHalfEdge
////                        edge = HalfEdges[edgeId],
////                        prev = HalfEdges[prevId],
////                        next = HalfEdges[nextId],
////                        twin = HalfEdges[twinId];

////                    //try
////                    //{
////                    //    if (edge == next)
////                    //        throw new System.Exception("Edge is Next!");
////                    //    if (edge == twin)
////                    //        throw new System.Exception("Edge is Twin!");
////                    //    if (edge == prev)
////                    //        throw new System.Exception("Edge is Prev!");
////                    //    if (twin == next)
////                    //        throw new System.Exception("Twin is Next!");
////                    //    if (twin == prev)
////                    //        throw new System.Exception("Twin is Prev!");
////                    //    if (next == prev)
////                    //        throw new System.Exception("Next is Prev!");
////                    //}
////                    //catch (System.Exception e)
////                    //{
////                    //    string dict = "";

////                    //    foreach (KeyValuePair<KeyValuePair<int, int>, int> kvp in EdgeLookup)
////                    //    {
////                    //        int a = kvp.Key.Key;
////                    //        int b = kvp.Key.Value;
////                    //        int c = kvp.Value;

////                    //        dict += string.Format("{0}/{1} : {2}\n", a, b, c);
////                    //    }

////                    //    Debug.LogError(dict);
////                    //    throw e;
////                    //}
////                    edge.Twin = twin;
////                    edge.Next = next;
////                    edge.Prev = prev;
////                }

////            }

////            for (int i = 0; i < vertexArr.Length; i++)
////            {
////                GenericNode vert = Verticies[i];
////                List<GenericHalfEdge> edges = new List<GenericHalfEdge>();//Unfortunately, this is extremely variable, on the plus, without a boundary on the graph, its always at least 3, that being said, it's always at least 1, reagardless of bounds
////                //long key = ((long)i << 32) + VertLookup[i];
////                KeyValuePair<int, int> kvp = new KeyValuePair<int, int>(i, VertLookup[i]);
////                GenericHalfEdge
////                    startEdge = HalfEdges[EdgeLookup[kvp]],
////                    currentEdge = startEdge;



////                //int swaps = 0;
////                do
////                {

////                    edges.Add(currentEdge);
////                    //Twin to make the edge face the vertex
////                    //next to advance the edge
////                    //This new edge should come out of the vertex (think like a triangle, we are working with vert B, we started with B-A, made it A-B, and advanced to B-C)
////                    currentEdge = currentEdge.Twin.Next;
////                    //swaps++;
////                } while (startEdge != currentEdge);

////                //vert.Edges = edges.ToArray();
////                vert.Edge = edges[0];

////            }
////            //2v + 2t * 3
////            //Sweet, thats almost sweep efficiancy, right?
////            //Guess its two sweep passes?
////            //The * 3 comes from each triangle being one element, but composed of 3 sub elements, which we need
////            //Guessing that means I can't/don't optomize this when I eventually bottleneck





////            GenericGraph graph = new GenericGraph
////            {
////                Nodes = Verticies,
////                Polygons = Polygons
////            };

////            return graph;
////        }

////    }
////    public class GenericGraph : IGraph<GenericNode, GenericHalfEdge, GenericPolygon>
////    {
////        public IEnumerable<GenericNode> Nodes { get; internal set; }
////        public IEnumerable<GenericPolygon> Polygons { get; internal set; }

////        IEnumerable<INode> IGraph.Nodes
////        {
////            get
////            {
////                return Nodes.Cast<INode>();
////            }
////        }

////        IEnumerable<IPolygon> IGraph.Polygons
////        {
////            get
////            {
////                return Polygons.Cast<IPolygon>();
////            }
////        }
////    }
////    public class GenericNode : INode<GenericNode, GenericHalfEdge, GenericPolygon>
////    {
////        public Vector3 Position { get; internal set; }
////        //Storage of all halfedges which point away from this vertex
////        public GenericHalfEdge Edge { get; internal set; }

////        IHalfEdge INode.Edge
////        {
////            get
////            {
////                return Edge;
////            }
////        }

////    }
////    public class GenericPolygon : IPolygon<GenericNode, GenericHalfEdge, GenericPolygon>
////    {
////        public Vector3 Position { get; internal set; }
////        public GenericHalfEdge Edge { get; internal set; }

////        IHalfEdge IPolygon.Edge
////        {
////            get
////            {
////                return Edge;
////            }
////        }
////        //public VoronoiVertex[] Verticies;
////    }
////    public class GenericHalfEdge : IHalfEdge<GenericNode, GenericHalfEdge, GenericPolygon>
////    {
////        //The vertex we are point to
////        public GenericNode Node
////        {
////            get;
////            internal set;
////        }
////        //The Polygon to our left
////        public GenericPolygon Polygon
////        {
////            get;
////            internal set;
////        }
////        //The Next Half edge
////        public GenericHalfEdge Next
////        {
////            get;
////            internal set;
////        }
////        //Our twin, point in the opposite direction
////        public GenericHalfEdge Twin
////        {
////            get;
////            internal set;
////        }
////        //The Previous hald edge
////        public GenericHalfEdge Prev
////        {
////            get;
////            internal set;
////        }

////        INode IHalfEdge.Node
////        {
////            get { return Node; }
////        }

////        IPolygon IHalfEdge.Polygon
////        {
////            get { return Polygon; }
////        }

////        IHalfEdge IHalfEdge.Next
////        {
////            get { return Next; }
////        }

////        IHalfEdge IHalfEdge.Twin
////        {
////            get { return Twin; }
////        }

////        IHalfEdge IHalfEdge.Prev
////        {
////            get { return Prev; }
////        }
////    }
////}


////namespace PlanetGrapher
////{
//public static class VD_GraphGenerator
//{
//    public static VD_Graph Generate(int division, bool slerp)
//    {
//        VD_Graph vd = Load(division, slerp);
//        if (vd == null)
//            vd = CreateAndSave(division, slerp);
//        return vd;
//    }
//    private static string GetFileName(int divisions, bool slerp)
//    {
//        return string.Format("VD_{0}{1}", divisions, (slerp ? "S" : "L"));
//    }
//    //Null on Failure
//    private static VD_Graph Load(int division, bool slerp)
//    {
//        string fName = GetFileName(division, slerp);
//        if (File.Exists(fName))
//        {
//            VD_Graph c = new VD_Graph();
//            c.Load(fName);
//            return c;
//        }
//        return null;
//    }
//    //Never returns Null (On expected conditions)
//    private static VD_Graph CreateAndSave(int division, bool slerp)
//    {
//        string fName = GetFileName(division, slerp);
//        ProceduralMeshBuilder pmb = new ProceduralMeshBuilder();
//        ProceduralPlatonicSolidGenerator.AddToMeshBuilder(pmb, ShapeType.Icosahedron, RadiusType.Normalized);
//        ProceduralMeshUtility.Subdivide(pmb, division, slerp);
//        ProceduralMeshUtility.Spherize(pmb);
//        VD_Graph vd = VD_GraphBuilder.CreateGraph(pmb);
//        vd.Save(fName);
//        return vd;
//    }
//}
//public static class VD_GraphSerizlizer
//{
//    public static void Save(VD_Graph graph, string fName)
//    {
//        graph.Save(fName);
//    }
//    public static VD_Graph Load(string fName)
//    {
//        VD_Graph graph = new VD_Graph();
//        graph.Load(fName);
//        return graph;
//    }
//}
//public static class VD_GraphBuilder
//{
//    public static VD_Graph CreateGraph(ProceduralMeshBuilder pmb)
//    {
//        return CreateGraph(pmb.Verticies.ToArray(), pmb.Triangles.ToArray());
//    }
//    public static VD_Graph CreateGraph(ProceduralVertex[] mv, ProceduralTriangle[] tris)
//    {
//        //VNode[] vNodes = new VNode[tris.Length];
//        //VEdge[] vEdges = new VEdge[tris.Length * 3];
//        //VPoly[] vPolys = new VPoly[mv.Length];
//        //DNode[] dNodes = new DNode[mv.Length];
//        //DEdge[] dEdges = new DEdge[tris.Length * 3];
//        //DPoly[] dPolys = new DPoly[tris.Length];
//        //PNode[] pNodes = new PNode[mv.Length];
//        KeyValuePair<long, int>[] dEdgeLookup = new KeyValuePair<long, int>[tris.Length * 3];
//        long[] dTwin = new long[tris.Length * 3];
//        VD_Graph graph = new VD_Graph()
//        {
//            DNodeLookup = new VD_DNode[mv.Length],
//            VPolyLookup = new VD_VPoly[mv.Length],

//            DEdgeLookup = new VD_DEdge[tris.Length * 3],
//            VEdgeLookup = new VD_VEdge[tris.Length * 3],

//            DPolyLookup = new VD_DPoly[tris.Length],
//            VNodeLookup = new VD_VNode[tris.Length],
//        };
//        for (int i = 0; i < mv.Length; i++)
//        {

//            graph.DNodeLookup[i] = new VD_DNode()
//            {
//                Id = i,
//                Position = mv[i].Position,
//                Graph = graph
//            };

//            graph.VPolyLookup[i] = new VD_VPoly()
//            {
//                Id = i,
//                Graph = graph
//            };

//            //graph.VPolyLookup[i].Id = i;
//            //graph.VPolyLookup[i].Graph = graph;
//        }
//        for (int i = 0; i < tris.Length; i++)
//        {
//            ProceduralTriangle triangle = tris[i];

//            graph.VNodeLookup[i] = new VD_VNode(i, graph);

//            graph.DPolyLookup[i] = new VD_DPoly()
//            {
//                Id = i,
//                Graph = graph,
//                Position = (mv[triangle.Pivot].Position + mv[triangle.Left].Position + mv[triangle.Right].Position) / 3f,
//            };

//            for (int j = 0; j < 3; j++)
//            {

//                int vertJ = triangle[j];
//                int vertJn = triangle[(j + 1) % 3];
//                int edgeId = i * 3 + j;

//                long
//                    key = ((long)vertJ << 32) + (long)vertJn,
//                    revKey = ((long)vertJn << 32) + (long)vertJ;

//                dEdgeLookup[edgeId] = new KeyValuePair<long, int>(key, edgeId);
//                dTwin[edgeId] = revKey;

//                graph.DEdgeLookup[edgeId] = new VD_DEdge()
//                {
//                    Id = edgeId,
//                    Graph = graph,
//                    OriginId = vertJ,
//                    PolygonId = i,
//                    NextId = (i * 3) + ((j + 1) % 3),
//                    PrevId = (i * 3) + ((j + 2) % 3),

//                };

//                graph.VEdgeLookup[edgeId] = new VD_VEdge(edgeId, graph);

//                graph.DPolyLookup[i].EdgeId = graph.DNodeLookup[vertJ].EdgeId = edgeId;
//            }
//        }

//        QuickSortExtensions.Quicksort(dEdgeLookup, new KeyCompare<long, int>());

//        for (int i = 0; i < tris.Length * 3; i++)
//        {
//            int twinId = BinarySearch.BinaryKeyValue(dEdgeLookup, dTwin[i]);
//            graph.DEdgeLookup[i].TwinId = twinId;
//        }

//        return graph;
//    }

//}

//public class VD_Graph : IFileSerializable, IBinarySerializable
//{
//    internal VD_DNode[] DNodeLookup { get; set; }
//    internal VD_DEdge[] DEdgeLookup { get; set; }
//    internal VD_DPoly[] DPolyLookup { get; set; }
//    internal VD_VNode[] VNodeLookup { get; set; }
//    internal VD_VEdge[] VEdgeLookup { get; set; }
//    internal VD_VPoly[] VPolyLookup { get; set; }

//    public const string ACTIVE_EXT = ".vdg";
//    public const int ACTIVE_VERSION = 0;

//    public void Save(BinaryWriter writer)
//    {
//        writer.Write(ACTIVE_VERSION);
//        int l = DNodeLookup.Length;
//        writer.Write(l);
//        for (int i = 0; i < l; i++)
//        {
//            //writer.Write(DNodeLookup[i].Id);
//            DNodeLookup[i].Save(writer);
//        }
//        l = DEdgeLookup.Length;
//        writer.Write(l);
//        for (int i = 0; i < l; i++)
//        {
//            //writer.Write(DEdgeLookup[i].Id);
//            DEdgeLookup[i].Save(writer);
//        }
//        l = DPolyLookup.Length;
//        writer.Write(l);
//        for (int i = 0; i < l; i++)
//        {
//            //writer.Write(DPolyLookup[i].Id);
//            DPolyLookup[i].Save(writer);
//            //writer.Write(DPolyLookup[i].EdgeId);

//            //writer.Write(DPolyLookup[i].Position.x);
//            //writer.Write(DPolyLookup[i].Position.y);
//            //writer.Write(DPolyLookup[i].Position.z);
//        }
//    }

//    public void Load(BinaryReader reader)
//    {
//        int version = reader.ReadInt32();
//        switch (version)
//        {
//            case ACTIVE_VERSION:
//                LoadActive(reader);
//                break;
//            default:
//                throw new Exception(string.Format("Invalid Version : {0}! Active Version : {1}.", version, ACTIVE_VERSION));
//        }
//    }
//    public void LoadActive(BinaryReader reader)
//    {
//        int l = reader.ReadInt32();
//        DNodeLookup = new VD_DNode[l];
//        VPolyLookup = new VD_VPoly[l];
//        for (int i = 0; i < l; i++)
//        {
//            DNodeLookup[i] = new VD_DNode()
//            {
//                Id = i,
//                Graph = this
//            };
//            DNodeLookup[i].Load(reader);
//            //    EdgeId = reader.ReadInt32(),
//            //    Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
//            //    Graph = this,
//            //};
//            VPolyLookup[i] = new VD_VPoly()
//            {
//                Id = i,
//                Graph = this,
//            };
//        }
//        l = reader.ReadInt32();
//        DEdgeLookup = new VD_DEdge[l];
//        VEdgeLookup = new VD_VEdge[l];
//        for (int i = 0; i < l; i++)
//        {
//            DEdgeLookup[i] = new VD_DEdge()
//            {
//                Id = i,
//                Graph = this
//            };
//            //    NextId = reader.ReadInt32(),
//            //    PrevId = reader.ReadInt32(),
//            //    TwinId = reader.ReadInt32(),
//            //    OriginId = reader.ReadInt32(),
//            //    PolygonId = reader.ReadInt32(),
//            //    Graph = this
//            //};
//            DEdgeLookup[i].Load(reader);
//            VEdgeLookup[i] = new VD_VEdge()
//            {
//                Id = i,
//                Graph = this,
//            };
//        }
//        l = reader.ReadInt32();
//        DPolyLookup = new VD_DPoly[l];
//        VNodeLookup = new VD_VNode[l];
//        for (int i = 0; i < l; i++)
//        {
//            DPolyLookup[i] = new VD_DPoly()
//            {
//                Id = i,
//                Graph = this
//            };
//            //    EdgeId = reader.ReadInt32(),
//            //    Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
//            //    Graph = this,
//            //};
//            DPolyLookup[i].Load(reader);
//            VNodeLookup[i] = new VD_VNode()
//            {
//                Id = i,
//                Graph = this,
//            };
//        }
//    }

//    public void Save(string fName, string fPath = null)
//    {
//        if (fPath == null)
//            fPath = Application.persistentDataPath;
//        string path = Path.Combine(fPath, Path.ChangeExtension(fName, ACTIVE_EXT));
//        using (FileStream writer = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
//        {
//            Save(writer);
//        }
//    }

//    public void Load(string fName, string fPath = null)
//    {
//        if (fPath == null)
//            fPath = Application.persistentDataPath;
//        string path = Path.Combine(fPath, Path.ChangeExtension(fName, ACTIVE_EXT));
//        using (FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read))
//        {
//            Load(reader);
//        }
//    }

//    public void Save(FileStream writer)
//    {
//        using (BinaryWriter bWriter = new BinaryWriter(writer))
//        {
//            Save(bWriter);
//        }
//    }

//    public void Load(FileStream reader)
//    {
//        using (BinaryReader bReader = new BinaryReader(reader))
//        {
//            Load(bReader);
//        }
//    }
//}
//public class VD_DNode : IBinarySerializable
//{
//    public VD_DNode(int id = -1, int edgeId = -1, Vector3 position = default(Vector3), VD_Graph graph = null)
//    {
//        Id = id;
//        EdgeId = edgeId;
//        Position = position;
//        Graph = graph;
//    }
//    internal VD_Graph Graph { get; set; }
//    internal int Id { get; set; }
//    internal int EdgeId { get; set; }

//    public Vector3 Position { get; internal set; }

//    public VD_VPoly Dual { get { return Graph.VPolyLookup[Id]; } }

//    public VD_DEdge Edge { get { return Graph.DEdgeLookup[EdgeId]; } }

//    public void Save(BinaryWriter writer)
//    {

//        writer.Write(EdgeId);

//        writer.Write(Position.x);
//        writer.Write(Position.y);
//        writer.Write(Position.z);
//    }

//    public void Load(BinaryReader reader)
//    {
//        EdgeId = reader.ReadInt32();
//        Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
//        //reader.Write(Position.x);
//        //writer.Write(Position.y);
//        //writer.Write(Position.z);
//    }
//}
//public class VD_DEdge : IBinarySerializable
//{
//    public VD_DEdge(int id = -1, int twinId = -1, int nextId = -1, int prevId = -1, int originId = -1, int polygonId = -1, VD_Graph graph = null)
//    {
//        Id = -id;
//        TwinId = twinId;
//        NextId = nextId;
//        PrevId = prevId;
//        OriginId = originId;
//        PolygonId = polygonId;
//        Graph = graph;
//    }

//    internal VD_Graph Graph { get; set; }
//    internal int Id { get; set; }
//    internal int TwinId { get; set; }
//    internal int NextId { get; set; }
//    internal int PrevId { get; set; }
//    internal int OriginId { get; set; }
//    internal int PolygonId { get; set; }

//    public VD_VEdge Dual { get { return Graph.VEdgeLookup[Id]; } }

//    public VD_DEdge Next { get { return Graph.DEdgeLookup[NextId]; } }

//    public VD_DNode Origin { get { return Graph.DNodeLookup[OriginId]; } }

//    public VD_DPoly Polygon { get { return Graph.DPolyLookup[PolygonId]; } }

//    public VD_DEdge Previous { get { return Graph.DEdgeLookup[PrevId]; } }

//    public VD_DEdge Twin { get { return Graph.DEdgeLookup[TwinId]; } }

//    public void Save(BinaryWriter writer)
//    {
//        writer.Write(NextId);
//        writer.Write(PrevId);
//        writer.Write(TwinId);

//        writer.Write(OriginId);
//        writer.Write(PolygonId);
//    }

//    public void Load(BinaryReader reader)
//    {
//        NextId = reader.ReadInt32();
//        PrevId = reader.ReadInt32();
//        TwinId = reader.ReadInt32();
//        OriginId = reader.ReadInt32();
//        PolygonId = reader.ReadInt32();
//    }
//}
//public class VD_DPoly : IBinarySerializable
//{
//    public VD_DPoly(int id = -1, int edgeId = -1, Vector3 position = default(Vector3), VD_Graph graph = null)
//    {
//        Id = id;
//        EdgeId = edgeId;
//        Position = position;
//        Graph = graph;
//    }

//    internal VD_Graph Graph { get; set; }
//    internal int Id { get; set; }
//    internal int EdgeId { get; set; }

//    public Vector3 Position { get; internal set; }

//    public VD_VNode Dual { get { return Graph.VNodeLookup[Id]; } }

//    public VD_DEdge Edge { get { return Graph.DEdgeLookup[EdgeId]; } }

//    public void Save(BinaryWriter writer)
//    {
//        writer.Write(EdgeId);

//        writer.Write(Position.x);
//        writer.Write(Position.y);
//        writer.Write(Position.z);
//    }

//    public void Load(BinaryReader reader)
//    {
//        EdgeId = reader.ReadInt32();
//        Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

//    }
//}
//public class VD_VNode
//{
//    public VD_VNode(int id = -1, VD_Graph graph = null)
//    {
//        Id = id;
//        Graph = graph;
//    }

//    internal VD_Graph Graph { get; set; }
//    internal int Id { get; set; }

//    public Vector3 Position { get { return Dual.Position; } }

//    public VD_DPoly Dual { get { return Graph.DPolyLookup[Id]; } }

//    public VD_VEdge Edge { get { return Dual.Edge.Dual; } }
//}
//public class VD_VEdge
//{
//    public VD_VEdge(int id = -1, VD_Graph graph = null)
//    {
//        Id = id;
//        Graph = graph;
//    }

//    internal VD_Graph Graph { get; set; }
//    internal int Id { get; set; }

//    public VD_DEdge Dual { get { return Graph.DEdgeLookup[Id]; } }

//    public VD_VEdge Next { get { return Dual.Twin.Next.Dual; } }

//    public VD_VNode Origin { get { return Dual.Polygon.Dual; } }

//    public VD_VPoly Polygon { get { return Dual.Origin.Dual; } }

//    public VD_VEdge Previous { get { return Dual.Previous.Twin.Dual; } }

//    public VD_VEdge Twin { get { return Dual.Twin.Dual; } }

//}
//public class VD_VPoly
//{
//    public VD_VPoly(int id = -1, VD_Graph graph = null)
//    {
//        Id = id;
//        Graph = graph;
//    }
//    internal VD_Graph Graph { get; set; }
//    internal int Id { get; set; }
//    public Vector3 Position { get { return Dual.Position; } }
//    public VD_DNode Dual { get { return Graph.DNodeLookup[Id]; } }
//    public VD_VEdge Edge { get { return Dual.Edge.Dual; } }
//}
////}