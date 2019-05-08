using System.Collections.Generic;
using System.IO;

namespace Graphing.Generic
{
    public class Graph<PolyT, EdgeT, NodeT>
        where NodeT : GraphNode<PolyT, EdgeT, NodeT>, new()
        where EdgeT : GraphEdge<PolyT, EdgeT, NodeT>, new()
        where PolyT : GraphPoly<PolyT, EdgeT, NodeT>, new()
    {
        public Graph(int nodeCapacity = 0, int edgeCapacity = 0, int polyCapacity = 0)
        {
            Nodes = new List<NodeT>(nodeCapacity);
            Edges = new List<EdgeT>(edgeCapacity);
            Polys = new List<PolyT>(polyCapacity);
        }

        public List<NodeT> Nodes { get; internal set; }

        public List<EdgeT> Edges { get; internal set; }

        public List<PolyT> Polys { get; internal set; }

        public GraphT Dual<GraphT>() where GraphT : Graph<PolyT, EdgeT, NodeT>, new()
        {
            var dual = new GraphT();
            dual.LoadDual(this);
            return dual;
        }

        internal virtual void Serialize(BinaryWriter writer)
        {
            writer.Write(Nodes.Count);
            writer.Write(Edges.Count);
            writer.Write(Polys.Count);

            foreach (var node in Nodes)
                node.Serialize(writer);

            foreach (var edge in Edges)
                edge.Serialize(writer);

            foreach (var poly in Polys)
                poly.Serialize(writer);
        }

        internal virtual void Deserialize(BinaryReader reader)
        {
            Nodes.Clear();
            var nodes = reader.ReadInt32();
            var edges = reader.ReadInt32();
            var polys = reader.ReadInt32();

            for (var i = 0; i < nodes; i++)
                Nodes.Add(new NodeT {Identity = i});
            for (var i = 0; i < edges; i++)
                Edges.Add(new EdgeT {Identity = i});
            for (var i = 0; i < polys; i++)
                Polys.Add(new PolyT {Identity = i});

            foreach (var node in Nodes)
                node.Deserialize(reader, this);
            foreach (var edge in Edges)
                edge.Deserialize(reader, this);
            foreach (var poly in Polys)
                poly.Deserialize(reader, this);
        }

        internal virtual void LoadDual<GraphT>(GraphT data) where GraphT : Graph<PolyT, EdgeT, NodeT>
        {
            var PolyDualLookup = new Dictionary<PolyT, NodeT>();
            var EdgeDualLookup = new Dictionary<EdgeT, EdgeT>();
            var NodeDualLookup = new Dictionary<NodeT, PolyT>();

            Nodes.Clear();
            Edges.Clear();
            Polys.Clear();

            foreach (var poly in data.Polys) Nodes.Add(PolyDualLookup[poly] = new NodeT());
            foreach (var edge in data.Edges) Edges.Add(EdgeDualLookup[edge] = new EdgeT());
            foreach (var node in data.Nodes) Polys.Add(NodeDualLookup[node] = new PolyT());


            foreach (var poly in data.Polys)
                PolyDualLookup[poly].LoadDual(poly, EdgeDualLookup[poly.Edge]);

            foreach (var node in data.Nodes)
                NodeDualLookup[node].LoadDual(node, EdgeDualLookup[node.Edge]);

            foreach (var edge in data.Edges)
            {
                var dualEdge = EdgeDualLookup[edge]; // EdgeDualLookup[edge];

                //From my VD_DEdge (Which is saved under Assets/ProceduralGraph/Graph in my first commit of github (since I will have gotten rid of it eventually))
                /*
                Dual_Edge = Dual(Edge)
                Dual_Next = Dual(Edge.Twin.Next)
                Dual_Previous = Dual(Edge.Previous.Twin)
                Dual_Twin  = Dual(Edge.Twin)
                */
                dualEdge.LoadDual(
                    edge,
                    PolyDualLookup[edge.Poly],
                    NodeDualLookup[edge.Node],
                    edge.Twin != null ? EdgeDualLookup[edge.Twin] : null,
                    edge.Twin != null && edge.Twin.Next != null ? EdgeDualLookup[edge.Twin.Next] : null
                    //(edge.Prev != null && edge.Prev.Twin != null) ? EdgeDualLookup[edge.Prev.Twin] : null
                );
            }

            Nodes.Sort((x, y) => { return x.Identity.CompareTo(y.Identity); });
            Edges.Sort((x, y) => { return x.Identity.CompareTo(y.Identity); });
            Polys.Sort((x, y) => { return x.Identity.CompareTo(y.Identity); });
        }
    }
}