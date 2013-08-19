using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Game.Entities.Components;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.Nodes
{
    public class PhysicNode : Node
    {
        public Base Base;

        public Physic Physic;

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

        public void OnCollision(Collision collision)
        {
            if (this != collision.Object0)
                throw new Exception();

            var o0 = (PhysicNode)collision.Object0;
            var o1 = (PhysicNode)collision.Object1;
            var i0 = o0.InverseInertiaWorld;
            var i1 = o1.InverseInertiaWorld;
            var r0 = collision.HitPoint - o0.Base.Position;
            var r1 = collision.HitPoint - o1.Base.Position;
            var p0 = o0.Physic;
            var p1 = o1.Physic;
            var v0 = p0.Velocity + Vector3d.Cross(p0.AngularVelocity, r0);
            var v1 = p1.Velocity + Vector3d.Cross(p1.AngularVelocity, r1);
            var dv = v0 - v1;
            var n = collision.Normal.Normalized();

            if (-Vector3d.Dot(dv, n) < -0.01)
                return;

            #region NORMAL Impulse
            {
                var kn = p0.InverseMass
                    + p1.InverseMass
                    + Vector3d.Dot(
                        n,
                        Vector3d.Cross(Vector3d.Cross(r0, n) * i0, r0)
                        + Vector3d.Cross(Vector3d.Cross(r1, n) * i1, r1)
                    );
                var dvn = Vector3d.Dot(dv, n);
                var P = Math.Max(-dvn / kn, 0);
                var Pn = P * n;

                o0.AddImpulse(r0, Pn);
                o1.AddImpulse(r1, -Pn);
            }
            #endregion

            #region TANGENT Impulse
            {
                var tangent = (dv - Vector3d.Dot(dv, n) * n);
                var tl = tangent.Length;
                if (tl == 0)
                    return;
                tangent /= tl;
                var kt = p0.InverseMass
                    + p1.InverseMass
                    + Vector3d.Dot(
                        tangent,
                        Vector3d.Cross(Vector3d.Cross(r0, tangent) * i0, r0)
                        + Vector3d.Cross(Vector3d.Cross(r1, tangent) * i1, r1)
                    );
                var dvt = Vector3d.Dot(dv, tangent);
                var P = -dvt / kt;
                var Pt = P * tangent;

                o0.AddImpulse(r0, Pt);
                o1.AddImpulse(r1, -Pt);
            }
            #endregion
        }
    }
}
