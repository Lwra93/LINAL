using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{
    public class Vector
    {

        private readonly float _x1;
        private readonly float _x2;
        private readonly float _y1;
        private readonly float _y2;

        public Vector(float x1, float y1, float x2, float y2)
        {

            this._x1 = x1;
            this._y1 = y1;
            this._x2 = x2;
            this._y2 = y2;

        }

        public float GetX1()
        {
            return _x1;
        }

        public float GetY1()
        {
            return _y1;
        }

        public float GetX2()
        {
            return _x2;
        }

        public float GetY2()
        {
            return _y2;
        }

    }
}
