using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace LINAL
{
    class MatrixMath
    {

        public Matrix Multiply(Matrix m1, Matrix m2)
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
                        {

                            number += m1.Get(i, j3) * m2.Get(j3, i2);

                        }

                        result.Add(i, i2, number);


                    }

                }
            }

            return result;
        }

        public Matrix Scale(Matrix m, Matrix scaling)
        {
            return Multiply(scaling, m);
        }

        public Matrix Translate(Matrix m, Matrix translate)
        {
            return Multiply(translate, m);
        }

        public Matrix GetScaleAndTranslate(Matrix scaling, Matrix translate)
        {

            for (int i = 0; i < scaling.GetRows(); i++)
                translate.Add(i,i,scaling.Get(i,i));

            return translate;


        }

        public Matrix GetScalingMatrix(Matrix m, float xScale, float yScale, float zScale = 0)
        {
            
            Matrix scalingMatrix = new Matrix(m.GetRows(), m.GetRows());
            scalingMatrix.Add(0, 0, xScale);
            scalingMatrix.Add(1, 1, yScale);

            if(zScale > 0)
                scalingMatrix.Add(2, 2, zScale);

            return scalingMatrix;

        }




        public Matrix GetTranslationMatrix(float x, float y, float z = 0)
        {
            Matrix translationMatrix = (z <= 0 ? new Matrix(3, 3) : new Matrix(4, 4));
            translationMatrix.MakeIdentityMatrix();
            translationMatrix.Add(0, 2, x);
            translationMatrix.Add(1, 2, y);

            if (z > 0)
                translationMatrix.Add(2, 2, z);

            return translationMatrix;

        }

        public Matrix GetRotationOffspring(Matrix m, double alpha)
        {
            
            Matrix rotationMatrix = new Matrix(m.GetRows(), m.GetRows());

            float cos = (float) Math.Cos(alpha * (Math.PI / 180.0));
            float sin = (float)Math.Sin(alpha * (Math.PI / 180.0));

            rotationMatrix.Add(0,0, cos);
            rotationMatrix.Add(0,1,-sin);
            rotationMatrix.Add(1,0,sin);
            rotationMatrix.Add(1,1, cos);

            return rotationMatrix;
            

        }

    }
}
