using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.AI;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Entities.Ships.Components;
using MartinZottmann.Game.Graphics;
using OpenTK;

namespace MartinZottmann.Game.Entities.Ships
{
    public class Ship : GameObject
    {
        public Steering<Ship> Streering;

        public Ship(ResourceManager resources)
            : base(resources)
        {
            AddComponent(new Target(this));

            Streering = new Steering<Ship>(this);

            AddChild(new Floor(resources) { Position = new Vector3d(0, -1, -1) });
            AddChild(new Floor(resources) { Position = new Vector3d(0, -1, 0) });
            AddChild(new Floor(resources) { Position = new Vector3d(0, -1, 1) });
            AddChild(new Terminal(resources) { Position = new Vector3d(0, 0, -1) });

            var physic = AddComponent(new Physic(this));
            foreach (var child in Children)
                physic.BoundingSphere.Radius = System.Math.Max(physic.BoundingSphere.Radius, child.Position.Length + child.GetComponent<Physic>().BoundingSphere.Radius);
            physic.BoundingBox.Max = new Vector3d(physic.BoundingSphere.Radius);
            physic.BoundingBox.Min = new Vector3d(-physic.BoundingSphere.Radius);
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            Streering.Update(delta_time);

            base.Update(delta_time, render_context);
        }

        // @todo
        //#if DEBUG
        //        public override void RenderHelpers(double delta_time, RenderContext render_context)
        //        {
        //            base.RenderHelpers(delta_time, render_context);
        //            RenderTarget(delta_time, render_context);
        //        }

        //        public virtual void RenderTarget(double delta_time, RenderContext render_context)
        //        {
        //            if (Target.Equals(Vector3d.Zero))
        //                return;

        //            var P = render_context.Projection;
        //            var V = render_context.View;
        //            GL.MatrixMode(MatrixMode.Projection);
        //            GL.LoadIdentity();
        //            GL.LoadMatrix(ref P);
        //            GL.MatrixMode(MatrixMode.Modelview);
        //            GL.LoadIdentity();
        //            GL.LoadMatrix(ref V);

        //            GL.LineWidth(1);
        //            GL.Begin(BeginMode.Lines);
        //            {
        //                GL.Color4(0.0f, 1.0f, 1.0f, 0.5f);
        //                GL.Vertex3(Position);
        //                GL.Vertex3(Target);

        //                GL.Vertex3(Target - Vector3d.UnitX);
        //                GL.Vertex3(Target + Vector3d.UnitX);
        //                GL.Vertex3(Target - Vector3d.UnitY);
        //                GL.Vertex3(Target + Vector3d.UnitY);
        //                GL.Vertex3(Target - Vector3d.UnitZ);
        //                GL.Vertex3(Target + Vector3d.UnitZ);
        //            }
        //            GL.End();
        //        }
        //#endif
    }
}
