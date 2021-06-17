using System.Collections.Generic;
using UnityEngine;

namespace ProceduralMeshFramework.Native
{


    public class ProceduralMesh
    {
        private readonly List<Mesh> Meshes;

        public ProceduralMesh(int meshes = 1)
        {
            Meshes = new List<Mesh>(meshes);
        }

        public int MeshCount { get; private set; }

        public Mesh GetMesh(int i)
        {
            return Meshes[i];
        }

        internal void CompileFromSubmeshes(List<ProceduralMeshBuilder> chunks)
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
            var
                myColors = new List<Color>();
            var
                myTriangles = new List<int>();

            for (var i = 0; i < chunks.Count; i++)
            {
                if (i >= Meshes.Count)
                {
                    mesh = new Mesh();
                    Meshes.Add(mesh);
                }
                else
                {
                    mesh = Meshes[i];
                }

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

                for (var j = 0; j < chunks[i].Verticies.Count; j++)
                {
                    var vert = chunks[i].Verticies[j];
                    myPositions.Add(vert.Position);
                    myNormals.Add(vert.Normal);
                    myUv1s.Add(vert.Uv);
                    myUv2s.Add(vert.Uv2);
                    myUv3s.Add(vert.Uv3);
                    myUv4s.Add(vert.Uv4);
                    myTangents.Add(vert.HandedTangent);
                    myColors.Add(vert.Color);
                }

                for (var j = 0; j < chunks[i].Triangles.Count; j++)
                    foreach (var k in chunks[i].Triangles[j])
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
            for (var i = chunks.Count; i < Meshes.Count; i++)
                Meshes[i].Clear();
        }
    }
}