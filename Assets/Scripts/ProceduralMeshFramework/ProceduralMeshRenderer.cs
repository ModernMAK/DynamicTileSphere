using System.Collections.Generic;
using UnityEngine;

namespace ProceduralMeshFramework
{
    public class ProceduralMeshRenderer : MonoBehaviour
    {
        [SerializeField] private Material[] _Materials;

        [SerializeField] private ProceduralMesh _Mesh;

        private List<GameObject> _SubMeshes;
        private int activeSubIndex, prevMaterialLength = -1;

        public ProceduralMesh Mesh
        {
            get { return _Mesh; }
            set
            {
                var update = (_Mesh != value || activeSubIndex != value.MeshCount) && value != null;
                _Mesh = value;
                if (update) UpdateMesh();
            }
        }

        public Material[] Materials
        {
            get { return _Materials; }
            set
            {
                var update = (_Materials != value || _Materials.Length != prevMaterialLength) && value != null;
                _Materials = value;
                prevMaterialLength = _Materials.Length;
                if (update) UpdateMaterial();
            }
        }

        private void OnEnable()
        {
            foreach (var go in _SubMeshes)
                go.SetActive(true);
        }

        private void OnDisable()
        {
            foreach (var go in _SubMeshes)
                go.SetActive(false);
        }

        public void ForceUpdate(bool forceMesh = true, bool forceMat = true)
        {
            if (forceMesh)
                UpdateMesh();
            if (forceMat)
                UpdateMaterial();
        }


        private void Awake()
        {
            _SubMeshes = new List<GameObject>();
        }

        public void Update()
        {
            if (_Mesh != null && activeSubIndex != _Mesh.MeshCount)
                UpdateMesh();
            if (_Materials != null && _Materials.Length != prevMaterialLength)
                UpdateMaterial();
        }

        private void UpdateMesh()
        {
            activeSubIndex = 0;
            for (var i = 0; i < _Mesh.MeshCount; i++)
                if (i >= _SubMeshes.Count)
                    _SubMeshes.Add(CreateSubMesh(_Mesh.GetMesh(i)));
                else
                    _SubMeshes[i].GetComponent<MeshFilter>().mesh = _Mesh.GetMesh(i);
            activeSubIndex = _Mesh.MeshCount;
        }

        private void UpdateMaterial()
        {
            foreach (var go in _SubMeshes)
                go.GetComponent<MeshRenderer>().materials = _Materials;
        }

        private GameObject CreateSubMesh(Mesh m)
        {
            var go = new GameObject(name + " (Submesh" + _SubMeshes.Count + ")");

            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.SetParent(transform, false);

            var mf = go.AddComponent<MeshFilter>();
            mf.mesh = m;

            var mc = go.AddComponent<MeshCollider>();
            mc.sharedMesh = m;

            var mr = go.AddComponent<MeshRenderer>();
            mr.materials = _Materials;

            return go;
        }
    }
}