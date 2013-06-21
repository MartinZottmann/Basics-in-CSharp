using MartinZottmann.Engine.Physics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    public class Physical : Entity
    {
        public bool Mark { get; set; }

        public Vector3d Scale = new Vector3d(1, 1, 1);

        public Vector3d Offset = Vector3d.Zero;

        public Quaterniond Orientation = Quaterniond.Identity;

        public Vector3d AngularForce = Vector3d.Zero;

        public Vector3d AngularVelocity = Vector3d.Zero;

        /// <summary>
        /// Scale * Rotation * Translation
        /// </summary>
        public Matrix4d Model
        {
            get
            {
                return Matrix4d.Scale(Scale)
                    * Matrix4d.CreateTranslation(Offset)
                    * Matrix4d.Rotate(Orientation)
                    * Matrix4d.CreateTranslation(Position);
            }
        }

        public AABB3d BoundingBox;

        public Vector3d Force = Vector3d.Zero;

        public double Mass = 10;

        public Vector3d Velocity = Vector3d.Zero;

        public Physical(ResourceManager resources) : base(resources) { }

        public override void Update(double delta_time)
        {
            AngularVelocity += (AngularForce / Mass) * delta_time;
            AngularForce = Vector3d.Zero;
            Orientation += new Quaterniond(AngularVelocity.X, AngularVelocity.Y, AngularVelocity.Z, 0) * Orientation * delta_time;
            Orientation.Normalize();

            Velocity += (Force / Mass) * delta_time;
            Force = Vector3d.Zero;
            Position += Velocity * delta_time;

            RenderContext.Model = Model;
        }

#if DEBUG
        public override void Render(double delta_time)
        {
            base.Render(delta_time);
            RenderVelocity(delta_time);
            RenderBoundingBox(delta_time);
        }

        public virtual void RenderVelocity(double delta_time)
        {
            var P = RenderContext.Projection;
            var V = RenderContext.View;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref P);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref V);

            GL.LineWidth(1);
            GL.Begin(BeginMode.Lines);
            {
                #region Velocity
                GL.Color4(color.R, color.G, color.B, (byte)127);
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

        public void RenderBoundingBox(double delta_time)
        {
            var P = RenderContext.Projection;
            var V = RenderContext.View;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref P);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref V);

            GL.Translate(Position);
            GL.LineWidth(1);
            GL.Begin(BeginMode.Lines);
            if (Mark)
                GL.Color3(1f, 1f, 0f);
            else
                GL.Color3(1f, 1f, 1f);
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
            GL.End();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
#endif
    }
}
