using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Graphics;
using MartinZottmann.Game.IO;
using OpenTK;
using System;
using System.Collections.Generic;

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

        protected Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>();

        public GameObject(ResourceManager resources)
        {
            Resources = resources;
        }

        public virtual void Dispose()
        {
            foreach (var component in components)
                component.Value.Dispose();
        }

        public T AddComponent<T>(T component) where T : IComponent
        {
            components.Add(typeof(T), component);

            return component;
        }

        public bool HasComponent<T>() where T : IComponent
        {
            return components.ContainsKey(typeof(T));
        }

        public T GetComponent<T>() where T : IComponent
        {
            return (T)components[typeof(T)];
        }

        public virtual void Update(double delta_time, RenderContext render_context)
        {
            foreach (var component in components)
                component.Value.Update(delta_time, render_context);
        }

        public virtual void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;

            foreach (var component in components)
                component.Value.Render(delta_time, render_context);
        }

        public virtual SaveValue SaveValue()
        {
            var status = new SaveValue(1);

            status.Add("Scale", Scale);
            status.Add("Position", Position);
            status.Add("Orientation", Orientation);
            var components_status = new SaveList();
            foreach (var component in components)
                components_status.TryAdd(component.Value);
            status.Add("components_status", components_status);

            return status;
        }

        public virtual void Load(SaveValue status)
        {
            status.TryGetValue<Vector3d>("Scale", ref Scale);
            status.TryGetValue<Vector3d>("Position", ref Position);
            status.TryGetValue<Quaterniond>("Orientation", ref Orientation);
            SaveList components_status = new SaveList();
            if (status.TryGetValue<SaveList>("components_status", ref components_status))
                foreach (var component_status in components_status)
                {
                    IComponent component = (IComponent)System.Activator.CreateInstance(
                        component_status.Key.Type,
                        new object[] { this }
                    );
                    component.Load(component_status.Value);
                    AddComponent(component);
                }
        }

        public override string ToString()
        {
            return String.Format("{0} (Position: {1})", GetType().FullName, Position);
        }
    }
}
