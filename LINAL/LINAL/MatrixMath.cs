using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Matrix Scale(Matrix m, float xScale, float yScale, float zScale)
        {
            
            Matrix scalingMatrix = new Matrix(m.GetRows(), m.GetRows());
            scalingMatrix.Add(0, 0, xScale);
            scalingMatrix.Add(1, 1, yScale);

            if(zScale > 0)
                scalingMatrix.Add(2, 2, zScale);

            return Multiply(scalingMatrix, m);

        }

        public Matrix Translate(Matrix m, float x, float y, float z)
        {

            if (z > 0)
                return Translate3D(m, x, y, z);
            else
                return Translate2D(m, x, y);

        }

        public Matrix Translate2D(Matrix m, float x, float y)
        {
            
            Matrix translationMatrix = new Matrix(3,3);
            translationMatrix.MakeIdentityMatrix();
            translationMatrix.Add(0,2,x);
            translationMatrix.Add(1,2,y);

            m.SetSizeWithData(m.GetRows()+1, m.GetColumns());

            return null;

        }

        public Matrix Translate3D(Matrix m, float x, float y, float z)
        {
            return null;
        }

    }
}
