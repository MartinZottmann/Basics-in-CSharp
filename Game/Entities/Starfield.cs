using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    public class Starfield : GameObject
    {
        const int num_stars = 100000;

        public Starfield(ResourceManager resources)
            : base(resources)
        {
            var vertex_data = new VertexP3C4[num_stars];
            for (int i = 0; i < num_stars; i++)
                vertex_data[i] = GetStar();

            var graphic = AddComponent(new Graphic(this));
            graphic.Model = new Engine.Graphics.OpenGL.Entity();
            graphic.Model.Mesh(new Mesh<VertexP3C4, uint>(vertex_data));
            graphic.Model.Mode = BeginMode.Points;
            graphic.Model.Program = Resources.Programs["normal"];
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            var graphic = GetComponent<Graphic>();
            var i = Random.Next(0, graphic.Model.Mesh().VerticesLength - 1);
            var bo = graphic.Model.VertexArrayObject.BufferObjects[0];
            (bo as BufferObject<VertexP3C4>).Write(i, GetStar());

            base.Update(delta_time, render_context);
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;

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
