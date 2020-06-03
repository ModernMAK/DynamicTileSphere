using System.IO;

namespace ModernMAK.Serialization
{
    public interface IBinarySerializer<TData>
    {
        void Write(BinaryWriter writer, TData data);
        TData Read(BinaryReader reader);
        bool TryRead(BinaryReader reader, out TData data);
        void ReadInto(BinaryReader reader, ref TData data);
        bool TryReadInto(BinaryReader reader, ref TData data);
    }
}