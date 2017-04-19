using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirmasLaborasAlgoritmu
{
    class MyDataHashTable<K, V> 
    {

        protected Node<K, V>[] table;
        protected int size = 0;
        protected float loadFactor;

        protected int index = 0;
        protected int rehashesCounter = 0;

        public MyDataHashTable(int initialCapacity, float loadFactor)
        {
            this.table = new Node<K,V>[initialCapacity];
            this.loadFactor = loadFactor;
        }

        public void Clear()
        {
            Array.Clear(table, 0, table.Length);
            size = 0;
            index = 0;
            rehashesCounter = 0;
        }

        public V Put (K key, V value)
        {

            if (size >= table.Length * loadFactor)
            {
                Rehash();
            }
            index = Hash(key);

            for (int i = 0; i < table.Length; i++)
            {
                int pointer = (index + i) % (table.Length);
                if (table[pointer] == null)
                {
                    table[pointer] = new Node<K, V>(key, value);
                    size++;
                    break;
                }
            }

            return value;
        }

        public void Rehash()
        {
            MyDataHashTable<K, V> newTable = 
                new MyDataHashTable<K, V>(this.table.Length * 2, loadFactor);
            for(int i = 1; i < this.table.Length; i++)
            {
                if (this.table[i] != null)
                {
                    newTable.Put(this.table[i].key, this.table[i].value);
                }
            }
            table = newTable.table;
            rehashesCounter++;
        }

        private int Hash(K key)
        {
            int h = key.GetHashCode();
            return Math.Abs(h) % table.Length;
        }

        public V Get (K key)
        {
            index = Hash(key);
            for(int i = 0; i < table.Length; i++)
            {
                int pointer = (index + i) % (table.Length);
                if (table[pointer] == null)
                {
                    return default(V);
                } else if(table[pointer].key.Equals(key))
                {
                    Node<K, V> res = table[index];
                    return res.value;
                }
            }
            return default(V);
        }

        public V Remove(K key)
        {

            index = Hash(key);
            for (int i = 0; i < table.Length; i++)
            {
                int pointer = (index + i * i) % (table.Length);
                if (table[pointer] != null && table[pointer].key.Equals(key))
                {
                    table[pointer] = null;
                    size--;
                    break;
                }
            }

            return default(V);
        }

        public bool Contains(K key)
        {
            return Get(key) != null;
        }

        public bool ContaisValue(V value)
        {
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i] != null && table[i].value.Equals(value));
                {
                    return true;
                }
            }
            return false;
        }

        public void WriteToFile(string fileName, int length)
        {
            byte[][] buffer = new byte[table.Length][];

            for (int i = 0; i < table.Length; i++)
            {
                if(table[i] != null)
                {
                    byte[] toBytes = Encoding.ASCII.GetBytes(table[i].key.ToString());
                    buffer[i] = toBytes;
                } else
                {
                    Random rand = new Random();
                    byte[] junk = new byte[length];
                    rand.NextBytes(junk);
                    buffer[i] = junk;
                }

            }
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
                {
                    foreach (var item in buffer)
                    {
                        for (int j = 0; j < item.Length; j++)
                            writer.Write(item[j]);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public bool FileContains(FileStream fs, K x, int wordLength)
        {
            int hash = Hash(x);

            for (int i = 0; i < fs.Length / wordLength; i++)
            {
                int pointer = (hash + i) % (int)(fs.Length / wordLength);
                byte[] buffer = new byte[wordLength];
                fs.Seek(wordLength * pointer, SeekOrigin.Begin);
                fs.Read(buffer, 0, wordLength);
                string element = Encoding.ASCII.GetString(buffer);

                if (element.Equals(x))
                {
                    return true;
                }
            }   
            return false;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (Node<K, V> node in table)
            {
                if (node != null)
                {
                    result.Append(node.ToString()).Append("\n");
                }
            }
            return result.ToString();
        }


        protected class Node<K, V>
        {
            public K key { get; set; }
            public V value { get; set; }

            public Node() { }

            public Node(K key, V value)
            {
                this.key = key;
                this.value = value;
            }

            public override string ToString()
            {
                return key + "=" + value;
            }
        }
    }
}
