//using System;
//using System;

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class SpaceGraphProto : MonoBehaviour
{
    public enum SpaceGraphParamType
    {
        Spheroid,
        SpheroidSpiral,
        Spiral
    }

    [Serializable]
    public struct SpaceGraphParameters
    {
        public SpaceGraphParamType Shape;
        public Vector3 Scale;
        public int MaximumEdges;
        public int MinimumEdges;
        public int MinNumPlanets;
        public int MaxNumPlanets;
        public int Arms;
        public Vector3 ArmScale;
        public Vector3 Up;
        public Vector3 Right;
        public float MaxEdgeLength;
        public float TangentalVelocity;
    }

    public class SpaceGraphGenerator
    {
        public static void TempDelauney(List<Vector3> verts)
        {
            verts.Sort((i, j) =>
            {
                if (i.x == j.x && i.y == j.y && i.z == j.z)
                    return 0;
                if (i.x < j.x || i.x == j.x && i.y < j.y || i.x == j.x && i.y == j.y && i.z < j.z)
                    return -1;
                return 1;
                //-1
                //xI < xJ || (xI = xJ && yI < yJ) || (xI = xJ && yI = yJ && zI = zJ)
                //0
                //xI = xJ && yI = yJ && zI && zJ
                //1
                //Else
            });
        }

        public static void RemoveLongEdges(ref IList<Vector3> v, ref IList<SpaceEdge> e, float maxRange,
            int desirededges = 1)
        {
            var rSqr = maxRange * maxRange;
            var myVertToEdgeList = new List<SpaceEdge>[v.Count];
            for (var i = 0; i < v.Count; i++)
                myVertToEdgeList[i] = new List<SpaceEdge>();
            foreach (var edge in e)
            {
                myVertToEdgeList[edge.To].Add(edge);
                myVertToEdgeList[edge.From].Add(edge);
            }

            var sorter = new SpaceEdgeSorter(v);
            for (var i = 0; i < v.Count; i++)
            {
                myVertToEdgeList[i].Sort(sorter);
                myVertToEdgeList[i].Reverse();
            }

            for (var i = 0; i < e.Count; i++)
            {
                var longestFrom = myVertToEdgeList[e[i].From][0];
                if (myVertToEdgeList[longestFrom.To].Count > desirededges &&
                    myVertToEdgeList[longestFrom.From].Count > desirededges)
                    if ((v[longestFrom.To] - v[longestFrom.From]).sqrMagnitude > rSqr)
                    {
                        myVertToEdgeList[longestFrom.From].Remove(longestFrom);

                        myVertToEdgeList[longestFrom.To].Remove(longestFrom);
                        Debug.Log(string.Format("Removed {0}->{1}", longestFrom.To, longestFrom.From));
                        //j--;
                    }
            }
        }


        public static List<SpaceEdge> GenerateDelauney(Vector3[] planetArr)
        {
            var planetEdges = new List<SpaceEdge>(planetArr.Length);
            for (var i = 0; i < planetArr.Length; i++)
            for (var k = i + 1; k < planetArr.Length; k++)
            {
                var e = new SpaceEdge {To = k, From = i};
                var midPoint = (planetArr[e.From] + planetArr[e.To]) / 2f;
                var rSquared = (planetArr[e.From] - midPoint).sqrMagnitude;

                if (!SphereSquareCheck(midPoint, rSquared, planetArr, i, k)) planetEdges.Add(e);
            }

            return planetEdges;
        }

        public static Vector3[] Scale(Vector3[] points, Vector3 scale)
        {
            for (var i = 0; i < points.Length; i++)
                points[i] = Scale(points[i], scale);
            return points;
        }

        public static Vector3 Scale(Vector3 point, Vector3 scale)
        {
            return new Vector3(point.x * scale.x, point.y * scale.y, point.z * scale.z);
        }

        public static Vector3[] Spiral(Vector3[] points, float r, Vector3 axis, float tanVelocity)
        {
            return SpiralSquared(points, r * r, axis, tanVelocity);
        }

        public static Vector3[] SpiralSquared(Vector3[] points, float rSqr, Vector3 axis, float tanVelocity)
        {
            for (var i = 0; i < points.Length; i++)
                points[i] = Quaternion.AngleAxis(points[i].sqrMagnitude / rSqr * tanVelocity, axis) * points[i];
            return points;
        }

        //Interesting wormhole shape when tanVelocity is huge
        public static SpaceGraph
            GenerateSpiral(int seed,
                SpaceGraphParameters param) // int minPlanets = 15, int maxPlanets = 255, Vector3 scale = default(Vector3), int arms = 1, Vector3 armScale = default(Vector3), Vector3 up = default(Vector3), Vector3 right = default(Vector3), float tangentalVelocity = 1f)
        {
            Vector3[] planetArr;
            using (var randLock = new TempRandomLock(seed))
            {
                var planetsToGen = Random.Range(param.MinNumPlanets, param.MaxNumPlanets + 1);
                planetArr = new Vector3[planetsToGen];
                Vector3 v, n;
                float l;
                var arm = 0;
                for (var i = 0; i < planetsToGen; i++)
                {
                    arm = Random.Range(0, param.Arms);
                    l = Random.value;
                    n = Quaternion.AngleAxis(arm * 360f / param.Arms, param.Up) * param.Right * l;
                    n = Scale(n, param.Scale);
                    v = Random.insideUnitSphere;

                    planetArr[i] = n + Scale(v, param.ArmScale) * l;
                }
            }

            planetArr = SpiralSquared(planetArr, param.Scale.sqrMagnitude + (param.ArmScale / 2f).sqrMagnitude,
                param.Up, param.TangentalVelocity);
            IList<SpaceEdge> planetEdges = GenerateDelauney(planetArr);
            IList<Vector3> planetVerts = planetArr;
            //RemoveLongEdges(ref planetVerts, ref planetEdges, param.MaxEdgeLength, Random.Range(param.MinimumEdges, param.MaximumEdges + 1));
            return new SpaceGraph(planetArr, planetEdges);
        }

        //Interesting wormhole shape when tanVelocity is huge
        public static SpaceGraph GenerateSpheroidSpiraled(int seed, SpaceGraphParameters param)
        {
            Vector3[] planetArr;
            using (var randLock = new TempRandomLock(seed))
            {
                var planetsToGen = Random.Range(param.MinNumPlanets, param.MaxNumPlanets + 1);
                planetArr = new Vector3[planetsToGen];
                Vector3 v;
                for (var i = 0; i < planetsToGen; i++)
                {
                    v = Random.insideUnitSphere;
                    planetArr[i] = Scale(v, param.Scale);
                }
            }

            planetArr = SpiralSquared(planetArr, param.Scale.sqrMagnitude, param.Up, param.TangentalVelocity);
            IList<Vector3> planetVerts = planetArr;
            IList<SpaceEdge> planetEdges = GenerateDelauney(planetArr);
            //RemoveLongEdges(ref planetVerts, ref planetEdges, param.MaxEdgeLength, Random.Range(param.MinimumEdges, param.MaximumEdges + 1));
            return new SpaceGraph(planetVerts, planetEdges);
        }

        //Basic Spheroid
        public static SpaceGraph GenerateSpheroid(int seed, SpaceGraphParameters param)
        {
            Vector3[] planetArr;
            using (var randLock = new TempRandomLock(seed))
            {
                var planetsToGen = Random.Range(param.MinNumPlanets, param.MaxNumPlanets + 1);
                planetArr = new Vector3[planetsToGen];
                Vector3 v;
                for (var i = 0; i < planetsToGen; i++)
                {
                    v = Random.insideUnitSphere;
                    planetArr[i] = Scale(v, param.Scale);
                }
            }

            IList<Vector3> planetVerts = planetArr;
            IList<SpaceEdge> planetEdges = GenerateDelauney(planetArr);
            RemoveLongEdges(ref planetVerts, ref planetEdges, param.MaxEdgeLength,
                Random.Range(param.MinimumEdges, param.MaximumEdges + 1));
            return new SpaceGraph(planetVerts, planetEdges);
        }

        public static SpaceGraph Generate(int seed, SpaceGraphParameters param)
        {
            switch (param.Shape)
            {
                case SpaceGraphParamType.Spheroid:
                    return GenerateSpheroid(seed, param);
                case SpaceGraphParamType.SpheroidSpiral:
                    return GenerateSpheroidSpiraled(seed, param);
                case SpaceGraphParamType.Spiral:
                    return GenerateSpiral(seed, param);
            }

            return null;
        }

        //True on collision
        public static bool SphereCheck(Vector3 origin, float radius, IEnumerable<Vector3> points, params int[] ignores)
        {
            return SphereSquareCheck(origin, radius * radius, points, ignores);
        }

        //True on collision
        public static bool SphereSquareCheck(Vector3 origin, float radiusSqr, IEnumerable<Vector3> points,
            params int[] ignores)
        {
            ICollection<int> ignoresCol = ignores;
            var index = 0;
            foreach (var point in points)
            {
                if (!ignoresCol.Contains(index) && (point - origin).sqrMagnitude <= radiusSqr)
                    return true;
                index++;
            }

            return false;
        }
        //private static void TempDelauneyPartition(List<Vector3> verts)
        //{
        //    if (verts.Count <= 2)
        //        return;

        //    int l = verts.Count / 2;
        //    TempDelauneyPartition(verts.GetRange(0, l));
        //    TempDelauneyPartition(verts.GetRange(l, verts.Count - l));
        //}
        //public static void Hull(List<Vector3> verts)

        public class SpaceEdgeSorter : IComparer<SpaceEdge>
        {
            private readonly IList<Vector3> verts;

            public SpaceEdgeSorter(IList<Vector3> v)
            {
                verts = v;
            }

            public int Compare(SpaceEdge x, SpaceEdge y)
            {
                return (verts[x.From] - verts[x.To]).sqrMagnitude.CompareTo((verts[y.From] - verts[y.To]).sqrMagnitude);
            }
        }
    }
}