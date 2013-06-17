﻿using MartinZottmann.Engine.Physics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    public class Physical : Entity
    {
        public bool Mark { get; set; }

        public AABB3d BoundingBox;

        public Vector3d Velocity = Vector3d.Zero;

        public double Angle = 0;

        public double AngleVelocity = 10;

        public Physical(Resources resources) : base(resources) { }

        public override void Update(double delta_time)
        {
            Angle += AngleVelocity * delta_time;
            if (Angle > 359)
                Angle -= 360;
            else if (Angle < 0)
                Angle += 360;

            Position += Velocity * delta_time;
            Velocity += new Vector3d(
                (randomNumber.NextDouble() - 0.5) * 10.0 * delta_time,
                (randomNumber.NextDouble() - 0.5) * 10.0 * delta_time,
                (randomNumber.NextDouble() - 0.5) * 10.0 * delta_time
            );
        }

#if DEBUG
        public override void Render(double delta_time)
        {
            base.Render(delta_time);
            RenderVelocity(delta_time);
        }

        public virtual void RenderVelocity(double delta_time)
        {
            GL.PushMatrix();
            {
                GL.PointSize(1);
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
            }
            GL.PopMatrix();
        }

        public void RenderBoundingBox()
        {
            var P = RenderContext.Projection;
            var V = RenderContext.View;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref P);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref V);

            GL.Translate(Position);
            GL.LineWidth(3);
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
