using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    // The space complexity of this class is just O(|v|) to hold the q which is the length of all nodes
    class ArrayQ : IQueue
    {

        private int size;
        private List<int> q;

        // makeQ is O(|v|) because you have to add each node.
        // Space taken up is O(|v|) as explained above
        public ArrayQ(List<double> distances)
        {
            size = distances.Count;
            q = new List<int>();
            for(int i = 0; i < size; i++) {
                q.Add(i);
            }
        }

        // O(1) because it doesn't do anything in the Array implementation of the queue
        public void Decreasekey(int key, List<double> distances)
        {
            return;
        }

        // O(|v|) because it needs to go through every spot in the array to find the lowest
        public int Deletemin(List<double> distances)
        {
            int lowest = 0;
            for(int i = 1; i < size; i++) {
                if(q[i] != -1) {
                    if(q[lowest] == -1 || distances[i] < distances[lowest]) {
                        lowest = i;
                    }
                }
            }
            int tmp = q[lowest];
            q[lowest] = -1;
            return tmp;
        }

        // O(1) because it takes constant time to add something to an array
        public void Insert(int node, List<double> distances)
        {
            q.Add(node);
        }

    } // end of class
}
