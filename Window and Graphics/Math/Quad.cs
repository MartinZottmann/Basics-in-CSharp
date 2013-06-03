using OpenTK;
using System;

namespace MartinZottmann.Math
{
    class Quad
    {
        public static Quad Zero { get { return new Quad(); } }

        public Vector2d[] points = new Vector2d[] {
            Vector2d.Zero,
            Vector2d.Zero,
            Vector2d.Zero,
            Vector2d.Zero
        };

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

        public Quad() { }

        public Quad(Vector2d[] points)
        {
            this.points = points;
        }
    }
}
