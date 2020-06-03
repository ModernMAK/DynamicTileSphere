using System.IO;

namespace ModernMAK.Serialization
{
    public interface IBinarySerializable
    {
        
        void Write(BinaryWriter writer);
        void Read(BinaryReader reader);
    }
}