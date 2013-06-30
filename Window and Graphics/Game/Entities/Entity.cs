using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MartinZottmann.Game.Entities
{
    public abstract class Entity : IDisposable
    {
        public static Random Random = new Random();

        public bool Destroyed = false;

        public Vector3d Position = Vector3d.Zero;

        public ResourceManager Resources { get; protected set; }

        protected List<Entity> children = new List<Entity>();

        public IEnumerable<Entity> Children { get { return children.AsReadOnly(); } }

        public Entity(ResourceManager resources)
        {
            Resources = resources;
        }

        public virtual void Dispose()
        {
            // @todo
        }

        public void AddChild(Entity child)
        {
            children.Add(child);
        }

        public void RemoveChild(Entity child)
        {
            children.Remove(child);
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

        public virtual void Update(double delta_time, RenderContext render_context)
        {
            if (children.Count == 0)
                return;

            children.RemoveAll(s => s.Destroyed);

            render_context = render_context.Push();
            children.ForEach(s => s.Update(delta_time, render_context));
            render_context = render_context.Pop();
        }

        public virtual void Render(double delta_time, RenderContext render_context)
        {
            if (children.Count == 0)
                return;

            render_context.Model = Matrix4d.Identity;
            render_context = render_context.Push();
            children.ForEach(s => s.Render(delta_time, render_context));
            render_context = render_context.Pop();
        }
    }
}
