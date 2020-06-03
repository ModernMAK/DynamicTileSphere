using System.Collections.Generic;
using System.IO;
using Graphing.Planet;
using Graphing.Position;
using ProceduralMeshFramework;
using UnityEngine;

namespace GraphToMesh
{
    
    public class MeshGraphConverter
    {
        public static PositionGraph CreateGraph(ProceduralMeshBuilder pmb)
        {
            return CreateGraph(pmb.Verticies.ToArray(), pmb.Triangles.ToArray());
        }

        public static PositionGraph CreateGraph(ProceduralVertex[] mv, ProceduralTriangle[] tris)
        {
            var graph = new PositionGraph(mv.Length, tris.Length * 3, tris.Length);
            var TwinLookup = new Dictionary<PositionEdge, int>(new PositionTwinEdgeComparar());
            //Initialize Nodes
            for (var i = 0; i < mv.Length; i++)
                graph.Nodes.Add(new PositionNode());
            //Initialize Polygons and Edges
            for (var i = 0; i < tris.Length; i++)
            {
                graph.Polys.Add(new PositionPoly());
                for (var j = 0; j < 3; j++)
                    graph.Edges.Add(new PositionEdge());
            }

            //Finalize Nodes (Stage 1)
            for (var i = 0; i < mv.Length; i++)
            {
                var node = graph.Nodes[i];
                node.Identity = i;
                node.Position = mv[i].Position;
            }

            //Finalize Polygons and Nodes (Stage 2)
            for (var i = 0; i < tris.Length; i++)
            {
                var poly = graph.Polys[i];
                poly.Identity = i;
                var triangle = tris[i];
                for (var j = 0; j < 3; j++)
                {
                    var edge = graph.Edges[i * 3 + (j + 0) % 3];
                    var nextEdge = graph.Edges[i * 3 + (j + 2) % 3];
                    var prevEdge = graph.Edges[i * 3 + (j + 1) % 3];
                    var node = graph.Nodes[triangle[j]];

                    edge.Identity = i * 3 + j;

                    prevEdge.Next = nextEdge.Prev = poly.Edge = node.Edge = edge;
                    edge.Next = nextEdge;
                    edge.Prev = prevEdge;
                    edge.Poly = poly;
                    edge.Node = node;
                }

                for (var j = 0; j < 3; j++)
                {
                    var edge = graph.Edges[i * 3 + (j + 0) % 3];
                    int value;
                    if (!TwinLookup.TryGetValue(edge, out value))
                    {
                        TwinLookup.Add(edge, i * 3 + j);
                    }
                    else
                    {
                        var twin = graph.Edges[value];
                        twin.Twin = edge;
                        edge.Twin = twin;
                    }
                }
            }

            graph = graph.Dual<PositionGraph>();

            return graph;
        }

        public static PositionGraph FetchGraph(int subdivisions, ShapeType shape = ShapeType.Icosahedron,
            bool slerp = false, bool clean = false)
        {
            var fName = (slerp ? "Slerped" : "NormalLerped") + shape + "_D" + subdivisions;
            var fPath = Application.persistentDataPath;
            var fExt = ".pos";
            var fullPath = Path.ChangeExtension(Path.Combine(fPath, fName), fExt);
            PositionGraph graph;
            if (File.Exists(fullPath) && !clean)
            {
                Debug.Log("Loaded : " + fullPath);
                graph = new PositionGraph();
                using (var fStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (var bReader = new BinaryReader(fStream))
                    {
                        graph.Deserialize(bReader);
                    }
                }
            }
            else
            {
                Debug.Log("Saved : " + fullPath);
                graph = CreateGraph(subdivisions, shape, slerp);
                using (var fStream = new FileStream(fullPath, FileMode.Create))
                {
                    using (var bWriter = new BinaryWriter(fStream))
                    {
                        graph.Serialize(bWriter);
                    }
                }
            }

            return graph;
        }

        private static PositionGraph CreateGraph(int subdivisions, ShapeType shape, bool slerp = false)
        {
            var pmb = new ProceduralMeshBuilder();
            ProceduralPlatonicSolidGenerator.AddToMeshBuilder(pmb, shape, RadiusType.Normalized);
            ProceduralMeshUtility.Subdivide(pmb, subdivisions, slerp);
            if (!slerp)
                ProceduralMeshUtility
                    .Spherize(pmb); //Normalizing means we spherize the graph, this puts the normalized, in normalized slerp
            return CreateGraph(pmb);
        }


        public static PlanetGraph FetchEmptyPlanetGraph(int subdivisions, ShapeType shape = ShapeType.Icosahedron,
            bool slerp = false, int partitionSize = 32, bool clean = false)
        {
            return CreateEmptyPlanetGraph(FetchGraph(subdivisions, shape, slerp, clean), partitionSize);
        }

        public static PlanetGraph CreateEmptyPlanetGraph(PositionGraph position, int partitionSize = 32)
        {
            int nCount = position.Nodes.Count,
                eCount = position.Edges.Count,
                pCount = position.Polys.Count,
                partCount = pCount / partitionSize + (pCount % partitionSize > 0 ? 1 : 0);
            var graph = new PlanetGraph(nCount, eCount, pCount, partitionSize);

            for (var i = 0; i < nCount; i++)
                graph.Nodes.Add(
                    new PlanetNode
                    {
                        Identity = i
                    }
                );

            for (var i = 0; i < eCount; i++)
                graph.Edges.Add(
                    new PlanetEdge
                    {
                        Identity = i
                    }
                );

            for (var i = 0; i < pCount; i++)
                graph.Polys.Add(
                    new PlanetPoly
                    {
                        Identity = i
                    }
                );
            for (var i = 0; i < partCount; i++)
                graph.Partitions.Add(new PlanetPartition(partitionSize));

            for (var i = 0; i < nCount || i < eCount || i < pCount; i++)
            {
                if (i < nCount)
                {
                    var node = graph.Nodes[i];
                    var refNode = position.Nodes[i];
                    node.Edge = graph.Edges[refNode.Edge.Identity];
                    node.Position = refNode.Position;
                }

                if (i < eCount)
                {
                    var edge = graph.Edges[i];
                    var refEdge = position.Edges[i];
                    edge.Next = refEdge.Next != null ? graph.Edges[refEdge.Next.Identity] : null;
                    edge.Twin = refEdge.Twin != null ? graph.Edges[refEdge.Twin.Identity] : null;
                    edge.Prev = refEdge.Prev != null ? graph.Edges[refEdge.Prev.Identity] : null;
                    edge.Node = graph.Nodes[refEdge.Node.Identity];
                    edge.Poly = graph.Polys[refEdge.Poly.Identity];
                }

                if (i < pCount)
                {
                    var poly = graph.Polys[i];
                    var refPoly = position.Polys[i];
                    poly.Edge = graph.Edges[refPoly.Edge.Identity];
                    graph.Partitions[i / partitionSize].Add(poly);
                }
            }

            return graph;
        }

        private class PositionTwinEdgeComparar : IEqualityComparer<PositionEdge>
        {
            public bool Equals(PositionEdge x, PositionEdge y)
            {
                var xNow = x.Node;
                var yNow = y.Node;
                var xNext = x.Next.Node;
                var yNext = y.Next.Node;
                return xNow == yNext && xNext == yNow;
            }

            public int GetHashCode(PositionEdge obj)
            {
                var objNow = obj.Node;
                var objNext = obj.Next.Node;
                return objNow.GetHashCode() ^ objNext.GetHashCode();
            }
        }
    }
}