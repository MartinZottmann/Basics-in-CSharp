using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MartinZottmann.Game.Entities
{
    [Serializable]
    public abstract class Entity : IDisposable, ISerializable
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

        public Entity(SerializationInfo info, StreamingContext context)
        {
            Destroyed = (bool)info.GetValue("Destroyed", typeof(bool));
            Position = (Vector3d)info.GetValue("Position", typeof(Vector3d));
            children = (List<Entity>)info.GetValue("children", typeof(List<Entity>));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Destroyed", Destroyed, typeof(bool));
            info.AddValue("Position", Position, typeof(Vector3d));
            info.AddValue("children", children, typeof(List<Entity>));
        }

        public virtual void Dispose()
        {
            foreach (var child in Children)
                child.Dispose();
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

            render_context = render_context.Push();
            children.ForEach(s => s.Render(delta_time, render_context));
            render_context = render_context.Pop();
        }
    }
}
