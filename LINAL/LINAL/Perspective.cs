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

        /*
         * Creates a new pespective
         */
        public Perspective(float near, float far)
        {

            _near = near;
            _far = far;
            _fieldOfView = 90;

        }

        /*
         * Sets the field of view
         */
        public void SetFieldOfView(float fieldOfView)
        {
            this._fieldOfView = fieldOfView;
        }

        /*
         * Returns the field of view
         */
        public float GetFieldOfView()
        {
            return this._fieldOfView;
        }

        /*
         * Sets the far value
         */
        public void SetFar(float far)
        {
            this._far = far;
        }

        /*
         * Returns the far value
         */
        public float GetFar()
        {
            return this._far;
        }

        /*
         * Sets the near value
         */
        public void SetNear(float near)
        {
            this._near = near;
        }

        /*
         * Returns the near value
         */
        public float GetNear()
        {
            return this._near;
        }

        /*
         * Calculates the scale based on the fieldofview and the near variable
         */
        public float GetScale()
        {
            var rad = GonioFactory.DegreesToRadians(_fieldOfView);
            return (_near*GonioFactory.GetTrigonometricByRadians(rad*0.5f, Trigonometric.Tangent));
        }

        /*
         * Returns the Pespective matrix
         */
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

        /*
         * Calculates the pespective screensize
         */

        public float GetScreenSize()
        {

            var tan = GonioFactory.GetTrigonometricByDegrees(_fieldOfView/2, Trigonometric.Tangent);
            var halfSize = tan*_far;
            return halfSize*2;

        }

    }
}
