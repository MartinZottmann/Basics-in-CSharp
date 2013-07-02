using OpenTK;
using System;

namespace MartinZottmann.Engine.Physics
{
    public struct AABB3d
    {
        public Vector3d Min;

        public Vector3d Max;

        public bool Intersect(ref Vector3d position, ref AABB3d b, ref  Vector3d b_position)
        {
            if (Max.X + position.X < b.Min.X + b_position.X) return false;
            if (Min.X + position.X > b.Max.X + b_position.X) return false;
            if (Max.Y + position.Y < b.Min.Y + b_position.Y) return false;
            if (Min.Y + position.Y > b.Max.Y + b_position.Y) return false;
            if (Max.Z + position.Z < b.Min.Z + b_position.Z) return false;
            if (Min.Z + position.Z > b.Max.Z + b_position.Z) return false;
            return true;
        }

        public bool Intersect(ref AABB3d b)
        {
            if (Max.X < b.Min.X) return false;
            if (Min.X > b.Max.X) return false;
            if (Max.Y < b.Min.Y) return false;
            if (Min.Y > b.Max.Y) return false;
            if (Max.Z < b.Min.Z) return false;
            if (Min.Z > b.Max.Z) return false;
            return true;
        }

        public bool Intersect(ref Vector3d point, ref Vector3d position)
        {
            Vector3d point_world;
            Vector3d.Add(ref point, ref position, out point_world);
            return point_world.X >= Min.X && point_world.X <= Max.X
                && point_world.Y >= Min.Y && point_world.Y <= Max.Y
                && point_world.Z >= Min.Z && point_world.Z <= Max.Z;
        }

        public bool Intersect(ref Ray3d r, ref Vector3d position)
        {
            double distance_min;
            double distance_max;
            return Intersect(ref r, ref position, out distance_min, out distance_max);
        }

        public bool Intersect(ref Ray3d r, ref Vector3d position, out double distance_min, out double distance_max)
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
