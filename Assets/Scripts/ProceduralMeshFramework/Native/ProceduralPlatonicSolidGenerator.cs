using Graphing.Position.Native;
using System;
using UnityEngine;

namespace ProceduralMeshFramework.Native
{

	public static class ProceduralPlatonicSolidGenerator
    {
        public static PositionGraph BuildGraph(ShapeType shape)
        {
            switch (shape)
            {
                case ShapeType.Tetrahedron:
                    return Tetrahedron.BuildGraph();
                case ShapeType.Octahedron:
                    return Octahedron.BuildGraph();
                case ShapeType.Cube:
                    return Cube.BuildGraph();
                case ShapeType.Icosahedron:
                    return Icosahedron.BuildGraph();
                case ShapeType.Dodecahedron:
                    return Dodecahedron.BuildGraph();
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape));
            }
        }
        public static float GetConversion(ShapeType shape, RadiusType radius)
        {
            switch (shape)
            {
                case ShapeType.Tetrahedron:
                    return Tetrahedron.GetRadius(radius);
                case ShapeType.Octahedron:
                    return Octahedron.GetRadius(radius);
                case ShapeType.Cube:
                    return Cube.GetRadius(radius);
                case ShapeType.Icosahedron:
                    return Icosahedron.GetRadius(radius);
                case ShapeType.Dodecahedron:
                    return Dodecahedron.GetRadius(radius);
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape));
            }
        }
    }
}