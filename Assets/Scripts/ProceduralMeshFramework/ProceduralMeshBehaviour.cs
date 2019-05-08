using System;
using UnityEngine;

namespace ProceduralMeshFramework
{
    [RequireComponent(typeof(ProceduralMeshRenderer))]
    public class ProceduralMeshBehaviour : MonoBehaviour
    {
        [SerializeField] protected bool _regenerate;


        public ProceduralMeshRenderer PMR { get; private set; }
        private ProceduralMesh PM { get; set; }
        protected ProceduralMeshBuilder PMB { get; set; }


        public void Awake()
        {
            PMR = GetComponent<ProceduralMeshRenderer>();
            PM = new ProceduralMesh();
            PMB = new ProceduralMeshBuilder();
            PMR.Mesh = PM;

            Initialize();
        }

        protected virtual void Initialize()
        {
        }

        public void Update()
        {
            if (_regenerate)
            {
                _regenerate = false;
                GenerateMesh();
                CompileMesh();
            }
        }

        private void CompileMesh()
        {
            PMB.Compile(PM);
        }

        protected virtual void GenerateMesh()
        {
            throw new NotImplementedException("GenerateMesh has not been provided for " + GetType());
        }
    }
}