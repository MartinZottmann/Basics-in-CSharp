using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using System;
using System.Drawing;

namespace MartinZottmann.Game.Entities
{
    public abstract class Entity : IDisposable
    {
        public static Random Random = new Random();

        public Vector3d Position = Vector3d.Zero;

        public ResourceManager Resources { get; protected set; }

        public Entity(ResourceManager resources)
        {
            Resources = resources;
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

        public virtual void Update(double delta_time, RenderContext render_context) { }

        public virtual void Render(double delta_time, RenderContext render_context) { }
    }
}
