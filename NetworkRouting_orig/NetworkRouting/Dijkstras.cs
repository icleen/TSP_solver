using System;
using System.Collections.Generic;
using System.Drawing;

namespace NetworkRouting
{
    public class Dijkstras
    {

		public static List<int> run(
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
            while((cur = Q.Deletemin(distances)) != -1)
            {
                adjs = adjacencyList[cur];
                foreach(int end in adjs)
                {
                    dis = distances[cur] + Distance(points[cur], points[end]);
                    if(dis < distances[end]) {
                        distances[end] = dis;
                        prev[end] = cur;
                        Q.Decreasekey(end, distances);
                    }
                }
            }

            return PathToEnd(startNode, stopNode, prev);

        } // end of run()

        private static List<int> PathToEnd(int start, int end, List<int> connections)
        {
            List<int> results = new List<int>();
            int cur = end;
            results.Add(cur);
            while(cur != start)
            {
                cur = connections[cur];
                results.Add(cur);
            }

            return results;
        }

        private static double Distance(PointF a, PointF b)
        {
            double y = Math.Pow((b.Y - a.Y), 2);
            double x = Math.Pow((b.X - a.X), 2);
            return Math.Sqrt(x + y);
        }

    } // end of class
}
