using System.Collections.Generic;

namespace Graphing.Planet
{

    public class PlanetPartition : List<PlanetPoly>
    {
        public PlanetPartition(int capacity = 0) : base(capacity)
        {
            IsDirty = true;
        }
        public new void Add(PlanetPoly item)
        {
            base.Add(item);
            item.OnChanged += Dirty;
        }
        public void Dirty()
        {
            IsDirty = true;
        }
        public bool IsDirty { get; private set; }
        public void Clean() { IsDirty = false; }
    }

}