//using UnityEngine;
//namespace PlanetGrapher
//{
//    [System.Serializable]
//    public struct IntRange { public int Min, Max; }
//    [System.Serializable]
//    public struct FloatRange { public float Min, Max; }
//    [System.Serializable]
//    public struct PlanetTectonicPlateParameters
//    {
//        public static PlanetTectonicPlateParameters Default { get { return new PlanetTectonicPlateParameters { GenerationParameters = Generation.Default, MovementParameters = Movement.Default }; } }

//        public Generation GenerationParameters;
//        public Movement MovementParameters;



//        [System.Serializable]
//        public struct Generation
//        {
//            public static Generation Default
//            {
//                get { return new Generation { ElevationScale = new IntRange { Min = -10, Max = 5 }, SkipPlateChance = 0.1f, DesiredPlates = 12, MaxPlates = 64, MinPlateSize = 12, DesiredPlateDepth = 8 }; }
//            }

//            public IntRange ElevationScale;

//            [Range(0f, 1f)]
//            public float SkipPlateChance;

//            //The initial number of plates created
//            public int DesiredPlates;
//            //The maximum number of plates (unless impossible)
//            public int MaxPlates;


//            //All plates will be at least this size (unless it is impossible), (Prioritized over MaxPlates)
//            public int MinPlateSize;
//            //All plates will be pruned to this "Radius" (unless its impossible)
//            public int DesiredPlateDepth;
//        }

//        [System.Serializable]
//        public struct Movement
//        {
//            public static Movement Default
//            {
//                get { return new Movement { AxisSpin = new FloatRange { Min = -10f, Max = 10f }, PlateSpin = new FloatRange { Min = -10f, Max = 10f }, AxisWeight = 0.5f }; }
//            }

//            public FloatRange AxisSpin;
//            public FloatRange PlateSpin;
//            [Range(0f, 1f)]
//            public float AxisWeight;
//        }
//    }
//}