using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{
    class Point
    {

        private float x;
        private float y;
        private float z;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float GetX()
        {
            return x;
        }

        public void SetX(float x)
        {
            this.x = x;
        }

        public float GetY()
        {
            return y;
        }

        public void SetY(float y)
        {
            this.y = y;
        }

        public float GetZ()
        {
            return z;
        }

        public void SetZ(float z)
        {
            this.z = z;
        }

        public bool Is3D()
        {
            return z > 0;
        }

    }
}
