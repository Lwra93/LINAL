using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINAL
{
    class GonioFactory
    {

        /*
         * Returns the Arctrigonometric of a function, based on degrees
         */
        public static float GetArcTrigonometricByDegrees(float value1, float value2, Trigonometric tri)
        {
            float radians = GetArcTrigonometricByRadians(value1, value2, tri);
            return RadiansToDegrees(radians);
        }

        /*
         * Returns the Arctrigonometric of a function, based on radians
         */
        public static float GetArcTrigonometricByRadians(float value1, float value2, Trigonometric tri)
        {
            double result = value1 / value2;

            if(tri == Trigonometric.Sine)
                return (float)Math.Asin(result);
            if (tri == Trigonometric.Cosine)
                return (float)Math.Acos(result);
            if (tri == Trigonometric.Tangent)
                return (float)Math.Atan(result);
            if (tri == Trigonometric.Tangent2)
                return (float) Math.Atan2(value1,value2);
            else
                return (float) Trigonometric.Undefined;
        }

        /*
         * Converts degrees to radians
         */
        public static float DegreesToRadians(float degrees)
        {
            return (float)(degrees*(Math.PI/180.0));
        }

        /*
         * Converts radians to degrees
         */
        public static float RadiansToDegrees(float radians)
        {
            return (float)(radians / Math.PI * 180);
        }

        /*
         * Returns the trigonometric of a function, based on radians
         */
        public static float GetTrigonometricByRadians(float radians, Trigonometric tri)
        {
            if (tri == Trigonometric.Sine)
                return (float)Math.Sin(radians);
            if (tri == Trigonometric.Cosine)
                return (float)Math.Cos(radians);
            if (tri == Trigonometric.Tangent)
                return (float)Math.Tan(radians);
            else
                return (float)Trigonometric.Undefined;
        }

        /*
         * Returns the trigonometric of a function, based on degrees
         */
        public static float GetTrigonometricByDegrees(float degrees, Trigonometric tri)
        {

            float radians = DegreesToRadians(degrees);
            return GetTrigonometricByRadians(radians, tri);

        }

    }

    /*
     * Enumeration for trigonometric functions
     */
    public enum Trigonometric
    {
        Undefined = 0,
        Sine = 1,
        Cosine = 2,
        Tangent = 3,
        Tangent2 = 4
    }
}
