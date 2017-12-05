using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {
        System.Drawing.Graphics g;
        System.Windows.Forms.PictureBox pictureBoxView;

        public ConvexHullSolver(System.Drawing.Graphics g, System.Windows.Forms.PictureBox pictureBoxView)
        {
            this.g = g;
            this.pictureBoxView = pictureBoxView;
        }

        public void Refresh()
        {
            // Use this especially for debugging and whenever you want to see what you have drawn so far
            pictureBoxView.Refresh();
        }

        public void Pause(int milliseconds)
        {
            // Use this especially for debugging and to animate your algorithm slowly
            pictureBoxView.Refresh();
            System.Threading.Thread.Sleep(milliseconds);
        }

        public void Solve(List<System.Drawing.PointF> pointList)
        {
            /*
            pointList.Sort();
            PointList pts = Divide(pointList.ToArray());
            pointList = new List<>(pts.ToArray());
            */
        }

        public PointList Divide(PointF[] pts) {
            int n = pts.Length;
            int n2 = n / 2;
            if (n == 1) {
                return pts;
            }
            PointF[] left = new PointF[n2];
            PointF[] right = new PointF[n - n2];
            // Array.Copy(old_array, start_index, new_array, 0, array_length)
            Array.Copy(pts, 0, left, 0, n2);
            Array.Copy(pts, n2, right, 0, n - n2);
            // for (int i = 0; i < n; i++) {
            //     if (i < n2) {
            //         lhs[i] = pts[i];
            //     }else {
            //         rhs[i - n2] = pts[i];
            //     }
            // }
            PointList lhs = Divide(new PointList(left));
            PointList rhs = Divide(new PointList(right));
            return Merge(lhs, rhs);
        }

        public PointList Merge(PointList lhs, PointList rhs) {
            // the base case
            if (lhs.Length == 1 || rhs.Length == 1) {
                return new PointList(lhs, rhs);
            }
            // finding the top connecting nodes
            PointF lpt_top = lhs.Rightmost(), rpt_top = rhs.Leftmost();
            PointF current;
            bool changed = true;
            bool growing = true;
            while(changed) {
                changed = false;

                growing = true;
                while(growing) {
                    current = lhs.prev();
                    if (Slope(current, rpt_top) > Slope(lpt_top, rpt_top)) {
                        lpt_top = current;
                        changed = true;
                    }else {
                        growing = false;
                        lhs.next();
                    }
                }

                growing = true;
                while(growing) {
                    current = rhs.next();
                    if (Slope(lpt_top, current) < Slope(lpt_top, rpt_top)) {
                        rpt_top = current;
                        changed = true;
                    }else {
                        growing = false;
                        rhs.prev();
                    }
                }
            }

            // finding the bottom connecting nodes
            PointF lpt_bot = lhs.Rightmost(), rpt_bot = rhs.Leftmost();
            bool changed = true;
            bool growing = true;
            while(changed) {
                changed = false;

                growing = true;
                while(growing) {
                    current = lhs.next();
                    if (Slope(current, rpt_bot) < Slope(lpt_bot, rpt_bot)) {
                        lpt_bot = current;
                        changed = true;
                    }else {
                        growing = false;
                        lhs.prev();
                    }
                }

                growing = true;
                while(growing) {
                    current = rhs.prev();
                    if (Slope(lpt_bot, current) > Slope(lpt_bot, rpt_bot)) {
                        rpt_bot = current;
                        changed = true;
                    }else {
                        growing = false;
                        rhs.next();
                    }
                }
            }

            // forming the new list with only the convex hull nodes
            List<PointF> list = new List<>();
            growing = true;
            current = lpt_bot;
            lhs.SetIndex(lpt_bot);
            while(growing) {
                if (current == lpt_top) {
                    growing = false;
                }
                list.Add(current);
                current = lhs.next();
            }

            growing = true;
            current = rpt_top;
            rhs.SetIndex(rpt_top);
            while(growing) {
                if (current == rpt_bot) {
                    growing = false;
                }
                list.Add(current);
                current = rhs.next();
            }

            return new PointList(list);
        }

        public double Slope(PointF pt1, PointF pt2) {
            double x = pt1.X - pt2.X, y = pt1.Y - pt2.Y;
            return y/x;
        }

    }
}
