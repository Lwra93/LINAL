using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{
    class Vector
    {

        private Point[] points = new Point[2];

        public Vector()
        {
            
        }

        public Vector(Point p1, Point p2)
        {
            points[0] = p1;
            points[1] = p2;
        }

        public void SetPoint(int index, Point point)
        {
            if (index >= 0 && index < points.Length)
                points[index] = point;
        }

        public void AddPoint(Point point)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] == null)
                    points[i] = point;
            }
        }

        public void RemovePoint(int index)
        {
            if (index >= 0 && index < points.Length)
                points[index] = null;
        }
        public void RemovePoint(Point point)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] == point)
                    points[i] = null;
            }
        }

        public void RemoveAll()
        {
            for (int i = 0; i < points.Length; i++)
                points[i] = null;
        }

        public Point GetPoint(int index)
        {
            if (index >= 0 && index < points.Length)
                return points[index];

            return null;
        }

        public bool IsProperVector()
        {
            int realPoints = 0;

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] != null) realPoints++;
            }

            return realPoints == points.Length;
        }

        public void Enlarge(float factor)
        {

            if (!IsProperVector())
                return;

            Point p1 = points[0];
            Point p2 = points[1];

            float deltaX = p2.GetX() - p1.GetX();
            float deltaY = p2.GetY() - p1.GetY();
            float deltaZ = p2.GetZ() - p1.GetZ();

            p2.SetX(deltaX*factor);
            p2.SetY(deltaY*factor);
            p2.SetZ(deltaZ*factor);

        }

    }
}
