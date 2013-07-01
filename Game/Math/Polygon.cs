using OpenTK;

namespace MartinZottmann.Math
{
    class Polygon
    {
        public static Polygon Zero { get { return new Polygon(); } }

        public Vector3d[] points;

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

        public Polygon() { }

        public Polygon(Vector3d[] points)
        {
            this.points = points;
        }
    }
}
