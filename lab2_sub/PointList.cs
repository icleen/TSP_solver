using System;
using System.Collections.Generic;
using System.Drawing;

namespace _2_convex_hull
{

    class PointComparator : IComparer<PointF>
    {
        public int Compare(PointF one, PointF two)
        {
            if (one.X == two.X)
            {
                return (int) (one.Y - two.Y + 0.5);
            }else
            {
                return (int) (one.X - two.X + 0.5);
            }
        }
    }
    class PointList
    {
        private PointF[] points;
        private int index = 0;

        public int Length {
            get {
                return points.Length;
            }
        }

        public int Index {
            get {
                return index;
            }
            set {
                index = value;
            }
        }

        public PointList(List<PointF> pointList) {
            points = pointList.ToArray();
        }

        public PointList(PointF[] pointList) {
            points = new PointF[pointList.Length];
            Array.Copy(pointList, 0, points, 0, pointList.Length);
        }

        public PointList(PointList pts1, PointList pts2) {
            points = new PointF[pts1.Length + pts2.Length];
            for(int i = 0; i < points.Length; i++)
            {
                if(i < pts1.Length)
                {
                    points[i] = pts1.points[i];
                }else
                {
                    points[i] = pts2.points[i - pts1.Length];
                }
            }
        }

        // O(n) where n is the number of nodes in the list
        public PointF Rightmost() {
            PointF highest = points[0];
            index = 0;
            for (int i = 1; i < points.Length; i++) {
                if (points[i].X > highest.X) {
                    highest = points[i];
                    index = i;
                }
            }
            return highest;
        }

        // O(n) where n is the number of nodes in the list
        public PointF Leftmost() {
            PointF lowest = points[0];
            index = 0;
            for (int i = 1; i < points.Length; i++) {
                if (points[i].X < lowest.X) {
                    lowest = points[i];
                    index = i;
                }
            }
            return lowest;
        }

        // O(1)
        public PointF next() {
            index = (index + 1) % points.Length;
            return points[index];
        }

        // O(1)
        public PointF prev() {
            index = (index - 1) % points.Length;
            if(index < 0)
            {
                index = points.Length + index;
            }
            return points[index];
        }

        public void SetIndex(PointF pt) {
            for(int i = 0; i < points.Length; i++) {
                if(points[i] == pt) {
                    index = i;
                    return;
                }
            }
        }

        public PointF[] ToArray() {
            return points;
        }

        public String ToString()
        {
            String output = "";
            foreach(var pt in this.points)
            {
                output += pt;
            }
            return output;
        }

    }
}
