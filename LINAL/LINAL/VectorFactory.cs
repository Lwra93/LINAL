using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace LINAL
{
    class VectorFactory
    {

        public static Vector Calcate(Vector v1, Vector v2)
        {

            float x1 = v1.GetX2() - v1.GetX1();
            float y1 = v1.GetY2() - v1.GetY1();
            float x2 = v2.GetX2() - v2.GetX1();
            float y2 = v2.GetY2() - v2.GetY1();

            float x = x1 + x2;
            float y = y1 + y2;

            return new Vector(v1.GetX1(), v1.GetY1(), (v1.GetX1()+x), (v1.GetY1()+y));

        }

        public static Vector Enlarge(Vector v, float factor)
        {

            float x = (v.GetX2() - v.GetX1())*factor;
            float y = (v.GetY2() - v.GetY1())*factor;


            return new Vector(v.GetX1(), v.GetY1(), (v.GetX1()+ x), (v.GetY1()+y));

        }

    }
}
