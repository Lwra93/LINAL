using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{
    class Plane
    {

        private List<Point> points = new List<Point>();

        private float formulaX;
        private float formulaY;
        private float formulaZ;
        private float formulaAnswer;


        public void Add(Point p)
        {
            points.Add(p);
        }

        public void Remove(Point p)
        {
            points.Remove(p);
        }

        public void Remove(int index)
        {
            points.RemoveAt(index);
        }

        public List<Point> GetPoints()
        {
            return points;
        }

        public Vector GetSupportVector()
        {

            Point p = points[0];
            return new Vector(p.GetX(), p.GetY(), p.GetZ());

        }

        public Vector GetNormalVector()
        {
            return null;
        }

        public double GetRadiansFromInproduct(float inproduct)
        {

            var vectors = GetDirectionalVectors();

            double result = inproduct/(vectors[0].GetLength()*vectors[1].GetLength());

            return Math.Acos(result);

        }

        public double GetAngleFromInproduct(float inproduct)
        {

            var radians = GetRadiansFromInproduct(inproduct);
            return radians/Math.PI*180;

        }

        public bool IsInPlane(Point p)
        {

            return (formulaX*p.GetX()) + (formulaY*p.GetY()) + (formulaZ*p.GetZ()) == formulaAnswer;

        }

        public float GetInproduct(Vector v1, Vector v2)
        {

            return v1.GetX()*v2.GetX() + v1.GetY()*v2.GetY() + v1.GetZ()*v2.GetZ();

        }

        public Vector GetCrossProduct(List<Vector> vectors)
        {

            float x = vectors[0].GetY()*vectors[1].GetZ() - vectors[1].GetY()*vectors[0].GetZ();
            float y = vectors[1].GetX()*vectors[0].GetZ() - vectors[0].GetX()*vectors[1].GetZ();
            float z = vectors[0].GetX()*vectors[1].GetY() - vectors[1].GetX()*vectors[0].GetY();
            return new Vector(x,y,z);

        }

        public List<Vector> GetDirectionalVectors()
        {

            List<Vector> v = new List<Vector>();

            for (int i = 1; i < points.Count; i++)
            {
                Point p = points[i];

                float x = p.GetX() - GetSupportVector().GetX();
                float y = p.GetY() - GetSupportVector().GetY();
                float z = p.GetZ() - GetSupportVector().GetZ();

                while (true)
                {
                    if (x%2 == 0 && y%2 == 0 && z%2 == 0)
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


                v.Add(new Vector(x,y,z));
            }

            return v;
        }

        public void BuildFormula()
        {

            var normalVector = GetCrossProduct(GetDirectionalVectors());

            formulaX = normalVector.GetX();
            formulaY = normalVector.GetY();
            formulaZ = normalVector.GetZ();

            formulaAnswer = GetInproduct(normalVector, GetSupportVector());

        }

        public bool IsProperPlane()
        {

            List<Point> p = new List<Point>();
            foreach (Point point in points)
            {
                if(!p.Contains(point))
                    p.Add(point);
            }

            return p.Count >= 3;

        }


    }
}
