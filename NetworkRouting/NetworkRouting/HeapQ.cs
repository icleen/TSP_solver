using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    // The space complexity of this class is just O(2*|v|) to
    // hold the q and the pointer to the q (p) which are each the length of all nodes
    class HeapQ : IQueue
    {
        private int size;
        private List<int> q;
        private List<int> p;

        // O(|v| * log(|v|)) because you have to run through each node and each Insert
        // costs potentially O(log(|v|)).  Space taken up is O(2*|v|) as explained above
        public HeapQ(List<double> distances)
        {
            size = distances.Count;
            q = new List<int>();
            p = new List<int>();
            for (int i = 0; i < size; i++) {
                Insert(i, distances);
            }
        }

        // O(log(|v|)) because in the worst case you have to repeat the switch
        // operation for each level in the tree and there are log(|v|) levels
        // in the tree, where |v| is the number of nodes.  However, in some cases
        // this will be O(1) because the changed node won't be lower than it's parent
        // and therefore will not repeat.
        public void Decreasekey(int key, List<double> distances)
        {
            int cur = p[key];
            int par = Parent(cur);
            int tmp;
            // runs as long as the key node (the node that changed)
            // is lower than it's parent
            while (distances[q[cur]] < distances[q[par]])
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

        // O(log(|v|)) because you have to repeat the switch operation for
        // each level in the tree and there are log(|v|) levels in the tree,
        // where |v| is the number of nodes
        public int Deletemin(List<double> distances)
        {
            int min = q[0];
            if(min == -1)
            {
                return min;
            }
            int par = 0;
            p[q[par]] = -1;
            int c1 = Child1(par);
            int c2 = Child2(par);
            // runs as many times as there are children,
            // ie. for the number of levels in the tree
            while( c1 < size )
            {
                if(c2 < size)
                {
                    if (q[c1] == -1 || q[c2] == -1)
                    {
                        if (q[c1] == -1 && q[c2] == -1)
                        {
                            break;
                        }
                        else if (q[c1] == -1)
                        {
                            q[par] = q[c2];
                            p[q[par]] = par;
                            par = c2;
                        }
                        else
                        {
                            q[par] = q[c1];
                            p[q[par]] = par;
                            par = c1;
                        }
                    }
                    else if (distances[q[c1]] < distances[q[c2]])
                    {
                        q[par] = q[c1];
                        p[q[par]] = par;
                        par = c1;
                    }
                    else
                    {
                        q[par] = q[c2];
                        p[q[par]] = par;
                        par = c2;
                    }
                }
                else
                {
                    if (q[c1] != -1)
                    {
                        q[par] = q[c1];
                        p[q[par]] = par;
                        par = c1;
                    }
                    break;
                }
                c1 = Child1(par);
                c2 = Child2(par);
            }
            q[par] = -1;
            return min;
        }

        // O(log(|v|)) because you might need to bubble to the top,
        // which is worst-case the Height of the tree, which is log of the number of total nodes in the tree
        public void Insert(int node, List<double> distances)
        {
            q.Add(node);
            int cur = q.Count - 1;
            p.Add(cur);
            int par = Parent(cur);

            int tmp;
            // runs as many times as the one above is higher than the one below
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

        override public string ToString()
        {
            StringBuilder ss = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                ss.Append(q[i]);
                ss.Append(" ");
            }
            ss.Append("\n");
            for (int i = 0; i < size; i++)
            {
                ss.Append(p[i]);
                ss.Append(" ");
            }
            ss.Append("\n");

            return ss.ToString();
        }

    } // end of class
}
