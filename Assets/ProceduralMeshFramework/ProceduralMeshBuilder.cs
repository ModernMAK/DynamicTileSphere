using System.Collections.Generic;
using UnityEngine;
public class ProceduralMeshBuilder
{
    //Helps relieve memore reallocations to supply an estimated vertex and triangle count
    public ProceduralMeshBuilder(int estimateVerticies = 3, int estimateTriangles = 1)
    {
        Verticies = new List<ProceduralVertex>(estimateVerticies);
        Triangles = new List<ProceduralTriangle>(estimateTriangles);
    }
    public ProceduralMeshBuilder(ProceduralMeshBuilder pmb) : this(pmb.Verticies.Count, pmb.Triangles.Count)
    {
        foreach (ProceduralVertex v in pmb.Verticies)
            AddVertex(v);
        foreach (ProceduralTriangle t in pmb.Triangles)
            AddTriangle(t);

    }

    public List<ProceduralVertex> Verticies { get; private set; }
    public List<ProceduralTriangle> Triangles { get; private set; }

    public int AddVertex(ProceduralVertex vertex)
    {
        int l = Verticies.Count;
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

    internal List<ProceduralMeshBuilder> CreateSubmeshes()
    {
        int estimatedChunks = Mathf.CeilToInt((float)Verticies.Count / ushort.MaxValue);
        List<ProceduralMeshBuilder> Chunks = new List<ProceduralMeshBuilder>(estimatedChunks);
        if (estimatedChunks > 1)
        {
            Dictionary<int, int> ToChunkVertex = new Dictionary<int, int>(ushort.MaxValue);
            int triangleIndex = 0;
            while (triangleIndex < Triangles.Count)
            {
                ProceduralMeshBuilder chunk = new ProceduralMeshBuilder();
                Chunks.Add(chunk);
                ToChunkVertex.Clear();
                bool hasFailed = false;
                while (triangleIndex < Triangles.Count && ToChunkVertex.Count < ushort.MaxValue && !hasFailed)
                {
                    int adding = 0;
                    int pivot = -1, left = -1, right = -1;
                    ProceduralTriangle triangle = Triangles[triangleIndex];
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
        else Chunks.Add(this);
        return Chunks;
    }

    //If mesh is null, a new mesh is created, otherwise, the mesh is overwritten
    public ProceduralMesh Compile(ProceduralMesh mesh = null)
    {
        if (mesh == null)
            mesh = new ProceduralMesh();

        //List<Vector3> positions = new List<Vector3>(Verticies.Count);
        //List<Vector3> normals = new List<Vector3>(Verticies.Count);
        //List<Vector4> tangents = new List<Vector4>(Verticies.Count);
        //List<Vector4>[] uvs = new List<Vector4>[4] { new List<Vector4>(Verticies.Count), new List<Vector4>(Verticies.Count), new List<Vector4>(Verticies.Count), new List<Vector4>(Verticies.Count) };
        //List<Color> colors = new List<Color>(Verticies.Count);
        //List<int> triangles = new List<int>(Triangles.Count * 3);


        //foreach (ProceduralVertex vertex in Verticies)
        //{
        //    positions.Add(vertex.Position);
        //    normals.Add(vertex.Normal);
        //    tangents.Add(vertex.HandedTangent);
        //    colors.Add(vertex.Color);
        //    for (int i = 0; i < 4; i++)
        //        uvs[i].Add(vertex.Uvs[i]);
        //}
        //foreach (ProceduralTriangle triangle in Triangles)
        //    triangles.AddRange(triangle);

        //mesh.Update(positions, uvs[0], uvs[1], uvs[2], uvs[3], normals, tangents, colors, triangles);

        mesh.CompileFromSubmeshes(this.CreateSubmeshes());// (Verticies, Triangles);

        return mesh;
    }

    public void Clear()
    {
        Verticies.Clear();
        Triangles.Clear();
    }
}