using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{
    class Vector
    {

        private float x, y, z, help;

        public Vector(float x, float y, float z, float help = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.help = help;
        }

        public Vector(Point p1, Point p2)
        {
            this.x = p2.GetX() - p1.GetX();
            this.y = p2.GetY() - p1.GetY();
            this.z = p2.GetZ() - p1.GetZ();
            this.help = 0;
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

        public void SetX(float x)
        {
            this.x = x;
        }

        public void SetY(float y)
        {
            this.y = y;
        }

        public void SetZ(float z)
        {
            this.z = z;
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
            return (float) Math.Sqrt(x*x + y*y + z*z);
        }

        public bool IsDependantOf(Vector v)
        {

            var factorX = x / v.GetX();
            var factorY = y / v.GetY();
            var factorZ = z / v.GetZ();

            return factorX == factorY && factorY == factorZ;

        }

        public Vector Add(Vector v)
        {

            return new Vector(x + v.GetX(), y + v.GetY(), z + v.GetZ());

        }

        public Vector Subtract(Vector v)
        {
            return new Vector(x - v.GetX(), y - v.GetY(), z - v.GetZ());
        }

        public void MakeUnitVector()
        {
            float length = GetLength();

            x /= length;
            y /= length;
            z /= length;
        }

        public void Enlarge(float factor)
        {

            x *= factor;
            y *= factor;
            z *= factor;

        }

        public Vector GetCrossProduct(Vector v)
        {

            var x = GetY() * v.GetZ() - v.GetY() * GetZ();
            var y = v.GetX() * GetZ() - GetX() * v.GetZ();
            var z = GetX() * v.GetY() - v.GetX() * GetY();
            return new Vector(x, y, z);

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
            Console.WriteLine("Vector x: " + x + ", y:" + y + ", z: " + z + ", Length: " + GetLength());
        }

    }
}
