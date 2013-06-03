using OpenTK;
using System;

namespace MartinZottmann
{
    class Polygon
    {
        public static Polygon Zero { get { return new Polygon(); } }

        public Vector2d[] points;

        public Vector2d this[int i]
        {
            get
            {
                return points[i];
            }
            set
            {
                points[i] = value;
            }
        }

        public Polygon() { }

        public Polygon(Vector2d[] points)
        {
            this.points = points;
        }
    }
}
