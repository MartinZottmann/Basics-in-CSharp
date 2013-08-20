using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Game.Entities.Components;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.Nodes
{
    public class PhysicNode : Node
    {
        public BaseComponent Base;

        public PhysicComponent Physic;

        public Matrix4d InertiaWorld { get { return Base.OrientationMatrix * Physic.Inertia * Base.InverseOrientationMatrix; } }

        public Matrix4d InverseInertiaWorld { get { return Base.OrientationMatrix * Physic.InverseInertia * Base.InverseOrientationMatrix; } }

        //public void AddForceRelative(Vector3d point, Vector3d force)
        //{
        //    Vector3d.Transform(ref point, ref GameObject.Orientation, out point);
        //    Vector3d.Transform(ref force, ref GameObject.Orientation, out force);

        //    Force += force;
        //    Torque += Vector3d.Cross(point, force);
        //}

        public void AddForce(Vector3d point, Vector3d force)
        {
            Physic.Force += force;
#if DEBUG
            if (Double.IsNaN(Physic.Force.X))
                throw new Exception();
#endif
            Physic.Torque += Vector3d.Cross(point, force);
#if DEBUG
            if (Double.IsNaN(Physic.Torque.X))
                throw new Exception();
#endif
        }

        public void AddImpulse(Vector3d point, Vector3d force)
        {
            Physic.Velocity += force * Physic.InverseMass;
#if DEBUG
            if (Double.IsNaN(Physic.Velocity.X))
                throw new Exception();
#endif
            Physic.AngularVelocity += Vector3d.Cross(point, force) * InverseInertiaWorld;
#if DEBUG
            if (Double.IsNaN(Physic.AngularVelocity.X))
                throw new Exception();
#endif
        }

        public Vector3d PointVelocity(Vector3d point)
        {
            return Vector3d.Cross(Physic.AngularVelocity, point) + Physic.Velocity;
        }

        public void UpdateVelocity(double delta_time)
        {
            Physic.Velocity += Physic.Force * Physic.InverseMass * delta_time;
#if DEBUG
            if (Double.IsNaN(Physic.Velocity.X))
                throw new Exception();
#endif

            Physic.AngularVelocity += Physic.Torque * Physic.InverseInertia * delta_time;
#if DEBUG
            if (Double.IsNaN(Physic.AngularVelocity.X))
                throw new Exception();
#endif

            //// Damping
            //const double damping = 0.98;
            //Velocity *= System.Math.Pow(damping, delta_time);
            //AngularVelocity *= System.Math.Pow(damping, delta_time);
        }

        public void UpdatePosition(double delta_time)
        {
            Base.Position += Physic.Velocity * delta_time;
#if DEBUG
            if (Double.IsNaN(Base.Position.X))
                throw new Exception();
#endif

            Physic.Force = Vector3d.Zero;

            Base.Orientation += 0.5 * new Quaterniond(Physic.AngularVelocity * delta_time, 0) * Base.Orientation;
            Base.Orientation.Normalize();
#if DEBUG
            if (Double.IsNaN(Base.Orientation.X))
                throw new Exception();
#endif

            Physic.Torque = Vector3d.Zero;
        }
    }
}
