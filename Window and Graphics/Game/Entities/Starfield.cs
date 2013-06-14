using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    class Starfield : Entity
    {
        const int num_stars = 100000;

        Engine.Graphics.OpenGL.Entity graphic;

        public Starfield(Resources resources)
            : base(resources)
        {
            var vertex_data = new VertexP3C4[num_stars];
            for (int i = 0; i < num_stars; i++)
            {
                vertex_data[i].position.X = randomNumber.Next(-1000, 1000);
                vertex_data[i].position.Y = randomNumber.Next(-1000, 1000);
                vertex_data[i].position.Z = randomNumber.Next(-1000, 1000);
                vertex_data[i].color.r = (float)randomNumber.NextDouble();
                vertex_data[i].color.g = (float)randomNumber.NextDouble();
                vertex_data[i].color.b = (float)randomNumber.NextDouble();
                vertex_data[i].color.a = (float)randomNumber.NextDouble();
            }

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Add(new Mesh<VertexP3C4, uint>(vertex_data));
            graphic.mode = BeginMode.Points;
            graphic.program = Resources.Programs["normal"];
        }

        public override void Render(double delta_time)
        {
            GL.PointSize(1);
            graphic.Draw();
        }
    }
}
