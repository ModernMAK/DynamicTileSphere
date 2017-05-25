using System.Collections.Generic;
using UnityEngine;

public class ProceduralMeshRenderer : MonoBehaviour
{

    private void OnEnable()
    {
        foreach (GameObject go in _SubMeshes)
            go.SetActive(true);
    }
    private void OnDisable()
    {
        foreach (GameObject go in _SubMeshes)
            go.SetActive(false);
    }

    [SerializeField]
    private ProceduralMesh _Mesh;
    [SerializeField]
    private Material[] _Materials;
    private List<GameObject> _SubMeshes;
    private int activeSubIndex = 0, prevMaterialLength = -1;
    public void ForceUpdate(bool forceMesh = true, bool forceMat = true)
    {
        if (forceMesh)
            UpdateMesh();
        if (forceMat)
            UpdateMaterial();
    }
    public ProceduralMesh Mesh
    {
        get { return _Mesh; }
        set
        {
            bool update = (_Mesh != value || activeSubIndex != value.MeshCount) && (value != null);
            _Mesh = value;
            if (update) UpdateMesh();
        }
    }
    public Material[] Materials
    {
        get { return _Materials; }
        set
        {
            bool update = (_Materials != value || _Materials.Length != prevMaterialLength) && (value != null);
            _Materials = value;
            prevMaterialLength = _Materials.Length;
            if (update) UpdateMaterial();
        }
    }


    private void Awake()
    {
        _SubMeshes = new List<GameObject>();
    }

    public void Update()
    {
        if (_Mesh != null && (activeSubIndex != _Mesh.MeshCount))
            UpdateMesh();
        if (_Materials != null && (_Materials.Length != prevMaterialLength))
            UpdateMaterial();
    }

    void UpdateMesh()
    {
        activeSubIndex = 0;
        for (int i = 0; i < _Mesh.MeshCount; i++)
        {
            if (i >= _SubMeshes.Count)
                _SubMeshes.Add(CreateSubMesh(_Mesh.GetMesh(i)));
            else
                _SubMeshes[i].GetComponent<MeshFilter>().mesh = _Mesh.GetMesh(i);
        }
        activeSubIndex = _Mesh.MeshCount;
    }
    void UpdateMaterial()
    {
        foreach (GameObject go in _SubMeshes)
            go.GetComponent<MeshRenderer>().materials = _Materials;
    }
    GameObject CreateSubMesh(Mesh m)
    {
        GameObject go = new GameObject(name + " (Submesh" + _SubMeshes.Count + ")");

        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.transform.SetParent(transform, false);

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = m;

        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = m;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.materials = _Materials;

        return go;
    }
}
