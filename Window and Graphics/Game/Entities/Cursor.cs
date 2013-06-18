using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    public class Cursor : Entity
    {
        Engine.Graphics.OpenGL.Entity graphic;

        Matrix4d Model = Matrix4d.Identity;

        public Ray3d ray;

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

        public void Set()
        {
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

            var IP = RenderContext.InvertedProjection;
            start = Vector4d.Transform(start, IP);
            start /= start.W;
            end = Vector4d.Transform(end, IP);
            end /= end.W;

            var IV = RenderContext.InvertedView;
            start = Vector4d.Transform(start, IV);
            start /= start.W;
            end = Vector4d.Transform(end, IV);
            end /= end.W;

            end = end - start;
            end.W = 1;

            ray = new Ray3d(
                new Vector3d(start.X, start.Y, start.Z),
                Vector3d.Normalize(new Vector3d(end.X, end.Y, end.Z))
            );

            //var scale = new Vector4d(RenderContext.Camera.Far, RenderContext.Camera.Far, RenderContext.Camera.Far, 1);
            //Vector4d.Multiply(ref end, ref scale, out end);
        }

        public override void Update(double delta_time)
        {
            Set();

            RenderContext.Model = Model;
        }

        public override void Render(double delta_time)
        {
            Resources.Programs["normal"].UniformLocations["PVM"].Set(RenderContext.ProjectionViewModel);
            graphic.Draw();
        }
    }
}
