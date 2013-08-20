using MartinZottmann.Engine.Entities;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class TargetComponent : IComponent
    {
        public Vector3d? Position;

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
