using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    class ArrayQ : IQueue
    {

        private int size;
        private List<int> q;

        public ArrayQ(List<double> distances)
        {
            size = distances.Count;
            q = new List<int>();
            for(int i = 0; i < size; i++) {
                q.Add(i);
            }
        }

        public void Decreasekey(int key, List<double> distances)
        {
            return;
        }

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

        public void Insert(int node, List<double> distances)
        {
            q.Add(node);
        }

    } // end of class
}
