using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    class Starfield : Drawable
    {
        const int num_stars = 100000;

        public Starfield(ResourceManager resources)
            : base(resources)
        {
            var vertex_data = new VertexP3C4[num_stars];
            for (int i = 0; i < num_stars; i++)
                vertex_data[i] = GetStar();

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Mesh(new Mesh<VertexP3C4, uint>(vertex_data));
            graphic.Mode = BeginMode.Points;
            graphic.Program = Resources.Programs["normal"];
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            var i = Random.Next(0, graphic.Mesh().VerticesLength - 1);
            var bo = graphic.VertexArrayObject.BufferObjects[0];
            (bo as BufferObject<VertexP3C4>).Write(i, GetStar());

            base.Update(delta_time, render_context);
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;
            graphic.Program.UniformLocations["PVM"].Set(render_context.ProjectionViewModel);

            GL.PointSize(1);
            base.Render(delta_time, render_context);
        }

        protected VertexP3C4 GetStar()
        {
            return new VertexP3C4(
                Random.Next(-1000, 1000),
                Random.Next(-1000, 1000),
                Random.Next(-1000, 1000),
                (float)Random.NextDouble(),
                (float)Random.NextDouble(),
                (float)Random.NextDouble(),
                (float)Random.NextDouble()
            );
        }
    }
}
