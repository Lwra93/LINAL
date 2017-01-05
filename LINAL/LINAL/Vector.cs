using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{
    public class Vector
    {

        private Point[] points = new Point[2];
        private float help;

        public Vector(Point p1, Point p2)
        {

            points[0] = p1;
            points[1] = p2;
            this.help = 1;
        }

        public Point GetPoint(int index)
        {
            return points[index];
        }

        public float GetX()
        {
            return points[1].GetX() - points[0].GetX();
        }

        public float GetY()
        {
            return points[1].GetY() - points[0].GetY();
        }

        public float GetZ()
        {
            return points[1].GetZ() - points[0].GetZ();
        }

        public float GetHelp()
        {
            return help;
        }

        public void SetHelp(int help)
        {
            this.help = help;
        }

        public float GetLength()
        {
            return (float) Math.Sqrt(GetX()*GetX() + GetY()*GetY() + GetZ()*GetZ());
        }

        public bool IsDependantOf(Vector v)
        {

            var factorX = GetX() / v.GetX();
            var factorY = GetY() / v.GetY();
            var factorZ = GetZ() / v.GetZ();

            return factorX == factorY && factorY == factorZ;

        }

        public Vector Add(Vector v)
        {

            float x = GetX() + v.GetX();
            float y = GetY() + v.GetY();
            float z = GetZ() + v.GetZ();

            Point p = new Point(GetPoint(0).GetX() + x, GetPoint(0).GetY() + y, GetPoint(0).GetZ() + z);

            return new Vector(GetPoint(0), p);

        }

        public Vector Subtract(Vector v)
        {
            float x = GetX() - v.GetX();
            float y = GetY() - v.GetY();
            float z = GetZ() - v.GetZ();

            Point p = new Point(GetPoint(0).GetX() + x, GetPoint(0).GetX() + y, GetPoint(0).GetZ()+z);

            return new Vector(GetPoint(0), p);
        }

        public void MakeUnitVector()
        {
            float length = GetLength();

            float x = GetX() / length;
            float y = GetY() / length;
            float z = GetZ() / length;

            points[1].SetX(x);
            points[1].SetY(y);
            points[1].SetZ(z);

        }

        public void Enlarge(float factor)
        {

            float x = GetX() * factor;
            float y = GetY() * factor;
            float z = GetZ() * factor;

            points[1].SetX(x);
            points[1].SetY(y);
            points[1].SetZ(z);

        }

        public Vector GetSimplified()
        {

            var x = GetX();
            var y = GetY();
            var z = GetZ();

            while (true)
            {
                if (x % 2 == 0 && y % 2 == 0 && z % 2 == 0)
                {
                    x /= 2;
                    y /= 2;
                    z /= 2;
                }
                else
                {
                    break;
                }
            }

            var p = new Point(points[0].GetX() + x, points[0].GetY() + y, points[0].GetZ() + z);

            return new Vector(points[0], p);


        }

        public Vector GetCrossProduct(Vector v)
        {

            var x = GetY() * v.GetZ() - v.GetY() * GetZ();
            var y = v.GetX() * GetZ() - GetX() * v.GetZ();
            var z = GetX() * v.GetY() - v.GetX() * GetY();

            var realX = points[0].GetX() + x;
            var realY = points[0].GetY() + y;
            var realZ = points[0].GetZ() + z;

            var point = new Point(realX, realY, realZ);

            return new Vector(points[0], point);

        }

        public float GetInproduct(Vector v)
        {
            //ax*bx + ay*by + az*bz
            return GetX() * v.GetX() + GetY() * v.GetY() + GetZ() * v.GetZ();
        }

        public void AddHelp()
        {
            this.help = 1;
        }

        public void RemoveHelp()
        {
            this.help = 0;
        }

        public void Print()
        {
            Console.WriteLine("Vector x: " + GetX() + ", y:" + GetY() + ", z: " + GetZ() + ", Length: " + GetLength());
        }

    }
}
