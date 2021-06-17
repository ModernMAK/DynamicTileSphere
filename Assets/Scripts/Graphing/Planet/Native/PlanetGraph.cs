using System.Collections.Generic;
using Graphing.Generic.Native;
using Graphing.Position.Generic.Native;

namespace Graphing.Planet.Native
{
    public class PlanetGraph : Graph<DataPolygon<PlanetPolyData>, Edge, DataVertex<PositionData>>
    {
        public PlanetGraph(int nodeCapacity = 0, int edgeCapacity = 0, int polyCapacity = 0, int partitionSize = 1) :
            base(nodeCapacity, edgeCapacity, polyCapacity)
        {
            //PartitionSize = partitionSize > 0 ? partitionSize : 1;
            //Partitions =
            //    new List<PlanetPartition>(nodeCapacity / PartitionSize + (nodeCapacity % PartitionSize > 0 ? 1 : 0));
        }

        //private int PartitionSize { get; set; }

        //public IList<PlanetPartition> Partitions { get; internal set; }

        //internal override void LoadDual<GraphT>(GraphT data)
        //{
        //    base.LoadDual(data);
        //    Partitions.Clear();
        //    var count = data.Polys.Count / PartitionSize + (data.Polys.Count % PartitionSize > 0 ? 1 : 0);
        //    for (var i = 0; i < count; i++)
        //    {
        //        var partition = new PlanetPartition(PartitionSize);
        //        for (var j = 0; j < PartitionSize; j++)
        //        {
        //            var polyId = i * PartitionSize + j;
        //            if (polyId > data.Polys.Count)
        //                break;
        //            partition.Add(Polys[polyId]);
        //        }

        //        Partitions.Add(partition);
        //    }
        //}
    }
}