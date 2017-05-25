using System.Collections.Generic;
using UnityEngine;

public class ProceduralMesh
{
    public ProceduralMesh(int meshes = 1) { Meshes = new List<Mesh>(meshes); }

    private List<Mesh> Meshes;
    public int MeshCount { get; private set; }

    public Mesh GetMesh(int i) { return Meshes[i]; }
    //public int GetMeshCount(int i) { return Meshes[i]; }

    internal void CompileFromSubmeshes(List<ProceduralMeshBuilder> chunks)// /*List<Vector3> positions, IList<Vector4> uv1s, IList<Vector4> uv2s, IList<Vector4> uv3s, IList<Vector4> uv4s, IList<Vector3> normals, IList<Vector4> tangents, IList<Color> colors,*/IList<ProceduralVertex> verticies, IList<ProceduralTriangle> triangles)
    {
        Mesh mesh = null;
        List<Vector3>
            myPositions = new List<Vector3>(),
            myNormals = new List<Vector3>();
        List<Vector4>
            myUv1s = new List<Vector4>(),
            myUv2s = new List<Vector4>(),
            myUv3s = new List<Vector4>(),
            myUv4s = new List<Vector4>(),
            myTangents = new List<Vector4>();
        List<Color>
            myColors = new List<Color>();
        List<int>
            myTriangles = new List<int>();

        for (int i = 0; i < chunks.Count; i++)
        {
            if (i >= Meshes.Count)
            {
                mesh = new Mesh();
                Meshes.Add(mesh);
            }
            else mesh = Meshes[i];
            mesh.Clear();

            myTriangles.Clear();
            myPositions.Clear();
            myNormals.Clear();
            myUv1s.Clear();
            myUv2s.Clear();
            myUv3s.Clear();
            myUv4s.Clear();
            myTangents.Clear();
            myColors.Clear();

            for (int j = 0; j < chunks[i].Verticies.Count; j++)
            {
                ProceduralVertex vert = chunks[i].Verticies[j];
                myPositions.Add(vert.Position);
                myNormals.Add(vert.Normal);
                myUv1s.Add(vert.Uv);
                myUv2s.Add(vert.Uv2);
                myUv3s.Add(vert.Uv3);
                myUv4s.Add(vert.Uv4);
                myTangents.Add(vert.HandedTangent);
                myColors.Add(vert.Color);
            }
            for (int j = 0; j < chunks[i].Triangles.Count; j++)
                foreach (int k in chunks[i].Triangles[j])
                    myTriangles.Add(k);

            mesh.SetVertices(myPositions);
            mesh.SetNormals(myNormals);
            mesh.SetTangents(myTangents);
            mesh.SetColors(myColors);
            mesh.SetUVs(0, myUv1s);
            mesh.SetUVs(1, myUv2s);
            mesh.SetUVs(2, myUv3s);
            mesh.SetUVs(3, myUv4s);
            mesh.SetTriangles(myTriangles, 0);
        }
        MeshCount = chunks.Count;
        for (int i = chunks.Count; i < Meshes.Count; i++)
            Meshes[i].Clear();

        //for(int i = 0; i< )

        //{
        //    int
        //        triangleCounter = 0,
        //        meshCounter = 0;

        //    HashSet<int> 
        //        triangleLookup = new HashSet<int>();       
        //    List<Vector3>
        //        partitionedPositions = new List<Vector3>(ushort.MaxValue),
        //        partitionedNormals = new List<Vector3>(ushort.MaxValue);
        //    List<Vector4>
        //        partitionedTangents = new List<Vector4>(ushort.MaxValue),
        //        partitionedUv1s = new List<Vector4>(ushort.MaxValue),
        //        partitionedUv2s = new List<Vector4>(ushort.MaxValue),
        //        partitionedUv3s = new List<Vector4>(ushort.MaxValue),
        //        partitionedUv4s = new List<Vector4>(ushort.MaxValue);
        //    List<Color>
        //        partitionedColors = new List<Color>(ushort.MaxValue);
        //    Mesh mesh = null;

        //    while (triangleCounter < triangles.Count/3)
        //    {
        //        if (Meshes.Count < meshCounter)
        //        {
        //            mesh = new Mesh();
        //            Meshes.Add(mesh);
        //        }
        //        else
        //        {
        //            mesh = Meshes[meshCounter];
        //            mesh.Clear();
        //        }
        //        meshCounter++;

        //        bool hasFailed = false;
        //        while (triangleCounter < triangles.Count / 3 && !hasFailed)
        //        {
        //            int adding = 0;
        //            if (triangleLookup.Contains(triangles[triangleCounter * 3 + 0]))
        //                adding++;
        //            if (triangleLookup.Contains(triangles[triangleCounter * 3 + 1]))
        //                adding++;
        //            if (triangleLookup.Contains(triangles[triangleCounter * 3 + 2]))
        //                adding++;
        //            hasFailed = (triangleLookup.Count + adding) <= ushort.MaxValue;
        //            if (!hasFailed)
        //            {
        //                partitionedPositions.Add()
        //                triangleCounter++;
        //            }
        //        }
        //    }



        //    //int 
        //    //    length = positions.Count,
        //    //    vertsLeft = length,
        //    //    vertCounter = 0,
        //    //    runningCounter = 0;

        //    //List<Vector3>
        //    //    partitionedPositions = new List<Vector3>(ushort.MaxValue),
        //    //    partitionedNormals = new List<Vector3>(ushort.MaxValue);
        //    //List<Vector4>
        //    //    partitionedTangents = new List<Vector4>(ushort.MaxValue),
        //    //    partitionedUv1s = new List<Vector4>(ushort.MaxValue),
        //    //    partitionedUv2s = new List<Vector4>(ushort.MaxValue),
        //    //    partitionedUv3s = new List<Vector4>(ushort.MaxValue),
        //    //    partitionedUv4s = new List<Vector4>(ushort.MaxValue);
        //    //List<Color>
        //    //    partitionedColors = new List<Color>(ushort.MaxValue);
        //    //do
        //    //{
        //    //    vertCounter = 0;
        //    //    partitionedPositions.Clear();
        //    //    partitionedNormals.Clear();
        //    //    partitionedTangents.Clear();
        //    //    partitionedUv1s.Clear();
        //    //    partitionedUv2s.Clear();
        //    //    partitionedUv3s.Clear();
        //    //    partitionedUv4s.Clear();
        //    //    partitionedColors.Clear();
        //    //    for (int i = 0; i < vertsLeft && i < ushort.MaxValue; i++)
        //    //    {
        //    //        vertCounter++;
        //    //        runningCounter++;

        //    //        partitionedPositions.Add(positions[runningCounter]);
        //    //        partitionedNormals.Add(normals[runningCounter]);
        //    //        partitionedTangents.Add(tangents[runningCounter]);
        //    //        partitionedUv1s.Add(uv1s[runningCounter]);
        //    //        partitionedUv2s.Add(uv2s[runningCounter]);
        //    //        partitionedUv3s.Add(uv3s[runningCounter]);
        //    //        partitionedUv4s.Add(uv4s[runningCounter]);
        //    //        partitionedColors.Add(colors[runningCounter]);
        //    //    }
        //    //    vertsLeft -= vertCounter;

        //    //} while (vertsLeft > 0);



    }   
}
