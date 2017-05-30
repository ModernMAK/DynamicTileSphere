
//using UnityEngine;
//namespace PlanetGrapher
//{
//    public static class PlanetGraphBuilder
//    {
//        public static PlanetGraph CreateGraph(CellGraph cGraph, int chunkSize)
//        {
//            PlanetCell[] cells;
//            PlanetEdge[] cellEdges;
//            PlanetChunk[] chunks;
//            int
//                cellCount = cGraph.CellLookup.Length,
//                edgeCount = cGraph.EdgeLookup.Length,
//                chunkCount = cellCount / chunkSize + (cellCount % chunkSize > 0 ? 1 : 0);
//            PlanetGraph pGraph = new PlanetGraph(chunkSize)
//            {
//                CellLookup = cells = new PlanetCell[cellCount],
//                EdgeLookup = cellEdges = new PlanetEdge[edgeCount],
//                ChunkLookup = chunks = new PlanetChunk[chunkCount]
//            };

//            for (int i = 0; i < chunkCount; i++)
//            {
//                chunks[i] = new PlanetChunk(i, pGraph);
//            }

//            Cell cCell;
//            CellEdge
//                start,
//                current;
//            for (int i = 0; i < cellCount; i++)
//            {
//                cCell = cGraph.CellLookup[i];
//                cells[i] = new PlanetCell()
//                {
//                    Id = i,
//                    EdgeId = cCell.EdgeId,
//                    Position = cCell.Position,
//                    Graph = pGraph,
//                    ChunkId = i / chunkSize
//                };
//                Vector3 pos = Vector3.zero;
//                start = cCell.Edge;
//                current = start;
//                int count = 0;
//                do
//                {
//                    count++;
//                    pos += current.Origin;
//                    current = current.Next;
//                }
//                while (start != current);
//                cells[i].Position = pos / count;
//                cells[i].IsHex = (count == 6);
//            }

//            CellEdge cEdge;
//            for (int i = 0; i < edgeCount; i++)
//            {
//                cEdge = cGraph.EdgeLookup[i];
//                cellEdges[i] = new PlanetEdge()
//                {
//                    Id = i,
//                    TwinIndex = cEdge.Twin.Id,
//                    NextIndex = cEdge.Next.Id,
//                    PrevIndex = cEdge.PrevIndex,
//                    CellIndex = cEdge.CellIndex,
//                    Origin = cEdge.Origin,
//                    Graph = pGraph,
//                };
//            }

//            return pGraph;
//        }
//    }
//}