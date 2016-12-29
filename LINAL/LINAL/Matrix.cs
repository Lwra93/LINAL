using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{
    class Matrix
    {

        private int _rows;
        private int _columns;
        private float[,] _data;

        public Matrix(int rows, int columns)
        {
            this._rows = rows;
            this._columns = columns;
            _data = new float[rows,columns];
        }

        public int GetRows()
        {
            return _rows;
        }

        public void SetRows(int rows)
        {
            this._rows = rows;
        }

        public int GetColumns()
        {
            return _columns;
        }

        public void SetColumns(int columns)
        {
            this._columns = columns;
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
            //TODO
        }

        

    }
}
