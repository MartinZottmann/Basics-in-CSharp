using System;
using System.Drawing;

namespace MartinZottmann
{
    public class Vector2
    {
        public float X { get; set; }

        public float Y { get; set; }

        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        public Vector2() : this(0, 0) { }

        public Vector2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public float Distance(Vector2 other)
        {
            return (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        public float DistanceSquared(Vector2 other)
        {
            return (float)(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        /*
         * If > 0, the angle between the two vectors is less than 90 degrees
         * If < 0, the angle between the two vectors is more than 90 degrees
         * If == 0, the angle between the two vectors is 90 degrees; that is, the vectors are orthogonal
         * If == 1, the angle between the two vectors is 0 degrees; that is, the vectors point in the same direction and are parallel
         * If == -1, the angle between the two vectors is 180 degrees; that is, the vectors point in opposite directions and are parallel
         */
        public float Dot(Vector2 other)
        {
            return X * other.X + Y * other.Y;
        }

        public Vector2 Normalize()
        {
            Vector2 vector = new Vector2();
            double length = this.Length;
            if (length != 0)
            {
                vector.X = (float)(X / length);
                vector.Y = (float)(Y / length);
            }
            return vector;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator *(Vector2 v, float length)
        {
            return new Vector2(v.X * length, v.Y * length);
        }
    }
}
