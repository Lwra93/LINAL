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

        public static Matrix GetScalingMatrix(Matrix m, float x, float y, float z= 0)
        {

            Matrix scalingMatrix = new Matrix(m.GetRows(), m.GetRows());
            scalingMatrix.Add(0, 0, x);
            scalingMatrix.Add(1, 1, y);

            if (z != 0)
                scalingMatrix.Add(2, 2, z);

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

        public static Matrix Get3DRotationMatrix(float alpha, Vector rotationVector, Point translateOver = null)
        {

            Matrix rotationMatrix = new Matrix(4,4);
            rotationMatrix.MakeIdentityMatrix();

            if (translateOver != null)
            {
                Matrix translation =
                    GetTranslationMatrix(new Point(-translateOver.GetX(), -translateOver.GetY(), -translateOver.GetZ()));
                translation.Multiply(rotationMatrix);
                rotationMatrix = translation;
            }

            float t1 = GonioFactory.GetArcTrigonometricByRadians(rotationVector.GetZ(), rotationVector.GetX(), Trigonometric.Tangent2);
            var yRotation = MatrixFactory.Rotate3DYAxis(t1, true);
            yRotation.Multiply(rotationMatrix);
            rotationMatrix = yRotation;

            float newX = (float)Math.Sqrt(rotationVector.GetX() * rotationVector.GetX() + rotationVector.GetZ() * rotationVector.GetZ());
            float t2 = GonioFactory.GetArcTrigonometricByRadians(rotationVector.GetY(), newX, Trigonometric.Tangent2);
            var zRotation = MatrixFactory.Rotate3DZAxis(t2, true);
            zRotation.Multiply(rotationMatrix);
            rotationMatrix = zRotation;

            Matrix rotate = MatrixFactory.Rotate3DXAxis(GonioFactory.DegreesToRadians(alpha), false);
            rotate.Multiply(rotationMatrix);
            rotationMatrix = rotate;

            var reverseZRotation = MatrixFactory.Rotate3DZAxis(t2, false);
            reverseZRotation.Multiply(rotationMatrix);
            rotationMatrix = reverseZRotation;

            var reverseYRotation = MatrixFactory.Rotate3DYAxis(t1, false);
            reverseYRotation.Multiply(rotationMatrix);
            rotationMatrix = reverseYRotation;

            if (translateOver != null)
            {
                Matrix translation = GetTranslationMatrix(translateOver);
                translation.Multiply(rotationMatrix);
                rotationMatrix = translation;
            }

            return rotationMatrix;

        }

        public static Matrix Rotate3DXAxis(float alpha, bool reverse)
        {

            Matrix rotationMatrix = new Matrix(4, 4);

            float cos = GonioFactory.GetTrigonometricByRadians(alpha, Trigonometric.Cosine);
            float sin = GonioFactory.GetTrigonometricByRadians(alpha, Trigonometric.Sine);

            if (reverse)
                sin = -sin;

            float[,] data =
            {
                {1, 0, 0, 0},
                {0, cos, -sin, 0},
                {0, sin, cos, 0},
                {0,0,0,1 }
            };

            rotationMatrix.SetData(data);

            return rotationMatrix;

        }

        public static Matrix Rotate3DYAxis(float alpha, bool reverse)
        {

            Matrix rotationMatrix = new Matrix(4, 4);

            float cos = GonioFactory.GetTrigonometricByRadians(alpha, Trigonometric.Cosine);
            float sin = GonioFactory.GetTrigonometricByRadians(alpha, Trigonometric.Sine);

            if (reverse)
                sin = -sin;

            float[,] data = {
                { cos, 0, -sin, 0},
                {0, 1, 0, 0},
                {sin, 0, cos, 0},
                {0,0,0,1 }
            };

            rotationMatrix.SetData(data);

            return rotationMatrix;

        }

        public static Matrix Rotate3DZAxis(float alpha, bool reverse)
        {

            Matrix rotationMatrix = new Matrix(4, 4);

            float cos = GonioFactory.GetTrigonometricByRadians(alpha, Trigonometric.Cosine);
            float sin = GonioFactory.GetTrigonometricByRadians(alpha, Trigonometric.Sine);

            if (reverse)
                sin = -sin;

            float[,] data = {
                {cos, -sin, 0, 0},
                {sin, cos, 0, 0},
                {0, 0, 1, 0},
                {0,0,0,1 }
            };

            rotationMatrix.SetData(data);

            return rotationMatrix;

        }

    }

}
