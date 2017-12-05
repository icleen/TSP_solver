using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    class HeapQ : IQueue
    {
        private int size;
        private List<int> q;
        private List<int> p;

        public HeapQ(List<double> distances)
        {
            size = distances.Count;
            q = new List<int>();
            p = new List<int>();
            for (int i = 0; i < size; i++) {
                Insert(i, distances);
            }
        }

        public void Decreasekey(int key, List<double> distances)
        {
            int cur = p[key];
            int par = Parent(cur);
            int tmp;
            while(distances[q[cur]] < distances[q[par]])
            {
                tmp = q[par];
                q[par] = q[cur];
                q[cur] = tmp;
                p[q[par]] = par;
                p[q[cur]] = cur;
                cur = par;
                par = Parent(cur);
            }
        }

        public int Deletemin(List<double> distances)
        {
            int min = q[0];
            int par = 0;
            p[q[par]] = -1;
            int c1 = Child1(par);
            int c2 = Child2(par);

            while( c2 < size )
            {
                if(q[c1] == -1 && q[c2] == -1) {
                    break;
                }
                if(q[c1] == -1) {
                    q[par] = q[c2];
                    p[q[par]] = par;
                    par = c2;
                }else if(q[c2] == -1 || distances[q[c1]] < distances[q[c2]]) {
                    q[par] = q[c1];
                    p[q[par]] = par;
                    par = c1;
                }else {
                    q[par] = q[c2];
                    p[q[par]] = par;
                    par = c2;
                }
                c1 = Child1(par);
                c2 = Child2(par);
            }
            q[par] = -1;
            return min;
        }

        public void Insert(int node, List<double> distances)
        {
            q.Add(node);
            int cur = q.Count - 1;
            p.Add(cur);
            int par = Parent(cur);

            int tmp;
            while(distances[q[cur]] < distances[q[par]])
            {
                tmp = q[par];
                q[par] = q[cur];
                q[cur] = tmp;
                p[q[cur]] = cur;
                p[q[par]] = par;
                cur = par;
                par = Parent(cur);
            }
        }


		private int Parent(int child)
		{
			if (child == 0)
			{
				return child;
			}
			int ret = (child + 1) / 2;
			return ret - 1;
		}

		private int Child1(int parent)
		{
			return (parent + 1) * 2 - 1;
		}

		private int Child2(int parent)
		{
			return (parent + 1) * 2;
		}

	} // end of class
}
