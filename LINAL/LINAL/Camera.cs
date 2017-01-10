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

        /*
         * Initializes the camera and creates the vectors for the first time
         */
        public Camera()
        {
           
            SetEye(0, 200, 200);
            SetLookAtPoint(0, 0, 0);
            SetUp(0, 1, 0);

            SetVectors();

        }

        /*
         * Initializes the Eye position
         */
        public void SetEye(float x, float y, float z)
        {
            _eye = (new Point(x, y, z)).MakeVector();
            _eye.SetHelp(1);
        }

        /*
         * Initializes the LookAt position
         */
        public void SetLookAtPoint(float x, float y, float z)
        {
            _lookAt = (new Point(x, y, z)).MakeVector();
            _lookAt.SetHelp(1);
        }

        /*
         * Initializes the Up position
         */ 
        public void SetUp(float x, float y, float z)
        {
            _up = (new Point(x, y, z)).MakeVector();
            _up.SetHelp(1);
        }

        /*
         * Rebuilds the camera vectors 
         */
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

        /*
         * Returns the camera as a matrix. Uses all updated values
         */
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

    }
}
