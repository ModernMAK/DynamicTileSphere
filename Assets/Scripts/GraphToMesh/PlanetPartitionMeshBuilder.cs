using System.Collections.Generic;
using Graphing.Planet;
using ProceduralMeshFramework;
using UnityEngine;

namespace GraphToMesh
{
    public class PlanetPartitionMeshBuilder : GraphMeshBuilder<PlanetGraph, PlanetPoly, PlanetEdge, PlanetNode>
    {
        public PlanetPartitionMeshBuilder(PlanetGraphMeshBuilder parent, PlanetPartition partition)
        {
            Parent = parent;
            Partition = partition;
        }

        public PlanetGraphMeshParameters Parameters
        {
            get { return Parent.Parameters; }
        }

        public PlanetGraphMeshBuilder Parent { get; private set; }
        public PlanetPartition Partition { get; private set; }

        public override void Generate()
        {
            if (Partition.IsDirty)
            {
                //Debug.Log("Dirty");
                Clear();
                foreach (var poly in Partition)
                    Render(poly);
                Partition.Clean();
            }
        }

        private Vector2 GetUvFromShape(int vert, int shape, int scale = 1, int offset = 0)
        {
            var t = (float) (vert * scale + offset) / (shape * scale) * Mathf.PI * 2f;
            return new Vector2((Mathf.Cos(t) + 1f) / 2f, (Mathf.Sin(t) + 1f) / 2f);
        }

        private Vector3 CalculatePosition(PlanetEdge edge, bool twinNode = false)
        {
            var pos = twinNode ? edge.Node.Position : edge.Twin.Node.Position;
            pos = Vector3.Lerp(edge.Poly.CenterLerp, pos, Parameters.Solidity);
            //pos += Parameters.GetElevationOffset(edge.Poly.Elevation, edge.Poly.CenterLerp.normalized);
            return Vector3.Scale(pos, Parameters.Scale(edge.Poly.Elevation));
        }

        protected override void GeneratePolygon(PlanetPoly polygon)
        {
            var Center = new ProceduralVertex(Vector3.Scale(polygon.CenterLerp, Parameters.Scale(polygon.Elevation)))
                .SetColor(Color.red);
            var Verts = new List<ProceduralVertex>(6); //Hex has 6, which is the most expected for the dodechasohedron

            var normal = Center.Position.normalized;
            var tangent = Quaternion.FromToRotation(Vector3.up, normal) * Vector3.right;

            Center = Center.SetNormal(normal);
            Center = Center.SetTangent(tangent);
            Center = Center.SetUv(Vector2.one / 2f);
            Center = Center.SetUv3(Vector3.one * polygon.TerrainType);


            var elevationDiscrepancy = false;
            foreach (var edge in polygon)
            {
                var vert = new ProceduralVertex(CalculatePosition(edge));
                elevationDiscrepancy |= edge.Poly.Elevation != edge.Twin.Poly.Elevation;
                vert = vert.SetColor(Color.red);
                vert = vert.SetNormal(normal);
                vert = vert.SetTangent(tangent);
                vert = vert.SetUv3(Vector3.one * polygon.TerrainType);
                Verts.Add(vert);
            }


            int i = 0, l = Verts.Count;
            var indicies = new int[l + 1];
            foreach (var vert in Verts)
            {
                var vertFinal = vert; //.SetPosition(Vector3.Lerp(Center.Position, vert.Position, Parameters.Solidity));
                vertFinal = vertFinal.SetUv(GetUvFromShape(i, l));
                indicies[i] = AddVertex(vertFinal);
                i++;
            }

            indicies[l] = AddVertex(Center);
            for (i = 0; i < l; i++) AddTriangle(indicies[l], indicies[(i + 1) % l], indicies[i]);

            //After all is said and done try rendering the edges amd corners, if we should
            if (Parameters.Solidity < 1f || Parameters.ElevationOffset != 0f && elevationDiscrepancy)
                foreach (var edge in polygon)
                    Render(edge);
        }

        protected override void GenerateEdge(PlanetEdge edge)
        {
            var indicies = new int[4];
            Vector3
                edgeEdgePos = CalculatePosition(edge),
                edgeTwinPos = CalculatePosition(edge, true),
                twinEdgePos = CalculatePosition(edge.Twin),
                twinTwinPos = CalculatePosition(edge.Twin, true);

            Vector3
                norm = (edgeEdgePos + edgeTwinPos + twinEdgePos + twinTwinPos).normalized,
                tan = Quaternion.FromToRotation(Vector3.up, norm) * Vector3.right;

            var
                terrainUv = new Vector3(edge.Poly.TerrainType, edge.Twin.Poly.TerrainType, edge.Poly.TerrainType);

            //Set the index of the vert to indicies
            //Set the Position to the edge of the Solid polygin (vai lerp)
            //Set color to red on edge side, green on twin side
            //Set uv3 to terrain
            //Set norm and tangent
            //Set uv from shape (square is rotated 45 degrees), so we make it 1/8 instead of 0/4 to accomplish this rotation
            indicies[0] = AddVertex(new ProceduralVertex(edgeEdgePos).SetColor(Color.red).SetUv3(terrainUv)
                .SetNormal(norm)
                .SetTangent(tan).SetUv(GetUvFromShape(1, 8)));
            indicies[1] = AddVertex(new ProceduralVertex(edgeTwinPos).SetColor(Color.red).SetUv3(terrainUv)
                .SetNormal(norm)
                .SetTangent(tan).SetUv(GetUvFromShape(3, 8)));
            indicies[2] = AddVertex(new ProceduralVertex(twinEdgePos).SetColor(Color.green).SetUv3(terrainUv)
                .SetNormal(norm).SetTangent(tan).SetUv(GetUvFromShape(5, 8)));
            indicies[3] = AddVertex(new ProceduralVertex(twinTwinPos).SetColor(Color.green).SetUv3(terrainUv)
                .SetNormal(norm).SetTangent(tan).SetUv(GetUvFromShape(7, 8)));

            AddTriangle(indicies[0], indicies[1], indicies[2]);
            AddTriangle(indicies[2], indicies[3], indicies[0]);
        }

        protected override void GenerateCorner(PlanetEdge edge)
        {
            var indicies = new int[3];
            Vector3
                edgePos = CalculatePosition(edge),
                edgeNextPos = CalculatePosition(edge.Prev.Twin),
                edgeNextNextPos = CalculatePosition(edge.Prev.Twin.Prev.Twin);

            Vector3
                norm = (edgePos + edgeNextPos + edgeNextNextPos).normalized,
                tan = Quaternion.FromToRotation(Vector3.up, norm) * Vector3.right;

            var
                terrainUv = new Vector3(edge.Poly.TerrainType, edge.Prev.Twin.Poly.TerrainType,
                    edge.Prev.Twin.Prev.Twin.Poly.TerrainType);

            //Set the index of the vert to indicies
            //Set the Position to the edge of the Solid polygin (vai lerp)
            //Set color to red on edge side, green on twin side
            //Set uv3 to terrain
            //Set norm and tangent
            //Set uv from shape (square is rotated 45 degrees), so we make it 1/8 instead of 0/4 to accomplish this rotation
            indicies[0] = AddVertex(new ProceduralVertex(edgePos).SetColor(Color.red).SetUv3(terrainUv).SetNormal(norm)
                .SetTangent(tan).SetUv(GetUvFromShape(1, 6)));
            indicies[1] = AddVertex(new ProceduralVertex(edgeNextPos).SetColor(Color.green).SetUv3(terrainUv)
                .SetNormal(norm).SetTangent(tan).SetUv(GetUvFromShape(3, 6)));
            indicies[2] = AddVertex(new ProceduralVertex(edgeNextNextPos).SetColor(Color.blue).SetUv3(terrainUv)
                .SetNormal(norm).SetTangent(tan).SetUv(GetUvFromShape(5, 6)));

            AddTriangle(indicies[2], indicies[1], indicies[0]);
            //AddTriangle(indicies[0], indicies[3], indicies[2]);
        }

        public override void Clear()
        {
            foreach (var edge in Edges)
                RemoveEdge(edge);
            foreach (var corner in Corners)
                RemoveCorner(corner);

            base.Clear();
        }

        public override void RemoveCorner(PolygonCorner<PlanetPoly, PlanetEdge, PlanetNode> corner)
        {
            base.RemoveCorner(corner);
            Parent.RemoveCorner(corner);
        }

        public override void AddCorner(PolygonCorner<PlanetPoly, PlanetEdge, PlanetNode> corner)
        {
            base.AddCorner(corner);
            Parent.AddCorner(corner);
        }

        public override bool ContainsCorner(PolygonCorner<PlanetPoly, PlanetEdge, PlanetNode> corner)
        {
            return Parent.ContainsCorner(corner);
        }

        public override void RemoveEdge(PolygonEdge<PlanetPoly, PlanetEdge, PlanetNode> edge)
        {
            base.RemoveEdge(edge);
            Parent.RemoveEdge(edge);
        }

        public override void AddEdge(PolygonEdge<PlanetPoly, PlanetEdge, PlanetNode> edge)
        {
            base.AddEdge(edge);
            Parent.AddEdge(edge);
        }

        public override bool ContainsEdge(PolygonEdge<PlanetPoly, PlanetEdge, PlanetNode> edge)
        {
            return Parent.ContainsEdge(edge);
        }

        public override void AddPolygon(PlanetPoly poly)
        {
            base.AddPolygon(poly);
            Parent.AddPolygon(poly);
        }

        public override bool ContainsPolygon(PlanetPoly poly)
        {
            return Parent.ContainsPolygon(poly);
        }

        public override void RemovePolygon(PlanetPoly poly)
        {
            base.RemovePolygon(poly);
            Parent.RemovePolygon(poly);
        }
    }
}