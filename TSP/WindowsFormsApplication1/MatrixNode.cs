using System;
using System.Collections.Generic;
using System.Text;

namespace TSP
{
    /// <summary>
    /// This class holds a matrix, the reduction of the node, a list of children,
    /// a current path and the list of unvisited cities
    /// Overall it requires O(n^2 + 2n) amount of space where n is the number of cities
    ///
    /// </summary>
    class MatrixNode
    {
        private int n;
        private double[,] matrix;
        private double reduction;
        private bool hasZero;

        public double Reduction
        {
            get { return reduction; }
        }

        private List<int> path;
        private List<int> unvisited;

        public static int total_count = 1;
        public static int pruned = 0;

        public MatrixNode(City[] cities)
        {
            n = cities.Length;
            reduction = 0;
            unvisited = new List<int>();
            path = new List<int>();
            path.Add(0);
            matrix = new double[n,n];
            for (int i = 0; i < n; i++) {
                if (i > 0) {
                    unvisited.Add(i);
                }
                for (int j = 0; j < n; j++) {
                    if (i != j) {
                        matrix[i,j] = cities[i].costToGetTo(cities[j]);
                    }else {
                        matrix[i,j] = Double.PositiveInfinity;
                    }
                }
            }
        }
        // The creation from a previous node is O(n^2) for the matrix copy
        public MatrixNode(MatrixNode parent)
        {
            n = parent.n;
            reduction = parent.reduction;
            matrix = new double[n,n];
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    matrix[i,j] = parent.matrix[i,j];
                }
            }
            path = new List<int>();
            foreach (int item in parent.path) {
                path.Add(item);
            }
            unvisited = new List<int>();
            foreach (int item in parent.unvisited) {
                unvisited.Add(item);
            }
        }
        // O(2n^2) time complexity to reduce the matrix
        public double Reduce()
        {
            double lowest;
            int low_row, low_col, row, col;

            for (row = 0; row < n; row++) { // make a zero for each row
                low_col = 0;
                lowest = Double.PositiveInfinity;
                for (col = 0; col < n; col++) {
                    if (matrix[row,col] < lowest) {
                        lowest = matrix[row,col];
                        low_col = col;
                    }
                }
                if (lowest != 0 && lowest != Double.PositiveInfinity) {
                    reduction += lowest;
                    for (col = 0; col < n; col++) {
                        matrix[row,col] -= lowest;
                    }
                    matrix[row,low_col] = 0;
                }
            }

            for (col = 0; col < n; col++) { // make a zero for each column
                low_row = 0;
                lowest = Double.PositiveInfinity;
                for (row = 0; row < n; row++) {
                    if (matrix[row,col] < lowest) {
                        lowest = matrix[row,col];
                        low_row = row;
                    }
                }
                if (lowest != 0 && lowest != Double.PositiveInfinity) {
                    reduction += lowest;
                    for (row = 0; row < n; row++) {
                        matrix[row,col] -= lowest;
                    }
                    matrix[low_row,col] = 0;
                }
            }
            return reduction;
        } // end of Reduce
        // This reduce is O(n + n^2) since we have to run through the source
        // city's row and the destination city's column which is O(n) and we
        // call the other reduction function which is O(n^2).  We can drop
        // the O(n) though, so it's just O(n^2)
        public double Reduce(int destination)
        {
            int prev = path[path.Count - 1];
            reduction += matrix[prev,destination];
            for (int i = 0; i < n; i++) {
                matrix[prev,i] = Double.PositiveInfinity;
                matrix[i,destination] = Double.PositiveInfinity;
            }
            matrix[destination,prev] = Double.PositiveInfinity;
            Reduce();
            unvisited.Remove(destination);
            path.Add(destination);
            return reduction;
        }

        // Expands the node by building all of it's children
        // and returning the list
        // This is O(n^3) for the reduction of possibly n children
        // since the reduction costs O(n^2)
        // The sort is probably O(nlogn) although I don't know what algorithm
        // the default c# function uses.  This is dropped in the final big-O.
        public List<MatrixNode> Expand(double bound)
        {
            List<MatrixNode> children = new List<MatrixNode>();
            MatrixNode child;
            double temp;
            foreach (int item in unvisited) {
                child = new MatrixNode(this);
                temp = child.Reduce(item);
                if (temp < bound) {
                    children.Add(child);
                }else {
                    pruned++;
                }
                total_count++;
            }
            children.Sort(delegate (MatrixNode x, MatrixNode y)
            {
                if (x.reduction < y.reduction) return -1;
                else if (x.reduction > y.reduction) return 1;
                else return 0;
            });
            return children;
        }
        // O(n^2) time complexity to check for non-zeroes
        public bool isCompletePath()
        {
            hasZero = false;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if (matrix[i, j] == 0) {
                        hasZero = true;
                    }
                    else if (matrix[i, j] != Double.PositiveInfinity) {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<int> FinishPath()
        {
            if(!hasZero) { // there are no zeroes, the current path is finished
                return path;
            }

            foreach (int item in unvisited) {
                path.Add(item);
            }
            unvisited.Clear();
            return path;
        }

    } // end of class

} // end of namespace descriptor
