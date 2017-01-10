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


        /*
         * Returns a scaling matrix, two and three dimensional
         */
        public static Matrix GetScalingMatrix(Matrix m, float x, float y, float z= 0)
        {

            Matrix scalingMatrix = new Matrix(m.GetRows(), m.GetRows());
            scalingMatrix.Set(0, 0, x);
            scalingMatrix.Set(1, 1, y);

            if (z != 0)
                scalingMatrix.Set(2, 2, z);

            return scalingMatrix;

        }

        /*
         * Returns a translation matrix, two and three dimensional
         */
        public static Matrix GetTranslationMatrix(float x, float y, float z = 0, bool threedim = false)
        {

            Matrix translationMatrix = null;
            if (threedim)
            {
                translationMatrix = new Matrix(4, 4);
                translationMatrix.MakeIdentityMatrix();
                translationMatrix.Set(0, 3, x);
                translationMatrix.Set(1, 3, y);
                translationMatrix.Set(2, 3, z);
            }
            else
            {
                translationMatrix = new Matrix(3, 3);
                translationMatrix.MakeIdentityMatrix();
                translationMatrix.Set(0, 2, x);
                translationMatrix.Set(1, 2, y);
            }

            return translationMatrix;

        }

        /*
         * Returns a 2D rotation matrix.
         */
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

        /*
         * Returns a 3D rotation matrix, over a vector, or a vector through the origin
         */
        public static Matrix Get3DRotationMatrix(float alpha, Vector rotationVector, Point translateOver = null)
        {

            Matrix rotationMatrix = new Matrix(4,4);
            rotationMatrix.MakeIdentityMatrix();

            if (translateOver != null)
            {
                Matrix translation =
                    GetTranslationMatrix(-translateOver.GetX(), -translateOver.GetY(), -translateOver.GetZ(), true);
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
                Matrix translation = GetTranslationMatrix(translateOver.GetX(), translateOver.GetY(), translateOver.GetZ(), true);
                translation.Multiply(rotationMatrix);
                rotationMatrix = translation;
            }

            return rotationMatrix;

        }

        /*
         * Returns a 3D matrix based on the X axis
         */
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

        /*
         * Returns a 3D matrix based on the Y axis
         */
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

        /*
         * Returns a 3D matrix based on the Z axis
         */
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
