using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.AI;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Ship : Physical
    {
        public Steering Streering;

        public Ship(ResourceManager resources)
            : base(resources)
        {
            Streering = new Steering(this);

            AddChild(new Floor(resources) { Position = new Vector3d(0, 0, -1) });
            AddChild(new Floor(resources));
            AddChild(new Floor(resources) { Position = new Vector3d(0, 0, 1) });
            AddChild(new Terminal(resources) { Position = new Vector3d(0, 0, -1) });

            foreach (var child in children)
            {
                if (!(child is Physical))
                    continue;
                var s = child as Physical;

                BoundingBox.Max.X = System.Math.Max(BoundingBox.Max.X, s.BoundingBox.Max.X + child.Position.X);
                BoundingBox.Max.Y = System.Math.Max(BoundingBox.Max.Y, s.BoundingBox.Max.Y + child.Position.Y);
                BoundingBox.Max.Z = System.Math.Max(BoundingBox.Max.Z, s.BoundingBox.Max.Z + child.Position.Z);
                BoundingBox.Min.X = System.Math.Min(BoundingBox.Min.X, s.BoundingBox.Min.X + child.Position.X);
                BoundingBox.Min.Y = System.Math.Min(BoundingBox.Min.Y, s.BoundingBox.Min.Y + child.Position.Y);
                BoundingBox.Min.Z = System.Math.Min(BoundingBox.Min.Z, s.BoundingBox.Min.Z + child.Position.Z);

                BoundingSphere.Radius = System.Math.Max(BoundingSphere.Radius, s.Position.Length + s.BoundingSphere.Radius);
            }
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            Streering.Update(delta_time);

            base.Update(delta_time, render_context);
        }
    }
}
