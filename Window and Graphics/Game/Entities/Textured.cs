using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    class Textured : Physical
    {
        public Textured(ResourceManager resources)
            : base(resources)
        {
            var shape = new Quad();
            shape.Translate(Matrix4.Scale(2) * Matrix4.CreateRotationX(-MathHelper.PiOver2));

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Mesh(shape);
            graphic.Program = Resources.Programs["plain_texture"];
            graphic.Texture = Resources.Textures["res/textures/pointer.png"];

            graphic.Program.UniformLocations["Texture"].Set(0);

            BoundingBox = shape.BoundingBox;
            BoundingSphere = shape.BoundingSphere;
        }

        public override void Render(double delta_time)
        {
            graphic.Program.UniformLocations["PVM"].Set(RenderContext.ProjectionViewModel);
            graphic.Draw();

#if DEBUG
            var P = RenderContext.Projection;
            var V = RenderContext.View;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref P);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref V);

            GL.LineWidth(5);
            GL.Begin(BeginMode.Lines);
            {
                GL.Color4(new Color4(0, 255, 255, (byte)127));
                GL.Vertex3(Position);
                GL.Vertex3(Target);

                GL.Color4(new Color4(255, 0, 255, (byte)127));
                GL.Vertex3(Position);
                GL.Vertex3(Position + Velocity);

                var angular_velocity_x = new Vector3d(AngularVelocity.X, 0, 0);
                angular_velocity_x = Vector3d.Cross(angular_velocity_x, Up);
                Vector3d.Transform(ref angular_velocity_x, ref Orientation, out angular_velocity_x);

                var angular_velocity_y = new Vector3d(0, AngularVelocity.Y, 0);
                angular_velocity_y = Vector3d.Cross(angular_velocity_y, Forward);
                Vector3d.Transform(ref angular_velocity_y, ref Orientation, out angular_velocity_y);

                var angular_velocity_z = new Vector3d(0, 0, AngularVelocity.Z);
                angular_velocity_z = Vector3d.Cross(angular_velocity_z, Right);
                Vector3d.Transform(ref angular_velocity_z, ref Orientation, out angular_velocity_z);

                GL.Color4(new Color4(255, 0, 0, (byte)127));
                GL.Vertex3(Position);
                GL.Vertex3(Position + ForwardRelative);

                GL.Vertex3(Position + ForwardRelative);
                GL.Vertex3(Position + ForwardRelative + angular_velocity_y);

                GL.Color4(new Color4(0, 255, 0, (byte)127));
                GL.Vertex3(Position);
                GL.Vertex3(Position + UpRelative);

                GL.Vertex3(Position + UpRelative);
                GL.Vertex3(Position + UpRelative + angular_velocity_x);

                GL.Color4(new Color4(0, 0, 255, (byte)127));
                GL.Vertex3(Position);
                GL.Vertex3(Position + RightRelative);

                GL.Vertex3(Position + RightRelative);
                GL.Vertex3(Position + RightRelative + angular_velocity_z);
            }
            GL.End();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            RenderVelocity(delta_time);
            RenderBoundingBox(delta_time);
#endif
        }
    }
}
