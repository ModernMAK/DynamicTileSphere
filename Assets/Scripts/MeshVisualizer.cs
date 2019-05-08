using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshVisualizer : MonoBehaviour
{
    public bool
        Draw,
        Normals,
        Tangents,
        Bitangents,
        TrianglesBased = true;

    public float
        Scale = 1f;

    private Mesh Mesh
    {
        get { return GetComponent<MeshFilter>().mesh; }
    }

    private void OnDrawGizmosSelected()
    {
        var tris = Mesh.triangles;
        var verts = Mesh.vertices;
        var norms = Mesh.normals;
        var tans = Mesh.tangents;

        var count = TrianglesBased ? tris.Length / 3 : verts.Length;

        for (var i = 0; i < count; i++)
        {
            var vert = Vector3.zero;
            var tan = Vector4.zero;
            var norm = Vector3.zero;

            if (TrianglesBased)
            {
                for (var k = 0; k < 3; k++)
                {
                    var j = tris[i * 3 + k];
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

    private void DrawNormal(Vector3 p, Vector3 n)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(p, p + Scale * n);
    }

    private void DrawTangent(Vector3 p, Vector4 t)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(p, p + Scale * (Vector3) t * t.w);
    }

    private void DrawBitangent(Vector3 p, Vector3 n, Vector4 t)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(p, p + Scale * Vector3.Cross(n, (Vector3) t * t.w));
    }
}