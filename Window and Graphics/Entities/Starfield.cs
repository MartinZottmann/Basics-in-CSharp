using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace MartinZottmann.Entities
{
    class Starfield : Entity
    {
        const int num_stars = 100000;

        Graphics.Entity graphic;

        public Starfield()
            : base()
        {
            graphic = new Graphics.Entity();
            graphic.mode = BeginMode.Points;
            graphic.vertices = new Graphics.Vertex3[num_stars];
            graphic.colors = new MartinZottmann.Graphics.Color4[num_stars];
            for (int i = 0; i < num_stars; i++)
            {
                graphic.vertices[i].x = randomNumber.Next(-1000, 1000);
                graphic.vertices[i].y = randomNumber.Next(-1000, 1000);
                graphic.vertices[i].z = randomNumber.Next(-1000, 1000);
                graphic.colors[i].r = (float)randomNumber.NextDouble();
                graphic.colors[i].g = (float)randomNumber.NextDouble();
                graphic.colors[i].b = (float)randomNumber.NextDouble();
                graphic.colors[i].a = (float)randomNumber.NextDouble();
            }
            graphic.Load();
        }

        public override void Render(double delta_time)
        {
            GL.Color3(color);
            GL.PointSize(3);
            graphic.Draw();
        }
    }
}
