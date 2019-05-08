using System;
using UnityEngine;

namespace GraphToMesh
{
    [Serializable]
    public struct PlanetGraphMeshParameters
    {
        public float Radius;
        public Vector3 DistortionScale;
        [Range(0f, 1f)] public float Solidity;
        public float ElevationOffset;

        public static PlanetGraphMeshParameters Default
        {
            get
            {
                return new PlanetGraphMeshParameters
                {
                    DistortionScale = Vector3.one,
                    Solidity = 1f,
                    Radius = 1f,
                    ElevationOffset = 0.01f
                };
            }
        }

        public Vector3 Scale(int elevation)
        {
            return (1f + elevation * ElevationOffset) * Radius * DistortionScale;
        }
    }
}