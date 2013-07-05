using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.AI;
using MartinZottmann.Game.Entities.Helper;
using MartinZottmann.Game.Entities.Ships.Components;
using MartinZottmann.Game.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Game.Entities.Ships
{
    [Serializable]
    public class Ship : Physical, INavigation
    {
        public Vector3d Target { get; set; }

        public Steering<Ship> Streering;

        public Ship(ResourceManager resources)
            : base(resources)
        {
            Streering = new Steering<Ship>(this);

            AddChild(new Floor(resources) { Position = new Vector3d(0, -1, -1) });
            AddChild(new Floor(resources) { Position = new Vector3d(0, -1, 0) });
            AddChild(new Floor(resources) { Position = new Vector3d(0, -1, 1) });
            AddChild(new Terminal(resources) { Position = new Vector3d(0, 0, -1) });

            foreach (var child in children)
            {
                if (!(child is Physical))
                    continue;
                var s = child as Physical;

                BoundingSphere.Radius = System.Math.Max(BoundingSphere.Radius, s.Position.Length + s.BoundingSphere.Radius);
            }
            BoundingBox.Max = new Vector3d(BoundingSphere.Radius);
            BoundingBox.Min = new Vector3d(-BoundingSphere.Radius);
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            Streering.Update(delta_time);

            base.Update(delta_time, render_context);
        }

#if DEBUG
        public override void RenderHelpers(double delta_time, RenderContext render_context)
        {
            base.RenderHelpers(delta_time, render_context);
            RenderTarget(delta_time, render_context);
        }

        public virtual void RenderTarget(double delta_time, RenderContext render_context)
        {
            if (Target.Equals(Vector3d.Zero))
                return;

            var P = render_context.Projection;
            var V = render_context.View;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref P);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref V);

            GL.LineWidth(1);
            GL.Begin(BeginMode.Lines);
            {
                GL.Color4(0.0f, 1.0f, 1.0f, 0.5f);
                GL.Vertex3(Position);
                GL.Vertex3(Target);

                GL.Vertex3(Target - Vector3d.UnitX);
                GL.Vertex3(Target + Vector3d.UnitX);
                GL.Vertex3(Target - Vector3d.UnitY);
                GL.Vertex3(Target + Vector3d.UnitY);
                GL.Vertex3(Target - Vector3d.UnitZ);
                GL.Vertex3(Target + Vector3d.UnitZ);
            }
            GL.End();
        }
#endif
    }
}
