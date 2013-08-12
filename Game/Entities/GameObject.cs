using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.IO;
using OpenTK;
using System;
using System.Collections.Generic;
using Component = MartinZottmann.Game.Entities.Components.Abstract;
using RenderContext = MartinZottmann.Game.Graphics.RenderContext;

namespace MartinZottmann.Game.Entities
{
    public class GameObject : IDisposable, ISaveable
    {
        public static Random Random = new Random();

        public OpenTK.Graphics.Color4 Mark { get; set; }

        public Vector3d Scale = new Vector3d(1, 1, 1);

        /// <summary>
        /// Scale * Rotation * Translation
        /// </summary>
        public Matrix4d Model
        {
            get
            {
                return Matrix4d.Scale(Scale)
                    * Matrix4d.Rotate(Orientation)
                    * Matrix4d.CreateTranslation(Position);
            }
        }

        public Vector3d Position = Vector3d.Zero;

        public Quaterniond Orientation = Quaterniond.Identity;

        public Vector3d Forward = -Vector3d.UnitZ;

        public Vector3d ForwardRelative { get { return Vector3d.Transform(Forward, Orientation); } }

        public Vector3d Up = Vector3d.UnitY;

        public Vector3d UpRelative { get { return Vector3d.Transform(Up, Orientation); } }

        public Vector3d Right = Vector3d.UnitX;

        public Vector3d RightRelative { get { return Vector3d.Transform(Right, Orientation); } }

        public readonly ResourceManager Resources;

        public List<GameObject> Children = new List<GameObject>();

        protected Dictionary<Type, Component> components = new Dictionary<Type, Component>();

        public GameObject(ResourceManager resources)
        {
            Resources = resources;
        }

        public virtual void Dispose()
        {
            foreach (var child in Children)
                child.Dispose();
        }

        public void AddChild(GameObject child)
        {
            Children.Add(child);
        }

        public void RemoveChild(GameObject child)
        {
            Children.Remove(child);
        }

        public T AddComponent<T>(T component) where T : Component
        {
            components.Add(typeof(T), component);

            return component;
        }

        public bool HasComponent<T>() where T : Component
        {
            return components.ContainsKey(typeof(T));
        }

        public T GetComponent<T>() where T : Component
        {
            return (T)components[typeof(T)];
        }

        public virtual void Update(double delta_time, RenderContext render_context)
        {
            foreach (var component in components)
                component.Value.Update(delta_time, render_context);

            if (Children.Count == 0)
                return;

            render_context = render_context.Push();
            Children.ForEach(s => s.Update(delta_time, render_context));
            render_context = render_context.Pop();
        }

        public virtual void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;

            foreach (var component in components)
                component.Value.Render(delta_time, render_context);

            if (Children.Count == 0)
                return;

            render_context = render_context.Push();
            Children.ForEach(s => s.Render(delta_time, render_context));
            render_context = render_context.Pop();
        }

        public virtual SaveValue SaveValue()
        {
            var status = new SaveValue(1);

            //status.Add("Destroyed", Destroyed);
            //status.Add("Position", Position);
            var children_status = new SaveList();
            Children.ForEach(s => children_status.TryAdd(s));
            status.Add("children", children_status);

            return status;
        }

        public virtual void Load(SaveValue status)
        {
            //status.TryGetValue<bool>("Destroyed", ref Destroyed);
            //status.TryGetValue<Vector3d>("Position", ref Position);
            SaveList children_status = new SaveList();
            if (status.TryGetValue<SaveList>("children", ref children_status))
                foreach (var child_status in children_status)
                {
                    var child = (GameObject)System.Activator.CreateInstance(
                        child_status.Key.Type,
                        new object[] { Resources }
                    );
                    child.Load(child_status.Value);
                    AddChild(child);
                }
        }

        public override string ToString()
        {
            return String.Format("{0} (Position: {1})", GetType().FullName, Position);
        }
    }
}
