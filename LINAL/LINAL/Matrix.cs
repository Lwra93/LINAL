using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace LINAL
{
    class Matrix
    {

        private float[,] _data;

        public Matrix(int rows, int columns)
        {
            _data = new float[rows,columns];
        }

        public int GetRows()
        {
            return _data.GetLength(0);
        }

        public int GetColumns()
        {
            return _data.GetLength(1);
        }

        public float[,] GetData()
        {
            return _data;
        }

        public void SetData(float[,] data)
        {
            _data = data;
        }

        public float Get(int row, int column)
        {
            if (row >= 0 && row < _data.GetLength(0) && column >= 0 && column < _data.GetLength(1))
                return _data[row, column];

            return 0;
        }

        public void Add(int row, int column, float number)
        {
            if (row >= 0 && row < _data.GetLength(0) && column >= 0 && column < _data.GetLength(1))
                _data[row, column] = number;
        }

        public void Remove(int row, int column)
        {
            if (row >= 0 && row < _data.GetLength(0) && column >= 0 && column < _data.GetLength(1))
                _data[row, column] = 0;
        }

        public void SetSizeWithData(int rows, int columns)
        {

            float[,] backup = _data;
            SetSizeWithoutData(rows, columns);

            for (int row = 0; row < backup.GetLength(0); row++)
            {
                for (int column = 0; column < backup.GetLength(1); column++)
                {

                    if (row < _data.GetLength(0) && column < _data.GetLength(1))
                        _data[row, column] = backup[row, column];

                }
            }


        }

        public void SetSizeWithoutData(int rows, int columns)
        {
            _data = new float[rows,columns];
        }

        public void MakeIdentityMatrix()
        {

            MakeNullMatrix();

            int column = 0;
            for (int row = 0; row < _data.GetLength(0); row++)
            {
                _data[row, column] = 1;
                column++;
            }

        }

        public void MakeNullMatrix()
        {
            for (int row = 0; row < _data.GetLength(0); row++)
            {
                for (int column = 0; column < _data.GetLength(1); column++)
                {

                    _data[row, column] = 0;

                }
            }
        }

        public void Translate(Point translateOver)
        {

            bool addedHelpRow = false;
            Matrix translation = MatrixFactory.GetTranslationMatrix(translateOver);

            if (translation.GetColumns() != GetRows())
            {
                AddHelpRow();
                addedHelpRow = true;
            }
                

            Multiply(translation);

            if(addedHelpRow)
                RemoveHelpRow();
            

        }

        public void Scale(Point p)
        {

            Matrix scaling = MatrixFactory.GetScalingMatrix(this, p);
            Multiply(scaling);

        }

        public void Rotate3D(float angle, Rotation r, Point p = null)
        {
            Rotate(angle, true, r, p);
        }

        public void Rotate2D(float angle, Point p = null)
        {
            Rotate(angle, false, Rotation.Undefined, p);
        }

        public void Rotate(float angle, bool threedim, Rotation r, Point p = null)
        {

            if (!threedim)
            {
                //Rotate around offspring
                if (p == null)
                {
                    Matrix rotation = MatrixFactory.Rotate2D(angle);
                    Multiply(rotation);
                }
                else
                {

                    Point inverse = new Point(-p.GetX(), -p.GetY(), -p.GetZ());
                    Translate(inverse);
                    Matrix rotation = MatrixFactory.Rotate2D(angle);
                    Multiply(rotation);
                    Translate(p);

                }
            }
            else
            {

                if (r == Rotation.Undefined)
                    return;

                for (int column = 0; column < GetColumns(); column++)
                {
                    float x = _data[0, column];
                    float y = _data[1, column];
                    float z = _data[2, column];

                    Matrix sub = new Matrix(3,1);
                    float[,] data = {{x}, {y}, {z}};
                    sub.SetData(data);

                    float t1 = GonioFactory.GetArcTrigonometricByRadians(z, x, Trigonometric.Tangent2);
                    var yRotation = MatrixFactory.Rotate3DYAxis(t1, true);
                    sub.Multiply(yRotation);

                    float newX = (float)Math.Sqrt(x*x + z*z);
                    float t2 = GonioFactory.GetArcTrigonometricByRadians(y, newX, Trigonometric.Tangent2);
                    var zRotation = MatrixFactory.Rotate3DZAxis(t2, true);

                    Matrix rotate = MatrixFactory.Rotate3DXAxis(angle, false);

                    sub.Multiply(rotate);

                    var reverseZRotation = MatrixFactory.Rotate3DZAxis(t2, false);
                    sub.Multiply(reverseZRotation);

                    var reverseYRotation = MatrixFactory.Rotate3DYAxis(t1, false);
                    sub.Multiply(reverseYRotation);

                    _data[0, column] = sub.Get(0, column);
                    _data[1, column] = sub.Get(1, column);
                    _data[2, column] = sub.Get(2, column);

                }

            }

        }

        public int GetHighestInRow(int column)
        {

            int num = 0;
            for (int i = 0; i < GetRows(); i++)
            {
                if (_data[i, column] > num)
                    num = i;
            }

            return num;

        }

        public void AddHelpRow()
        {
            
            SetSizeWithData(GetRows()+1,GetColumns());

            for (int i = 0; i < GetColumns(); i++)
                _data[GetRows()-1, i] = 1;

        }

        public void RemoveHelpRow()
        {
            SetSizeWithData(GetRows() - 1, GetColumns());
        }

        public void SwitchValues(int row1, int row2)
        {

            for (int i = 0; i < GetColumns(); i++)
            {

                float backup = _data[row1, i];
                _data[row1, i] = _data[row2, i];
                _data[row2, i] = backup;

            }

        }

        public void Print()
        {
            for (int row = 0; row < _data.GetLength(0); row++)
            {
                for (int column = 0; column < _data.GetLength(1); column++)
                {

                    Console.Write(_data[row, column] + " ");

                }

                Console.WriteLine();
            }
        }

        public void Invert()
        {

            if (GetDeterminant() == 0)
                return;

            Matrix inversion = new Matrix(GetRows(), GetRows());
            inversion.MakeIdentityMatrix();

            if (_data[0, 0] == 0f)
            {
                int row = GetHighestInRow(0);
                SwitchValues(0,row);
                inversion.SwitchValues(0,row);
            }

            for (int column = 0; column < GetColumns(); column++)
            {

                if (_data[column, column] != 1)
                {
                    float divideBy = _data[column, column];
                    ModifyRow(divideBy, Operator.DIVIDE, column);
                    inversion.ModifyRow(divideBy, Operator.DIVIDE, column);
                }

                for (int row = 0; row < GetRows(); row++)
                {
                    if (row == column)
                        continue;

                    if (_data[row, column] == 0)
                        continue;

                    float multiplyBy = _data[row, column]/_data[column, column];

                    for (int col = 0; col < GetColumns(); col++)
                    {
                        float value = _data[column, col]*multiplyBy;
                        float inversionValue = inversion.Get(column, col)*multiplyBy;

                        ModifyRow(value, Operator.SUBTRACT, row, col);
                        inversion.ModifyRow(inversionValue, Operator.SUBTRACT, row, col);
                    }

                }

            }

            _data = inversion.GetData();

        }



        public void ModifyRow(float value, Operator op, int row, int column = -1)
        {

            if (column > -1)
            {
                if (op == Operator.ADD)
                    _data[row, column] += value;
                else if (op == Operator.SUBTRACT)
                    _data[row, column] -= value;
                else if (op == Operator.DIVIDE)
                    _data[row, column] /= value;
                else if (op == Operator.MULTIPLY)
                    _data[row, column] *= value;

                return;
            }

            for (column = 0; column < GetColumns(); column++)
            {

                if (op == Operator.ADD)
                    _data[row, column] += value;
                else if (op == Operator.SUBTRACT)
                    _data[row, column] -= value;
                else if (op == Operator.DIVIDE)
                    _data[row, column] /= value;
                else if (op == Operator.MULTIPLY)
                    _data[row, column] *= value;

            }


        }

        public float GetDeterminant()
        {
            //2D
            if (GetColumns() < 3)
                return _data[0, 0]*_data[1, 1] - _data[0, 1]*_data[1, 0];

            //3D
            else
            {
                float a = _data[0, 0]*(_data[1, 1]*_data[2, 2] - _data[1, 2]*_data[2, 1]);
                float b = _data[0, 1] * (_data[1, 0] * _data[2, 2] - _data[2, 0] * _data[1, 2]);
                float c = _data[0, 2] * (_data[1, 0] * _data[2, 1] - _data[2, 0] * _data[1, 1]);
                return a - b - c;
            }
        }

        public void Multiply(Matrix matrix)
        {

            if (matrix.GetColumns() != GetRows())
                return;

            Matrix result = new Matrix(matrix.GetRows(), GetColumns());

            for (int i2 = 0; i2 < GetColumns(); i2++)
            {

                for (int j2 = 0; j2 < GetRows(); j2++)
                {

                    for (int i = 0; i < matrix.GetRows(); i++)
                    {

                        float number = 0;

                        for (int j3 = 0; j3 < GetRows(); j3++) //Of columns. Maakt niet uit. Rows M1 == Columns M2
                            number += matrix.Get(i, j3) * Get(j3, i2);

                        result.Add(i, i2, number);
                    }

                }
            }


            _data = result.GetData();

        }
    }

    public enum Operator
    {
        ADD = 0,
        SUBTRACT = 1,
        DIVIDE = 2,
        MULTIPLY = 3,
    }
}
