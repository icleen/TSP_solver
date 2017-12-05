using System;
using System.Collections.Generic;
using System.Text;

namespace TSP
{
    /// <summary>
    /// This class holds a matrix, the reduction of the node, a list of children,
    /// </summary>
    class TSP_Solver
    {

        public City[] Solve(City[] cities)
        {
            MatrixNode root = new MatrixNode(cities);
            double lowB = root.reduce();
            MatrixNode head = root;
            while (!head.isComplete()) // while there are still unseen branches
            {
                head = head.Expand();
            }
            List<int> temp = head.FinishPath();
            City[] path = new City[cities.Count];
            int i = 0;
            for (int item in temp) {
                path[i++] = cities[item];
            }
            return path;
        }

    } // end of class

} // end of namespace descriptor
