using OpenTK;
using System;

namespace MartinZottmann.Engine.Physics
{
    public class Collision : IComparable<Collision>
    {
        public Vector3d HitPoint;

        public Vector3d Normal;

        public object Object0;

        public object Object1;

        public object Parent;

        public double PenetrationDepth;

        public int CompareTo(Collision other)
        {
            return HitPoint.Length.CompareTo(other.HitPoint.Length);
        }

        public override string ToString()
        {
            return String.Format(
                "{0}: ({1}, {2}, {3}, {4}, {5}, {6})",
                this.GetType(),
                HitPoint,
                Normal,
                Object0,
                Object1,
                Parent,
                PenetrationDepth
            );
        }
    }
}
