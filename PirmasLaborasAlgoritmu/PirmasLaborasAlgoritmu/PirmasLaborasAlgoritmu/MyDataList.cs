using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirmasLaborasAlgoritmu
{
    class MyDataList : DataList
    {
        class MyLinkedListNode
        {
            public MyLinkedListNode nextNode { get; set; }
            public MyLinkedListNode prevNode { get; set; }
            public int data { get; set; }
            public MyLinkedListNode(int data)
            {
                this.data = data;
            }
        }

        MyLinkedListNode headNode;
        MyLinkedListNode prevNode;
        MyLinkedListNode currentNode;

        public MyDataList(int n, int seed)
        {
            biggestVal = 0;
            length = n;
            Random rand = new Random(seed);
            headNode = new MyLinkedListNode(rand.Next(100000));
            currentNode = headNode;
            for (int i = 1; i < length; i++)
            {
                prevNode = currentNode;
                int nextInt = rand.Next(100000);
                currentNode.nextNode = new MyLinkedListNode(nextInt);
                currentNode = currentNode.nextNode;
                if (nextInt > biggestVal)
                {
                    biggestVal = nextInt;
                }
            }
            currentNode.nextNode = null;
        }

        public override void GoHead()
        {
            currentNode = headNode;
            prevNode = null;
        }

        public override void GoHeadSkipOne()
        {
            GoHead();
            Next();
        }

        public override void Next()
        {
            prevNode = currentNode;
            currentNode = currentNode.nextNode;
        }

        public override void Swap(int a, int b)
        {
            prevNode.data = a;
            currentNode.data = b;
        }

        public override int CurrentVal()
        {
            return currentNode.data;
        }

        public override int PreviousVal()
        {
            return prevNode.data;
        }

        public override void Replace(int a)
        {
            currentNode.data = a;
        }
    }
}
