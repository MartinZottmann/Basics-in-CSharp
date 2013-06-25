using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.AI;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Ship : Physical
    {
        Physical[] components;

        public Steering Streering;

        public Ship(ResourceManager resources)
            : base(resources)
        {
            Streering = new Steering(this);

            components = new Physical[] {
                new Floor(resources) { Position = new Vector3d(0, 0, -1) },
                new Floor(resources),
                new Floor(resources) { Position = new Vector3d(0, 0, 1) }
            };

            foreach (var component in components)
            {
                BoundingBox.Max.X = System.Math.Max(BoundingBox.Max.X, component.BoundingBox.Max.X + component.Position.X);
                BoundingBox.Max.Y = System.Math.Max(BoundingBox.Max.Y, component.BoundingBox.Max.Y + component.Position.Y);
                BoundingBox.Max.Z = System.Math.Max(BoundingBox.Max.Z, component.BoundingBox.Max.Z + component.Position.Z);
                BoundingBox.Min.X = System.Math.Min(BoundingBox.Min.X, component.BoundingBox.Min.X + component.Position.X);
                BoundingBox.Min.Y = System.Math.Min(BoundingBox.Min.Y, component.BoundingBox.Min.Y + component.Position.Y);
                BoundingBox.Min.Z = System.Math.Min(BoundingBox.Min.Z, component.BoundingBox.Min.Z + component.Position.Z);
            }

            BoundingSphere = new Engine.Physics.Sphere3d(Vector3d.Zero, 1);
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            Streering.Update(delta_time);

            render_context = render_context.Push();
            foreach (var component in components)
                component.Update(delta_time, render_context);
            render_context = render_context.Pop();

            base.Update(delta_time, render_context);
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;

            render_context = render_context.Push();
            foreach (var component in components)
                component.Render(delta_time, render_context);
            render_context = render_context.Pop();

#if DEBUG
            RenderVelocity(delta_time, render_context);
            RenderBoundingBox(delta_time, render_context);
#endif
        }
    }
}
