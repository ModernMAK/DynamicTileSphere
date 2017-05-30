
//using System.Collections;
//using System.Collections.Generic;
//namespace PlanetGrapher
//{
//    public class PlanetChunk : IEnumerable<PlanetCell>
//    {
//        public PlanetChunk(int identity = -1, PlanetGraph graph = null)
//        {
//            Identity = identity;
//            Graph = graph;
//            Dirty = true;
//        }


//        public bool Dirty { get; set; }
//        internal int Identity { get; set; }
//        internal PlanetGraph Graph { get; set; }

//        public IEnumerator<PlanetCell> GetEnumerator()
//        {
//            return new PlanetChunkEnumerator(this);
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }
//    }
//}