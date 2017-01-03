using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{

    class Camera
    {

        private readonly Vector _eye;
        private readonly Vector _lookAt;
        private readonly Vector _up;

        private Vector _x, _y, _z;

        public Camera()
        {
            
            this._eye = new Vector(2,2,2,1);
            this._lookAt = new Vector(1,1,1,1);
            this._up = new Vector(0,1,0,1);

            SetVectors();
        }

        public Matrix Get(int screenSize)
        {
            
            Recalculate(screenSize);
            SetVectors();
            return GetCamera();

        }

        public void Recalculate(int screenSize)
        {

            float eyeX = (float) ((screenSize/2) + ((_eye.GetX() + 1)/_eye.GetHelp())*screenSize*0.5);
            float eyeY = (float)((screenSize / 2) + ((_eye.GetY() + 1) / _eye.GetHelp()) * screenSize * 0.5);
            float eyeZ = -_eye.GetZ();

            _eye.SetX(eyeX);
            _eye.SetY(eyeY);
            _eye.SetZ(eyeZ);

            float lookAtX = (float)((screenSize / 2) + ((_lookAt.GetX() + 1) / _eye.GetHelp()) * screenSize * 0.5);
            float lookAtY = (float)((screenSize / 2) + ((_lookAt.GetY() + 1) / _eye.GetHelp()) * screenSize * 0.5);
            float lookAtZ = -_lookAt.GetZ();

            _lookAt.SetX(eyeX);
            _lookAt.SetY(eyeY);
            _lookAt.SetZ(eyeZ);

            SetVectors();

        }

        public void SetVectors()
        {
            _z = _eye.Subtract(_lookAt);
            _z.MakeUnitVector();

            _y = _up;
            _y.MakeUnitVector();

            _x = _y.GetCrossProduct(_z);
            _x.MakeUnitVector();

            _y = _z.GetCrossProduct(_x);
            _y.MakeUnitVector();
        }

        public Matrix GetCamera()
        {

            Matrix m = new Matrix(4, 4);
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

            m.SetData(data);

            return m;


        }



    }
}
