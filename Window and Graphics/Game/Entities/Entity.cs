using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace MartinZottmann.Game.Entities
{
    public abstract class Entity : IDisposable
    {
        protected static Random randomNumber = new Random();

        public Color color;

        public Vector3d Position = Vector3d.Zero;

        public ResourceManager Resources { get; protected set; }

        public RenderContext RenderContext;

        public Entity(ResourceManager resources)
        {
            Resources = resources;

            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomNumber.Next(names.Length)];
            color = Color.FromKnownColor(randomColorName);
        }

        public virtual void Dispose()
        {
            // @todo
        }

        public virtual void Reposition(double max_x, double max_y, double max_z)
        {
            if (Position.X < -max_x)
                Position.X = max_x;
            else if (Position.X > max_x)
                Position.X = -max_x;

            if (Position.Y < -max_y)
                Position.Y = max_y;
            else if (Position.Y > max_y)
                Position.Y = -max_y;

            if (Position.Z < -max_z)
                Position.Z = max_z;
            else if (Position.Z > max_z)
                Position.Z = -max_z;
        }

        public virtual void Update(double delta_time) { }

        public virtual void Render(double delta_time) { }
    }
}
