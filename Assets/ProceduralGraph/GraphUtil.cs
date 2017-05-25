//using System;
//using System.Collections.Generic;
//using System.IO;

//namespace PlanetGrapher
//{
//    public interface IBinarySerializable
//    {
//        void Save(BinaryWriter writer);
//        void Load(BinaryReader reader);
//    }
//    public interface IFileSerializable
//    {
//        void Save(string fName, string fPath = null);
//        void Load(string fName, string fPath = null);
//        void Save(FileStream writer);
//        void Load(FileStream reader);
//    }

//    public class KeyCompare<K, V> : IComparer<KeyValuePair<K, V>>
//        where K : IComparable<K>
//    {
//        public int Compare(KeyValuePair<K, V> x, KeyValuePair<K, V> y)
//        {
//            return x.Key.CompareTo(y.Key);
//        }
//    }
//    public static class BinarySearch
//    {
//        public static V BinaryKeyValue<K, V>(KeyValuePair<K, V>[] inputArray, K key)
//             where K : IComparable<K>
//        {
//            return BinaryKeyValue(inputArray, key, 0, inputArray.Length - 1);
//        }
//        public static V BinaryKeyValue<K, V>(KeyValuePair<K, V>[] inputArray, K key, int min, int max)
//             where K : IComparable<K>
//        {
//            int index = BinaryKeySearch(inputArray, key, min, max);
//            if (index == -1)
//                throw new KeyNotFoundException("Key : " + key.ToString());
//            return inputArray[index].Value;
//        }

//        public static int BinaryKeySearch<K, V>(KeyValuePair<K, V>[] inputArray, K key)
//            where K : IComparable<K>
//        {
//            return BinaryKeySearch(inputArray, key, 0, inputArray.Length - 1);
//        }
//        public static int BinaryKeySearch<K, V>(KeyValuePair<K, V>[] inputArray, K key, int min, int max)
//            where K : IComparable<K>
//        {
//            while (min <= max)
//            {
//                int mid = (min + max) / 2;
//                int delta = key.CompareTo(inputArray[mid].Key);
//                if (delta > 0)
//                {
//                    min = mid + 1;
//                }
//                else if (delta < 0)
//                {
//                    max = mid - 1;
//                }
//                else return mid;
//            }
//            return -1;
//        }

//    }
//    public static class QuickSortExtensions
//    {
//        public static void Quicksort<T, C>(T[] input, C comparar) where C : IComparer<T>
//        {
//            Quicksort(input, 0, input.Length - 1, comparar);
//        }
//        public static void Quicksort<T, C>(T[] input, int low, int high, C comparar) where C : IComparer<T>

//        {
//            int pivot_loc = 0;
//            Stack<int> stack = new Stack<int>(input.Length);
//            stack.Push(low);
//            stack.Push(high);

//            while (stack.Count > 0)
//            {
//                high = stack.Pop();
//                low = stack.Pop();

//                pivot_loc = Partition(input, low, high, comparar);
//                if (pivot_loc - 1 > low)
//                {
//                    stack.Push(low);
//                    stack.Push(pivot_loc - 1);
//                }


//                if (pivot_loc + 1 < high)
//                {
//                    stack.Push(pivot_loc + 1);
//                    stack.Push(high);
//                }

//            }
//            //Recursive
//            //if (low < high)
//            //{
//            //    pivot_loc = Partition(input, low, high, comparar);
//            //    Quicksort(input, low, pivot_loc - 1, comparar);
//            //    Quicksort(input, pivot_loc + 1, high, comparar);
//            //}
//        }
//        private static int Partition<T, C>(T[] input, int low, int high, C comparar) where C : IComparer<T>
//        {
//            T pivot = input[high];
//            int i = low - 1;

//            for (int j = low; j < high; j++)
//            {
//                int comp = comparar.Compare(input[j], pivot);
//                if (comp <= 0)
//                {
//                    i++;
//                    Swap(input, i, j);
//                }
//            }
//            Swap(input, i + 1, high);
//            return i + 1;
//        }


//        public static void Quicksort<T>(T[] input) where T : IComparable<T>
//        {
//            Quicksort(input, 0, input.Length - 1);
//        }
//        public static void Quicksort<T>(T[] input, int low, int high) where T : IComparable<T>
//        {

//            int pivot_loc = 0;
//            Stack<int> stack = new Stack<int>(input.Length);
//            stack.Push(low);
//            stack.Push(high);

//            while (stack.Count > 0)
//            {
//                high = stack.Pop();
//                low = stack.Pop();

//                pivot_loc = Partition(input, low, high);
//                if (pivot_loc - 1 > low)
//                {
//                    stack.Push(low);
//                    stack.Push(pivot_loc - 1);
//                }


//                if (pivot_loc + 1 < high)
//                {
//                    stack.Push(pivot_loc + 1);
//                    stack.Push(high);
//                }

//            }

//        }
//        private static int Partition<T>(T[] input, int low, int high) where T : IComparable<T>
//        {
//            T pivot = input[high];
//            int i = low - 1;

//            for (int j = low; j < high - 1; j++)
//            {
//                int comp = input[j].CompareTo(pivot);
//                if (comp <= 0)
//                {
//                    i++;
//                    Swap(input, i, j);
//                }
//            }
//            Swap(input, i + 1, high);
//            return i + 1;
//        }
//        private static void Swap<T>(T[] ar, int a, int b)
//        {
//            T temp = ar[a];
//            ar[a] = ar[b];
//            ar[b] = temp;
//        }

//    }
//}