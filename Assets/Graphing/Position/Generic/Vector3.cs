#if UNITY
using UnityEngine;
#else
#endif

namespace UnityEngineSub
{
    public struct Vector3
    {
        public Vector3(float x, float y, float z = 0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float x, y, z;

        public float sqrMagnitude
        {
            get
            {
                return
                    (x * x) +
                    (y * y) +
                    (z * z);
            }
        }
        public float magnitude
        {
            get
            {
                return (float)System.Math.Sqrt(sqrMagnitude);
            }
        }
        public Vector3 normalized
        {
            get
            {
                if (sqrMagnitude == 0f)
                    return new Vector3();//zero vector

                float m = magnitude;
                return new Vector3(x / m, y / m, z / m);


            }
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);
        }
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);
        }
        public static Vector3 operator *(Vector3 left, float right)
        {
            return new Vector3(left.x * right, left.y * right, left.z * right);
        }
        public static Vector3 operator /(Vector3 left, float right)
        {
            return new Vector3(left.x / right, left.y / right, left.z / right);
        }
    }
}
