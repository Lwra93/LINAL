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


    }
}
