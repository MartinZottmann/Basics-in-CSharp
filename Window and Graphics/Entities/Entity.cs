using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace MartinZottmann.Entities
{
    public class Entity
    {
        public Color color;

        protected static Random randomNumber = new Random();

        public Vector2d Position = Vector2d.Zero;

        public Entity()
        {
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomNumber.Next(names.Length)];
            color = Color.FromKnownColor(randomColorName);
        }

        public virtual void Reposition(float max_x, float max_y)
        {
            if (Position.X < 0)
            {
                Position.X = max_x;
            }
            else if (Position.X > max_x)
            {
                Position.X = 0;
            }

            if (Position.Y < 0)
            {
                Position.Y = max_y;
            }
            else if (Position.Y > max_y)
            {
                Position.Y = 0;
            }
        }

        public virtual void Update(double delta_time) { }

        public virtual void Render(double delta_time)
        {
            GL.PushMatrix();
            GL.PointSize(2);
            GL.Color3(color);
            GL.Translate(Position.X, Position.Y, 0);
            GL.Begin(BeginMode.Points);
            GL.Vertex2(0, 0);
            GL.End();
            GL.PopMatrix();
        }
    }
}
