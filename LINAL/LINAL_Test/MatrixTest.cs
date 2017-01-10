using System;
using LINAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LINAL_Test
{
    [TestClass]
    public class MatrixTest
    {

        private Matrix mockingMatrix;

        [TestInitialize]
        public void init()
        {
            mockingMatrix = new Matrix(3, 3);
            float[,] data =
            {
                {
                    0, 1, 2
                },
                {
                    1, 2, 4
                },
                {
                    8, 4, 2
                }
            };

            mockingMatrix.SetData(data);

        }

        [TestMethod]
        public void MatrixInversion()
        {

            
            mockingMatrix.Invert();

            Matrix m2 = new Matrix(3,3);
            float[,] data2 =
            {
                {
                    -2, 1, 0
                },
                {
                    5, ((float)-8/3), ((float)1/3)
                },
                {
                    -2, ((float)4/3), ((float)-1/6)
                }
            };
            m2.SetData(data2);

            bool identical = true;

            for (int row = 0; row < mockingMatrix.GetRows(); row++)
            {
                for (int column = 0; column < mockingMatrix.GetColumns(); column++)
                {

                    if (mockingMatrix.Get(row, column) != m2.Get(row, column))
                    {
                        identical = false;
                        break;
                    }


                }
            }

            Assert.AreEqual(true,identical);

        }

        [TestMethod]
        public void MatrixScaling()
        {

            mockingMatrix.Scale(1.1f, 1.1f, 1.1f, false);

            Matrix m2 = new Matrix(3, 3);
            float[,] data2 =
            {
                {
                    0, 1.1f, 2.2f
                },
                {
                    1.1f, 2.2f, 4.4f
                },
                {
                    8.8f, 4.4f, 2.2f
                }
            };
            m2.SetData(data2);

            bool identical = true;

            for (int row = 0; row < mockingMatrix.GetRows(); row++)
            {
                for (int column = 0; column < mockingMatrix.GetColumns(); column++)
                {

                    if (mockingMatrix.Get(row, column) != m2.Get(row, column))
                    {
                        identical = false;
                        break;
                    }


                }
            }

            Assert.AreEqual(true, identical);

        }

        [TestMethod]
        public void MatrixTranslate()
        {

            mockingMatrix.Translate(5, 3, 12, true);

            Matrix m2 = new Matrix(3, 3);
            float[,] data2 =
            {
                {
                    5, 6, 7
                },
                {
                    4, 5, 7
                },
                {
                    20, 16, 14
                }
            };
            m2.SetData(data2);

            bool identical = true;

            for (int row = 0; row < mockingMatrix.GetRows(); row++)
            {
                for (int column = 0; column < mockingMatrix.GetColumns(); column++)
                {

                    if (mockingMatrix.Get(row, column) != m2.Get(row, column))
                    {
                        identical = false;
                        break;
                    }


                }
            }

            Assert.AreEqual(true, identical);

        }
    }
}
