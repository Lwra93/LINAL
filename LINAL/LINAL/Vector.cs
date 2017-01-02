using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{
    class Vector
    {

        private float x, y, z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(Point p1, Point p2)
        {
            this.x = p2.GetX() - p1.GetX();
            this.y = p2.GetY() - p1.GetY();
            this.z = p2.GetZ() - p1.GetZ();
        }

        public float GetX()
        {
            return x;
        }

        public float GetY()
        {
            return y;
        }

        public float GetZ()
        {
            return z;
        }

        public double GetLength()
        {

            double xd = (double) Math.Pow(x, 2);
            double yd = (double) Math.Pow(y, 2);
            double zd = (double)Math.Pow(z, 2);

            return Math.Sqrt(xd + yd + zd);

        }

        public void Enlarge(float factor)
        {

            x *= factor;
            y *= factor;
            z *= factor;

        }

    }
}
