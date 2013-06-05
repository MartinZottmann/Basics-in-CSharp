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

        public Vector3d Position = Vector3d.Zero;

        public Entity()
        {
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomNumber.Next(names.Length)];
            color = Color.FromKnownColor(randomColorName);
        }

        public virtual void Reposition(double max_x, double max_y, double max_z)
        {
            if (Position.X < -max_x)
            {
                Position.X = max_x;
            }
            else if (Position.X > max_x)
            {
                Position.X = -max_x;
            }

            if (Position.Y < -max_y)
            {
                Position.Y = max_y;
            }
            else if (Position.Y > max_y)
            {
                Position.Y = -max_y;
            }

            if (Position.Z < -max_z)
            {
                Position.Z = max_z;
            }
            else if (Position.Z > max_z)
            {
                Position.Z = -max_z;
            }
        }

        public virtual void Update(double delta_time) { }

        public virtual void Render(double delta_time)
        {
            GL.PushMatrix();
            {
                GL.PointSize(3);
                GL.Translate(Position.X, Position.Y, Position.Z);

                GL.Begin(BeginMode.Points);
                {
                    GL.Color3(color);
                    GL.Vertex3(0, 0, 0);
                }
                GL.End();
            }
            GL.PopMatrix();
        }
    }
}
