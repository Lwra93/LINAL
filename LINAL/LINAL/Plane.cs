using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{
    public class Plane
    {

        private readonly List<Point> _points = new List<Point>();

        private float formulaX;
        private float formulaY;
        private float formulaZ;
        private float formulaAnswer;

        /*
         * Adds a point to the plane
         */
        public void Add(Point p)
        {
            _points.Add(p);
        }

        /*
         * Removes a point from the plane
         */
        public void Remove(Point p)
        {
            _points.Remove(p);
        }

        /*
         * Removes a point from the plane
         */
        public void Remove(int index)
        {
            _points.RemoveAt(index);
        }

        /*
         * Gets all points
         */
        public List<Point> GetPoints()
        {
            return _points;
        }

        /*
         * Returns the support vector
         */
        public Vector GetSupportVector()
        {

            return _points[0].MakeVector();

        }

        /*
         * Returns the normal vector
         */
        public Vector GetNormalVector()
        {
            return GetDirectionalVectors()[0].GetCrossProduct(GetDirectionalVectors()[1]);
        }

        /*
         * Returns the radians of the inproduct
         */
        public double GetRadiansFromInproduct(float inproduct)
        {

            var vectors = GetDirectionalVectors();

            double result = inproduct/(vectors[0].GetLength()*vectors[1].GetLength());

            return Math.Acos(result);

        }

        /*
         * Returns the angle of the inproduct
         */
        public double GetAngleFromInproduct(float inproduct)
        {

            var radians = GetRadiansFromInproduct(inproduct);
            return radians/Math.PI*180;

        }

        /*
         * Checks if a point resides within the plane
         */
        public bool IsInPlane(Point p)
        {

            return (formulaX*p.GetX()) + (formulaY*p.GetY()) + (formulaZ*p.GetZ()) == formulaAnswer;

        }


        /*
         * Returns the directional vectors of the plane
         */
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


                v.Add((new Point(x,y,z)).MakeVector());
            }

            return v;
        }

        /*
         * Builds the formula to see wether a point resides in a plane
         */
        public void BuildFormula()
        {

            var vectors = GetDirectionalVectors();

            if (vectors[0].IsDependantOf(vectors[1]))
                return;

            var normalVector = vectors[0].GetCrossProduct(vectors[1]);

            formulaX = normalVector.GetX();
            formulaY = normalVector.GetY();
            formulaZ = normalVector.GetZ();

            formulaAnswer = normalVector.GetInproduct(GetSupportVector());

        }

    }
}
