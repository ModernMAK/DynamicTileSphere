using System;
using UnityEngine;

namespace ProceduralMeshFramework.NNative
{
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
                builder.AddVertex(v * ProceduralMeshFramework.ProceduralPlatonicSolidGenerator.GetConversion(shape, radius) * scale);
            foreach (var t in prototype.Triangles)
                builder.AddTriangle(t.Pivot + offset, t.Left + offset, t.Right + offset);
        }

        public static ProceduralMeshBuilder CreateBuilder(ShapeType shape, RadiusType radius = RadiusType.Circumradius,
            float scale = 1f)
        {
            var builder = new ProceduralMeshBuilder(GetBuilderPrototype(shape));
            var conversion = ProceduralMeshFramework.ProceduralPlatonicSolidGenerator.GetConversion(shape, radius);
            if (scale * conversion != 1f) ProceduralMeshUtility.ModifyRadius(builder, scale * conversion);
            return builder;
        }

      
    }
}