using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshVisualizer : MonoBehaviour
{

    public bool
        Draw = false,
        Normals,
        Tangents,
        Bitangents,
        TrianglesBased = true;
    public float
        Scale = 1f;
    Mesh Mesh { get { return GetComponent<MeshFilter>().mesh; } }
    private void OnDrawGizmosSelected()
    {
        int[] tris = Mesh.triangles;
        Vector3[] verts = Mesh.vertices;
        Vector3[] norms = Mesh.normals;
        Vector4[] tans = Mesh.tangents;

        int count = TrianglesBased ? tris.Length / 3 : verts.Length;

        for (int i = 0; i < count; i++)
        {
            Vector3 vert = Vector3.zero;
            Vector4 tan = Vector4.zero;
            Vector3 norm = Vector3.zero;

            if (TrianglesBased)
            {
                for (int k = 0; k < 3; k++)
                {
                    int j = tris[i * 3 + k];
                    vert += verts[j];
                    tan += tans[j];
                    norm += norms[j];
                }

                vert /= 3;
                tan /= 3;
                norm /= 3;

            }
            else
            {
                vert = verts[i];
                tan = tans[i];
                norm = norms[i];
            }

            if (Normals)
                DrawNormal(vert, norm);
            if (Tangents)
                DrawTangent(vert, tan);
            if (Bitangents)
                DrawBitangent(vert, norm, tan);
        }
    }
    void DrawNormal(Vector3 p, Vector3 n)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(p, p + Scale * n);
    }
    void DrawTangent(Vector3 p, Vector4 t)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(p, p + Scale * (Vector3)t * t.w);
    }
    void DrawBitangent(Vector3 p, Vector3 n, Vector4 t)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(p, p + Scale * Vector3.Cross(n, (Vector3)t * t.w));
    }
}
