using OpenTK;
using System;

namespace MartinZottmann.Engine.Physics
{
    public struct AABB3d
    {
        public Vector3d Min;

        public Vector3d Max;

        public bool Intersect(Vector3d point, Vector3d position)
        {
            Vector3d.Add(ref point, ref position, out point);
            return point.X >= Min.X && point.X <= Max.X
                && point.Y >= Min.Y && point.Y <= Max.Y
                && point.Z >= Min.Z && point.Z <= Max.Z;
        }

        public bool Intersect(ref Ray3d r, Vector3d position)
        {
            double distance_min;
            double distance_max;
            return Intersect(ref r, position, out distance_min, out distance_max);
        }

        public bool Intersect(ref Ray3d r, Vector3d position, out double distance_min, out double distance_max)
        {
            distance_min = Double.MinValue;
            distance_max = Double.MaxValue;
            double min, max, min_y, max_y, min_z, max_z;
            min = ((r.Sign[0] ? Max : Min).X + position.X - r.Origin.X) * r.DirectionFraction.X;
            max_y = ((r.Sign[1] ? Min : Max).Y + position.Y - r.Origin.Y) * r.DirectionFraction.Y;
            if (min > max_y)
                return false;
            max = ((r.Sign[0] ? Min : Max).X + position.X - r.Origin.X) * r.DirectionFraction.X;
            min_y = ((r.Sign[1] ? Max : Min).Y + position.Y - r.Origin.Y) * r.DirectionFraction.Y;
            if (min_y > max)
                return false;
            if (min_y > min)
                min = min_y;
            if (max_y < max)
                max = max_y;
            min_z = ((r.Sign[2] ? Max : Min).Z + position.Z - r.Origin.Z) * r.DirectionFraction.Z;
            max_z = ((r.Sign[2] ? Min : Max).Z + position.Z - r.Origin.Z) * r.DirectionFraction.Z;
            if (min > max_z || min_z > max)
                return false;
            if (min_z > min)
                min = min_z;
            if (max_z < max)
                max = max_z;
            if (min > distance_min)
                distance_min = min;
            if (max < distance_max)
                distance_max = max;
            return true;
        }
    }
}
