using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{

    class Camera
    {

        private Vector _eye;
        private Vector _lookAt;
        private Vector _up;

        private Vector _x, _y, _z;

        public Camera()
        {
            
            Point eyePoint = new Point(45, 45, 0);
            Point lookAtPoint = new Point(45, 45,-45);
            Point upPoint = new Point(0,1,0);

            this._eye = eyePoint.MakeVector();
            _eye.SetHelp(1);
            this._lookAt = lookAtPoint.MakeVector();
            _lookAt.SetHelp(1);
            this._up = upPoint.MakeVector();
            _up.SetHelp(1);

            SetVectors();

        }

        public void turn(float alpha)
        {
            
            Matrix m = new Matrix(4, 1);
            float[,] data1 =
            {
                {
                    _lookAt.GetX()
                },
                {
                    _lookAt.GetY()
                },
                {
                    _lookAt.GetZ()
                },
                {
                    1
                }
            };

            m.SetData(data1);

            m.Rotate3D(alpha, new Point(_eye.GetX(), _eye.GetY(), _eye.GetZ()));

            Point p = new Point(m.Get(0, 0), m.Get(1, 0), m.Get(2, 0));
            this._lookAt = p.MakeVector();
            SetVectors();

        }

        public void SetVectors()
        {
            _z = _eye.Subtract(_lookAt);
            _y = _up;
            _x = _y.GetCrossProduct(_z);
            _y = _z.GetCrossProduct(_x);

            _x.MakeUnitVector();
            _y.MakeUnitVector();
            _z.MakeUnitVector();
        }

        public Matrix Get()
        {

            Matrix cameraMatrix = new Matrix(4, 4);
            float[,] data =
            {
                {
                    _x.GetX(), _x.GetY(), _x.GetZ(), -_x.GetInproduct(_eye)
                },
                {
                    _y.GetX(), _y.GetY(), _y.GetZ(), -_y.GetInproduct(_eye)
                },
                {
                    _z.GetX(), _z.GetY(), _z.GetZ(), -_z.GetInproduct(_eye)
                },
                {
                    0, 0, 0, 1
                }
            };

            cameraMatrix.SetData(data);

            return cameraMatrix;


        }

        public Point GetEyePoint()
        {
            
            return new Point(_eye.GetX(), _eye.GetY(), _eye.GetZ());

        }



    }
}
