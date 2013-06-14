using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace MartinZottmann.Game.Entities
{
    class Grid : Entity
    {
        Engine.Graphics.OpenGL.Entity circle;

        Engine.Graphics.OpenGL.Entity line_y;

        Engine.Graphics.OpenGL.Entity grid_xz;

        public Grid(Resources resources)
            : base(resources)
        {
            color = Color.LightGray;

            var program = Resources.Programs["normal"];

            var amount = 360;
            var radius = 100;
            var vertices = new VertexP3C4[amount + 1];
            for (var i = 0; i <= amount; i++)
            {
                double angle = 2 * System.Math.PI * i / amount;
                vertices[i] = new VertexP3C4(radius * (float)System.Math.Cos(angle), 0, radius * (float)System.Math.Sin(angle), 255, 255, 255, 127);
            }
            circle = new Engine.Graphics.OpenGL.Entity();
            circle.Add(new Mesh<VertexP3C4, uint>(vertices));
            circle.mode = BeginMode.LineLoop;
            circle.Program = program;

            line_y = new Engine.Graphics.OpenGL.Entity();
            line_y.Add(
                new Mesh<VertexP3C4, uint>(
                    new VertexP3C4[] {
                        new VertexP3C4(0, 1000, 0, 255, 255, 255, 127),
                        new VertexP3C4(0, 0, 0, 255, 255, 255, 127),
                        new VertexP3C4(0, 0, 0, 255, 255, 255, 127),
                        new VertexP3C4(0, -1000, 0, 255, 255, 255, 127)
                    }
                )
            );
            line_y.mode = BeginMode.Lines;
            line_y.Program = program;

            amount = 10;
            var space = 100;
            vertices = new VertexP3C4[4 * (2 * amount + 1)];
            for (var i = -amount; i <= amount; i++)
            {
                vertices[4 * (i + amount) + 0] = new VertexP3C4(amount * space, 0, i * space, 255, 255, 255, 127);
                vertices[4 * (i + amount) + 1] = new VertexP3C4(-amount * space, 0, i * space, 255, 255, 255, 127);
                vertices[4 * (i + amount) + 2] = new VertexP3C4(i * space, 0, amount * space, 255, 255, 255, 127);
                vertices[4 * (i + amount) + 3] = new VertexP3C4(i * space, 0, -amount * space, 255, 255, 255, 127);
            }
            grid_xz = new Engine.Graphics.OpenGL.Entity();
            grid_xz.Add(new Mesh<VertexP3C4, uint>(vertices));
            grid_xz.mode = BeginMode.Lines;
            grid_xz.Program = program;
        }

        public override void Render(double delta_time)
        {
            RenderContext.Model = Matrix4d.Identity;
            Resources.Programs["normal"].UniformLocations["PVM"].Set(RenderContext.ProjectionViewModel);

            circle.Draw();
            line_y.Draw();
            grid_xz.Draw();
        }
    }
}
