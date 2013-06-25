using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Helper;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    public class Physical : Drawable, INavigation
    {
        public OpenTK.Graphics.Color4 Mark { get; set; }

        public Vector3d Target { get; set; }

        public double thrust = 1000;

        public Vector3d AngularForce = Vector3d.Zero;

        public Vector3d AngularVelocity = Vector3d.Zero;

        public AABB3d BoundingBox;

        public Sphere3d BoundingSphere;

        public Vector3d Force = Vector3d.Zero;

        public double Mass = 10;

        public Vector3d Velocity = Vector3d.Zero;

        public Physical(ResourceManager resources) : base(resources) { }

        public override void Update(double delta_time, RenderContext render_context)
        {
            AngularVelocity += (AngularForce / Mass) * delta_time;
            AngularForce = Vector3d.Zero;
            Orientation += 0.5 * new Quaterniond(AngularVelocity.X, AngularVelocity.Y, AngularVelocity.Z, 0) * Orientation * delta_time;
            Orientation.Normalize();

            Velocity += (Force / Mass) * delta_time;
            Force = Vector3d.Zero;
            Position += Velocity * delta_time;

            // Damping
            const double damping = 0.98;
            Velocity *= System.Math.Pow(damping, delta_time);
            AngularVelocity *= System.Math.Pow(damping, delta_time);
        }

        public void AddForceRelative(Vector3d point, Vector3d force)
        {
            Vector3d.Transform(ref point, ref Orientation, out point);
            Vector3d.Transform(ref force, ref Orientation, out force);

            Force += force;
            AngularForce += Vector3d.Cross(point, force);
        }

        public void AddForce(Vector3d point, Vector3d force)
        {
            Force += force;
            AngularForce += Vector3d.Cross(point, force);
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
                #endregion

                #region Contact to circle
                GL.Color4(1, 1, 1, 0.2);
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
