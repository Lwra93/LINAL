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

        public static Matrix Multiply(Matrix m1, Matrix m2)
        {

            if (m1.GetColumns() != m2.GetRows())
                return null;

            Matrix result = new Matrix(m1.GetRows(), m2.GetColumns());

            for (int i2 = 0; i2 < m2.GetColumns(); i2++)
            {

                for (int j2 = 0; j2 < m2.GetRows(); j2++)
                {

                    for (int i = 0; i < m1.GetRows(); i++)
                    {

                        float number = 0;

                        for (int j3 = 0; j3 < m2.GetRows(); j3++) //Of columns. Maakt niet uit. Rows M1 == Columns M2
                            number += m1.Get(i, j3)*m2.Get(j3, i2);

                        result.Add(i, i2, number);
                    }

                }
            }

            return result;
        }

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

        public static Matrix Rotate2D(double alpha)
        {

            Matrix rotationMatrix = new Matrix(2,2);

            float cos = (float) Math.Cos(alpha*(Math.PI/180.0));
            float sin = (float) Math.Sin(alpha*(Math.PI/180.0));

            float[,] data = {
                {cos,-sin},
                {sin,cos}
            };

            rotationMatrix.SetData(data);

            return rotationMatrix;

        }

        public static Matrix Rotate3DXAxis(Matrix m, double alpha)
        {

            Matrix rotationMatrix = new Matrix(3, 3);
            float cos = (float)Math.Cos(alpha * (Math.PI / 180.0));
            float sin = (float)Math.Sin(alpha * (Math.PI / 180.0));

            float[,] data = { 
                {1,0,0},
                {0,cos,-sin},
                {0,sin,cos}
            };

            rotationMatrix.SetData(data);

            return rotationMatrix;

        }

        public static Matrix Rotate3DYAxis(Matrix m, double alpha)
        {

            Matrix rotationMatrix = new Matrix(3, 3);
            float cos = (float)Math.Cos(alpha * (Math.PI / 180.0));
            float sin = (float)Math.Sin(alpha * (Math.PI / 180.0));

            float[,] data = {
                {cos, 0, -sin},
                {0, 1, 0},
                {sin, 0, cos}
            };

            rotationMatrix.SetData(data);

            return rotationMatrix;

        }

        public static Matrix Rotate3DZAxis(Matrix m, double alpha)
        {

            Matrix rotationMatrix = new Matrix(3, 3);
            float cos = (float)Math.Cos(alpha * (Math.PI / 180.0));
            float sin = (float)Math.Sin(alpha * (Math.PI / 180.0));

            float[,] data = {
                {cos, -sin, 0},
                {sin, cos, 0},
                {0, 0, 1}
            };

            rotationMatrix.SetData(data);

            return rotationMatrix;

        }

    }

    public enum Rotation
    {
        
        Undefined = 0,
        X = 1,
        Y = 2,
        Z = 3
        

    }
}
