using System;
using System.Collections.Generic;
using System.Drawing;

namespace NetworkRouting
{
    public class Dijkstras
    {
        private static int adjCount = 0;

        // O(|v|+|E|) space to hold all of the nodes for the graph,
        // plus 2*|v| for the previous and distances lists required.
        // For the heapQ we have O(2*|v|) and for the ArrayQ we have O(|v|),
        // as explained in their class descriptions.  In the end we also return
        // the path which could worst-case cost O(|v|).
        // Overall we have O(6*|v| + |E|) in the worst-case.
		public static Tuple<List<int>, List<double>> run(
            List<PointF> points, List<HashSet<int>> adjacencyList,
            int startNode, int stopNode, bool useArray)
        {
			List<double> distances = new List<double>();
			List<int> prev = new List<int>();
			for (int i = 0; i < points.Count; i++)
			{
                distances.Add(int.MaxValue);
				prev.Add(-1);
			}
			distances[startNode] = 0;

			IQueue Q;
			if (useArray)
			{
				Q = new ArrayQ(distances);
			}
			else
			{
				Q = new HeapQ(distances);
			}

            int cur;
            double dis;
            HashSet<int> adjs;
            // this is O(|v| * |v|) for the array and O(|v| * 2log(|v|)) for the heap.
            // the O(|v|) is common to both because this while loop runs |v| times.
            // The O(|v|) for the array is due to the Deletemin, and the O(2log(|v|))
            // for the heap is because it has two operations that are O(log(|v|)).
            while((cur = Q.Deletemin(distances)) != -1)
            {
                adjs = adjacencyList[cur];
                foreach(int end in adjs) // runs 3 times
                {
                    dis = distances[cur] + Distance(points[cur], points[end]);
                    if(dis < distances[end]) {
                        distances[end] = dis;
                        prev[end] = cur;
                        Q.Decreasekey(end, distances); // O(1) for ArrayQ and O(log|v|) for HeapQ
                    }
                }
            }

            return new Tuple<List<int>, List<double>>(PathToEnd(startNode, stopNode, prev), distances);

        } // end of run()

        // O(|v|) because the path could potentially have every node in it.  However, this is highly unlikely.
        // Space is O(|v|) for the same reason.
        private static List<int> PathToEnd(int start, int end, List<int> connections)
        {
            List<int> results = new List<int>();
            int cur = end;
            results.Add(cur);
            while(cur != start)
            {
                if(cur == -1)
                {
                    return null;
                }
                cur = connections[cur];
                results.Add(cur);
            }

            return results;
        } // end of PathToEnd()

        private static double Distance(PointF a, PointF b)
        {
            double y = Math.Pow((b.Y - a.Y), 2);
            double x = Math.Pow((b.X - a.X), 2);
            return Math.Sqrt(x + y);
        }

    } // end of class
}
