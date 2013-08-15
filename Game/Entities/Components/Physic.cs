using MartinZottmann.Engine.Physics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MartinZottmann.Game.Entities.Components
{
    public class Physic : Abstract
    {
        public double thrust = 10.0;

        public Vector3d Torque = Vector3d.Zero;

        public Vector3d AngularVelocity = Vector3d.Zero;

        public AABB3d BoundingBox;

        public Sphere3d BoundingSphere;

        public Vector3d Force = Vector3d.Zero;

        public Vector3d Velocity = Vector3d.Zero;

        public Matrix4d OrientationMatrix { get; set; }

        public Matrix4d InverseOrientationMatrix { get; set; }

        #region Mass

        protected double mass = 10.0;

        public double Mass { get { return mass; } set { mass = value; inverse_mass = 1.0 / value; } }

        protected double inverse_mass = 1.0 / 10.0;

        public double InverseMass { get { return inverse_mass; } set { inverse_mass = value; mass = 1.0 / value; } }

        #endregion

        #region Inertia

        protected Matrix4d inertia = Matrix4d.Identity;

        protected Matrix4d inverse_inertia = Matrix4d.Identity.Inverted();

        protected Matrix4d inertia_world;

        protected Matrix4d inverse_inertia_world;

        public Matrix4d Inertia { get { return inertia; } set { inertia = value; inverse_inertia = value.Inverted(); } }

        public Matrix4d InverseInertia { get { return inverse_inertia; } set { inverse_inertia = value; inertia = value.Inverted(); } }

        public Matrix4d InertiaWorld { get { return inertia_world; } set { inertia_world = value; } }

        public Matrix4d InverseInertiaWorld { get { return inverse_inertia_world; } set { inverse_inertia_world = value; } }

        #endregion

        public Physic(GameObject game_object) : base(game_object) { }

        public virtual void UpdateVelocity(double delta_time)
        {
            Velocity += Force * InverseMass * delta_time;

            AngularVelocity += Torque * InverseInertia * delta_time;

            //// Damping
            //const double damping = 0.98;
            //Velocity *= System.Math.Pow(damping, delta_time);
            //AngularVelocity *= System.Math.Pow(damping, delta_time);

            UpdateMatrix();
        }

        public virtual void UpdatePosition(double delta_time)
        {
            GameObject.Position += Velocity * delta_time;

            Force = Vector3d.Zero;

            GameObject.Orientation += 0.5 * new Quaterniond(AngularVelocity * delta_time, 0) * GameObject.Orientation;
            GameObject.Orientation.Normalize();

            Torque = Vector3d.Zero;

            UpdateMatrix();
        }

        protected void UpdateMatrix()
        {
            OrientationMatrix = Matrix4d.CreateFromQuaternion(ref GameObject.Orientation);
            InverseOrientationMatrix = OrientationMatrix.Inverted();
            InertiaWorld = OrientationMatrix * Inertia * InverseOrientationMatrix;
            InverseInertiaWorld = OrientationMatrix * InverseInertia * InverseOrientationMatrix;
        }

        public void AddForceRelative(Vector3d point, Vector3d force)
        {
            Vector3d.Transform(ref point, ref GameObject.Orientation, out point);
            Vector3d.Transform(ref force, ref GameObject.Orientation, out force);

            Force += force;
            Torque += Vector3d.Cross(point, force);
        }

        public void AddForce(Vector3d point, Vector3d force)
        {
            Force += force;
            Torque += Vector3d.Cross(point, force);
        }

        public void AddImpulse(Vector3d point, Vector3d force)
        {
            Velocity += force * InverseMass;
            AngularVelocity += Vector3d.Cross(point, force) * InverseInertiaWorld;
        }

        public Vector3d PointVelocity(Vector3d point)
        {
            return Vector3d.Cross(AngularVelocity, point) + Velocity;
        }

        public virtual SortedSet<Collision> Intersect(ref Ray3d ray, ref Matrix4d model_parent)
        {
            var game_object = GameObject;

            Matrix4d model_world = game_object.Model * model_parent;
            var hits = new SortedSet<Collision>();

            //if (!Physic.BoundingBox.Intersect(ref ray, ref position_world))
            //    return hits;

            var collision = BoundingSphere.At(ref model_world).Collides(ref ray);
            if (collision == null)
                return hits;

            foreach (var child in game_object.Children)
                if (child.HasComponent<Physic>())
                    foreach (var hit in child.GetComponent<Physic>().Intersect(ref ray, ref model_world))
                    {
                        hit.Parent = GameObject;
                        hits.Add(hit);
                    }

            var best = hits.Min;
            hits.Clear();
            hits.Add(
                new Collision()
                {
                    HitPoint = collision.HitPoint,
                    Normal = collision.Normal,
                    Object0 = ray,
                    Object1 = GameObject,
                    PenetrationDepth = collision.PenetrationDepth
                }
            );
            if (best != null)
                hits.Add(best);

            return hits;
        }

        public virtual void OnCollision(Collision collision)
        {
            Debug.Assert(GameObject == collision.Object0);

            var o0 = (GameObject)collision.Object0;
            var o1 = (GameObject)collision.Object1;
            var r0 = collision.HitPoint - o0.Position;
            var r1 = collision.HitPoint - o1.Position;
            var p0 = o0.GetComponent<Physic>();
            var p1 = o1.GetComponent<Physic>();
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
                        Vector3d.Cross(Vector3d.Cross(r0, n) * p0.InverseInertiaWorld, r0)
                        + Vector3d.Cross(Vector3d.Cross(r1, n) * p1.InverseInertiaWorld, r1)
                    );
                var dvn = Vector3d.Dot(dv, n);
                var P = Math.Max(-dvn / kn, 0);
                var Pn = P * n;

                p0.AddImpulse(r0, Pn);
                p1.AddImpulse(r1, -Pn);
            }
            #endregion

            #region TANGENT Impulse
            {
                var tangent = dv - (Vector3d.Dot(dv, n) * n).Normalized();
                var kt = p0.InverseMass
                    + p1.InverseMass
                    + Vector3d.Dot(
                        tangent,
                        Vector3d.Cross(Vector3d.Cross(r0, tangent) * p0.InverseInertiaWorld, r0)
                        + Vector3d.Cross(Vector3d.Cross(r1, tangent) * p1.InverseInertiaWorld, r1)
                    );
                var dvt = Vector3d.Dot(dv, tangent);
                var P = -dvt / kt;
                var Pt = P * tangent;

                p0.AddImpulse(r0, Pt);
                p1.AddImpulse(r1, -Pt);
            }
            #endregion
        }
    }
}
