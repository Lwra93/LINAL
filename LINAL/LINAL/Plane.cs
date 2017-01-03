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

        private readonly List<Point> _points = new List<Point>();

        private float formulaX;
        private float formulaY;
        private float formulaZ;
        private float formulaAnswer;


        public void Add(Point p)
        {
            _points.Add(p);
        }

        public void Remove(Point p)
        {
            _points.Remove(p);
        }

        public void Remove(int index)
        {
            _points.RemoveAt(index);
        }

        public List<Point> GetPoints()
        {
            return _points;
        }

        public Vector GetSupportVector()
        {

            Point p = _points[0];
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
            //ax*bx + ay*by + az*bz
            return v1.GetX()*v2.GetX() + v1.GetY()*v2.GetY() + v1.GetZ()*v2.GetZ();
        }

        public Vector GetCrossProduct(List<Vector> vectors = null)
        {

            if (vectors == null)
                vectors = GetDirectionalVectors();

            var x = vectors[0].GetY()*vectors[1].GetZ() - vectors[1].GetY()*vectors[0].GetZ();
            var y = vectors[1].GetX()*vectors[0].GetZ() - vectors[0].GetX()*vectors[1].GetZ();
            var z = vectors[0].GetX()*vectors[1].GetY() - vectors[1].GetX()*vectors[0].GetY();
            return new Vector(x,y,z);

        }

        public List<Vector> GetDirectionalVectors()
        {

            var v = new List<Vector>();

            for (var i = 1; i < _points.Count; i++)
            {
                var p = _points[i];

                var x = p.GetX() - GetSupportVector().GetX();
                var y = p.GetY() - GetSupportVector().GetY();
                var z = p.GetZ() - GetSupportVector().GetZ();

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

            var vectors = GetDirectionalVectors();

            if (vectors[0].IsDependantOf(vectors[1]))
                return;

            var normalVector = GetCrossProduct(vectors);

            formulaX = normalVector.GetX();
            formulaY = normalVector.GetY();
            formulaZ = normalVector.GetZ();

            formulaAnswer = GetInproduct(normalVector, GetSupportVector());

        }

        public bool IsProperPlane()
        {

            var p = new List<Point>();
            foreach (var point in _points)
            {
                if(!p.Contains(point))
                    p.Add(point);
            }

            return p.Count >= 3;

        }


    }
}
