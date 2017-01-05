using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace LINAL
{
    class MatrixFactory
    {

        

        public static Matrix GetScaleAndTranslate(Matrix scaling, Matrix translate)
        {

            for (int i = 0; i < scaling.GetRows(); i++)
                translate.Add(i, i, scaling.Get(i, i));

            return translate;


        }

        public static Matrix GetScalingMatrix(Matrix m, Point p)
        {

            Matrix scalingMatrix = new Matrix(m.GetRows(), m.GetRows());
            scalingMatrix.Add(0, 0, p.GetX());
            scalingMatrix.Add(1, 1, p.GetY());

            if (p.Is3D())
                scalingMatrix.Add(2, 2, p.GetZ());

            return scalingMatrix;

        }

        public static Matrix GetTranslationMatrix(Point p)
        {

            Matrix translationMatrix = null;
            if (p.Is3D())
            {
                translationMatrix = new Matrix(4, 4);
                translationMatrix.MakeIdentityMatrix();
                translationMatrix.Add(0, 3, p.GetX());
                translationMatrix.Add(1, 3, p.GetY());
                translationMatrix.Add(2, 3, p.GetZ());
            }
            else
            {
                translationMatrix = new Matrix(3, 3);
                translationMatrix.MakeIdentityMatrix();
                translationMatrix.Add(0, 2, p.GetX());
                translationMatrix.Add(1, 2, p.GetY());
            }

            return translationMatrix;

        }

        public static Matrix Rotate2D(float alpha)
        {

            Matrix rotationMatrix = new Matrix(2,2);

            float cos = GonioFactory.GetTrigonometricByDegrees(alpha, Trigonometric.Cosine);
            float sin = GonioFactory.GetTrigonometricByDegrees(alpha, Trigonometric.Sine);

            float[,] data = {
                {cos,-sin},
                {sin,cos}
            };

            rotationMatrix.SetData(data);

            return rotationMatrix;

        }

        public static Matrix Rotate3DXAxis(float alpha, bool reverse)
        {

            Matrix rotationMatrix = new Matrix(3, 3);
            float cos = GonioFactory.GetTrigonometricByDegrees(alpha, Trigonometric.Cosine);
            float sin = GonioFactory.GetTrigonometricByDegrees(alpha, Trigonometric.Sine);

            if (reverse)
                sin = -sin;

            float[,] data = { 
                {1,0,0},
                {0,cos,-sin},
                {0,sin,cos}
            };

            rotationMatrix.SetData(data);

            return rotationMatrix;

        }

        public static Matrix Rotate3DYAxis(float alpha, bool reverse)
        {

            Matrix rotationMatrix = new Matrix(3, 3);
            float cos = GonioFactory.GetTrigonometricByDegrees(alpha, Trigonometric.Cosine);
            float sin = GonioFactory.GetTrigonometricByDegrees(alpha, Trigonometric.Sine);

            if (reverse)
                sin = -sin;

            float[,] data = {
                {cos, 0, -sin},
                {0, 1, 0},
                {sin, 0, cos}
            };

            rotationMatrix.SetData(data);

            return rotationMatrix;

        }

        public static Matrix Rotate3DZAxis(float alpha, bool reverse)
        {

            Matrix rotationMatrix = new Matrix(3, 3);
            float cos = GonioFactory.GetTrigonometricByDegrees(alpha, Trigonometric.Cosine);
            float sin = GonioFactory.GetTrigonometricByDegrees(alpha, Trigonometric.Sine);

            if (reverse)
                sin = -sin;

            float[,] data = {
                {cos, -sin, 0},
                {sin, cos, 0},
                {0, 0, 1}
            };

            rotationMatrix.SetData(data);

            return rotationMatrix;

        }

    }

}
