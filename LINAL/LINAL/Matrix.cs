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
            _rows = rows;
            _columns = columns;
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
                

            Matrix result = MatrixFactory.Multiply(translation, this);

            if(result != null)
                _data = result.GetData();

            if(addedHelpRow)
                RemoveHelpRow();
            

        }

        public void Scale(Point p)
        {

            Matrix scaling = MatrixFactory.GetScalingMatrix(this, p);
            Matrix result = MatrixFactory.Multiply(scaling, this);

            if (result != null)
                _data = result.GetData();

        }

        public void Rotate2D(double angle, Point p = null)
        {

            //Rotate around offspring
            if (p == null)
            {
                Matrix rotation = MatrixFactory.Rotate2D(angle);
                Matrix result = MatrixFactory.Multiply(rotation, this);
                if (result != null)
                    _data = result.GetData();
            }
            else
            {
                
                Point inverse = new Point(-p.GetX(), -p.GetY(), -p.GetZ());
                Translate(inverse);
                Matrix rotation = MatrixFactory.Rotate2D(angle);
                Matrix result = MatrixFactory.Multiply(rotation, this);

                if (result != null)
                    _data = result.GetData();

                Translate(p);

            }

        }

        public void AddHelpRow()
        {
            
            SetSizeWithData(_rows+1,_columns);

            for (int i = 0; i < _columns; i++)
                _data[_rows-1, i] = 1;

        }

        public void RemoveHelpRow()
        {
            SetSizeWithData(_rows - 1, _columns);
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
