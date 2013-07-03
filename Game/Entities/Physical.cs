using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Helper;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Diagnostics;

namespace MartinZottmann.Game.Entities
{
    public class Physical : Drawable, INavigation
    {
        public OpenTK.Graphics.Color4 Mark { get; set; }

        public Vector3d Target { get; set; }

        public double thrust = 10.0;

        public Vector3d Torque = Vector3d.Zero;

        public Vector3d AngularVelocity = Vector3d.Zero;

        public AABB3d BoundingBox;

        public Sphere3d BoundingSphere;

        public Vector3d Force = Vector3d.Zero;

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

        public Vector3d Velocity = Vector3d.Zero;

        public Matrix4d OrientationMatrix { get; set; }

        public Matrix4d InverseOrientationMatrix { get; set; }

        public Physical(ResourceManager resources) : base(resources) { }

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
            Position += Velocity * delta_time;

            Force = Vector3d.Zero;

            Orientation += 0.5 * new Quaterniond(AngularVelocity * delta_time, 0) * Orientation;
            Orientation.Normalize();

            Torque = Vector3d.Zero;

            UpdateMatrix();
        }

        protected void UpdateMatrix()
        {
            OrientationMatrix = Matrix4d.CreateFromQuaternion(ref Orientation);
            InverseOrientationMatrix = OrientationMatrix.Inverted();
            InertiaWorld = OrientationMatrix * Inertia * InverseOrientationMatrix;
            InverseInertiaWorld = OrientationMatrix * InverseInertia * InverseOrientationMatrix;
        }

        public void AddForceRelative(Vector3d point, Vector3d force)
        {
            Vector3d.Transform(ref point, ref Orientation, out point);
            Vector3d.Transform(ref force, ref Orientation, out force);

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

        public virtual void OnCollision(Collision collision)
        {
            Debug.Assert(this == collision.Object0);

            var o0 = collision.Object0 as Physical;
            var o1 = collision.Object1 as Physical;
            var r0 = collision.HitPoint - o0.Position;
            var r1 = collision.HitPoint - o1.Position;
            var v0 = o0.Velocity + Vector3d.Cross(o0.AngularVelocity, r0);
            var v1 = o1.Velocity + Vector3d.Cross(o1.AngularVelocity, r1);
            var dv = v0 - v1;

            if (-Vector3d.Dot(dv, collision.Normal) < -0.01)
                return;

            #region NORMAL Impulse
            var e = 0.0;
            var normDiv = Vector3d.Dot(collision.Normal, collision.Normal) * (
                (o0.InverseMass + o1.InverseMass)
                + Vector3d.Dot(
                    collision.Normal,
                    Vector3d.Cross(Vector3d.Cross(r0, collision.Normal) * o0.InverseInertiaWorld, r0)
                    + Vector3d.Cross(Vector3d.Cross(r1, collision.Normal) * o1.InverseInertiaWorld, r1)
                )
            );
            var jn = -1 * (1 + e) * Vector3d.Dot(dv, collision.Normal) / normDiv;
            jn += (collision.PenetrationDepth * 1.5);
            var Pn = collision.Normal * jn;

            o0.AddImpulse(r0, Pn);
            o1.AddImpulse(r1, -1 * Pn);
            #endregion

            #region TANGENT Impulse
            var tangent = dv - (Vector3d.Dot(dv, collision.Normal) * collision.Normal);
            tangent.Normalize();
            var k_tangent = o0.InverseMass
                + o1.InverseMass
                + Vector3d.Dot(
                    tangent,
                    Vector3d.Cross(Vector3d.Cross(r0, tangent) * o0.InverseInertiaWorld, r0)
                    + Vector3d.Cross(Vector3d.Cross(r1, tangent) * o1.InverseInertiaWorld, r1)
                );
            var Pt = -1 * Vector3d.Dot(dv, tangent) / k_tangent * tangent;

            o0.AddImpulse(r0, Pt);
            o1.AddImpulse(r1, -1 * Pt);
            #endregion
        }

        public Vector3d PointVelocity(Vector3d point)
        {
            return Vector3d.Cross(AngularVelocity, point) + Velocity;
        }

        public virtual SortedSet<Collision> Intersect(ref Ray3d ray, ref Vector3d position)
        {
            Vector3d position_world;
            Vector3d.Add(ref Position, ref position, out position_world);
            var hits = new SortedSet<Collision>();

            if (!BoundingBox.Intersect(ref ray, ref position_world))
                return hits;

            var collision = BoundingSphere.At(ref position_world).Collides(ref ray);
            if (collision == null)
                return hits;

            hits.Add(
                new Collision()
                {
                    HitPoint = collision.HitPoint,
                    Normal = collision.Normal,
                    Object0 = ray,
                    Object1 = this,
                    PenetrationDepth = collision.PenetrationDepth
                }
            );

            foreach (var child in children)
                if (child is Physical)
                    foreach (var hit in (child as Physical).Intersect(ref ray, ref position_world))
                    {
                        hit.Parent = this;
                        hits.Add(hit);
                    }

            return hits;
        }

#if DEBUG
        public override void RenderHelpers(double delta_time, RenderContext render_context)
        {
            base.RenderHelpers(delta_time, render_context);
            RenderTarget(delta_time, render_context);
            RenderVelocity(delta_time, render_context);
            RenderAngularVelocity(delta_time, render_context);
            RenderBoundingBox(delta_time, render_context);
        }

        public virtual void RenderTarget(double delta_time, RenderContext render_context)
        {
            if (Target.Equals(Vector3d.Zero))
                return;

            var P = render_context.Projection;
            var V = render_context.View;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref P);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref V);

            GL.LineWidth(1);
            GL.Begin(BeginMode.Lines);
            {
                GL.Color4(0.0f, 1.0f, 1.0f, 0.5f);
                GL.Vertex3(Position);
                GL.Vertex3(Target);

                GL.Vertex3(Target - Vector3d.UnitX);
                GL.Vertex3(Target + Vector3d.UnitX);
                GL.Vertex3(Target - Vector3d.UnitY);
                GL.Vertex3(Target + Vector3d.UnitY);
                GL.Vertex3(Target - Vector3d.UnitZ);
                GL.Vertex3(Target + Vector3d.UnitZ);
            }
            GL.End();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        public virtual void RenderVelocity(double delta_time, RenderContext render_context)
        {
            var P = render_context.Projection;
            var V = render_context.View;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref P);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref V);

            GL.LineWidth(1);
            GL.Begin(BeginMode.Lines);
            {
                #region Velocity
                GL.Color4(1.0f, 1.0f, 0.0f, 0.5f);
                GL.Vertex3(Position);
                GL.Vertex3(Position + Velocity);

                GL.Vertex3(Position + Velocity - Vector3d.UnitX);
                GL.Vertex3(Position + Velocity + Vector3d.UnitX);
                GL.Vertex3(Position + Velocity - Vector3d.UnitY);
                GL.Vertex3(Position + Velocity + Vector3d.UnitY);
                GL.Vertex3(Position + Velocity - Vector3d.UnitZ);
                GL.Vertex3(Position + Velocity + Vector3d.UnitZ);
                #endregion

                #region Force
                GL.Color4(1.0f, 0.75f, 0.0f, 0.5f);
                GL.Vertex3(Position + Velocity);
                GL.Vertex3(Position + Velocity + Force);

                GL.Vertex3(Position + Velocity + Force - Vector3d.UnitX);
                GL.Vertex3(Position + Velocity + Force + Vector3d.UnitX);
                GL.Vertex3(Position + Velocity + Force - Vector3d.UnitY);
                GL.Vertex3(Position + Velocity + Force + Vector3d.UnitY);
                GL.Vertex3(Position + Velocity + Force - Vector3d.UnitZ);
                GL.Vertex3(Position + Velocity + Force + Vector3d.UnitZ);
                #endregion

                #region Contact to circle
                GL.Color4(1.0f, 1.0f, 1.0f, 0.2f);
                GL.Vertex3(Position);
                var position_on_y = new Vector3d(Position.X, 0, Position.Z);
                GL.Vertex3(position_on_y);

                double radius = 100;
                Vector3d center = Vector3d.Zero;
                Vector3d difference = position_on_y - center;
                Vector3d contact = center + difference / difference.Length * radius;

                GL.Vertex3(position_on_y);
                GL.Vertex3(contact);
                #endregion
            }
            GL.End();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        public virtual void RenderAngularVelocity(double delta_time, RenderContext render_context)
        {
            var P = render_context.Projection;
            var V = render_context.View;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref P);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref V);

            GL.LineWidth(5);
            GL.Begin(BeginMode.Lines);
            {
                var angular_velocity_x = new Vector3d(AngularVelocity.X, 0, 0);
                angular_velocity_x = Vector3d.Cross(angular_velocity_x, Up);
                Vector3d.Transform(ref angular_velocity_x, ref Orientation, out angular_velocity_x);

                var angular_velocity_y = new Vector3d(0, AngularVelocity.Y, 0);
                angular_velocity_y = Vector3d.Cross(angular_velocity_y, Forward);
                Vector3d.Transform(ref angular_velocity_y, ref Orientation, out angular_velocity_y);

                var angular_velocity_z = new Vector3d(0, 0, AngularVelocity.Z);
                angular_velocity_z = Vector3d.Cross(angular_velocity_z, Right);
                Vector3d.Transform(ref angular_velocity_z, ref Orientation, out angular_velocity_z);

                GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);

                GL.Vertex3(Position + ForwardRelative);
                GL.Vertex3(Position + ForwardRelative + angular_velocity_y);

                GL.Color4(0.0f, 1.0f, 0.0f, 0.5f);

                GL.Vertex3(Position + UpRelative);
                GL.Vertex3(Position + UpRelative + angular_velocity_x);

                GL.Color4(0.0f, 0.0f, 1.0f, 0.5f);

                GL.Vertex3(Position + RightRelative);
                GL.Vertex3(Position + RightRelative + angular_velocity_z);
            }
            GL.End();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        public virtual void RenderBoundingBox(double delta_time, RenderContext render_context)
        {
            var P = render_context.Projection;
            var V = render_context.View;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref P);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref V);

            GL.Translate(Position);
            GL.LineWidth(1);
            GL.Begin(BeginMode.Lines);
            {
                GL.Color4(Mark.R, Mark.G, Mark.B, Mark.A);
                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Max.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z);

                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Max.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z);

                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Max.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z);
            }
            GL.End();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
#endif
    }
}
