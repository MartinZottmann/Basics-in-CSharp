using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    public class Cursor : Drawable
    {

        public Ray3d Ray;

        public Plane3d Plane = new Plane3d(Vector3d.Zero, Vector3d.UnitY);

        public Cursor(ResourceManager resources)
            : base(resources)
        {
            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Mesh(
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
            graphic.Mode = BeginMode.LineLoop;
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

            Ray = new Ray3d(
                new Vector3d(start.X, start.Y, start.Z),
                Vector3d.Normalize(new Vector3d(end.X, end.Y, end.Z))
            );

            Ray.Intersect(Plane, out Position);
        }

        public override void Update(double delta_time)
        {
            Set();

            RenderContext.Model = Model;
        }

        public override void Render(double delta_time)
        {
            Resources.Programs["normal"].UniformLocations["PVM"].Set(RenderContext.ProjectionViewModel);
            System.Console.WriteLine(RenderContext.Model);
            graphic.Draw();
        }
    }
}
