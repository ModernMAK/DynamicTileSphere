using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProceduralVertex
{
    private static readonly Vector3 DEFAULT_VECTOR3 = Vector3.zero;
    //private static readonly Vector2 DEFAULT_VECTOR2 = Vector2.zero;
    private static readonly Vector4 DEFAULT_VECTOR4 = Vector4.zero;
    private static readonly Color DEFAULT_COLOR = Color.black;
    private static readonly bool DEFAULT_RIGHT_HANDEDNESS = true;


    //Constructors, Contains all logical permutations of creating a vertex, general rules
    //[] Denotes optional
    //Position [-> Uv [-> Uv2 [-> Uv3 [-> Uv4]]]] [-> Normal [-> Tangent -> Handedness (Vector3 Only)] [-> Color]
    //Clusterfuck, i know, it leads to alot of permutations, but you can use any/all of them


    //Position


    //Was Playing Minecraft when I realized, why am I not using methodchaining (Minecraft uses it alot last time i tried my hand at modding)
    public ProceduralVertex(Vector3 position): this()
    {
        Position = position;

        Normal = DEFAULT_VECTOR3;
        RightHanded = DEFAULT_RIGHT_HANDEDNESS;
        Tangent = DEFAULT_VECTOR3;

        uvs = new Vector4[4];
        Color = DEFAULT_COLOR;

        Uv = DEFAULT_VECTOR4;
        Uv2 = DEFAULT_VECTOR4;
        Uv3 = DEFAULT_VECTOR4;
        Uv4 = DEFAULT_VECTOR4;

    }//, DEFAULT_VECTOR2) { }

    //void Test()
    //{
    //    ProceduralVertex myVert = new ProceduralVertex(Vector3.zero);

    //}

    //Several methods equal to the 128 combinatiosn I was going to do earlier... Although since I ditched Created a Vector4,3,2 for each uv.... its back to the 32 I had below....
    //SetUvChannel().SetUv().SEtPosition();
    public ProceduralVertex SetUvChannel(int channel, Vector4 uv) { Uvs[channel] = uv; return this; }
    public ProceduralVertex SetUv(Vector4 uv) { return SetUvChannel(0, uv); }
    public ProceduralVertex SetUv2(Vector4 uv) { return SetUvChannel(1, uv); }
    public ProceduralVertex SetUv3(Vector4 uv) { return SetUvChannel(2, uv); }
    public ProceduralVertex SetUv4(Vector4 uv) { return SetUvChannel(3, uv); }
    public ProceduralVertex SetColor(Color color) { Color = color; return this; }
    public ProceduralVertex SetPosition(Vector3 position) { Position = position; return this; }
    public ProceduralVertex SetNormal(Vector3 normal) { Normal = normal; return this; }
    public ProceduralVertex SetTangent(Vector4 tangent) { HandedTangent = tangent; return this; }
    public ProceduralVertex SetTangent(Vector3 tangent, bool rightHanded = true) { Tangent = tangent; RightHanded = rightHanded; return this; }
    ////+Uv(V2)
    //public ProceduralVertex(Vector3 position, Vector2 uv) : this(position, uv, DEFAULT_VECTOR2) { }
    ////+Uv +Uv2(V2)
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2) : this(position, uv, uv2, DEFAULT_VECTOR2) { }
    ////+Uv +Uv2 +Uv3(V2)
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3) : this(position, uv, uv2, uv3, DEFAULT_VECTOR2) { }
    ////+Uv +Uv2 +Uv3 +Uv4
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector2 uv4) : this(position, uv, uv2, uv3, uv4, DEFAULT_COLOR) { }

    ////+Normal
    //public ProceduralVertex(Vector3 position, Vector3 normal) : this(position, DEFAULT_VECTOR2, normal) { }
    ////+Normal +Uv(V2)
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector3 normal) : this(position, uv, DEFAULT_VECTOR2, normal) { }
    ////+Normal +Uv +Uv2(V2)
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector3 normal) : this(position, uv, uv2, DEFAULT_VECTOR2, normal) { }
    ////+Normal +Uv +Uv2 +Uv3(V2)
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector3 normal) : this(position, uv, uv2, uv3, DEFAULT_VECTOR2, normal) { }
    ////+Normal +Uv +Uv2 +Uv3 +Uv4(V2)
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector2 uv4, Vector3 normal) : this(position, uv, uv2, uv3, uv4, normal, DEFAULT_COLOR) { }

    ////+Normal +Tangent/Handedness
    //public ProceduralVertex(Vector3 position, Vector3 normal, Vector3 tangent, bool handedness) : this(position, DEFAULT_VECTOR2, normal, tangent, handedness) { }
    ////+Normal +Uv(V2) +Tangent/Handedness
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector3 normal, Vector3 tangent, bool handedness) : this(position, uv, DEFAULT_VECTOR2, normal, tangent, handedness) { }
    ////+Normal +Uv +Uv2(V2) +Tangent/Handedness
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector3 normal, Vector3 tangent, bool handedness) : this(position, uv, uv2, DEFAULT_VECTOR2, normal, tangent, handedness) { }
    ////+Normal +Uv +Uv2 +Uv3(V2) +Tangent/Handedness
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector3 normal, Vector3 tangent, bool handedness) : this(position, uv, uv2, uv3, DEFAULT_VECTOR2, normal, tangent, handedness) { }
    ////+Normal +Uv +Uv2 +Uv3 +Uv4(V2) +Tangent/Handedness
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector2 uv4, Vector3 normal, Vector3 tangent, bool handedness) : this(position, uv, uv2, uv3, uv4, normal, tangent, handedness, DEFAULT_COLOR) { }

    ////+Normal +Tangent
    //public ProceduralVertex(Vector3 position, Vector3 normal, Vector4 tangent) : this(position, DEFAULT_VECTOR2, normal, tangent) { }
    ////+Normal +Uv(V2) +Tangent
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector3 normal, Vector4 tangent) : this(position, uv, DEFAULT_VECTOR2, normal, tangent) { }
    ////+Normal +Uv +Uv2(V2) +Tangent
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector3 normal, Vector4 tangent) : this(position, uv, uv2, DEFAULT_VECTOR2, normal, tangent) { }
    ////+Normal +Uv +Uv2 +Uv3(V2) +Tangent
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector3 normal, Vector4 tangent) : this(position, uv, uv2, uv3, DEFAULT_VECTOR2, normal, tangent) { }
    ////+Normal +Uv +Uv2 +Uv3 +Uv4(V2) +Tangent
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector2 uv4, Vector3 normal, Vector4 tangent) : this(position, uv, uv2, uv3, uv4, normal, tangent, DEFAULT_COLOR) { }

    ////+Color
    //public ProceduralVertex(Vector3 position, Color color) : this(position, DEFAULT_VECTOR2, color) { }

    ////+Uv(V2) +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Color color) : this(position, uv, DEFAULT_VECTOR2, color) { }
    ////+Uv +Uv2(V2) +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Color color) : this(position, uv, uv2, DEFAULT_VECTOR2, color) { }
    ////+Uv +Uv2 +Uv3(V2) +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Color color) : this(position, uv, uv2, uv3, DEFAULT_VECTOR2, color) { }
    ////+Uv +Uv2 +Uv3 +Uv4(V2) +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector2 uv4, Color color) : this(position, uv, uv2, uv3, uv4, DEFAULT_VECTOR3, color) { }

    ////+Normal +Color
    //public ProceduralVertex(Vector3 position, Vector3 normal, Color color) : this(position, DEFAULT_VECTOR2, normal, color) { }
    ////+Normal +Uv(V2) +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector3 normal, Color color) : this(position, uv, DEFAULT_VECTOR2, normal, color) { }
    ////+Normal +Uv +Uv2(V2) +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector3 normal, Color color) : this(position, uv, uv2, DEFAULT_VECTOR2, normal, color) { }
    ////+Normal +Uv +Uv2 +Uv3(V2) +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector3 normal, Color color) : this(position, uv, uv2, uv3, DEFAULT_VECTOR2, normal, color) { }
    ////+Normal +Uv +Uv2 +Uv3 +Uv4(V2) +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector2 uv4, Vector3 normal, Color color) : this(position, uv, uv2, uv3, uv4, normal, DEFAULT_VECTOR3, DEFAULT_RIGHT_HANDEDNESS, color) { }

    ////+Normal +Tangent/Handedness +Color
    //public ProceduralVertex(Vector3 position, Vector3 normal, Vector3 tangent, bool handedness, Color color) : this(position, DEFAULT_VECTOR2, normal, tangent, handedness, color) { }
    ////+Normal +Uv(V2) +Tangent/Handedness +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector3 normal, Vector3 tangent, bool handedness, Color color) : this(position, uv, DEFAULT_VECTOR2, normal, tangent, handedness, color) { }
    ////+Normal +Uv(V2) +Uv2 +Tangent/Handedness +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector3 normal, Vector3 tangent, bool handedness, Color color) : this(position, uv, uv2, DEFAULT_VECTOR2, normal, tangent, handedness, color) { }
    ////+Normal +Uv(V2) +Uv2 +Uv3 +Tangent/Handedness +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector3 normal, Vector3 tangent, bool handedness, Color color) : this(position, uv, uv2, uv3, DEFAULT_VECTOR2, normal, tangent, handedness, color) { }
    ////+Normal +Uv(V2) +Uv2 +Uv3 +Uv4 +Tangent/Handedness +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector2 uv4, Vector3 normal, Vector3 tangent, bool handedness, Color color) //: this(position, uv, uv2,uv3,uv4, normal, tangent, handedness,color) { }
    //{
    //    Position = position;
    //    Uv = uv;
    //    Uv2 = uv2;
    //    Uv3 = uv3;
    //    Uv4 = uv4;
    //    Normal = normal;
    //    Tangent = tangent;
    //    RightHanded = handedness;
    //    Color = color;

    //}

    ////+Normal +Tangent +Color
    //public ProceduralVertex(Vector3 position, Vector3 normal, Vector4 tangent, Color color) : this(position, DEFAULT_VECTOR2, normal, tangent, color) { }
    ////+Normal +Uv(V2) +Tangent +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector3 normal, Vector4 tangent, Color color) : this(position, uv, DEFAULT_VECTOR2, normal, tangent, color) { }
    ////+Normal +Uv +Uv2(V2) +Tangent +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector3 normal, Vector4 tangent, Color color) : this(position, uv, uv2, DEFAULT_VECTOR2, normal, tangent, color) { }
    ////+Normal +Uv +Uv2 +Uv3(V2) +Tangent +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector3 normal, Vector4 tangent, Color color) : this(position, uv, uv2, uv3, DEFAULT_VECTOR2, normal, tangent, color) { }
    ////+Normal +Uv +Uv2 +Uv3 +Uv4(V2) +Tangent +Color
    //public ProceduralVertex(Vector3 position, Vector2 uv, Vector2 uv2, Vector2 uv3, Vector2 uv4, Vector3 normal, Vector4 tangent, Color color) : this(position, uv, uv2, uv3, uv4, normal, color)
    //{
    //    HandedTangent = tangent;
    //}

    public Vector3 Position { get; set; }

    public Vector3 Normal { get; set; }
    public Vector3 Tangent { get; set; }
    public bool RightHanded { get; set; }
    public Vector4 HandedTangent
    {
        get { return new Vector4(Tangent.x, Tangent.y, Tangent.z, RightHanded ? 1f : -1f); }
        set
        {
            Tangent = new Vector3(value.x, value.y, value.z);
            RightHanded = (value.w >= 0f);
        }
    }

    public Color Color { get; set; }
    private Vector4[] uvs;
    public Vector4[] Uvs
    {
        get
        {
            if (uvs == null)
                uvs = new Vector4[4];
            return uvs;
        }
        private set { uvs = value; }
    }
    public Vector4 Uv { get { return Uvs[0]; } set { Uvs[0] = value; } }
    public Vector4 Uv2 { get { return Uvs[1]; } set { Uvs[1] = value; } }
    public Vector4 Uv3 { get { return Uvs[2]; } set { Uvs[2] = value; } }
    public Vector4 Uv4 { get { return Uvs[3]; } set { Uvs[3] = value; } }

    public static ProceduralVertex Lerp(ProceduralVertex a, ProceduralVertex b, float time)
    {
        //ProceduralVertex temp =
        return new ProceduralVertex(Vector3.Lerp(a.Position, b.Position, time))
        {
            Normal = Vector3.Lerp(a.Normal, b.Normal, time),

            Tangent = Vector3.Lerp(a.Tangent, ((a.RightHanded != b.RightHanded) ? -1f : 1f) * b.Tangent, time),
            RightHanded = a.RightHanded,

            Uv = Vector4.Lerp(a.Uv, b.Uv, time),
            Uv2 = Vector4.Lerp(a.Uv2, b.Uv2, time),
            Uv3 = Vector4.Lerp(a.Uv3, b.Uv3, time),
            Uv4 = Vector4.Lerp(a.Uv4, b.Uv4, time)
        };
        //return temp;
    }
    public static ProceduralVertex Slerp(ProceduralVertex a, ProceduralVertex b, float time)
    {
        //ProceduralVertex temp =
        return new ProceduralVertex(Vector3.Slerp(a.Position, b.Position, time))
        {
            Normal = Vector3.Slerp(a.Normal, b.Normal, time),

            Tangent = Vector3.Slerp(a.Tangent, ((a.RightHanded != b.RightHanded) ? -1f : 1f) * b.Tangent, time),
            RightHanded = a.RightHanded,

            //No Slerps for vector4, so we cheat
            Uv = Vector4.Lerp(a.Uv, b.Uv, time),
            Uv2 = Vector4.Lerp(a.Uv2, b.Uv2, time),
            Uv3 = Vector4.Lerp(a.Uv3, b.Uv3, time),
            Uv4 = Vector4.Lerp(a.Uv4, b.Uv4, time)
        };
        //return temp;
    }

    //public ProceduralVertex Add(ProceduralVertex left, ProceduralVertex right, bool ignoreNormal = false, bool ignoreTangent = false, bool ignoreUvs = false, bool ignoreColor = false)
    //{
    //    ProceduralVertex temp = new ProceduralVertex(left.Position + right.Position)
    //    {
    //        Color = left.Color + (ignoreColor ? Color.clear : right.Color),
    //        Normal = left.Color + (ignoreColor ? Color.clear : right.Color),
    //    }
    //    return temp;
    //}

    public static ProceduralVertex operator +(ProceduralVertex left, ProceduralVertex right)
    {
        ProceduralVertex temp = new ProceduralVertex(left.Position + right.Position)
        {
            Color = left.Color + right.Color,
            Normal = left.Normal + right.Normal,
            Uv = left.Uv + right.Uv,
            Uv2 = left.Uv2 + right.Uv2,
            Uv3 = left.Uv3 + right.Uv3,
            Uv4 = left.Uv4 + right.Uv4,
        };

        if (left.RightHanded != right.RightHanded)
            temp.Tangent = left.Tangent - right.Tangent;
        else
            temp.Tangent = left.Tangent + right.Tangent;
        temp.RightHanded = left.RightHanded;
        return temp;
    }
    public static ProceduralVertex operator -(ProceduralVertex left, ProceduralVertex right)
    {
        ProceduralVertex temp = new ProceduralVertex(left.Position - right.Position)
        {
            Color = left.Color - right.Color,
            Normal = left.Normal - right.Normal,
            Uv = left.Uv - right.Uv,
            Uv2 = left.Uv2 - right.Uv2,
            Uv3 = left.Uv3 - right.Uv3,
            Uv4 = left.Uv4 - right.Uv4,
        };

        if (left.RightHanded != right.RightHanded)
            temp.Tangent = left.Tangent + right.Tangent;
        else
            temp.Tangent = left.Tangent - right.Tangent;
        temp.RightHanded = left.RightHanded;
        return temp;
    }
    public static ProceduralVertex operator *(ProceduralVertex pv, float scalar)
    {
        ProceduralVertex temp = pv;
        temp.Color *= scalar;
        temp.Tangent *= scalar;
        temp.Normal *= scalar;
        temp.Position *= scalar;
        temp.Uv *= scalar;
        temp.Uv2 *= scalar;
        temp.Uv3 *= scalar;
        temp.Uv4 *= scalar;
        return temp;
    }
    public static ProceduralVertex operator *(Quaternion rotation, ProceduralVertex pv)
    {
        ProceduralVertex temp = new ProceduralVertex()
        {
            //pv.Color = rotation * pv.Color; //Was Curious if tihs was a thing, it isn't.
            Tangent = rotation * pv.Tangent,
            Normal = rotation * pv.Normal,
            Position = rotation * pv.Position,
            Uv = rotation * pv.Uv,
            Uv2 = rotation * pv.Uv2,
            Uv3 = rotation * pv.Uv3,
            Uv4 = rotation * pv.Uv4
        };
        return temp;
    }
    public static ProceduralVertex operator /(ProceduralVertex pv, float scalar)
    {
        ProceduralVertex temp = new ProceduralVertex()
        {
            Color = pv.Color / scalar,
            Tangent = pv.Tangent / scalar,
            Normal = pv.Normal / scalar,
            Position = pv.Position / scalar,
            Uv = pv.Uv / scalar,
            Uv2 = pv.Uv2 / scalar,
            Uv3 = pv.Uv3 / scalar,
            Uv4 = pv.Uv4 / scalar
        };
        return temp;
    }
}
