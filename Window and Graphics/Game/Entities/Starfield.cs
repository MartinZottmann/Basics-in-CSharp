using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    class Starfield : Entity
    {
        const int num_stars = 100000;

        Engine.Graphics.OpenGL.Entity graphic;

        public Starfield(ResourceManager resources)
            : base(resources)
        {
            var vertex_data = new VertexP3C4[num_stars];
            for (int i = 0; i < num_stars; i++)
            {
                vertex_data[i].Position.X = randomNumber.Next(-1000, 1000);
                vertex_data[i].Position.Y = randomNumber.Next(-1000, 1000);
                vertex_data[i].Position.Z = randomNumber.Next(-1000, 1000);
                vertex_data[i].Color.r = (float)randomNumber.NextDouble();
                vertex_data[i].Color.g = (float)randomNumber.NextDouble();
                vertex_data[i].Color.b = (float)randomNumber.NextDouble();
                vertex_data[i].Color.a = (float)randomNumber.NextDouble();
            }

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Mesh(new Mesh<VertexP3C4, uint>(vertex_data));
            graphic.Mode = BeginMode.Points;
            graphic.Program = Resources.Programs["normal"];
        }

        public override void Render(double delta_time)
        {
            GL.PointSize(1);
            graphic.Draw();
        }
    }
}
