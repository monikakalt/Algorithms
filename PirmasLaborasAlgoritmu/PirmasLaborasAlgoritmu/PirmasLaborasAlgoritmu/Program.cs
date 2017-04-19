using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirmasLaborasAlgoritmu
{
    class Program
    {
        static Random rand;
        public static ulong opMcout;
        public static ulong opDcout;

        static void Main(string[] args)
        {
            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            rand = new Random(seed);
            Bandwidth_Tests();
        }

        public static void InsertionSort(DataArray items)
        {
            for(int i = 1; i < items.Length; i++)
            {
                int j = i;
                while ((j > 0) && (items[j] < items[j - 1]))
                {
                    items.Swap(j, items[j], items[j - 1]);
                    j--;
                }
            }
        }

        public static void InsertionSort(DataList items)
        {
            items.GoHeadSkipOne();
            for(int i = 0; i < items.Length; i++)
            {
                if(items.CurrentVal() < items.PreviousVal())
                {
                    items.Swap(items.CurrentVal(), items.PreviousVal());
                    for (int z = i; z > 0; z--)
                    {
                        items.GoHeadSkipOne();
                        for(int q = 0; q < z - 1; q++)
                        {
                            items.Next();
                        }
                        if (items.CurrentVal() < items.PreviousVal())
                        {
                            items.Swap(items.CurrentVal(), items.PreviousVal());
                        }
                    }
                    items.GoHeadSkipOne();
                    for (int z = 0; z < i; z++)
                    {
                        items.Next();
                    }
                } else {
                    items.Next();
                }

            }
        }

        public static void CountingSort(DataArray items)
        {
            int[] helperArray = Enumerable.Repeat(0, items.BiggestVal + 1).ToArray();
            int[] results = new int[items.Length];
            for(int i = 0; i < items.Length; i++)
            {
                helperArray[items[i]]++;
            }
            int accumulatedVal = helperArray[0];
            for (int i = 1; i < helperArray.Length; i++)
            {
                helperArray[i] += accumulatedVal;
                accumulatedVal = helperArray[i];
            }
            for(int i = 0; i < items.Length; i++)
            {
                int index = --helperArray[items[i]];
                int item = items[i];
                results[index] = items[i];
            }
            for(int i = 0; i < items.Length; i++)
            {
                items.Replace(i, results[i]);
            }

        }

        public static void CountingSort(DataList items)
        {
            int[] helperArray = Enumerable.Repeat(0, items.BiggestVal + 2).ToArray();
            int[] results = new int[items.Length];
            items.GoHead();
            for (int i = 0; i < items.Length; i++)
            {
                helperArray[items.CurrentVal()]++;
                items.Next();
            }
            int accumulatedVal = helperArray[0];
            for(int i = 1; i < helperArray.Length; i++)
            {
                helperArray[i] += accumulatedVal;
                accumulatedVal = helperArray[i];
            }
            items.GoHead();
            for (int i = 0; i < items.Length; i++)
            {
                int index = --helperArray[items.CurrentVal()];
                int item = items.CurrentVal();
                results[index] = items.CurrentVal();
                items.Next();
            }
            items.GoHead();
            for(int i = 0; i < items.Length; i++)
            {
                items.Replace(results[i]);
                items.Next();
            }
        }

        public static void Test_Array_List(int seed)
        {
            int n = 12;
            MyDataArray myarray = new MyDataArray(n, seed);
            Console.WriteLine("\n Insertion sort method on array \n");
            myarray.Print(n);
            InsertionSort(myarray);
            myarray.Print(n);
            MyDataList mylist = new MyDataList(n, seed);
            Console.WriteLine("\n Insertion sort method on list \n");
            mylist.Print(n);
            InsertionSort(mylist);
            mylist.Print(n);
            MyDataArray myCountingArray = new MyDataArray(n, seed);
            Console.WriteLine("\n\n Counting sort method on array \n");
            myCountingArray.Print(n);
            CountingSort(myCountingArray);
            myCountingArray.Print(n);
            MyDataList myCountingList = new MyDataList(n, seed);
            Console.WriteLine("\n Counting sort method on list \n");
            myCountingList.Print(n);
            CountingSort(myCountingList);
            myCountingList.Print(n);
        }

        public static void Test_File_Array_List(int seed)
        {
            int n = 12;
            string filename;
            filename = @"mydataarray.dat";
            MyFileArray myfilearray = new MyFileArray(filename, n, seed);
            using (myfilearray.fs = new FileStream(filename, FileMode.Open,
           FileAccess.ReadWrite))
            {
                Console.WriteLine("\n\n Insertion sort file array \n");
                myfilearray.Print(n);
                InsertionSort(myfilearray);
                myfilearray.Print(n);
            }
            filename = @"mydatalist.dat";
            MyFileList myfilelist = new MyFileList(filename, n, seed);
            using (myfilelist.fs = new FileStream(filename, FileMode.Open,
           FileAccess.ReadWrite))
            {
                Console.WriteLine("\n Insertion sort file list \n");
                myfilelist.Print(n);
                InsertionSort(myfilelist);
                myfilelist.Print(n);
            }
            filename = @"myCountingArray.dat";
            MyFileList myFileCountingArray = new MyFileList(filename, n, seed);
            using (myFileCountingArray.fs = new FileStream(filename, FileMode.Open,
           FileAccess.ReadWrite))
            {
                Console.WriteLine("\n\n Counting sort file array \n");
                myFileCountingArray.Print(n);
                CountingSort(myFileCountingArray);
                myFileCountingArray.Print(n);
            }
            filename = @"myCountingList.dat";
            MyFileList myFileCountingList = new MyFileList(filename, n, seed);
            using (myFileCountingList.fs = new FileStream(filename, FileMode.Open,
           FileAccess.ReadWrite))
            {
                Console.WriteLine("\n Counting sort file list \n");
                myFileCountingList.Print(n);
                CountingSort(myFileCountingList);
                myFileCountingList.Print(n);
            }
        }

        static string RandomName(int size, int seed) {

            StringBuilder builder = new StringBuilder();
            char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
            builder.Append(ch);
            for (int i = 1; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 97)));
                builder.Append(ch);
            }
            return builder.ToString();
        }


        static void Bandwidth_Tests()
        {
            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            ConsoleKeyInfo cki;
            bool exit = false;
            do {
                DisplayMenu();
                cki = Console.ReadKey();
                Console.Clear();
                switch (cki.KeyChar.ToString())
                {
                    case "1":
                        Console.WriteLine(">1. Test ");
                        Test_Array_List(seed);
                        break;
                    case "2":
                        Console.WriteLine(">2. Test ");
                        Test_File_Array_List(seed);
                        break;
                    case "3":
                        Console.WriteLine(">3. Analysis");
                        INSERTION_Analysis_Array_List(seed);
                        break;
                    case "4":
                        Console.WriteLine(">4. Analysis");
                        INSERTION_Analysis_File_Array_List(seed);
                        break;
                    case "5":
                        Console.WriteLine(">5. Analysis");
                        COUNTING_Analysis_Array_List(seed);
                        break;
                    case "6":
                        Console.WriteLine(">6. Analysis");
                        COUNTING_Analysis_File_Array_List(seed);
                        break;
                    case "7":
                        Console.WriteLine(">7. Test");
                        Test_Hash_Tables(seed);
                        break;
                    case "8":
                        Console.WriteLine(">8. Analysis");
                        HASHTABLE_Analysis(seed);
                        break;
                    case "9":
                        exit = true;
                        break;
                }
            } while (exit == false);
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\n Menu\n");
            Console.WriteLine(">1. Test Array / List");
            Console.WriteLine(">2. Test FILE Array / FILE List");
            Console.WriteLine(">3. INSERTION Analysis Array / List");
            Console.WriteLine(">4. INSERTION Analysis FILE Array / FILE List");
            Console.WriteLine(">5. COUNTING Analysis Array / List");
            Console.WriteLine(">6. COUNTING Analysis FILE Array / FILE List");
            Console.WriteLine(">7. HASHTABLES Test");
            Console.WriteLine(">8. HASTABLES Analysis");
            Console.WriteLine(">9. Exit \n");
            Console.Write(">");
        }


        static void INSERTION_Analysis_Array_List(int seed)
        {
            int n = 100;
            Console.WriteLine("\n INSERTION sort");
            Console.WriteLine("\n ARRAY \n N Run Time Op M Count Op D Count\n");
            for (int i = 0; i < 7; i++)
            {
                MyDataArray myarray = new MyDataArray(n, seed);
                Stopwatch myTimer = new Stopwatch();
                myTimer.Start();
                InsertionSort(myarray);
                myTimer.Stop();
                Console.WriteLine(" {0,6:N0} {1} {2,15:N0} {3,15:N0}", n, myTimer.Elapsed, opMcout, opDcout);
                n = n * 2;
                GC.Collect();
            }
            n = 100;
            Console.WriteLine("\n LIST \n N Run Time Op M Count Op D Count\n");
            for (int i = 0; i < 7; i++)
            {
                MyDataList mylist = new MyDataList(n, seed);
                Stopwatch myTimer = new Stopwatch();
                myTimer.Start();
                InsertionSort(mylist);
                myTimer.Stop();
                Console.WriteLine(" {0,6:N0} {1} {2,15:N0} {3,15:N0}", n, myTimer.Elapsed, opMcout, opDcout);
                n = n * 2;
                GC.Collect();
            }

        }

        public static void COUNTING_Analysis_Array_List(int seed)
        {
            int n = 100;
            Console.WriteLine("\n COUNTING sort");
            Console.WriteLine("\n ARRAY \n N Run Time Op M Count Op D Count\n");
            for (int i = 0; i < 7; i++)
            {
                MyDataArray myarray = new MyDataArray(n, seed);
                Stopwatch myTimer = new Stopwatch();
                myTimer.Start();
                CountingSort(myarray);
                myTimer.Stop();
                Console.WriteLine(" {0,6:N0} {1} {2,15:N0} {3,15:N0}", n, myTimer.Elapsed, opMcout, opDcout);
                n = n * 2;
                GC.Collect();
            }
            n = 100;
            Console.WriteLine("\n LIST \n N Run Time Op M Count Op D Count\n");
            for (int i = 0; i < 7; i++)
            {
                MyDataList mylist = new MyDataList(n, seed);
                Stopwatch myTimer = new Stopwatch();
                myTimer.Start();
                CountingSort(mylist);
                myTimer.Stop();
                Console.WriteLine(" {0,6:N0} {1} {2,15:N0} {3,15:N0}", n, myTimer.Elapsed, opMcout, opDcout);
                n = n * 2;
                GC.Collect();
            }
        }


        public static void INSERTION_Analysis_File_Array_List(int seed)
        {
            string filename;
            filename = @"mydataarray.dat";
            int n = 100;
            Console.WriteLine("\n INSERTION sort");
            Console.WriteLine("\n FILE ARRAY \n N Run Time Op M Count Op D Count\n");
            for (int i = 0; i < 7; i++)
            {
                MyFileArray myfilearray = new MyFileArray(filename, n, seed);
                Stopwatch myTimer = new Stopwatch();
                using (myfilearray.fs = new FileStream(filename, FileMode.Open,
               FileAccess.ReadWrite))
                {
                    myTimer.Start();
                    InsertionSort(myfilearray);
                    myTimer.Stop();
                }
                Console.WriteLine(" {0,6:N0} {1} {2,15:N0} {3,15:N0}", n,
               myTimer.Elapsed, opMcout, opDcout);
                n = n * 2;
                GC.Collect();
            }
            filename = @"mydatalist.dat";
            n = 100;
            Console.WriteLine("\n FILE LIST \n N Run Time Op M Count Op D Count\n");
            for (int i = 0; i < 7; i++)
            {
                MyFileList myfilelist = new MyFileList(filename, n, seed);
                Stopwatch myTimer = new Stopwatch();
                using (myfilelist.fs = new FileStream(filename, FileMode.Open,
               FileAccess.ReadWrite))
                {
                    myTimer.Start();
                    InsertionSort(myfilelist);
                    myTimer.Stop();
                }
                Console.WriteLine(" {0,6:N0} {1} {2,15:N0} {3,15:N0}", n,
               myTimer.Elapsed, opMcout, opDcout);
                n = n * 2;
                GC.Collect();
            }
        }

        public static void COUNTING_Analysis_File_Array_List(int seed)
        {
            string filename = @"mydataarray.dat";
            Console.WriteLine("\n COUNTING sort");
            int n = 100;
            Console.WriteLine("\n FILE ARRAY \n N Run Time Op M Count Op D Count\n");
            for (int i = 0; i < 7; i++)
            {
                MyFileArray myfilearray = new MyFileArray(filename, n, seed);
                Stopwatch myTimer = new Stopwatch();
                using (myfilearray.fs = new FileStream(filename, FileMode.Open,
               FileAccess.ReadWrite))
                {
                    myTimer.Start();
                    CountingSort(myfilearray);
                    myTimer.Stop();
                }
                Console.WriteLine(" {0,6:N0} {1} {2,15:N0} {3,15:N0}", n,
               myTimer.Elapsed, opMcout, opDcout);
                n = n * 2;
                GC.Collect();
            }
            filename = @"mydatalist.dat";
            n = 100;
            Console.WriteLine("\n FILE LIST \n N Run Time Op M Count Op D Count\n");
            for (int i = 0; i < 7; i++)
            {
                MyFileList myfilelist = new MyFileList(filename, n, seed);
                Stopwatch myTimer = new Stopwatch();
                using (myfilelist.fs = new FileStream(filename, FileMode.Open,
               FileAccess.ReadWrite))
                {
                    myTimer.Start();
                    CountingSort(myfilelist);
                    myTimer.Stop();
                }
                Console.WriteLine(" {0,6:N0} {1} {2,15:N0} {3,15:N0}", n,
               myTimer.Elapsed, opMcout, opDcout);
                n = n * 2;
                GC.Collect();
            }
        }

        public static void Test_Hash_Tables(int seed)
        {
            int nameLength = 10;
            int n = 12;
            int initialCapacity = n * 5;
            float loadFactor = (float)0.25;
            List<string> names = new List<string>();

            MyDataHashTable<string, string> myHashTable = new MyDataHashTable<string, string>(initialCapacity, loadFactor);
            for (int i = 0; i < n; i++)
            {
                string generatedName = RandomName(nameLength, seed);
                Console.WriteLine(generatedName);
                names.Add(generatedName);
                myHashTable.Put(generatedName, generatedName);
            }
            names.Add(RandomName(10, seed));
            Console.WriteLine(myHashTable);
            Console.WriteLine("Search test:");

            foreach (var name in names)
            {
                Console.WriteLine(name + " " + myHashTable.Contains(name));
            }
            Console.WriteLine("\n");


            Console.WriteLine("\n Search FILE Test \n");
            string filename = @"mydatatable.dat";
            myHashTable.WriteToFile(filename, nameLength);
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (var name in names)
                {
                    Console.WriteLine(name + " " + myHashTable.FileContains(fs, name, nameLength));
                }
            }
        }

        public static void HASHTABLE_Analysis(int seed)
        {
            int nameLength = 10;
            int n = 100;
            int initialCapacity = n * 5;
            float loadFactor = (float)0.25;
            List<string> names = new List<string>();

            string filename = @"mydatatable.dat";
            Console.WriteLine("\n HASHTABLE ARRAY \n N Run Time Op M Count Op D Count\n");
            for (int i = 0; i < 7; i++)
            {
                Stopwatch myTimer = new Stopwatch();

                MyDataHashTable<string, string> myHashTable = new MyDataHashTable<string, string>(initialCapacity, loadFactor);
                for (int q = 0; q < n * i; q++)
                {
                    string generatedName = RandomName(nameLength, seed);
                    names.Add(generatedName);
                    myHashTable.Put(generatedName, generatedName);
                }
                names.Add(RandomName(10, seed));

                myTimer.Start();
                foreach (var name in names)
                {
                    myHashTable.Contains(name);
                }
                myTimer.Stop();

                Console.WriteLine(" {0,6:N0} {1} {2,15:N0} {3,15:N0}", n,
               myTimer.Elapsed, opMcout, opDcout);
                n = n * 2;
                GC.Collect();
            }
            n = 100;
            Console.WriteLine("\n HASHTABLE FILE ARRAY \n N Run Time Op M Count Op D Count\n");
            for (int i = 0; i < 7; i++)
            {
                Stopwatch myTimer = new Stopwatch();

                MyDataHashTable<string, string> myHashTable = new MyDataHashTable<string, string>(initialCapacity, loadFactor);
                for (int q = 0; q < n * i; q++)
                {
                    string generatedName = RandomName(nameLength, seed);
                    names.Add(generatedName);
                    myHashTable.Put(generatedName, generatedName);
                }
                names.Add(RandomName(10, seed));

                myHashTable.WriteToFile(filename, nameLength);
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                {
                    myTimer.Start();
                    foreach (var name in names)
                    {
                        myHashTable.Contains(name);
                    }
                    myTimer.Stop();
                }
                Console.WriteLine(" {0,6:N0} {1} {2,15:N0} {3,15:N0}", n, myTimer.Elapsed, opMcout, opDcout);
                n = n * 2;
                GC.Collect();
            }


        }
    }

    abstract class DataArray
    {
        protected int length;
        protected int biggestVal;
        public int Length { get { return length; } }
        public int BiggestVal { get { return biggestVal; } }
        public abstract int this[int index] { get; }
        public abstract void Swap(int j, int a, int b);
        public abstract void Replace(int index, int a);
        public void Print(int n)
        {
            Console.WriteLine("---------------------------------");
            for (int i = 0; i < n; i++)
                Console.Write(" {0} ", this[i]);
            Console.WriteLine();
        }
    }

    abstract class DataList
    {
        protected int length;
        protected int biggestVal;
        public int Length { get { return length; } }
        public int BiggestVal { get { return biggestVal; } }
        public abstract void GoHead();
        public abstract void GoHeadSkipOne();
        public abstract void Next();
        public abstract int CurrentVal();
        public abstract int PreviousVal();
        public abstract void Swap(int a, int b);
        public abstract void Replace(int a);
        public void Print(int n)
        {
            GoHead();
            Console.WriteLine("---------------------------------");
            for (int i = 0; i < n; i++)
            {
                Console.Write(" {0} ", CurrentVal());
                Next();
            }
            Console.WriteLine();
        }
    }

}
