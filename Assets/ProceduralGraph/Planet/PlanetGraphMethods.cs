
//using System.Collections.Generic;
//using UnityEngine;
//namespace PlanetGrapher
//{
//    public static class PlanetGraphMethods
//    {
//        private class PlateQueue : Queue<PlanetCell>
//        {
//            public PlateQueue(int plateId, int desiredHeight)
//            {
//                Plate = new PlanetTectonicPlate(plateId, desiredHeight);//) { Id = plateId, IsOcean = oceanPlate };
//            }
//            public PlanetTectonicPlate Plate { get; private set; }
//            public int PlateId { get { return Plate.Id; } }
//        }
//        //Plates and process queue are altered
//        //processQueue is empty on completion
//        private static void TectonicPlateFloodFill(List<PlanetTectonicPlate> plates, Queue<PlateQueue> processQueue, PlanetTectonicPlateParameters parameters)
//        {
//            if (parameters.GenerationParameters.SkipPlateChance == 1f)
//                throw new System.ArgumentException("SkipChance cannot be 1! As this means no plates are ever created!");
//            PlateQueue activeQueue = null;
//            Queue<PlanetTectonicPlate> modified = new Queue<PlanetTectonicPlate>();
//            //While process is not empty
//            while (processQueue.Count > 0)
//            {
//                //Pop off a queu
//                activeQueue = processQueue.Dequeue();
//                //if empty throw error
//                if (activeQueue.Count <= 0)
//                {
//                    throw new System.Exception("Empty Queue in process!");
//                }
//                //Not the last one? Then check if we want to skip
//                if (processQueue.Count != 0 && Random.value <= parameters.GenerationParameters.SkipPlateChance)
//                {
//                    processQueue.Enqueue(activeQueue);
//                    continue;
//                }
//                //Popo off a cell
//                PlanetCell activeCell = activeQueue.Dequeue();
//                //If cell has already been set
//                if (activeCell.TectonicPlateId != PlanetCell.DEFAULT_PLATE_ID)
//                {
//                    //Only insert if still needs to process
//                    if (activeQueue.Count > 0)
//                    {
//                        processQueue.Enqueue(activeQueue);
//                    }
//                    continue;//Stop processing on this pass
//                }
//                //Set cell
//                activeCell.TectonicPlateId = activeQueue.PlateId;
//                //Add cell to plate
//                plates[activeQueue.PlateId].Add(activeCell);
//                //Go over neighbors
//                PlanetCell edgeCell = null;
//                foreach (PlanetEdge edge in activeCell)
//                {
//                    //Get neighbor
//                    edgeCell = edge.Twin.Cell;
//                    //If neighbor hasnt been set
//                    if (edgeCell.TectonicPlateId == PlanetCell.DEFAULT_PLATE_ID)
//                        activeQueue.Enqueue(edgeCell);//Add to queue
//                }
//                //If queue not empty
//                if (activeQueue.Count > 0)
//                {
//                    //add to process
//                    processQueue.Enqueue(activeQueue);
//                }
//            }
//        }
//        private static Queue<PlanetTectonicPlate> TectonicPlatePrune(List<PlanetTectonicPlate> plates, Queue<PlanetTectonicPlate> unprunedPlates, PlanetTectonicPlateParameters parameters)
//        {
//            Dictionary<int, int> DistanceLookup = new Dictionary<int, int>();
//            Queue<PlanetCell> DistanceQueue = new Queue<PlanetCell>();
//            Queue<PlanetTectonicPlate> prunedQueue = new Queue<PlanetTectonicPlate>();
//            PlanetTectonicPlate activePlate = null;
//            while (unprunedPlates.Count > 0)
//            {
//                activePlate = unprunedPlates.Dequeue();
//                if (activePlate.Count <= 0)
//                    continue;//Skip empty plates
//                DistanceLookup.Clear();
//                //Pick an arbitrary center cell
//                //We could use the origin cell, (0)
//                //But this gives us some interesting variation in the pressure
//                //activePlate.CenterId = Random.Range(0, activePlate.Count);
//                //The distance from the center to the center is always 0
//                DistanceLookup[activePlate.Center.Id] = 0;
//                //The breadth first queue
//                DistanceQueue.Enqueue(activePlate.Center);
//                //While queue is not empty
//                while (DistanceQueue.Count > 0)
//                {
//                    PlanetCell activeCell = DistanceQueue.Dequeue(), edgeCell = null;
//                    if (activePlate.Id != activeCell.TectonicPlateId)
//                        continue;//Skip enquement if not same plate
//                    int activeDepth = DistanceLookup[activeCell.Id], nextDepth = activeDepth + 1, edgeDepth = -1;
//                    foreach (PlanetEdge edge in activeCell)
//                    {
//                        edgeCell = edge.Twin.Cell;
//                        edgeDepth = -1;
//                        //Have we reached the edge cell?
//                        if (!DistanceLookup.TryGetValue(edgeCell.Id, out edgeDepth) || edgeDepth > nextDepth)
//                        {
//                            //We either,
//                            //A) have not, so we reached it at the nextDepth
//                            //B) have, but we found a shorter path
//                            DistanceLookup[edgeCell.Id] = nextDepth;
//                            //Now enqueue it
//                            DistanceQueue.Enqueue(activeCell);
//                        }
//                    }
//                }
//                //Now we prune
//                PlanetTectonicPlate prunePlate = new PlanetTectonicPlate(plates.Count, Random.Range(parameters.GenerationParameters.ElevationScale.Min, parameters.GenerationParameters.ElevationScale.Max));// { Id = plates.Count, IsOcean = Random.value <= 0.5f };
//                for (int j = 0; j < activePlate.Count; j++)
//                {
//                    PlanetCell activeCell = activePlate[j];
//                    int depth = 0;
//                    if (!(DistanceLookup.TryGetValue(activeCell.Id, out depth) && depth <= parameters.GenerationParameters.DesiredPlateDepth))
//                    {
//                        activePlate.Remove(activeCell);
//                        j--;
//                        activeCell.TectonicPlateId = prunePlate.Id;
//                        prunePlate.Add(activeCell);
//                    }
//                }
//                if (prunePlate.Count > 0)
//                {
//                    ShufflePlate(prunePlate);
//                    prunedQueue.Enqueue(prunePlate);
//                    //prunePlate.CenterId = Random.Range(0, prunePlate.Count);
//                    plates.Add(prunePlate);
//                }
//            }
//            return prunedQueue;
//        }
//        private static void ShufflePlate(PlanetTectonicPlate plate)
//        {
//            int n = plate.Count;
//            while (n > 1)
//            {
//                n--;
//                int k = Random.Range(0, n + 1);
//                PlanetCell value = plate[k];
//                plate[k] = plate[n];
//                plate[n] = value;
//            }
//        }
//        private static void TectonicPlateClean(List<PlanetTectonicPlate> plates)
//        {
//            //Clean Plates
//            for (int i = 0; i < plates.Count; i++)
//            {
//                if (plates[i].Count <= 0)
//                {
//                    plates.RemoveAt(i);
//                    i--;
//                }
//                else if (plates[i].Id != i)//We shifted, so now fix all Cells
//                {
//                    plates[i].Id = i;
//                    foreach (PlanetCell cell in plates[i])
//                        cell.TectonicPlateId = i;
//                }
//            }
//        }
//        private static void TectonicPlateMerge(List<PlanetTectonicPlate> plates, PlanetTectonicPlateParameters parameters)
//        {

//            List<PlanetTectonicPlate> SortedPlates = new List<PlanetTectonicPlate>(plates);
//            Dictionary<int, int> NeighborEdges = new Dictionary<int, int>();
//            SortedPlates.Sort((PlanetTectonicPlate x, PlanetTectonicPlate y) => { return x.Count.CompareTo(y.Count); });
//            //int empties = 0;
//            while (SortedPlates[0].Count <= 0)
//                SortedPlates.RemoveAt(0);
//            PlanetTectonicPlate activePlate = null;
//            //While a plate is less than the minimimum plate size
//            //OR
//            //The plate cound is acceptable (below max)
//            while (SortedPlates.Count > 1 && SortedPlates[0].Count < parameters.GenerationParameters.MinPlateSize) //First make each plate a minimum
//                                                                                                                   //|| (parameters.GenerationParameters.MaxPlates > 0 && SortedPlates.Count > parameters.GenerationParameters.MaxPlates)))
//            {
//                NeighborEdges.Clear();
//                activePlate = SortedPlates[0];//Mostly for convienience
//                foreach (PlanetCell cell in activePlate)
//                {
//                    PlanetCell edgeCell = null;
//                    PlanetTectonicPlate edgePlate = null;
//                    foreach (PlanetEdge edge in cell)
//                    {
//                        edgeCell = edge.Twin.Cell;
//                        edgePlate = plates[edgeCell.TectonicPlateId];

//                        if (edgePlate.Id != activePlate.Id)
//                        {
//                            if (!NeighborEdges.ContainsKey(edgePlate.Id))
//                                NeighborEdges[edgePlate.Id] = 1;
//                            else
//                                NeighborEdges[edgePlate.Id]++;

//                        }
//                    }
//                }
//                KeyValuePair<int, int>? largeKvp = null;
//                foreach (KeyValuePair<int, int> kvp in NeighborEdges)
//                {
//                    if (largeKvp == null)
//                        largeKvp = kvp;
//                    else if (kvp.Value > largeKvp.Value.Value || (kvp.Value == largeKvp.Value.Value && Random.value <= 0.5f))
//                    {
//                        largeKvp = kvp;
//                    }
//                }
//                //neighborPlates.Sort((PlanetTectonicPlate x, PlanetTectonicPlate y) => { return x.Count.CompareTo(y.Count); });
//                TectonicPlateMerge(plates, activePlate.Id, largeKvp.Value.Key);
//                //empties++;//Smallest is now empty
//                SortedPlates.RemoveAt(0);
//                SortedPlates.Sort((PlanetTectonicPlate x, PlanetTectonicPlate y) => { return x.Count.CompareTo(y.Count); });
//            }
//            while (parameters.GenerationParameters.MaxPlates > 0 && SortedPlates.Count > parameters.GenerationParameters.MaxPlates)//Next merge plates so that it is below max
//            {
//                NeighborEdges.Clear();
//                int id = Random.Range(0, Random.Range(0, SortedPlates.Count));//Tend toward smaller plates, but not always the smallest
//                activePlate = SortedPlates[id];//Mostly for convienience
//                foreach (PlanetCell cell in activePlate)
//                {
//                    PlanetCell edgeCell = null;
//                    PlanetTectonicPlate edgePlate = null;
//                    foreach (PlanetEdge edge in cell)
//                    {
//                        edgeCell = edge.Twin.Cell;
//                        edgePlate = plates[edgeCell.TectonicPlateId];

//                        if (edgePlate.Id != activePlate.Id)
//                        {
//                            if (!NeighborEdges.ContainsKey(edgePlate.Id))
//                                NeighborEdges[edgePlate.Id] = 1;
//                            else
//                                NeighborEdges[edgePlate.Id]++;

//                        }
//                    }
//                }
//                KeyValuePair<int, int>? largeKvp = null;
//                foreach (KeyValuePair<int, int> kvp in NeighborEdges)
//                {
//                    if (largeKvp == null)
//                        largeKvp = kvp;
//                    else if (kvp.Value > largeKvp.Value.Value || (kvp.Value == largeKvp.Value.Value && Random.value <= 0.5f))
//                    {
//                        largeKvp = kvp;
//                    }
//                }
//                //neighborPlates.Sort((PlanetTectonicPlate x, PlanetTectonicPlate y) => { return x.Count.CompareTo(y.Count); });
//                TectonicPlateMerge(plates, activePlate.Id, largeKvp.Value.Key);
//                //empties++;//Smallest is now empty
//                SortedPlates.RemoveAt(id);
//                SortedPlates.Sort((PlanetTectonicPlate x, PlanetTectonicPlate y) => { return x.Count.CompareTo(y.Count); });
//            }
//        }
//        private static void TectonicPlateMerge(List<PlanetTectonicPlate> plates, int small, int big)
//        {
//            plates[big].AddRange(plates[small]);
//            foreach (PlanetCell cell in plates[small])
//                cell.TectonicPlateId = plates[big].Id;
//            plates[small].Clear();
//        }
//        //public static int GetSign(float f)
//        //{
//        //    return (f > 0f ? 1 : (f < 0f ? -1 : 0));
//        //}
//        private static int GetCollisionType(Vector3 p1, Vector3 v1, Vector3 p2, Vector3 v2)
//        {
//            float dot = Vector3.Dot(v2 - v1, p2 - p1);
//            return (dot > 0f ? -1 : (dot < 0f ? 1 : 0));
//        }
//        private static void CalculateCellMovement(PlanetGraph graph, PlanetTectonicPlateParameters parameters)
//        {

//            float AxisSpin = Random.Range(parameters.MovementParameters.AxisSpin.Min, parameters.MovementParameters.AxisSpin.Max);

//            foreach (PlanetTectonicPlate plate in graph.PlateLookup)
//                plate.Spin = Random.Range(parameters.MovementParameters.PlateSpin.Min, parameters.MovementParameters.PlateSpin.Max);

//            foreach (PlanetCell cell in graph.Cells)
//            {
//                Vector3 pN = cell.Position.normalized;
//                Vector3 planetMovement = Quaternion.AngleAxis(AxisSpin, Vector3.up) * pN;
//                Vector3 plateMovement = Vector3.zero;
//                plateMovement = Quaternion.AngleAxis(cell.TectonicPlate.Spin, cell.TectonicPlate.Center.Position.normalized) * pN;

//                //cell.TectonicPlateVector =// Vector3.ProjectOnPlane(
//                //    Vector3.Lerp(plateMovement - pN, planetMovement - pN, parameters.AxisWeight).normalized;
//                ////, pN);//This is fine
//                cell.TectonicPlateVector = Vector3.ProjectOnPlane(Vector3.Lerp(plateMovement - pN, planetMovement - pN, parameters.MovementParameters.AxisWeight).normalized, pN);//This is fine
//                if (cell.TectonicPlateVector.sqrMagnitude > 1f)
//                    throw new System.Exception("Cell Error : " + cell.TectonicPlateVector);
//            }
//        }
//        private static void CalculateMovement(PlanetGraph graph, PlanetTectonicPlateParameters parameters)
//        {
//            CalculateCellMovement(graph, parameters);
//            CalculateBoundaries(graph, parameters);
//        }
//        private static void CalculateBoundaries(PlanetGraph graph, PlanetTectonicPlateParameters parameters)
//        {
//            //Boundaries are calculated a little wierdly
//            //FIRST
//            //All boundaries between a cell and a plate ARE THE SAME
//            //NOTE, not between a cell anda cell, a cell and a plate
//            //This enforces the idea that the boundaries are fault lines along the cell, as apposed to the cell edges
//            HashSet<int> InspectedEdges = new HashSet<int>();
//            Queue<List<PlanetEdge>> EdgeQueue = new Queue<List<PlanetEdge>>();
//            foreach (PlanetEdge edge in graph.EdgeLookup)
//            {
//                if (!edge.IsPlateBoundary)
//                    continue;
//                if (InspectedEdges.Contains(edge.Id))
//                    continue;
//                List<PlanetEdge> edgeList = new List<PlanetEdge>();
//                PlanetEdge edgeStart = edge;
//                while (edgeStart.Prev.Twin.Cell.TectonicPlate == edgeStart.Twin.Cell.TectonicPlate)
//                {
//                    if (edgeStart.Prev == edge)
//                        break;
//                    edgeStart = edgeStart.Prev;
//                }
//                PlanetEdge edgeEnd = edgeStart;
//                do
//                {

//                    edgeList.Add(edgeEnd);
//                    InspectedEdges.Add(edgeEnd.Id);
//                    if (edgeEnd.Next == edgeStart)
//                        break;
//                    edgeEnd = edgeEnd.Next;

//                } while (edgeEnd.Next.Twin.Cell.TectonicPlate == edgeEnd.Twin.Cell.TectonicPlate);
//                if (edgeList.Count > 0)
//                    EdgeQueue.Enqueue(edgeList);

//            }

//            while (EdgeQueue.Count > 0)
//            {
//                List<PlanetEdge> edgeList = EdgeQueue.Dequeue();
//                //Vector3 up = Vector3.zero;
//                Vector3 boundaryVector = Vector3.zero;
//                Vector3 boundaryOrigin = Vector3.zero;
//                LinkedList<PlanetEdge> linkedEdges = new LinkedList<PlanetEdge>(edgeList);


//                foreach (PlanetEdge edge in edgeList)
//                {
//                    boundaryVector += edge.Twin.Cell.TectonicPlateVector;
//                }
//                //up /= edges.Count;
//                //up.Normalize();
//                boundaryVector /= linkedEdges.Count;
//                int strength = 1;
//                while (linkedEdges.Count > 0)
//                {
//                    PlanetEdge leftEdge = linkedEdges.First.Value;
//                    PlanetEdge rightEdge = linkedEdges.Last.Value;

//                    if (linkedEdges.Count > 1)//Do we have more than 1?
//                    {
//                        boundaryOrigin += strength * (rightEdge.Origin + rightEdge.Twin.Origin) / 2f;
//                        linkedEdges.RemoveLast();
//                    }
//                    boundaryOrigin += strength * (leftEdge.Origin + leftEdge.Twin.Origin) / 2f;
//                    linkedEdges.RemoveFirst();
//                }
//                boundaryOrigin.Normalize();
//                int collisionType = GetCollisionType(edgeList[0].Cell.Position, edgeList[0].Cell.TectonicPlateVector, boundaryOrigin, boundaryVector);
//                Vector3 right = (edgeList[0].Origin - boundaryOrigin).normalized;
//                Vector3 up = ((edgeList[0].Origin + boundaryOrigin) / 2f).normalized;//Average of two ups form the correct up
//                Vector3 forward = (edgeList[0].Cell.Position - boundaryOrigin).normalized;

//                Vector3 pressure = Vector3.ProjectOnPlane((edgeList[0].Cell.TectonicPlateVector - boundaryVector) / 2f, up);
//                Vector3 tension = Vector3.Project(pressure, forward) / 2f;
//                Vector3 shear = Vector3.Project(pressure, right) / 2f;

//                foreach (PlanetEdge edge in edgeList)
//                    edge.BoundaryPressure = new Vector2(shear.magnitude, collisionType * tension.magnitude);
//                //edgeList[0].BoundaryPressure = new Vector2(shear.magnitude, collisionType * tension.magnitude);

//                //activeEdge = edgeList;
//                //while (activeEdge.Twin.Cell.TectonicPlate == edgeList.Twin.Cell.TectonicPlate)
//                //{
//                //    activeEdge.BoundaryPressure = edgeList.BoundaryPressure;
//                //    activeEdge = activeEdge.Next;
//                //    if (activeEdge == edgeList)
//                //        break;//Incase the plate is surrounded by another plate, if this happens, we will reach this flag
//                //}

//            }


//            ////This needs to be fixed
//            //foreach (PlanetEdge edge in graph.EdgeLookup)
//            //{
//            //    if (!edge.IsPlateBoundary)
//            //        continue;
//            //    Vector3 right = (edge.Origin - edge.Twin.Origin).normalized;
//            //    Vector3 up = ((edge.Origin + edge.Twin.Origin) / 2f).normalized;//Average of two ups form the correct up
//            //    Vector3 forward = (edge.Cell.Position - edge.Twin.Cell.Position).normalized;

//            //    //Vector3 cellTension = Vector3.Project(edge.Cell.TectonicPlateVector, forward);
//            //    //Vector3 cellShear = Vector3.Project(edge.Cell.TectonicPlateVector, right);

//            //    //Vector3 twinTension = Vector3.Project(edge.Twin.Cell.TectonicPlateVector, forward);
//            //    //Vector3 twinShear = Vector3.Project(edge.Twin.Cell.TectonicPlateVector, right);
//            //    //int collisionType = GetCollisionType(edge.Cell.Position, edge.Cell.TectonicPlateVector, edge.Twin.Cell.Position, edge.Twin.Cell.TectonicPlateVector);

//            //    ////X is Shear, and ranges from (0_None to 1_Full)
//            //    ////Y is Pressure, and ranges from (0_None to 1_Full)
//            //    //edge.BoundaryPressure = new Vector2((cellShear - twinShear).magnitude / 2f, collisionType * (cellTension - twinTension).magnitude / 2f);

//            //    ////Z is CollisonType and ranges from (-1_Colliding to 0_Shearing to 1_Seperating), note that while 0 implies pressure is 0 and shear is 1, this may not occur ( pressure will be 0 but shear may not be 1)

//            //    ////edge.Twin.BoundaryPressure = -1f * edge.BoundaryPressure;

//            //    Vector3 boundaryVector = Vector3.ProjectOnPlane((edge.Cell.TectonicPlateVector - edge.Twin.Cell.TectonicPlateVector) / 2f, up);
//            //    if (boundaryVector.sqrMagnitude > 1f)
//            //        throw new System.Exception("Calculating Boundary Error : " + boundaryVector);


//            //    Vector3 tension = Vector3.Project(boundaryVector, forward) / 2f;

//            //    Vector3 shear = Vector3.Project(boundaryVector, right) / 2f;
//            //    int collisionType = -GetCollisionType(edge.Cell.Position, edge.Cell.TectonicPlateVector, edge.Twin.Cell.Position, edge.Twin.Cell.TectonicPlateVector);

//            //    //X is Shear, and ranges from (0_None to 1_Full)
//            //    //Y is Pressure, and ranges from (0_None to 1_Full)
//            //    edge.BoundaryPressure = new Vector2(shear.magnitude, collisionType * tension.magnitude);
//            //    if (edge.BoundaryPressure.sqrMagnitude > 1f)
//            //    {
//            //        Debug.Log("BV:" + boundaryVector + " -> " + boundaryVector.magnitude + "\nT:" + tension + " -> " + tension.magnitude + "\nS:" + shear + " -> " + shear.magnitude + "\nBP:" + edge.BoundaryPressure + " -> " + edge.BoundaryPressure.magnitude);
//            //        throw new System.Exception("Edge Boundary Error : " + edge.BoundaryPressure);
//            //    }
//            //}
//        }


//        public static void TectonicPlateGenerator(PlanetGraph graph, PlanetTectonicPlateParameters parameters)
//        {
//            List<PlanetTectonicPlate> Plates = new List<PlanetTectonicPlate>();
//            Queue<PlateQueue> processQueue = new Queue<PlateQueue>();
//            Queue<PlanetTectonicPlate> unprunedPlates = new Queue<PlanetTectonicPlate>();
//            PlateQueue activeQueue = null;
//            PlanetCell activeCell = null;
//            //Debug.Log("A:" + Plates.Count);
//            for (int i = 0; i < parameters.GenerationParameters.DesiredPlates; i++)
//            {
//                activeQueue = new PlateQueue(Plates.Count, Random.Range(parameters.GenerationParameters.ElevationScale.Min, parameters.GenerationParameters.ElevationScale.Max));
//                activeCell = graph.CellLookup[Random.Range(0, graph.CellLookup.Length)];
//                Plates.Add(activeQueue.Plate);
//                unprunedPlates.Enqueue(activeQueue.Plate);
//                activeQueue.Enqueue(activeCell);
//                processQueue.Enqueue(activeQueue);
//            }
//            //Debug.Log("B:" + Plates.Count);
//            TectonicPlateFloodFill(Plates, processQueue, parameters);
//            //Debug.Log("C:" + Plates.Count);
//            //Clean plates for Pruning
//            TectonicPlateClean(Plates);
//            foreach (PlanetTectonicPlate plate in Plates)
//                ShufflePlate(plate);//Shuffle the order of the cells, randomly assigns a new center. This will also make the fact that we dont do a depth check in our flood fill useful
//                                    //Dont shuffle after pruning
//                                    //Debug.Log("D:" + Plates.Count);
//            do
//            {
//                unprunedPlates = TectonicPlatePrune(Plates, unprunedPlates, parameters);
//            }
//            while (unprunedPlates.Count > 0);
//            //Debug.Log("E:" + Plates.Count);
//            //Clean plates for merging
//            TectonicPlateClean(Plates);
//            //Debug.Log("F:" + Plates.Count);
//            TectonicPlateMerge(Plates, parameters);
//            //Debug.Log("G:" + Plates.Count);
//            //Clean plates for finalizing
//            TectonicPlateClean(Plates);
//            //Debug.Log("H:" + Plates.Count);
//            graph.PlateLookup = Plates.ToArray();
//            CalculateMovement(graph, parameters);
//        }

//    }
//}