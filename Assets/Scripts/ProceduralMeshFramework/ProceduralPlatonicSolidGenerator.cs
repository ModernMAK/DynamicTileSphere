using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMeshFramework
{
    public static class PlatonicSolidGenerator
    {
        private const float
            PHI = 1.61803398875f,
            INV_PHI = 1f / PHI,

            //These 4 numbers are used for the Cirucm/Mid/In radius stuff
            XI = 1.17557050458f,
            SQRT_6 = 2.44948974278f,
            SQRT_2 = 1.41421356237f,
            SQRT_3 = 1.73205080757f;

        const int ScaleJobBatchSize = 1024;

        private struct ScaleJob : IJobParallelFor
        {
            [ReadOnly] public float Scale;
            public NativeArray<float3> Vertexes;

            public void Execute(int index)
            {
                Vertexes[index] = Vertexes[index] * Scale;
            }
        }

        private static JobHandle Scale(float radius, NativeArray<float3> vertexes, JobHandle depends = default)
        {
            return new ScaleJob()
            {
                Scale = radius,
                Vertexes = vertexes
            }.Schedule(vertexes.Length, ScaleJobBatchSize, depends);
        }

        private static NativeArray<T> GetNative<T>(T[] original, Allocator allocator) where T : struct
        {
            var native = new NativeArray<T>(original.Length, allocator);
            native.CopyFrom(original);
            return native;
        }

        public static class Tetrahedron
        {
            public const float
                Circumradius = SQRT_3 / SQRT_2,
                Midradius = 1 / SQRT_2,
                Inradius = 1 / SQRT_6;


            public static readonly float3[] Vertexes = new float3[4]
            {
                new Vector3(1f, 1f, 1f).normalized,
                new Vector3(1f, -1f, -1f).normalized,
                new Vector3(-1f, 1f, -1f).normalized,
                new Vector3(-1f, -1f, 1f).normalized
            };

            public static NativeArray<float3> GetNativeVertexes(Allocator allocator) =>
                GetNative(Vertexes, allocator);

            //As a platonic solid, vertex doubles as normal
            public static float3[] Normals => Vertexes;

            public static NativeArray<float3> GetNativeNormals(Allocator allocator) =>
                GetNative(Normals, allocator);

            public static float4[] Tangents =>
                throw new NotImplementedException("Tangents is arbitrary, as long as it's consistent");

            public static NativeArray<float4> GetNativeTangents(Allocator allocator) =>
                GetNative(Tangents, allocator);

            private static readonly int3[] Triangles = new int3[4]
            {
                new int3(0, 1, 2),
                new int3(3, 1, 0),
                new int3(0, 2, 3),
                new int3(3, 2, 1)
            };

            public static NativeArray<float4> GetNativeTriangles(Allocator allocator) =>
                GetNative(Tangents, allocator);

            public static float GetRadiusScale(RadiusType radiusType)
            {
                switch (radiusType)
                {
                    case RadiusType.Circumradius:
                        return Circumradius;
                    case RadiusType.Midradius:
                        return Midradius;
                    case RadiusType.Inradius:
                        return Inradius;
                    case RadiusType.Normalized:
                        return 1f;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(radiusType), radiusType, null);
                }
            }

            public static JobHandle Scale(RadiusType radiusType, NativeArray<float3> vertexes,
                JobHandle depends = default)
                => PlatonicSolidGenerator.Scale(GetRadiusScale(radiusType), vertexes, depends);
        }


        public static class Octahedron
        {
            public const float
                Circumradius = SQRT_2,
                Midradius = 1,
                Inradius = SQRT_2 / SQRT_3;


            public static readonly float3[] Vertexes = new float3[6]
            {
                new Vector3(1f, 0f, 0f).normalized,
                new Vector3(0f, 1f, 0f).normalized,
                new Vector3(0f, 0f, 1f).normalized,
                new Vector3(-1f, 0f, 0f).normalized,
                new Vector3(0f, -1f, 0f).normalized,
                new Vector3(0f, 0f, -1f).normalized
            };

            public static NativeArray<float3> GetNativeVertexes(Allocator allocator) =>
                GetNative(Vertexes, allocator);

            //As a platonic solid, vertex doubles as normal
            public static float3[] Normals => Vertexes;

            public static NativeArray<float3> GetNativeNormals(Allocator allocator) =>
                GetNative(Normals, allocator);

            public static float4[] Tangents =>
                throw new NotImplementedException("Tangents is arbitrary, as long as it's consistent");

            public static NativeArray<float4> GetNativeTangents(Allocator allocator) =>
                GetNative(Tangents, allocator);

            private static readonly int3[] Triangles = new int3[8]
            {
                new int3(0, 1, 2),
                new int3(0, 2, 4),
                new int3(0, 4, 5),
                new int3(0, 5, 1),
                new int3(3, 2, 1),
                new int3(3, 4, 2),
                new int3(3, 5, 4),
                new int3(3, 1, 5),
            };

            public static NativeArray<float4> GetNativeTriangles(Allocator allocator) =>
                GetNative(Tangents, allocator);

            public static float GetRadiusScale(RadiusType radiusType)
            {
                switch (radiusType)
                {
                    case RadiusType.Circumradius:
                        return Circumradius;
                    case RadiusType.Midradius:
                        return Midradius;
                    case RadiusType.Inradius:
                        return Inradius;
                    case RadiusType.Normalized:
                        return 1f;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(radiusType), radiusType, null);
                }
            }

            public static JobHandle Scale(RadiusType radiusType, NativeArray<float3> vertexes,
                JobHandle depends = default)
                => PlatonicSolidGenerator.Scale(GetRadiusScale(radiusType), vertexes, depends);
        }

        public static class Cube
        {
            public const float
                Circumradius = SQRT_3,
                Midradius = SQRT_2,
                Inradius = 1f;


            public static readonly float3[] Vertexes = new float3[8]
            {
                new Vector3(1f, 1f, 1f).normalized,
                new Vector3(1f, 1f, -1f).normalized,
                new Vector3(1f, -1f, 1f).normalized,
                new Vector3(1f, -1f, -1f).normalized,
                new Vector3(-1f, 1f, 1f).normalized,
                new Vector3(-1f, 1f, -1f).normalized,
                new Vector3(-1f, -1f, 1f).normalized,
                new Vector3(-1f, -1f, -1f).normalized
            };

            public static NativeArray<float3> GetNativeVertexes(Allocator allocator) =>
                GetNative(Vertexes, allocator);

            //As a platonic solid, vertex doubles as normal
            public static float3[] Normals => Vertexes;

            public static NativeArray<float3> GetNativeNormals(Allocator allocator) =>
                GetNative(Normals, allocator);

            public static float4[] Tangents =>
                throw new NotImplementedException("Tangents is arbitrary, as long as it's consistent");

            public static NativeArray<float4> GetNativeTangents(Allocator allocator) =>
                GetNative(Tangents, allocator);

            private static readonly int3[] Triangles = new int3[12]
            {
                //Bot
                new int3(2, 1, 0),
                new int3(1, 2, 3),
                //For
                new int3(5, 4, 0),
                new int3(1, 5, 0),
                //Right
                new int3(1, 3, 7),
                new int3(7, 5, 1),
                //Top
                new int3(4, 5, 7),
                new int3(7, 6, 4),
                //Back
                new int3(2, 6, 7),
                new int3(2, 7, 3),
                //Left
                new int3(4, 2, 0),
                new int3(2, 4, 6),
            };

            public static NativeArray<float4> GetNativeTriangles(Allocator allocator) =>
                GetNative(Tangents, allocator);

            public static float GetRadiusScale(RadiusType radiusType)
            {
                switch (radiusType)
                {
                    case RadiusType.Circumradius:
                        return Circumradius;
                    case RadiusType.Midradius:
                        return Midradius;
                    case RadiusType.Inradius:
                        return Inradius;
                    case RadiusType.Normalized:
                        return 1f;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(radiusType), radiusType, null);
                }
            }

            public static JobHandle Scale(RadiusType radiusType, NativeArray<float3> vertexes,
                JobHandle depends = default)
                => PlatonicSolidGenerator.Scale(GetRadiusScale(radiusType), vertexes, depends);
        }

        public static class Icosahedron
        {
            public const float
                Circumradius = PHI * PHI / SQRT_3,
                Midradius = PHI,
                Inradius = XI * PHI;


            public static readonly float3[] Vertexes = new float3[12]
            {
                new Vector3(0f, 1f, PHI).normalized,
                new Vector3(0f, -1f, PHI).normalized,
                new Vector3(0f, 1f, -PHI).normalized,
                new Vector3(0f, -1f, -PHI).normalized,

                new Vector3(1f, PHI, 0f).normalized,
                new Vector3(-1f, PHI, 0f).normalized,
                new Vector3(1f, -PHI, 0f).normalized,
                new Vector3(-1f, -PHI, 0f).normalized,

                new Vector3(PHI, 0f, 1f).normalized,
                new Vector3(PHI, 0f, -1f).normalized,
                new Vector3(-PHI, 0f, 1f).normalized,
                new Vector3(-PHI, 0f, -1f).normalized
            };

            public static NativeArray<float3> GetNativeVertexes(Allocator allocator) =>
                GetNative(Vertexes, allocator);

            //As a platonic solid, vertex doubles as normal
            public static float3[] Normals => Vertexes;

            public static NativeArray<float3> GetNativeNormals(Allocator allocator) =>
                GetNative(Normals, allocator);

            public static float4[] Tangents =>
                throw new NotImplementedException("Tangents is arbitrary, as long as it's consistent");

            public static NativeArray<float4> GetNativeTangents(Allocator allocator) =>
                GetNative(Tangents, allocator);

            private static readonly int3[] Triangles = new int3[20]
            {
                //One peak, at 8
                new int3(0, 1, 8),
                new int3(4, 0, 8),
                new int3(9, 4, 8),
                new int3(6, 9, 8),
                new int3(1, 6, 8),

                //The other peak, at 11
                new int3(5, 2, 11),
                new int3(2, 3, 11),
                new int3(3, 7, 11),
                new int3(7, 10, 11),
                new int3(10, 5, 11),

                //The Strip
                new int3(0, 10, 1),
                new int3(10, 7, 1),
                new int3(7, 6, 1),
                new int3(7, 3, 6),
                new int3(3, 9, 6),
                new int3(2, 9, 3),
                new int3(2, 4, 9),
                new int3(2, 5, 4),
                new int3(4, 5, 0),
                new int3(5, 10, 0)
            };

            public static NativeArray<float4> GetNativeTriangles(Allocator allocator) =>
                GetNative(Tangents, allocator);

            public static float GetRadiusScale(RadiusType radiusType)
            {
                switch (radiusType)
                {
                    case RadiusType.Circumradius:
                        return Circumradius;
                    case RadiusType.Midradius:
                        return Midradius;
                    case RadiusType.Inradius:
                        return Inradius;
                    case RadiusType.Normalized:
                        return 1f;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(radiusType), radiusType, null);
                }
            }

            public static JobHandle Scale(RadiusType radiusType, NativeArray<float3> vertexes,
                JobHandle depends = default)
                => PlatonicSolidGenerator.Scale(GetRadiusScale(radiusType), vertexes, depends);
        }

        public static class Dodecahedron
        {
            public const float
                Circumradius = PHI * PHI / XI,
                Midradius = PHI * PHI,
                Inradius = SQRT_3 * PHI;

            private const float
                INV_SQRT_3 = 1f / SQRT_3;


            public static readonly float3[] Vertexes = new float3[20 + 12]
            {
                new Vector3(1f, 1f, 1f) * INV_SQRT_3, //0
                new Vector3(1f, 1f, -1f) * INV_SQRT_3, //1
                new Vector3(1f, -1f, 1f) * INV_SQRT_3, //2
                new Vector3(1f, -1f, -1f) * INV_SQRT_3, //3
                new Vector3(-1f, 1f, 1f) * INV_SQRT_3, //4
                new Vector3(-1f, 1f, -1f) * INV_SQRT_3, //5
                new Vector3(-1f, -1f, 1f) * INV_SQRT_3, //6
                new Vector3(-1f, -1f, -1f) * INV_SQRT_3, //7

                new Vector3(0f, INV_PHI, PHI) * INV_SQRT_3, //8
                new Vector3(0f, -INV_PHI, PHI) * INV_SQRT_3, //9
                new Vector3(0f, INV_PHI, -PHI) * INV_SQRT_3, //10
                new Vector3(0f, -INV_PHI, -PHI) * INV_SQRT_3, //11

                new Vector3(INV_PHI, PHI, 0f) * INV_SQRT_3, //12
                new Vector3(-INV_PHI, PHI, 0f) * INV_SQRT_3, //13
                new Vector3(INV_PHI, -PHI, 0f) * INV_SQRT_3, //14
                new Vector3(-INV_PHI, -PHI, 0f) * INV_SQRT_3, //15

                new Vector3(PHI, 0f, INV_PHI) * INV_SQRT_3, //16
                new Vector3(PHI, 0f, -INV_PHI) * INV_SQRT_3, //17
                new Vector3(-PHI, 0f, INV_PHI) * INV_SQRT_3, //18
                new Vector3(-PHI, 0f, -INV_PHI) * INV_SQRT_3, //19  


                new Vector3(2f + 2 * PHI + INV_PHI, 2f + PHI, 0) * INV_SQRT_3 / 5, //20 ~ AVG(0, 16, 17, 1, 12)
                new Vector3(2f + PHI, 2f, 0 + INV_PHI + 2 * PHI) * INV_SQRT_3 / 5, //21 ~ AVG(2, 16, 0, 8, 9)
                new Vector3(2f+2*PHI+INV_PHI, -2f-PHI, 0f) * INV_SQRT_3 / 5, //22 ~ AVG(14,3,17,16,2)
                
                new Vector3(0, 0, 0) * INV_SQRT_3 / 5,
                new Vector3(0, 0, 0) * INV_SQRT_3 / 5,
                new Vector3(0, 0, 0) * INV_SQRT_3 / 5,
                new Vector3(0, 0, 0) * INV_SQRT_3 / 5,
                new Vector3(0, 0, 0) * INV_SQRT_3 / 5,
                new Vector3(0, 0, 0) * INV_SQRT_3 / 5,
                new Vector3(0, 0, 0) * INV_SQRT_3 / 5,
                new Vector3(0, 0, 0) * INV_SQRT_3 / 5,
                new Vector3(0, 0, 0) * INV_SQRT_3 / 5,
            };

            /*
                {0, 12, 13, 4, 8},
                {1, 10, 5, 13, 12},
                {13, 5, 19, 18, 4},
                {6, 9, 8, 4, 18},
                {2, 9, 6, 15, 14},
                {1, 17, 3, 11, 10},
                {7, 19, 5, 10, 11},
                {3, 14, 15, 7, 11},
                {19, 7, 15, 6, 18}
             */
            public static NativeArray<float3> GetNativeVertexes(Allocator allocator) =>
                GetNative(Vertexes, allocator);

            //As a platonic solid, vertex doubles as normal
            public static float3[] Normals => Vertexes;

            public static NativeArray<float3> GetNativeNormals(Allocator allocator) =>
                GetNative(Normals, allocator);

            public static float4[] Tangents =>
                throw new NotImplementedException("Tangents is arbitrary, as long as it's consistent");

            public static NativeArray<float4> GetNativeTangents(Allocator allocator) =>
                GetNative(Tangents, allocator);

            private static readonly int3[] Triangles = new int3[60]
            {
                new int3(0, 20, 16),
                new int3(16, 20, 17),
                new int3(17, 20, 1),
                new int3(1, 20, 12),
                new int3(12, 20, 0),
                
                new int3(2, 21, 16),
                new int3(16, 21, 0),
                new int3(0, 21, 8),
                new int3(8, 21, 9),
                new int3(9, 21, 2),
                
                new int3(14, 22, 3),
                new int3(3, 22, 17),
                new int3(17, 22, 16),
                new int3(16, 22, 2),
                new int3(2, 22, 14),
                
                new int3(0, 23, 12),
                new int3(12, 23, 13),
                new int3(13, 23, 4),
                new int3(4, 23, 8),
                new int3(8, 23, 0),
                
                new int3(1, 24, 10),
                new int3(10, 24, 5),
                new int3(5, 24, 13),
                new int3(13, 24, 12),
                new int3(12, 24, 1),
                
                new int3(13, 25, 5),
                new int3(5, 25, 19),
                new int3(19, 25, 18),
                new int3(18, 25, 4),
                new int3(4, 25, 13),
                
                new int3(6, 26, 9),
                new int3(9, 26, 8),
                new int3(8, 26, 4),
                new int3(4, 26, 18),
                new int3(18, 26, 6),
                
                new int3(2, 27, 9),
                new int3(9, 27, 6),
                new int3(6, 27, 15),
                new int3(15, 27, 14),
                new int3(14, 27, 2),
                
                new int3(1, 28, 17),
                new int3(17, 28, 3),
                new int3(3, 28, 11),
                new int3(11, 28, 10),
                new int3(10, 28, 1),
                
                new int3(7, 29, 19),
                new int3(19, 29, 5),
                new int3(5, 29, 10),
                new int3(10, 29, 11),
                new int3(11, 29, 7),
                
                new int3(3, 30, 14),
                new int3(14, 30, 15),
                new int3(15, 30, 7),
                new int3(7, 30, 11),
                new int3(11, 30, 3),
                
                new int3(19, 31, 7),
                new int3(7, 31, 15),
                new int3(15, 31, 6),
                new int3(6, 31, 18),
                new int3(18, 31, 19),
                
            };

            public static NativeArray<float4> GetNativeTriangles(Allocator allocator) =>
                GetNative(Tangents, allocator);

            public static float GetRadiusScale(RadiusType radiusType)
            {
                switch (radiusType)
                {
                    case RadiusType.Circumradius:
                        return Circumradius;
                    case RadiusType.Midradius:
                        return Midradius;
                    case RadiusType.Inradius:
                        return Inradius;
                    case RadiusType.Normalized:
                        return 1f;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(radiusType), radiusType, null);
                }
            }

            public static JobHandle Scale(RadiusType radiusType, NativeArray<float3> vertexes,
                JobHandle depends = default)
                => PlatonicSolidGenerator.Scale(GetRadiusScale(radiusType), vertexes, depends);
        }
    }

    public static class ProceduralPlatonicSolidGenerator
    {
        private const float
            PHI = 1.61803398875f,
            INV_PHI = 1f / PHI,

            //These 4 numbers are used for the Cirucm/Mid/In radius stuff
            XI = 1.17557050458f,
            SQRT_6 = 2.44948974278f,
            SQRT_2 = 1.41421356237f,
            SQRT_3 = 1.73205080757f;

        //ALL Shapes have a circumradius of 1, to convert to their original circumradius, multiply by the circumradius, to conver to their mid or inradius, divide by the circumradius (when at 1), then multiply by the inradius 
        //Dont know what these are? Look at this (https://en.wikipedia.org/wiki/Platonic_solid) bout midway down the page
        public const float
            TetrahedronCircumradius = SQRT_3 / SQRT_2,
            TetrahedronMidradius = 1 / SQRT_2,
            TetrahedronInradius = 1 / SQRT_6,
            CubeCircumradius = SQRT_3,
            CubeMidradius = SQRT_2,
            CubeInradius = 1f,
            OctahedronCircumradius = SQRT_2,
            OctahedronMidradius = 1,
            OctahedronInradius = SQRT_2 / SQRT_3,
            DodecahedronCircumradius = PHI * PHI / XI,
            DodecahedronMidradius = PHI * PHI,
            DodecahedronInradius = SQRT_3 * PHI,
            IcosahedronCircumradius = PHI * PHI / SQRT_3,
            IcosahedronMidradius = PHI,
            IcosahedronInradius = XI * PHI;


        private static readonly ProceduralVertex[] TetrahedronVerticies = new ProceduralVertex[4]
        {
            new ProceduralVertex(new Vector3(1f, 1f, 1f).normalized),
            new ProceduralVertex(new Vector3(1f, -1f, -1f).normalized),
            new ProceduralVertex(new Vector3(-1f, 1f, -1f).normalized),
            new ProceduralVertex(new Vector3(-1f, -1f, 1f).normalized)
        };

        private static readonly ProceduralVertex[] OctahedronVerticies = new ProceduralVertex[6]
        {
            new ProceduralVertex(new Vector3(1f, 0f, 0f).normalized),
            new ProceduralVertex(new Vector3(0f, 1f, 0f).normalized),
            new ProceduralVertex(new Vector3(0f, 0f, 1f).normalized),
            new ProceduralVertex(new Vector3(-1f, 0f, 0f).normalized),
            new ProceduralVertex(new Vector3(0f, -1f, 0f).normalized),
            new ProceduralVertex(new Vector3(0f, 0f, -1f).normalized)
        };

        private static readonly ProceduralVertex[] CubeVerticies = new ProceduralVertex[8]
        {
            new ProceduralVertex(new Vector3(1f, 1f, 1f).normalized),
            new ProceduralVertex(new Vector3(1f, 1f, -1f).normalized),
            new ProceduralVertex(new Vector3(1f, -1f, 1f).normalized),
            new ProceduralVertex(new Vector3(1f, -1f, -1f).normalized),
            new ProceduralVertex(new Vector3(-1f, 1f, 1f).normalized),
            new ProceduralVertex(new Vector3(-1f, 1f, -1f).normalized),
            new ProceduralVertex(new Vector3(-1f, -1f, 1f).normalized),
            new ProceduralVertex(new Vector3(-1f, -1f, -1f).normalized)
        };

        private static readonly ProceduralVertex[] IcosahedronVerticies = new ProceduralVertex[12]
        {
            new ProceduralVertex(new Vector3(0f, 1f, PHI).normalized),
            new ProceduralVertex(new Vector3(0f, -1f, PHI).normalized),
            new ProceduralVertex(new Vector3(0f, 1f, -PHI).normalized),
            new ProceduralVertex(new Vector3(0f, -1f, -PHI).normalized),

            new ProceduralVertex(new Vector3(1f, PHI, 0f).normalized),
            new ProceduralVertex(new Vector3(-1f, PHI, 0f).normalized),
            new ProceduralVertex(new Vector3(1f, -PHI, 0f).normalized),
            new ProceduralVertex(new Vector3(-1f, -PHI, 0f).normalized),

            new ProceduralVertex(new Vector3(PHI, 0f, 1f).normalized),
            new ProceduralVertex(new Vector3(PHI, 0f, -1f).normalized),
            new ProceduralVertex(new Vector3(-PHI, 0f, 1f).normalized),
            new ProceduralVertex(new Vector3(-PHI, 0f, -1f).normalized)
        };

        private static readonly ProceduralVertex[] DodecahedronVerticies = new ProceduralVertex[20]
        {
            new ProceduralVertex(new Vector3(1f, 1f, 1f).normalized), //0
            new ProceduralVertex(new Vector3(1f, 1f, -1f).normalized), //1
            new ProceduralVertex(new Vector3(1f, -1f, 1f).normalized), //2
            new ProceduralVertex(new Vector3(1f, -1f, -1f).normalized), //3
            new ProceduralVertex(new Vector3(-1f, 1f, 1f).normalized), //4
            new ProceduralVertex(new Vector3(-1f, 1f, -1f).normalized), //5
            new ProceduralVertex(new Vector3(-1f, -1f, 1f).normalized), //6
            new ProceduralVertex(new Vector3(-1f, -1f, -1f).normalized), //7

            new ProceduralVertex(new Vector3(0f, INV_PHI, PHI).normalized), //8
            new ProceduralVertex(new Vector3(0f, -INV_PHI, PHI).normalized), //9
            new ProceduralVertex(new Vector3(0f, INV_PHI, -PHI).normalized), //10
            new ProceduralVertex(new Vector3(0f, -INV_PHI, -PHI).normalized), //11

            new ProceduralVertex(new Vector3(INV_PHI, PHI, 0f).normalized), //12
            new ProceduralVertex(new Vector3(-INV_PHI, PHI, 0f).normalized), //13
            new ProceduralVertex(new Vector3(INV_PHI, -PHI, 0f).normalized), //14
            new ProceduralVertex(new Vector3(-INV_PHI, -PHI, 0f).normalized), //15

            new ProceduralVertex(new Vector3(PHI, 0f, INV_PHI).normalized), //16
            new ProceduralVertex(new Vector3(PHI, 0f, -INV_PHI).normalized), //17
            new ProceduralVertex(new Vector3(-PHI, 0f, INV_PHI).normalized), //18
            new ProceduralVertex(new Vector3(-PHI, 0f, -INV_PHI).normalized) //19            
        };

        private static readonly ProceduralMeshBuilder TetrahedronPrototype = BuildTetrahedronPrototype();

        private static readonly ProceduralMeshBuilder OctahedronPrototype = BuildOctahedronPrototype();

        private static readonly ProceduralMeshBuilder CubePrototype = BuildCubePrototype();

        private static readonly ProceduralMeshBuilder IcosahedronPrototype = BuildIcosahedronPrototype();

        private static readonly ProceduralMeshBuilder DodecahedronPrototype = BuildDodecahedronPrototype();

        private static ProceduralVertex ModifyVertex(ProceduralVertex proceduralVertex,
            Quaternion rotation, /* bool normalize = true,*/ bool right = false)
        {
            //if (normalize)
            //    vertex.Position = vertex.Position.normalized;
            proceduralVertex = rotation * proceduralVertex;
            proceduralVertex.Normal = proceduralVertex.Position.normalized;
            var vertexRotation = Quaternion.FromToRotation(Vector3.forward, proceduralVertex.Normal);
            proceduralVertex.Tangent = vertexRotation * Vector3.right;
            proceduralVertex.RightHanded = right;
            return proceduralVertex;
        }

        private static ProceduralMeshBuilder BuildTetrahedronPrototype()
        {
            var verts = TetrahedronVerticies;
            var builder = new ProceduralMeshBuilder();
            var rotation = Quaternion.FromToRotation(Vector3.up, verts[0].Position);
            var vertexCount = verts.Length;

            for (var i = 0; i < vertexCount; i++)
            {
                var modifiedVertex = ModifyVertex(verts[i], rotation);
                builder.AddVertex(modifiedVertex);
            }

            builder.AddTriangle(0, 1, 2);
            builder.AddTriangle(3, 1, 0);
            builder.AddTriangle(0, 2, 3);
            builder.AddTriangle(3, 2, 1);

            return builder;
        }

        private static ProceduralMeshBuilder BuildOctahedronPrototype()
        {
            var verts = OctahedronVerticies;
            var builder = new ProceduralMeshBuilder();
            var rotation = Quaternion.FromToRotation(Vector3.up, verts[0].Position);
            var vertexCount = verts.Length;

            for (var i = 0; i < vertexCount; i++)
            {
                var modifiedVertex = ModifyVertex(verts[i], rotation);
                builder.AddVertex(modifiedVertex);
            }

            builder.AddTriangle(0, 1, 2);
            builder.AddTriangle(0, 2, 4);
            builder.AddTriangle(0, 4, 5);
            builder.AddTriangle(0, 5, 1);

            builder.AddTriangle(3, 2, 1);
            builder.AddTriangle(3, 4, 2);
            builder.AddTriangle(3, 5, 4);
            builder.AddTriangle(3, 1, 5);

            return builder;
        }

        private static ProceduralMeshBuilder BuildCubePrototype()
        {
            var verts = CubeVerticies;
            var builder = new ProceduralMeshBuilder();
            var rotation = Quaternion.identity;
            var vertexCount = verts.Length;

            for (var i = 0; i < vertexCount; i++)
            {
                var modifiedVertex = ModifyVertex(verts[i], rotation, false);
                builder.AddVertex(modifiedVertex);
            }

            //Bot
            builder.AddTriangle(2, 1, 0);
            builder.AddTriangle(1, 2, 3);
            //For
            builder.AddTriangle(5, 4, 0);
            builder.AddTriangle(1, 5, 0);
            //Right
            builder.AddTriangle(1, 3, 7);
            builder.AddTriangle(7, 5, 1);
            //Top
            builder.AddTriangle(4, 5, 7);
            builder.AddTriangle(7, 6, 4);
            //Back
            builder.AddTriangle(2, 6, 7);
            builder.AddTriangle(2, 7, 3);
            //Left
            builder.AddTriangle(4, 2, 0);
            builder.AddTriangle(2, 4, 6);


            return builder;
        }

        private static ProceduralMeshBuilder BuildIcosahedronPrototype()
        {
            var verts = IcosahedronVerticies;
            var builder = new ProceduralMeshBuilder();
            var rotation = Quaternion.FromToRotation(Vector3.up, verts[0].Position);
            var vertexCount = verts.Length;

            for (var i = 0; i < vertexCount; i++)
            {
                var modifiedVertex = ModifyVertex(verts[i], rotation);
                builder.AddVertex(modifiedVertex);
            }

            //One peak, at 8
            builder.AddTriangle(0, 1, 8);
            builder.AddTriangle(4, 0, 8);
            builder.AddTriangle(9, 4, 8);
            builder.AddTriangle(6, 9, 8);
            builder.AddTriangle(1, 6, 8);

            //The other peak, at 11
            builder.AddTriangle(5, 2, 11);
            builder.AddTriangle(2, 3, 11);
            builder.AddTriangle(3, 7, 11);
            builder.AddTriangle(7, 10, 11);
            builder.AddTriangle(10, 5, 11);

            //The Strip
            builder.AddTriangle(0, 10, 1);
            builder.AddTriangle(10, 7, 1);
            builder.AddTriangle(7, 6, 1);
            builder.AddTriangle(7, 3, 6);
            builder.AddTriangle(3, 9, 6);
            builder.AddTriangle(2, 9, 3);
            builder.AddTriangle(2, 4, 9);
            builder.AddTriangle(2, 5, 4);
            builder.AddTriangle(4, 5, 0);
            builder.AddTriangle(5, 10, 0);

            return builder;
        }

        private static ProceduralMeshBuilder BuildDodecahedronPrototype()
        {
            var verts = DodecahedronVerticies;
            var builder = new ProceduralMeshBuilder();
            var rotation = Quaternion.FromToRotation(Vector3.up, verts[0].Position); //Quaternion.identity;
            var vertexCount = verts.Length;

            for (var i = 0; i < vertexCount; i++)
            {
                //After 20, it is our midpoints, which are not on the circumradius, but the inneradius... I dont know the ratio, so we just dont normalize the vector
                var modifiedVertex = ModifyVertex(verts[i], rotation);

                builder.AddVertex(modifiedVertex);
            }

            var groups = new int[12, 5]
            {
                {0, 16, 17, 1, 12},
                {2, 16, 0, 8, 9},
                {14, 3, 17, 16, 2},
                {0, 12, 13, 4, 8},
                {1, 10, 5, 13, 12},
                {13, 5, 19, 18, 4},
                {6, 9, 8, 4, 18},
                {2, 9, 6, 15, 14},
                {1, 17, 3, 11, 10},
                {7, 19, 5, 10, 11},
                {3, 14, 15, 7, 11},
                {19, 7, 15, 6, 18}
            };

            for (var i = 0; i < groups.GetLength(0); i++)
            {
                var l = groups.GetLength(1);
                var midpoint = new ProceduralVertex();
                for (var j = 0; j < l; j++) midpoint += verts[groups[i, j]];
                midpoint /= l;
                midpoint = ModifyVertex(midpoint, rotation);
                var midpointIndex = builder.AddVertex(midpoint);
                for (var j = 0; j < l; j++) builder.AddTriangle(midpointIndex, groups[i, j], groups[i, (j + 1) % l]);
            }

            return builder;
        }

        private static ProceduralMeshBuilder GetBuilderPrototype(ShapeType shape)
        {
            ProceduralMeshBuilder builder;
            switch (shape)
            {
                case ShapeType.Tetrahedron:
                    builder = TetrahedronPrototype;
                    break;
                case ShapeType.Octahedron:
                    builder = OctahedronPrototype;
                    break;
                case ShapeType.Cube:
                    builder = CubePrototype;
                    break;
                case ShapeType.Icosahedron:
                    builder = IcosahedronPrototype;
                    break;
                case ShapeType.Dodecahedron:
                    builder = DodecahedronPrototype;
                    break;
                default:
                    throw new Exception("Shape must be of an enumerated type!");
            }

            return builder;
        }

        public static void AddToMeshBuilder(ProceduralMeshBuilder builder, ShapeType shape, RadiusType radius,
            float scale = 1f)
        {
            var prototype = GetBuilderPrototype(shape); //,shape,radius,scale)
            var offset = builder.Verticies.Count;
            foreach (var v in prototype.Verticies)
                builder.AddVertex(v * GetConversion(shape, radius) * scale);
            foreach (var t in prototype.Triangles)
                builder.AddTriangle(t.Pivot + offset, t.Left + offset, t.Right + offset);
        }

        public static ProceduralMeshBuilder CreateBuilder(ShapeType shape, RadiusType radius = RadiusType.Circumradius,
            float scale = 1f)
        {
            var builder = new ProceduralMeshBuilder(GetBuilderPrototype(shape));
            var conversion = GetConversion(shape, radius);
            if (scale * conversion != 1f) ProceduralMeshUtility.ModifyRadius(builder, scale * conversion);

            return builder;
        }

        public static float GetConversion(ShapeType shape, RadiusType radius)
        {
            switch (shape)
            {
                case ShapeType.Tetrahedron:
                    switch (radius)
                    {
                        case RadiusType.Circumradius:
                            return TetrahedronCircumradius;
                        case RadiusType.Midradius:
                            return TetrahedronMidradius;
                        case RadiusType.Inradius:
                            return TetrahedronInradius;
                        case RadiusType.Normalized:
                            return 1f;
                    }

                    break;
                case ShapeType.Octahedron:
                    switch (radius)
                    {
                        case RadiusType.Circumradius:
                            return OctahedronCircumradius;
                        case RadiusType.Midradius:
                            return OctahedronMidradius;
                        case RadiusType.Inradius:
                            return OctahedronInradius;
                        case RadiusType.Normalized:
                            return 1f;
                    }

                    break;
                case ShapeType.Cube:
                    switch (radius)
                    {
                        case RadiusType.Circumradius:
                            return CubeCircumradius;
                        case RadiusType.Midradius:
                            return CubeMidradius;
                        case RadiusType.Inradius:
                            return CubeInradius;
                        case RadiusType.Normalized:
                            return 1f;
                    }

                    break;
                case ShapeType.Icosahedron:
                    switch (radius)
                    {
                        case RadiusType.Circumradius:
                            return IcosahedronCircumradius;
                        case RadiusType.Midradius:
                            return IcosahedronMidradius;
                        case RadiusType.Inradius:
                            return IcosahedronInradius;
                        case RadiusType.Normalized:
                            return 1f;
                    }

                    break;
                case ShapeType.Dodecahedron:
                    switch (radius)
                    {
                        case RadiusType.Circumradius:
                            return DodecahedronCircumradius;
                        case RadiusType.Midradius:
                            return DodecahedronMidradius;
                        case RadiusType.Inradius:
                            return DodecahedronInradius;
                        case RadiusType.Normalized:
                            return 1f;
                    }

                    break;
                default:
                    throw new Exception("Shape must be of an enumerated type!");
            }

            throw new Exception("Radius must be of an enumerated type!");
        }
    }
}