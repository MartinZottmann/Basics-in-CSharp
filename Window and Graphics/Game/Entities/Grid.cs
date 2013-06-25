﻿using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace MartinZottmann.Game.Entities
{
    class Grid : Drawable
    {
        Engine.Graphics.OpenGL.Entity circle;

        Engine.Graphics.OpenGL.Entity line_y;

        public Grid(ResourceManager resources)
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
                vertices[i] = new VertexP3C4(radius * (float)System.Math.Cos(angle), 0, radius * (float)System.Math.Sin(angle), 1, 1, 1, 0.5f);
            }
            circle = new Engine.Graphics.OpenGL.Entity();
            circle.Mesh(new Mesh<VertexP3C4, uint>(vertices));
            circle.Mode = BeginMode.LineLoop;
            circle.Program = program;

            line_y = new Engine.Graphics.OpenGL.Entity();
            line_y.Mesh(
                new Mesh<VertexP3C4, uint>(
                    new VertexP3C4[] {
                        new VertexP3C4(0, 1000, 0, 1, 0, 0, 0.5f),
                        new VertexP3C4(0, 0, 0, 1, 1, 1, 0.5f),
                        new VertexP3C4(0, 0, 0, 1, 1, 1, 0.5f),
                        new VertexP3C4(0, -1000, 0, 1, 0, 0, 0.5f)
                    }
                )
            );
            line_y.Mode = BeginMode.Lines;
            line_y.Program = program;

            amount = 10;
            var space = 100;
            vertices = new VertexP3C4[4 * (2 * amount + 1)];
            for (var i = -amount; i <= amount; i++)
            {
                vertices[4 * (i + amount) + 0] = new VertexP3C4(amount * space, 0, i * space, 1, 1, 1, 0.5f);
                vertices[4 * (i + amount) + 1] = new VertexP3C4(-amount * space, 0, i * space, 1, 1, 1, 0.5f);
                vertices[4 * (i + amount) + 2] = new VertexP3C4(i * space, 0, amount * space, 1, 1, 1, 0.5f);
                vertices[4 * (i + amount) + 3] = new VertexP3C4(i * space, 0, -amount * space, 1, 1, 1, 0.5f);
            }
            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Mesh(new Mesh<VertexP3C4, uint>(vertices));
            graphic.Mode = BeginMode.Lines;
            graphic.Program = program;
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;
            Resources.Programs["normal"].UniformLocations["PVM"].Set(render_context.ProjectionViewModel);

            GL.LineWidth(1);
            circle.Draw();
            line_y.Draw();
            graphic.Draw();
        }
    }
}
