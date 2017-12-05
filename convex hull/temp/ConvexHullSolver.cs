using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {
        System.Drawing.Graphics g;
        System.Windows.Forms.PictureBox pictureBoxView;
        PointF[] points;
        int pauseTime = 100;

        public ConvexHullSolver(System.Drawing.Graphics g, System.Windows.Forms.PictureBox pictureBoxView)
        {
            this.g = g;
            this.pictureBoxView = pictureBoxView;
        }

        public void Refresh()
        {
            // Use this especially for debugging and whenever you want to see what you have drawn so far
            pictureBoxView.Refresh();
            if(points != null)
            {
                Pen pen = new Pen(Color.Blue, 1);
                for(int i = 0; i < points.Length; i++)
                {
                    if(i > 0)
                    {
                        this.g.DrawLine(pen, points[i - 1], points[i]);
                    }
                }
                this.g.DrawLine(pen, points[points.Length - 1], points[0]);
            }

        }

        public void Pause(int milliseconds)
        {
            // Use this especially for debugging and to animate your algorithm slowly
            pictureBoxView.Refresh();
            Refresh();
            System.Threading.Thread.Sleep(milliseconds);
        }

        public void Solve(List<System.Drawing.PointF> pointList)
        {
            pointList = pointList.OrderBy(p => p.X).ToList(); // O(nlogn) if the algorithm is well implemented, O(n^2) otherwise
            PointList pts = Divide(pointList.ToArray());
            this.points = pts.ToArray();
            Refresh();
        }

        public PointList Divide(PointF[] pts)
        {
            int n = pts.Length;
            int n2 = n / 2;
            if (n == 1)
            {
                return new PointList(pts);
            }
            // O(n) to create 2 sub arrays of size n/2
            PointF[] left = new PointF[n2];
            PointF[] right = new PointF[n - n2];
            for (int i = 0; i < n; i++) {
                if (i < n2) {
                    left[i] = pts[i];
                }else {
                    right[i - n2] = pts[i];
                }
            }
            PointList lhs = Divide(left);
            PointList rhs = Divide(right);
            return Merge(lhs, rhs);
            // Total Time:
            // T(n) = 2T(n/2) + O(n^2) -> a = 2, b = 2, d = 1; 1 = 1 = log2 2 -> d = logb a
            // -> O(nlogn)
            // Total Space:
            // O(2n) for each layer down, O(n) for the allocation of the 2 arrays
            // that divide the original and O(n) for the returned list.
        }

        public PointList Merge(PointList lhs, PointList rhs)
        {
            // the base case
            PointList p = null;
            if (lhs.Length == 1 && rhs.Length == 1)
            {
                p = new PointList(lhs, rhs);
                return p;
            }
            // finding the top connecting nodes
            /*
            O(n) because you will visit each node a maximum of one time within the while loop
            (with the possible exception of the first node checked, which may get visited twice, at most)
            where n is the number of nodes given before they are split in the Divide function
            */
            // Finding the rightmost of the lhs and the leftmost of the rhs are a combined O(n)
            PointF lpt_top = lhs.Rightmost(), rpt_top = rhs.Leftmost();
            PointF current;
            bool changed = true;
            bool growing = true;
            while (changed)
            {
                changed = false;

                growing = true;
                while (growing)
                {
                    current = lhs.prev();
                    if (Slope(current, rpt_top) > Slope(lpt_top, rpt_top))
                    {
                        lpt_top = current;
                        changed = true;
                    }
                    else
                    {
                        growing = false;
                        lhs.next();
                    }
                }

                growing = true;
                while (growing)
                {
                    current = rhs.next();
                    if (Slope(lpt_top, current) < Slope(lpt_top, rpt_top))
                    {
                        rpt_top = current;
                        changed = true;
                    }
                    else
                    {
                        growing = false;
                        rhs.prev();
                    }
                }
            }
            // finding the bottom connecting nodes
            /*
            O(n) because you will visit each node a maximum of one time within the while loop
            (with the possible exception of the first node checked, which may get visited twice, at most)
            where n is the number of nodes given before they are split in the Divide function
            */
            // Finding the rightmost of the lhs and the leftmost of the rhs are a combined O(n)
            PointF lpt_bot = lhs.Rightmost(), rpt_bot = rhs.Leftmost();
            changed = true;
            growing = true;
            while (changed)
            {
                changed = false;

                growing = true;
                while (growing)
                {
                    current = lhs.next();
                    if (Slope(current, rpt_bot) < Slope(lpt_bot, rpt_bot))
                    {
                        lpt_bot = current;
                        changed = true;
                    }
                    else
                    {
                        growing = false;
                        lhs.prev();
                    }
                }

                growing = true;
                while (growing)
                {
                    current = rhs.prev();
                    if (Slope(lpt_bot, current) > Slope(lpt_bot, rpt_bot))
                    {
                        rpt_bot = current;
                        changed = true;
                    }
                    else
                    {
                        growing = false;
                        rhs.next();
                    }
                }
            }

            // forming the new list with only the convex hull nodes
            /*
            O(n) because you might have to add all n nodes from the lhs and rhs
            bounding lists to the new bounding list.
            Usually this will only be a fraction of the actual number of nodes given to Divide()
            */
            List<PointF> list = new List<PointF>();
            growing = true;
            current = lpt_bot;
            lhs.SetIndex(lpt_bot);
            while (growing)
            {
                if (current == lpt_top)
                {
                    growing = false;
                }
                list.Add(current);
                current = lhs.next();
            }

            growing = true;
            current = rpt_top;
            rhs.SetIndex(rpt_top);
            while (growing)
            {
                if (current == rpt_bot)
                {
                    growing = false;
                }
                list.Add(current);
                current = rhs.next();
            }
            p = new PointList(list);
            return p;
            // Total Time: O(5n) = O(n)
            // Total Space: O(n) for the new list allocated at the end
        }

        // O(n^2) for division of y / x where n is the number of bits in y and x;
        // In relation to the big-O of the total algorithm,
        // this is a fraction of the time and so can probably be ignored
        public double Slope(PointF pt1, PointF pt2)
        {
            double x = pt1.X - pt2.X, y = pt1.Y - pt2.Y;
            return y / x;
        }
    }
}
