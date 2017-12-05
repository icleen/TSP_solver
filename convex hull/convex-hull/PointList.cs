using System.Drawing.PointF;

namespace _2_convex_hull
{
    class PointList
    {
        private PointF[] points;
        private static int index = 0;

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
            Array.Copy(pts1.points, 0, points, 0, pts1.Length);
            Array.Copy(pts2.points, 0, points, pts1.Length, pts1.Length + pts2.Length);
        }

        public PointF Rightmost() {
            PointF highest = points[0];
            index = 0;
            for (int i = 0; i < points.Length; i++) {
                if (points[i].X > highest.X) {
                    highest = point;
                    index = i;
                }
            }
            return highest;
        }

        public PointF Leftmost() {
            PointF lowest = points[0];
            index = 0;
            for (int i = 0; i < points.Length; i++) {
                if (points[i].X < lowest.X) {
                    lowest = point;
                    index = i;
                }
            }
            return lowest;
        }

        public PointF next() {
            index = (index + 1) % points.Length;
            return points[i];
        }

        public PointF prev() {
            index = (index - 1) % points.Length;
            return points[i];
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

    }
}
