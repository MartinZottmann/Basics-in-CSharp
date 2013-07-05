using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.IO;
using OpenTK;
using System;
using System.Collections.Generic;
using RenderContext = MartinZottmann.Game.Graphics.RenderContext;

namespace MartinZottmann.Game.Entities
{
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

        public virtual SaveValue SaveValue()
        {
            var status = new SaveValue(1);

            status.Add("Destroyed", Destroyed);
            status.Add("Position", Position);
            var children_status = new SaveList();
            children.ForEach(s => children_status.TryAdd(s));
            status.Add("children", children_status);

            return status;
        }

        public virtual void Load(SaveValue status)
        {
            status.TryGetValue<bool>("Destroyed", ref Destroyed);
            status.TryGetValue<Vector3d>("Position", ref Position);
            SaveList children_status = new SaveList();
            if (status.TryGetValue<SaveList>("children", ref children_status))
                foreach (var child_status in children_status)
                {
                    var child = (Entity)System.Activator.CreateInstance(
                        child_status.Key.Type,
                        new object[] { Resources }
                    );
                    child.Load(child_status.Value);
                    AddChild(child);
                }
        }

        public override string ToString()
        {
            return String.Format("{0} (Position: {1})", this.GetType().FullName, Position);
        }
    }
}
