using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{
    class Perspective
    {

        private float _near, _far, _fieldOfView;

        public Perspective(float near, float far)
        {

            _near = near;
            _far = far;
            _fieldOfView = 90;

        }

        public void SetFieldOfView(float fieldOfView)
        {
            this._fieldOfView = fieldOfView;
        }

        public float GetFieldOfView()
        {
            return this._fieldOfView;
        }

        public void SetFar(float far)
        {
            this._far = far;
        }

        public float GetFar()
        {
            return this._far;
        }

        public void SetNear(float near)
        {
            this._near = near;
        }

        public float GetNear()
        {
            return this._near;
        }

        public float GetScale()
        {
            var rad = GonioFactory.DegreesToRadians(_fieldOfView);
            return (_near*GonioFactory.GetTrigonometricByRadians(rad*0.5f, Trigonometric.Tangent));
        }

        public Matrix Get()
        {
            
            Matrix m = new Matrix(4,4);
            float[,] data =
            {
                {
                    GetScale(), 0, 0, 0
                },
                {
                    0, GetScale(), 0, 0
                },
                {
                    0, 0, -_far/(_far - _near), -1
                },
                {
                    0, 0, (-_far*_near)/(_far - _near), 0
                }
            };

            m.SetData(data);

            return m;


        }


    }
}
