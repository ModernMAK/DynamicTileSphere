using Graphing.Position.Generic.Native;
using Graphing.Position.Native;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMeshFramework.NNative
{

    public static class SharedUtil 
    {
        public static int[] CalculateTwins(IList<int> Indexes, int PolyCount)
        {
            int PolyVertCount = Indexes.Count / PolyCount;
            var twins = new int[Indexes.Count];
            for (var i = 0; i < twins.Length; i++)
                twins[i] = -1; // Init to -1 to allow skipping on 0

            // This algo is a mess; but simply put
            //  ~ ITERATE OVER EACH POLYGON (PolyA)
            //      ~ ITERATE OVER EACH EDGE (PolyA Edge) ... we use current and next index of the triangle as an 'edge'
            //      > SKIP IF EDGE HAS TWIN           
            //      > FIND TWIN
            //          ~ ITERATE OVER EACH POLY (PolyB) ...skip PolyA so PolyA != PolyB
            //              ~ ITERATE OVER EACH EDGE (PolyB Edge)
            //              > SKIP IF EDGE HAS TWIN
            //                  ~ COMPARE EDGE 
            //                  > If Found Go to next PolyA Edge (break)

            for (var i = 0; i < PolyCount; i++)
            {
                var iOffset = i * PolyVertCount;
                for (var j = 0; j < PolyVertCount; j++)
                {
                    if (twins[iOffset + j] != -1)
                        continue;

                    var jNext = (j + 1) % PolyVertCount;

                    var a = Indexes[iOffset + j];
                    var b = Indexes[iOffset + jNext];

                    var found = false;
                    for (var k = 0; k < PolyCount && k != i && !found; k++)
                    {
                        var kOffset = k * PolyVertCount;
                        for (var l = 0; l < PolyVertCount && !found; l++)
                        {
                            if (twins[kOffset + l] != -1)
                                continue;

                            var lNext = (l + 1) % PolyVertCount;

                            var c = Indexes[kOffset + l];
                            var d = Indexes[kOffset + lNext];

                            if (a == d && b == c)
                            {
                                twins[iOffset + j] = kOffset + l;
                                twins[kOffset + l] = iOffset + j;
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }
            return twins;
        }

        public static int[] CalculatePolyEdges(int PolyVertCount, int PolyCount)
        {
            var polys = new int[PolyCount];
            for (var i = 0; i < polys.Length; i++)
                polys[i] = PolyVertCount * i;
            return polys;
        }
        public static int[] CalculateNodeEdges(IList<int> Indexes, int SoftVertexCount)
        {
            var nodes = new int[SoftVertexCount];
            var found = 0;
            for (var i = 0; i < nodes.Length; i++)
                nodes[i] = -1;
            for (var i = 0; i < Indexes.Count && found < SoftVertexCount; i++)
			{
                var index = Indexes[i];
                if (nodes[index] == -1)
                {
                    nodes[index] = i;
                    found++;
                }
			}
            return nodes;
        }
        public static int[] Flatten(int[,] arr)
		{
            var u = arr.GetLength(0);
            var v =arr.GetLength(1);
            var flat = new int[u * v];
            for(var i = 0; i < flat.Length; i++)
			{
                var j = i / u;
                var k = i % u;
                flat[i] = arr[j, k];
			}
            return flat;
		}

        public static PositionGraph BuildGraph(IList<float3> Vertexes, IList<int> Indexes, IList<int> Twins, IList<int> NodeEdges, IList<int> PolyEdges)
        {
            var PolyCount = PolyEdges.Count;
            var PolyVertCount = Indexes.Count / PolyCount;

            var graph = new PositionGraph(Vertexes.Count, PolyCount * PolyVertCount, PolyCount);
            //Initilaize Vertexes
            for (var i = 0; i < Vertexes.Count; i++)
            {
                var node = graph.Nodes[i];
                node.Data = new PositionData { Position = Vertexes[i] };
                node.Edge = NodeEdges[i];
                graph.Nodes[i] = node;
            }
            // Initilalize Polygons & Edges
            for (var polyIndex = 0; polyIndex < PolyCount; polyIndex++)
            {
                var poly = graph.Polygons[polyIndex];
                poly.Edge = PolyEdges[polyIndex];
                graph.Polygons[polyIndex] = poly;


                var edgeOffset = polyIndex * PolyVertCount;
                //Initialize Edges
                for (var edgeIndex = 0; edgeIndex < PolyVertCount; edgeIndex++)
                {
                    var edge = graph.Edges[edgeOffset + edgeIndex];

                    edge.Next = edgeOffset + (edgeIndex + 1) % PolyVertCount;
                    edge.Prev = edgeOffset + (edgeIndex + PolyVertCount - 1) % PolyVertCount;

                    var fullIndex = polyIndex * PolyVertCount + edgeIndex;

                    edge.Twin = Twins[fullIndex];
                    edge.Node = Indexes[fullIndex];

                    edge.Poly = polyIndex;

                    graph.Edges[edgeOffset + edgeIndex] = edge;
                }

            }
            return graph;
        }

        public const float
            PHI = 1.61803398875f,

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

    


    public static class Tetrahedron
    {
        private const float
            SQRT_6 = SharedUtil.SQRT_6,
            SQRT_2 = SharedUtil.SQRT_2,
            SQRT_3 = SharedUtil.SQRT_3;

        //ALL Shapes have a circumradius of 1, to convert to their original circumradius, multiply by the circumradius, to conver to their mid or inradius, divide by the circumradius (when at 1), then multiply by the inradius 
        //Dont know what these are? Look at this (https://en.wikipedia.org/wiki/Platonic_solid) bout midway down the page
        public const float
            CircumRadius = SQRT_3 / SQRT_2,
            MidRadius = 1 / SQRT_2,
            InRadius = 1 / SQRT_6;

        public static float GetRadius(RadiusType radius)
        {
            switch (radius)
            {
                case RadiusType.Circumradius:
                    return CircumRadius;
                case RadiusType.Midradius:
                    return MidRadius;
                case RadiusType.Inradius:
                    return InRadius;
                case RadiusType.Normalized:
                    return 1f;//This is always 1 because the array is normalized.
                default:
                    throw new ArgumentOutOfRangeException(nameof(radius));
            }
        }

        public const int PolyCount = 4;//4 Polygons
        public const int SoftVertCount = 4;//4 Polygons (Triangles), 3 Half Edges Per Polygon (Triangle)
        public const int PolyVertCount = 3;
        private static readonly float3[] Vertexes = new float3[SoftVertCount]
        {
            math.normalize(new float3(1f, 1f, 1f)),
            math.normalize(new float3(1f, -1f, -1f)),
            math.normalize(new float3(-1f, 1f, -1f)),
            math.normalize(new float3(-1f, -1f, 1f))
        };
        /// <summary>
        /// 4 Triangles ~ [PolygonIndex, VertexIndex]
        /// Order matters if converting to mesh, not converting to graph
        /// </summary>
        private static readonly int[,] _Indexes = new int[PolyCount, PolyVertCount]
        {
            { 0, 1, 2 },
            { 3, 1, 0 },
            { 0, 2, 3 },
            { 3, 2, 1 },
        };
        private static readonly int[] Indexes = SharedUtil.Flatten(_Indexes);
        private static readonly int[] Twins = SharedUtil.CalculateTwins(Indexes, PolyCount);
        private static readonly int[] NodeEdges = SharedUtil.CalculateNodeEdges(Indexes, SoftVertCount);
        private static readonly int[] PolyEdges = SharedUtil.CalculatePolyEdges(PolyVertCount, PolyCount);


        /// <summary>
        /// Builds a soft graph.
        /// </summary>
        /// <returns></returns>
        public static PositionGraph BuildGraph() => SharedUtil.BuildGraph(Vertexes, Indexes, Twins, NodeEdges, PolyEdges);
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
            


        

        private static readonly float3[] OctahedronVerticies = new float3[6]
        {
            math.normalize(new float3(1f, 0f, 0f)),
            math.normalize(new float3(0f, 1f, 0f)),
            math.normalize(new float3(0f, 0f, 1f)),
            math.normalize(new float3(-1f, 0f, 0f)),
            math.normalize(new float3(0f, -1f, 0f)),
            math.normalize(new float3(0f, 0f, -1f))
        };

        private static readonly float3[] CubeVerticies = new float3[8]
        {
            math.normalize(new float3(1f, 1f, 1f)),
            math.normalize(new float3(1f, 1f, -1f)),
            math.normalize(new float3(1f, -1f, 1f)),
            math.normalize(new float3(1f, -1f, -1f)),
            math.normalize(new float3(-1f, 1f, 1f)),
            math.normalize(new float3(-1f, 1f, -1f)),
            math.normalize(new float3(-1f, -1f, 1f)),
            math.normalize(new float3(-1f, -1f, -1f))
        };

        private static readonly float3[] IcosahedronVerticies = new float3[12]
        {
            math.normalize(new float3(0f, 1f, PHI)),
            math.normalize(new float3(0f, -1f, PHI)),
            math.normalize(new float3(0f, 1f, -PHI)),
            math.normalize(new float3(0f, -1f, -PHI)),

            math.normalize(new float3(1f, PHI, 0f)),
            math.normalize(new float3(-1f, PHI, 0f)),
            math.normalize(new float3(1f, -PHI, 0f)),
            math.normalize(new float3(-1f, -PHI, 0f)),

            math.normalize(new float3(PHI, 0f, 1f)),
            math.normalize(new float3(PHI, 0f, -1f)),
            math.normalize(new float3(-PHI, 0f, 1f)),
            math.normalize(new float3(-PHI, 0f, -1f))
        };

        private static readonly float3[] DodecahedronVerticies = new float3[20]
        {
            math.normalize(new float3(1f, 1f, 1f)), //0
            math.normalize(new float3(1f, 1f, -1f)), //1
            math.normalize(new float3(1f, -1f, 1f)), //2
            math.normalize(new float3(1f, -1f, -1f)), //3
            math.normalize(new float3(-1f, 1f, 1f)), //4
            math.normalize(new float3(-1f, 1f, -1f)), //5
            math.normalize(new float3(-1f, -1f, 1f)), //6
            math.normalize(new float3(-1f, -1f, -1f)), //7

            math.normalize(new float3(0f, INV_PHI, PHI)), //8
            math.normalize(new float3(0f, -INV_PHI, PHI)), //9
            math.normalize(new float3(0f, INV_PHI, -PHI)), //10
            math.normalize(new float3(0f, -INV_PHI, -PHI)), //11

            math.normalize(new float3(INV_PHI, PHI, 0f)), //12
            math.normalize(new float3(-INV_PHI, PHI, 0f)), //13
            math.normalize(new float3(INV_PHI, -PHI, 0f)), //14
            math.normalize(new float3(-INV_PHI, -PHI, 0f)), //15

            math.normalize(new float3(PHI, 0f, INV_PHI)), //16
            math.normalize(new float3(PHI, 0f, -INV_PHI)), //17
            math.normalize(new float3(-PHI, 0f, INV_PHI)), //18
            math.normalize(new float3(-PHI, 0f, -INV_PHI)) //19            
        };

        private static readonly ProceduralMeshBuilder TetrahedronPrototype = BuildTetrahedronPrototype();

        private static readonly ProceduralMeshBuilder OctahedronPrototype = BuildOctahedronPrototype();

        private static readonly ProceduralMeshBuilder CubePrototype = BuildCubePrototype();

        private static readonly ProceduralMeshBuilder IcosahedronPrototype = BuildIcosahedronPrototype();

        private static readonly ProceduralMeshBuilder DodecahedronPrototype = BuildDodecahedronPrototype();

        private static float3 ModifyVertex(float3 proceduralVertex,
            Quaternion rotation, /* bool normalize = true,*/ bool right = false)
        {
            //if (normalize)
            //    vertex.Position = vertex.Position;
            proceduralVertex = rotation * proceduralVertex;
            proceduralVertex.Normal = proceduralVertex.Position;
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
                var midpoint = new float3();
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