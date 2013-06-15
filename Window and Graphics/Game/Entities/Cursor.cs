using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    public class Cursor : Entity
    {
        Engine.Graphics.OpenGL.Entity graphic;

        Matrix4d Model = Matrix4d.Identity;

        public Cursor(Resources resources)
            : base(resources)
        {
            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Add(
                new Mesh<VertexP3C4, uint>(
                    new VertexP3C4[] {
                        new VertexP3C4(-1, 0, 0, 1, 1, 1, 1),
                        new VertexP3C4(0, 0, 1, 1, 0, 1, 1),
                        new VertexP3C4(1, 0, 0, 1, 1, 1, 1),
                        new VertexP3C4(0, 0, -1, 0, 1, 1, 1)
                    },
                    new uint[] { 0, 1, 2, 3 }
                )
            );
            graphic.mode = BeginMode.Quads;
            graphic.Program = Resources.Programs["normal"];
        }

        public override void Render(double delta_time)
        {
            RenderContext.Model = Model;
            Resources.Programs["normal"].UniformLocations["PVM"].Set(RenderContext.ProjectionViewModel);

            //graphic.Draw();

            var start = new Vector4d(
                (RenderContext.Window.Mouse.X / (double)RenderContext.Window.Width - 0.5) * 2.0,
                ((RenderContext.Window.Height - RenderContext.Window.Mouse.Y) / (double)RenderContext.Window.Height - 0.5) * 2.0,
                -1.0,
                1.0
            );
            var end = new Vector4d(
                (RenderContext.Window.Mouse.X / (double)RenderContext.Window.Width - 0.5) * 2.0,
                ((RenderContext.Window.Height - RenderContext.Window.Mouse.Y) / (double)RenderContext.Window.Height - 0.5) * 2.0,
                0,
                1.0
            );
            GL.LineWidth(3);

            var P = RenderContext.Projection;
            var V = RenderContext.View;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref P);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref V);

            GL.Begin(BeginMode.Lines);
            GL.Color3(1f, 1f, 0f);
            GL.Vertex4(0, 0, 0, 1);
            GL.Vertex4(start);
            GL.Vertex4(0, 0, 0, 1);
            GL.Vertex4(end);
            GL.Vertex4(start);
            GL.Vertex4(end);
            GL.End();

            var IP = Matrix4d.Invert(P);
            start = Vector4d.Transform(start, IP);
            start /= start.W;
            end = Vector4d.Transform(end, IP);
            end /= end.W;

            GL.Begin(BeginMode.Lines);
            GL.Color3(1f, 0f, 1f);
            GL.Vertex4(0, 0, 0, 1);
            GL.Vertex4(start);
            GL.Vertex4(0, 0, 0, 1);
            GL.Vertex4(end);
            GL.Vertex4(start);
            GL.Vertex4(end);
            GL.End();

            var IV = Matrix4d.Invert(V);
            start = Vector4d.Transform(start, IV);
            start /= start.W;
            end = Vector4d.Transform(end, IV);
            end /= end.W;

            end = end - start;
            end.W = 1;
            var scale = new Vector4d(RenderContext.Camera.Far, RenderContext.Camera.Far, RenderContext.Camera.Far, 1);
            Vector4d.Multiply(ref end, ref scale, out end);

            GL.Begin(BeginMode.Lines);
            GL.Color4(0f, 1f, 1f, 1f);
            GL.Vertex4(start);
            GL.Vertex4(end);
            GL.Color4(0f, 1f, 1f, 0.5f);
            GL.Vertex4(0, 0, 0, 1);
            GL.Vertex4(start);
            GL.Vertex4(0, 0, 0, 1);
            GL.Vertex4(end);
            GL.Color4(0f, 1f, 1f, 0.25f);
            GL.Vertex4(start);
            GL.Vertex4(start.X, 0, start.Z, start.W);
            GL.Vertex4(start);
            GL.Vertex4(0, start.Y, start.Z, start.W);
            GL.Vertex4(start);
            GL.Vertex4(start.X, start.Y, 0, start.W);
            GL.Vertex4(end);
            GL.Vertex4(end.X, 0, end.Z, end.W);
            GL.Vertex4(end);
            GL.Vertex4(0, end.Y, end.Z, end.W);
            GL.Vertex4(end);
            GL.Vertex4(end.X, end.Y, 0, end.W);
            GL.End();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
    }
}
