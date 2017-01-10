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

        /*
         * Creates a new matrix
         */
        public Matrix(int rows, int columns)
        {
            _data = new float[rows,columns];
        }

        /*
         * Gets the amount of rows of the matrix
         */
        public int GetRows()
        {
            return _data.GetLength(0);
        }

        /*
         * Gets the amount of columns of the matrix
         */
        public int GetColumns()
        {
            return _data.GetLength(1);
        }

        /*
         * Gets the entire dataset of the matrix
         */
        public float[,] GetData()
        {
            return _data;
        }

        /*
         * Sets the entire dataset of the matrix
         */
        public void SetData(float[,] data)
        {
            _data = data;
        }

        /*
         * Gets on particular number in the datamatrix, based on the row and the column
         */
        public float Get(int row, int column)
        {
            if (row >= 0 && row < _data.GetLength(0) && column >= 0 && column < _data.GetLength(1))
                return _data[row, column];

            return 0;
        }

        /*
         * Sets the data of one particular place in the dataset
         */
        public void Set(int row, int column, float number)
        {
            if (row >= 0 && row < _data.GetLength(0) && column >= 0 && column < _data.GetLength(1))
                _data[row, column] = number;
        }

        /*
         * Adds the data of a vector to the dataset
         */
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

        /*
         * Removes a number from the dataset, based on row and column
         */
        public void Remove(int row, int column)
        {
            if (row >= 0 && row < _data.GetLength(0) && column >= 0 && column < _data.GetLength(1))
                _data[row, column] = 0;
        }

        /*
         * Sets the size of the dataset, keeping the data in the dataset if possible
         */
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

        /*
         * Sets the size of the dataset, completely removing the previous data
         */
        public void SetSizeWithoutData(int rows, int columns)
        {
            _data = new float[rows,columns];
        }

        /*
         * Resets all values and makes identity matrix
         */
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

        /*
         * Resets all values and makes null matrix
         */
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

        /*
         * Translates current dataset over x, y , z. Also checks if translation is three dimensional
         */
        public void Translate(float x, float y, float z = 0, bool threedim = false)
        {

            bool addedHelpRow = false;
            Matrix translation = MatrixFactory.GetTranslationMatrix(x, y, z, threedim);

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

        /*
         * Scales current dataset over x, y, z. Also checks if the dataset contains points instead of vectors
         */
        public void Scale(float x, float y, float z = 0, bool points = true)
        {

            Matrix scaling = MatrixFactory.GetScalingMatrix(this, x, y, z);

            if (points)
                ScalePoints(scaling);
            else
                ScaleVectors(scaling);
                
        }

        /*
         * Scales points of vectors
         */
        public void ScalePoints(Matrix scaling)
        {

            Dictionary<string, List<int>> linkedPoints = new Dictionary<string, List<int>>();

            for (int column = 0; column < GetColumns(); column++)
            {
                string point = _data[0, column] + ":" + _data[1, column] + ":" + _data[2, column];
                if (!linkedPoints.ContainsKey(point))
                    linkedPoints.Add(point, new List<int>());

                linkedPoints[point].Add(column);

            }

            Dictionary<int, Point> modifiedPoints = new Dictionary<int, Point>();

            Matrix a = ConvertToVectors();
            scaling.Multiply(a);

            int vector = 0;

            for (int column = 0; column < GetColumns(); column += 2, vector++)
            {

                float difX = scaling.Get(0, vector) - a.Get(0, vector);
                float difY = scaling.Get(1, vector) - a.Get(1, vector);
                float difZ = scaling.Get(2, vector) - a.Get(2, vector);

                float x1 = _data[0, column];
                float y1 = _data[1, column];
                float z1 = _data[2, column];

                float x2 = _data[0, column + 1];
                float y2 = _data[1, column + 1];
                float z2 = _data[2, column + 1];

                modifiedPoints.Add(column, new Point(x1 - (difX / 2), y1 - (difY / 2), z1 - (difZ / 2)));
                modifiedPoints.Add(column + 1, new Point(x2 + (difX / 2), y2 + (difY / 2), z2 + (difZ / 2)));

            }

            foreach (string key in linkedPoints.Keys)
            {

                string[] str = key.Split(':');
                Point original = new Point(float.Parse(str[0]), float.Parse(str[1]), float.Parse(str[2]));

                float deltaX = 0;
                float deltaY = 0;
                float deltaZ = 0;

                foreach (int column in linkedPoints[key])
                {

                    if (!modifiedPoints.ContainsKey(column))
                        continue;

                    Point p = modifiedPoints[column];

                    deltaX += (p.GetX() - original.GetX());
                    deltaY += (p.GetY() - original.GetY());
                    deltaZ += (p.GetZ() - original.GetZ());
                }

                Point newPoint = new Point(original.GetX() + deltaX, original.GetY() + deltaY, original.GetZ() + deltaZ);

                foreach (int column in linkedPoints[key])
                {

                    _data[0, column] = newPoint.GetX();
                    _data[1, column] = newPoint.GetY();
                    _data[2, column] = newPoint.GetZ();

                }

            }

        }

        /*
         * Scales just vectors
         */
        public void ScaleVectors(Matrix scaling)
        {
            scaling.Multiply(this);
            _data = scaling.GetData();
        }

        /*
         * Used to rotate the dataset
         */
        public void Rotate(float angle, bool threedim, Point p1, Point p2 = null)
        {

            if(threedim)
                Rotate3D(angle, p1, p2);
            else
                Rotate2D(angle, p1);

        }

        /*
         * Rotate a 3D matrix a vector going through the origin, or not
         */
        public void Rotate3D(float angle, Point p1, Point p2 = null)
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

        /*
         * Rotates the dataset over a point, or the origin
         */
        public void Rotate2D(float angle, Point p = null)
        {
            if (p == null)
            {
                Matrix rotation = MatrixFactory.Rotate2D(angle);
                Multiply(rotation);
            }
            else
            {

                Point inverse = new Point(-p.GetX(), -p.GetY(), -p.GetZ());
                Translate(-p.GetX(), -p.GetY(), -p.GetZ(), false);
                Matrix rotation = MatrixFactory.Rotate2D(angle);
                Multiply(rotation);
                Translate(p.GetX(), p.GetY(), p.GetZ(), false);

            }
        }

        /*
         * Gets the highest number in the sequence in the matrix. Used for inversion
         */
        public int GetColumnOfHighestInRow(int column)
        {

            int num = 0;
            for (int i = 0; i < GetRows(); i++)
            {
                if (_data[i, column] > num)
                    num = i;
            }

            return num;

        }

        /*
         * Adds a help row to sustain conventions
         */
        public void AddHelpRow()
        {
            
            SetSizeWithData(GetRows()+1,GetColumns());

            for (int i = 0; i < GetColumns(); i++)
                _data[GetRows()-1, i] = 1;

        }

        /*
         * Removes the help row that sustains conventions
         */
        public void RemoveHelpRow()
        {
            SetSizeWithData(GetRows() - 1, GetColumns());
        }

        /*
         * Switches rowdata in the matrix
         */
        public void SwitchValues(int row1, int row2)
        {

            for (int i = 0; i < GetColumns(); i++)
            {

                float backup = _data[row1, i];
                _data[row1, i] = _data[row2, i];
                _data[row2, i] = backup;

            }

        }

        /*
         * Prints the matrix
         */
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

        /*
         * Inverts the matrix. Cannot be used with points, only vectors
         */
        public void Invert()
        {

            if (GetDeterminant() == 0)
                return;

            Matrix inversion = new Matrix(GetRows(), GetRows());
            inversion.MakeIdentityMatrix();

            if (_data[0, 0] == 0f)
            {
                int row = GetColumnOfHighestInRow(0);
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

        /*
         * Modifies the values in the row
         */
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
         
        /*
         * Determines the determinant of the matrix
         */
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

        /*
         * Dot-product. Multiplies the matrix with another
         */
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

                        result.Set(firstRows, secondColumns, num);

                    }

                }
            }

            _data = result.GetData();

        }

        /*
         * Convert points to vectors
         */
        public Matrix ConvertToVectors()
        {

            Matrix m = new Matrix(GetRows(), GetColumns()/2);
            float[,] data = new float[GetRows(),GetColumns()/2];
            int newColumn = 0;

            for (int column = 0; column < GetColumns(); column+=2, newColumn++)
            {

                float x = _data[0, column + 1] - _data[0, column];
                float y = _data[1, column + 1] - _data[1, column];
                float z = _data[2, column + 1] - _data[2, column];

                data[0, newColumn] = x;
                data[1, newColumn] = y;
                data[2, newColumn] = z;
                data[3, newColumn] = 1;

            }

            m.SetData(data);

            return m;
        }

        /*
         * Gets the middle point of a matrix
         */
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

        /*
         * Get the middle of the furthest and nearest X coordinate
         */
        public float GetWidth()
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

                if (!existsInList)
                    points.Add(new Point(x, y, z));

            }

            float x1 = float.MinValue;
            float x2 = float.MaxValue;

            foreach (Point p in points)
            {

                if (p.GetX() > x1)
                    x1 = p.GetX();
                else if (p.GetX() < x2)
                    x2 = p.GetX();

            }

            return x2 - x1;

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
