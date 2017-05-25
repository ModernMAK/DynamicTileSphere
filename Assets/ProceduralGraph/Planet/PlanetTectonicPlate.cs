using System.Collections.Generic;
namespace PlanetGrapher
{

    public class PlanetTectonicPlate : List<PlanetCell>
    {
        public PlanetTectonicPlate(int id, int desiredHeight)
        {
            Id = id;
            DesiredHeight = desiredHeight;
        }

        public int CenterId { get { return 0; } }
        public PlanetCell Center { get { return this[CenterId]; } }

        public int Id { get; internal set; }
        public bool IsOcean { get { return DesiredHeight < 0; } }

        public int DesiredHeight { get; private set; }
        public float Spin { get; internal set; }
    }
}