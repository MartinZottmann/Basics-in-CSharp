using OpenTK;

namespace MartinZottmann.Math
{
    class Quad
    {
        public static Quad Zero { get { return new Quad(); } }

        public Vector3d[] points = new Vector3d[] {
            Vector3d.Zero,
            Vector3d.Zero,
            Vector3d.Zero,
            Vector3d.Zero
        };

        public Vector3d this[int i]
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

        public Quad(Vector3d[] points)
        {
            this.points = points;
        }
    }
}
