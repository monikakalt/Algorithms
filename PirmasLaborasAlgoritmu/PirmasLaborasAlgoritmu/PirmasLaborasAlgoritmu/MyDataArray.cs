using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirmasLaborasAlgoritmu
{
    class MyDataArray : DataArray
    {
        int[] data;

        public MyDataArray(int n, int seed)
        {
            biggestVal = 0;
            data = new int[n];
            length = n;
            Random rand = new Random(seed);
            for(int i = 0; i < length; i++)
            {
                int nextInt = rand.Next(100000);
                data[i] = nextInt;
                if(nextInt > biggestVal)
                {
                    biggestVal = nextInt;
                }

            }
        }

        public int Length { get { return length; } }

        public override int this[int index]
        {
            get { return data[index]; }

        }

        public override void Swap(int j, int a, int b)
        {
            data[j - 1] = a;
            data[j] = b;
        }

        public override void Replace(int index, int a)
        {
            data[index] = a;
        }
    }
}
