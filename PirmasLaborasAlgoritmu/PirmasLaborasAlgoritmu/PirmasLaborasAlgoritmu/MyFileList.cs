using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirmasLaborasAlgoritmu
{
    class MyFileList : DataList
    {
        int prevNode;
        int currentNode;
        int nextNode;
        int currentVal;
        int prevVal;

        public MyFileList(string filename, int n, int seed)
        {
            biggestVal = 0;
            length = n;
            Random rand = new Random(seed);
            if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename,
               FileMode.Create)))
                {
                    writer.Write(4);
                    for (int j = 0; j < length; j++)
                    {
                        int randomInt = rand.Next(100000);
                        if(randomInt > biggestVal)
                        {
                            biggestVal = randomInt;
                        }
                        writer.Write(randomInt);
                        writer.Write((j + 1) * 8 + 4);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public FileStream fs { get; set; }

        public override void GoHead()
        {
            Byte[] data = new Byte[12];
            fs.Seek(0, SeekOrigin.Begin);
            fs.Read(data, 0, 4);
            currentNode = BitConverter.ToInt32(data, 0);
            prevNode = -1;
            fs.Seek(currentNode, SeekOrigin.Begin);
            fs.Read(data, 0, 12);
            currentVal = BitConverter.ToInt32(data, 0);
            prevVal = -1;
            nextNode = BitConverter.ToInt32(data, 4);
        }

        public override void Next()
        {
            Byte[] data = new Byte[12];
            fs.Seek(nextNode, SeekOrigin.Begin); fs.Read(data, 0, 8);
            prevNode = currentNode;
            currentNode = nextNode;
            prevVal = currentVal;
            currentVal = BitConverter.ToInt32(data, 0);
            nextNode = BitConverter.ToInt32(data, 4);
        }

        public override void Swap(int a, int b)
        {
            Byte[] data;
            fs.Seek(prevNode, SeekOrigin.Begin);
            data = BitConverter.GetBytes(a);
            fs.Write(data, 0, 4);
            fs.Seek(currentNode, SeekOrigin.Begin);
            data = BitConverter.GetBytes(b);
            fs.Write(data, 0, 4);
        }

        public override int CurrentVal()
        {
            return currentVal;
        }

        public override void GoHeadSkipOne()
        {
            GoHead();
            Next();
        }

        public override int PreviousVal()
        {
            return prevVal;
        }

        public override void Replace(int a)
        {
            Byte[] data = BitConverter.GetBytes(a);
            fs.Seek(currentNode, SeekOrigin.Begin);
            fs.Write(data, 0, 4);
        }
    }
}
