using System;
using System.IO;
using ModernMAK.Graphing.Native.Generic;
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
          
            public Graph(TPolyData polyData, TEdgeData edgeData, TNodeData nodeData)
            {
                Polygons = polyData;
                Edges = edgeData;
                Nodes = nodeData;
            }
            public TPolyData Polygons { get; protected set; }
            public TEdgeData Edges { get; protected set; }
            public TNodeData Nodes { get; protected set; }
            
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
        public Graph(PolygonData polyData, EdgeData edgeData, NodeData nodeData) : base(polyData, edgeData, nodeData)
        {
        }

        public Graph() : base(new PolygonData(), new EdgeData(), new NodeData())
        {
            
        }
    }
}