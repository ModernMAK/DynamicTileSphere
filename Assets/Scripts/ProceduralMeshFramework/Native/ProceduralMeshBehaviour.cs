using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProceduralMeshFramework.Native
{
    [RequireComponent(typeof(ProceduralMeshRenderer))]
    public class ProceduralMeshBehaviour : MonoBehaviour
    {
        [SerializeField] public bool Regenerate;


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
            if (Regenerate)
            {
                Regenerate = false;
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