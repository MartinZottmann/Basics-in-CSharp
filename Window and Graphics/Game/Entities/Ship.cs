using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Helper;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Ship : Physical, INavigation
    {
        public Vector3d Target { get; set; }

        Physical[] components;

        public Ship(ResourceManager resources)
            : base(resources)
        {
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

        public override void Update(double delta_time)
        {
            foreach (var component in components)
            {
                component.RenderContext = RenderContext;
                component.Update(delta_time);
            }
            
            base.Update(delta_time);
        }

        public override void Render(double delta_time)
        {
            foreach (var component in components)
                component.Render(delta_time);

#if DEBUG
            RenderVelocity(delta_time);
            RenderBoundingBox(delta_time);
#endif
        }
    }
}
