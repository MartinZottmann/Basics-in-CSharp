using OpenTK;

namespace MartinZottmann.Engine.Physics
{
    public class Plane3d
    {
        public Vector3d Origin { get; protected set; }

        public Vector3d Normal { get; protected set; }

        public Plane3d(Vector3d origin, Vector3d normal)
        {
            Origin = origin;
            Normal = normal;
        }
    }
}