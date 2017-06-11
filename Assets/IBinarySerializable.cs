using System.IO;

//namespace Graphing.Generic
//{
public interface IBinarySerializable
{
    /// <summary>
    /// Writes from the active object using the given Binary Writer
    /// </summary>
    /// <param name="writer">The Binary Writer to write to</param>
    void Serialize(BinaryWriter writer);
    /// <summary>
    /// Reads into the active object using the given Binary Reader
    /// </summary>
    /// <param name="writer">The Binary Reader to read from</param>
    void Deserialize(BinaryReader reader);
}
//}