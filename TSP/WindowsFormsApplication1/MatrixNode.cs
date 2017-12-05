using System;
using System.Collections.Generic;
using System.Text;

namespace TSP
{
    /// <summary>
    /// This class holds a matrix, the reduction of the node, a list of children,
    /// </summary>
    class MatrixNode
    {
        private int n;
        private double[,] matrix;
        private double reduction;

        public double Reduction
        {
            get { return reduction; }
        }

        private List<int> path;
        private List<int> unvisited;

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
        // and returning the lowest one
        public List<MatrixNode> Expand(double bound)
        {
            List<MatrixNode> children = new List<MatrixNode>();
            MatrixNode child;
            double temp;
            foreach (int item in unvisited) {
                child = new MatrixNode(this);
                temp = child.Reduce(item);
                if (temp < bound)
                    children.Add(child);
            }
            children.Sort(delegate (MatrixNode x, MatrixNode y)
            {
                if (x.reduction < y.reduction) return -1;
                else if (x.reduction > y.reduction) return 1;
                else return 0;
            });
            return children;
        }

        public bool isCompletePath()
        {
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if (matrix[i,j] != 0 && matrix[i,j] != Double.PositiveInfinity) {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<int> FinishPath()
        {
            if(!isCompletePath()) { // there are non-zeroes, so it doesn't work
                return null;
            }

            foreach (int item in unvisited) {
                path.Add(item);
            }
            unvisited.Clear();
            return path;
        }

    } // end of class

} // end of namespace descriptor
