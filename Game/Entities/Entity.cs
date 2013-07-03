using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.IO;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities
{
    using SaveTuple = Tuple<string, SaveObject>;
    using SaveList = List<Tuple<string, SaveObject>>;

    public abstract class Entity : IDisposable, ISaveable
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

        public virtual SaveObject Save()
        {
            var status = new SaveObject();

            status.Add("Destroyed", Destroyed);
            status.Add("Position", Position);
            var children_status = new SaveList();
            children.ForEach(s => children_status.Add(new SaveTuple(s.GetType().FullName, s.Save())));
            status.Add("children", children_status);

            return status;
        }

        public virtual void Load(SaveObject status)
        {
            status.TryGetValue<bool>("Destroyed", ref Destroyed);
            status.TryGetValue<Vector3d>("Position", ref Position);
            SaveList children_status = new SaveList();
            if (status.TryGetValue<SaveList>("children", ref children_status))
                foreach (var child_status in children_status)
                    children
                        .FindAll(s => s.GetType().IsEquivalentTo(Type.GetType(child_status.Item1)))
                        .ForEach(s => s.Load(child_status.Item2));
        }
    }
}
