using System;
using System.IO;
using ModernMAK.Serialization;

namespace ModernMAK.Graphing.Native
{
    namespace Generic
    {
        
        public class Graph<TPolyData, TEdgeData, TNodeData> : IBinarySerializable, IDisposable
            where TPolyData : PolygonData
            where TEdgeData : EdgeData
            where TNodeData : NodeData
        {
            public TPolyData Polygons { get; private set; }
            public TEdgeData Edges { get; private set; }
            public TNodeData Nodes { get; private set; }
            
            public void Write(BinaryWriter writer)
            {
                Polygons.Write(writer);
                Edges.Write(writer);
                Nodes.Write(writer);
            }

            public void Read(BinaryReader reader)
            {
                Polygons.Read(reader);
                Edges.Read(reader);
                Nodes.Read(reader);
            }

            public virtual void Dispose()
            {
                Polygons.Dispose();
                Edges.Dispose();
                Nodes.Dispose();
            }
        }
    }
    public class Graph : Generic.Graph<PolygonData,EdgeData,NodeData>
    {
    }
    public class PositionGraph<TPos> : Generic.Graph<PolygonData,EdgeData,PositionNodeData<TPos>> where TPos : struct

    {
    }

}