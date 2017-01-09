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
    public class Matrix
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

        public void AddVector(int column, Vector v)
        {

            if (GetRows() < 4)
                SetSizeWithData(4, GetColumns());

            if(column >= GetColumns())
                SetSizeWithData(GetRows(), GetColumns()+1);
            if (column >= GetColumns())
                SetSizeWithData(GetRows(), GetColumns() + 1);

            if (column >= 0)
            {
                _data[0, column] = v.GetPoint(0).GetX();
                _data[1, column] = v.GetPoint(0).GetY();
                _data[2, column] = v.GetPoint(0).GetZ();
                if (v.GetHelp() > 0)
                    _data[3, column] = 1;

                _data[0, column+1] = v.GetPoint(1).GetX();
                _data[1, column+1] = v.GetPoint(1).GetY();
                _data[2, column+1] = v.GetPoint(1).GetZ();
                if (v.GetHelp() > 0)
                    _data[3, column+1] = 1;
            }

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
                

            translation.Multiply(this);
            _data = translation.GetData();

            if(addedHelpRow)
                RemoveHelpRow();
            

        }

        public void Scale(float x, float y, float z = 0)
        {

            Matrix scaling = MatrixFactory.GetScalingMatrix(this, x, y, z);
            scaling.Multiply(this);
            _data = scaling.GetData();
        }

        public void Rotate3D(float angle, Point p1, Point p2 = null)
        {
            Rotate(angle, true, p1, p2);
        }

        public void Rotate2D(float angle, Point p1, Point p2 = null)
        {
            Rotate(angle, false, p1, p2);
        }

        public void Rotate(float angle, bool threedim, Point p1, Point p2 = null)
        {

            if (!threedim)
            {
                //Rotate around offspring
                if (p1 == null)
                {
                    Matrix rotation = MatrixFactory.Rotate2D(angle);
                    Multiply(rotation);
                }
                else
                {

                    Point inverse = new Point(-p1.GetX(), -p1.GetY(), -p1.GetZ());
                    Translate(inverse);
                    Matrix rotation = MatrixFactory.Rotate2D(angle);
                    Multiply(rotation);
                    Translate(p1);

                }
            }
            else
            {

                Vector v = null;
                Point over = null;

                if (p2 == null)
                    v = new Vector(new Point(0, 0, 0), p1);
                else
                {

                    v = new Vector(p1, p2);
                    over = p1;
                }

                Matrix rotation = MatrixFactory.Get3DRotationMatrix(angle, v, over);

                rotation.Multiply(this);
                _data = rotation.GetData();

            }

        }

        public void RotateReplacement(int column, Vector v, float alpha, Point p)
        {

            float x = _data[0, column];
            float y = _data[1, column];
            float z = _data[2, column];

            Matrix sub = new Matrix(4, 1);
            float[,] data = { { x }, { y }, { z } };
            sub.SetData(data);

            

            _data[0, column] = sub.Get(0, 0);
            _data[1, column] = sub.Get(1, 0);
            _data[2, column] = sub.Get(2, 0);
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

            if (matrix.GetRows() != GetColumns())
                return;

            Matrix result = new Matrix(GetRows(), matrix.GetColumns());

            for (int secondColumns = 0; secondColumns < matrix.GetColumns(); secondColumns++)
            {

                for (int secondRows = 0; secondRows < matrix.GetRows(); secondRows++)
                {

                    for (int firstRows = 0; firstRows < GetRows(); firstRows++)
                    {
                        float num = 0;

                        for (int firstColumns = 0; firstColumns < GetColumns(); firstColumns++)
                            num += Get(firstRows, firstColumns) * matrix.Get(firstColumns, secondColumns);

                        result.Add(firstRows, secondColumns, num);

                    }

                }
            }

            _data = result.GetData();

        }

        public Matrix ConvertToVectors()
        {

            Matrix m = new Matrix(GetRows(), GetColumns()/2);
            float[,] data = new float[GetRows(),GetColumns()/2];


            for (int column = 0; column < GetColumns()/2; column++)
            {

                float x = _data[0, column + 1] - _data[0, column];
                float y = _data[1, column + 1] - _data[1, column];
                float z = _data[2, column + 1] - _data[2, column];

                data[0, column] = x;
                data[1, column] = y;
                data[2, column] = z;
                data[3, column] = 1;

                column++;

            }

            m.SetData(data);

            return m;
        }

        public void Recalculate(double screenSize)
        {

            for (int column = 0; column < _data.GetLength(1); column++)
            {

                float x = _data[0, column + 1] - _data[0, column];
                float y = _data[1, column + 1] - _data[1, column];
                float z = _data[2, column + 1] - _data[2, column];
                float w = _data[3, column + 1] - _data[3, column];

                x = (float) ((screenSize / 2) + ((x + 1) / w) * screenSize * 0.5);
                y = (float)((screenSize / 2) + ((y + 1) / w) * screenSize * 0.5);
                z = -z;

                _data[0, column + 1] = x;
                _data[1, column + 1] = y;
                _data[2, column + 1] = z;
                _data[3, column + 1] = w;

                column++;

            }

        }

        public Point GetMiddle()
        {

            List<Point> points = new List<Point>();

            for (int column = 0; column < GetColumns(); column++)
            {

                float x = _data[0, column];
                float y = _data[1, column];
                float z = _data[2, column];

                bool existsInList = false;

                foreach (Point p in points)
                {
                    if (p.GetX() == x && p.GetY() == y && p.GetZ() == z)
                    {
                        existsInList = true;
                        break;
                    }                    
                }

                if(!existsInList)
                    points.Add(new Point(x,y,z));

            }

            float x1 = float.MinValue;
            float x2 = float.MaxValue;
            float y1 = float.MinValue;
            float y2 = float.MaxValue;
            float z1 = float.MinValue;
            float z2 = float.MaxValue;

            foreach (Point p in points)
            {

                if (p.GetX() > x1)
                    x1 = p.GetX();
                else if (p.GetX() < x2)
                    x2 = p.GetX();

                if (p.GetY() > y1)
                    y1 = p.GetY();
                else if (p.GetY() < y2)
                    y2 = p.GetY();

                if (p.GetZ() > z1)
                    z1 = p.GetZ();
                else if (p.GetZ() < z2)
                    z2 = p.GetZ();

            }

            float newX = x1 + ((x2 - x1)/2);
            float newY = y1 + ((y2 - y1) / 2);
            float newZ = z1 + ((z2 - z1) / 2);

            return new Point(newX, newY, newZ);

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
