using System.Collections.Generic;
using UnityEngine;

namespace ProceduralMeshFramework.Native
{
    public class ProceduralMeshBuilder
    {
        //Helps relieve memory reallocations to supply an estimated vertex and triangle count
        public ProceduralMeshBuilder(int estimateVerticies = 3, int estimateTriangles = 1)
        {
            Verticies = new List<ProceduralVertex>(estimateVerticies);
            Triangles = new List<ProceduralTriangle>(estimateTriangles);
        }

        public ProceduralMeshBuilder(ProceduralMeshBuilder pmb) : this(pmb.Verticies.Count, pmb.Triangles.Count)
        {
            foreach (var v in pmb.Verticies)
                AddVertex(v);
            foreach (var t in pmb.Triangles)
                AddTriangle(t);
        }

        public List<ProceduralVertex> Verticies { get; private set; }
        public List<ProceduralTriangle> Triangles { get; private set; }

        public int AddVertex(ProceduralVertex vertex)
        {
            var l = Verticies.Count;
            Verticies.Add(vertex);
            return l;
        }

        public void AddTriangle(ProceduralVertex pivotVertex, ProceduralVertex leftVertex, ProceduralVertex rightVertex)
        {
            AddTriangle(AddVertex(pivotVertex), AddVertex(leftVertex), AddVertex(rightVertex));
        }

        public void AddTriangle(int pivotVertex, int leftVertex, int rightVertex)
        {
            AddTriangle(new ProceduralTriangle(pivotVertex, leftVertex, rightVertex));
        }

        public void AddTriangle(ProceduralTriangle triangle)
        {
            Triangles.Add(triangle);
        }

        internal virtual List<ProceduralMeshBuilder> CreateSubmeshes()
        {
            var estimatedChunks = Mathf.CeilToInt((float) Verticies.Count / ushort.MaxValue);
            var Chunks = new List<ProceduralMeshBuilder>(estimatedChunks);
            if (estimatedChunks > 1)
            {
                var ToChunkVertex = new Dictionary<int, int>(ushort.MaxValue);
                var triangleIndex = 0;
                while (triangleIndex < Triangles.Count)
                {
                    var chunk = new ProceduralMeshBuilder();
                    Chunks.Add(chunk);
                    ToChunkVertex.Clear();
                    var hasFailed = false;
                    while (triangleIndex < Triangles.Count && ToChunkVertex.Count < ushort.MaxValue && !hasFailed)
                    {
                        var adding = 0;
                        int pivot = -1, left = -1, right = -1;
                        var triangle = Triangles[triangleIndex];
                        if (!ToChunkVertex.TryGetValue(triangle.Pivot, out pivot))
                        {
                            adding++;
                            pivot = -1;
                        }

                        if (!ToChunkVertex.TryGetValue(triangle.Left, out left))
                        {
                            adding++;
                            left = -1;
                        }

                        if (!ToChunkVertex.TryGetValue(triangle.Right, out right))
                        {
                            adding++;
                            right = -1;
                        }

                        hasFailed = ToChunkVertex.Count + adding > ushort.MaxValue;
                        if (!hasFailed)
                        {
                            if (pivot == -1)
                                pivot = ToChunkVertex[triangle.Pivot] = chunk.AddVertex(Verticies[triangle.Pivot]);
                            if (left == -1)
                                left = ToChunkVertex[triangle.Left] = chunk.AddVertex(Verticies[triangle.Left]);
                            if (right == -1)
                                right = ToChunkVertex[triangle.Right] = chunk.AddVertex(Verticies[triangle.Right]);

                            chunk.AddTriangle(pivot, left, right);

                            triangleIndex++;
                        }
                    }
                }
            }
            else
            {
                Chunks.Add(this);
            }

            return Chunks;
        }

        //If mesh is null, a new mesh is created, otherwise, the mesh is overwritten
        public ProceduralMesh Compile(ProceduralMesh mesh = null)
        {
            if (mesh == null)
                mesh = new ProceduralMesh();

            mesh.CompileFromSubmeshes(CreateSubmeshes());

            return mesh;
        }

        protected void ClearMeshBuilder()
        {
            Verticies.Clear();
            Triangles.Clear();
        }

        public virtual void Clear()
        {
            ClearMeshBuilder();
        }
    }
}