using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirmasLaborasAlgoritmu
{
    class MyFileArray : DataArray
    {

        public MyFileArray(string filename, int n, int seed)
        {
            int[] data = new int[n];
            length = n;
            biggestVal = 0;
            Random rand = new Random(seed);
            for (int i = 0; i < length; i++)
            {
                int nextInt = rand.Next(100000);
                data[i] = nextInt;
                if (nextInt > biggestVal)
                {
                    biggestVal = nextInt;
                }
            }
            if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    for (int j = 0; j < length; j++)
                        writer.Write(data[j]);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public FileStream fs { get; set; }

        public override int this[int index]
        {
            get
            {
                Byte[] data = new Byte[4];
                fs.Seek(4 * index, SeekOrigin.Begin);
                fs.Read(data, 0, 4);
                int result = BitConverter.ToInt32(data, 0);
                return result;
            }
        }

        public override void Swap(int j, int a, int b)
        {
            Byte[] data = new Byte[8];
            BitConverter.GetBytes(a).CopyTo(data, 0);
            BitConverter.GetBytes(b).CopyTo(data, 4);
            fs.Seek(4 * (j - 1), SeekOrigin.Begin);
            fs.Write(data, 0, 8);
        }

        public override void Replace(int index, int a)
        {
            Byte[] data = new Byte[4];
            BitConverter.GetBytes(a).CopyTo(data, 0);
            fs.Seek(4 * index , SeekOrigin.Begin);
            fs.Write(data, 0, 4);
        }
    }
}
