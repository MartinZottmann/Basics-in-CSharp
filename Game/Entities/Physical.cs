using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;

namespace MartinZottmann.Game.Entities
{
    public abstract class Physical : Drawable
    {
        public OpenTK.Graphics.Color4 Mark { get; set; }

        public Physic Physic;

        public Physical(ResourceManager resources)
            : base(resources)
        {
            Physic = new Physic(this);
        }

        public virtual void UpdateVelocity(double delta_time)
        {
            Physic.UpdateVelocity(delta_time);
        }

        public virtual void UpdatePosition(double delta_time)
        {
            Physic.UpdatePosition(delta_time);
        }

        // @todo
        //#if DEBUG
        //        public override void RenderHelpers(double delta_time, RenderContext render_context)
        //        {
        //            base.RenderHelpers(delta_time, render_context);
        //            RenderVelocity(delta_time, render_context);
        //            RenderAngularVelocity(delta_time, render_context);
        //            RenderBoundingBox(delta_time, render_context);
        //        }

        //        public virtual void RenderVelocity(double delta_time, RenderContext render_context)
        //        {
        //            var P = render_context.Projection;
        //            var V = render_context.ViewModel;
        //            GL.MatrixMode(MatrixMode.Projection);
        //            GL.LoadIdentity();
        //            GL.LoadMatrix(ref P);
        //            GL.MatrixMode(MatrixMode.Modelview);
        //            GL.LoadIdentity();
        //            GL.LoadMatrix(ref V);
        //            var velocity = Velocity * InverseOrientationMatrix;
        //            var force = Force * InverseOrientationMatrix;

        //            GL.LineWidth(1);
        //            GL.Begin(BeginMode.Lines);
        //            {
        //                #region Velocity
        //                GL.Color4(1.0f, 1.0f, 0.0f, 0.5f);
        //                GL.Vertex3(Vector3d.Zero);
        //                GL.Vertex3(velocity);

        //                GL.Vertex3(velocity - Vector3d.UnitX);
        //                GL.Vertex3(velocity + Vector3d.UnitX);
        //                GL.Vertex3(velocity - Vector3d.UnitY);
        //                GL.Vertex3(velocity + Vector3d.UnitY);
        //                GL.Vertex3(velocity - Vector3d.UnitZ);
        //                GL.Vertex3(velocity + Vector3d.UnitZ);
        //                #endregion

        //                #region Force
        //                GL.Color4(1.0f, 0.75f, 0.0f, 0.5f);
        //                GL.Vertex3(velocity);
        //                GL.Vertex3(velocity + force);

        //                GL.Vertex3(velocity + force - Vector3d.UnitX);
        //                GL.Vertex3(velocity + force + Vector3d.UnitX);
        //                GL.Vertex3(velocity + force - Vector3d.UnitY);
        //                GL.Vertex3(velocity + force + Vector3d.UnitY);
        //                GL.Vertex3(velocity + force - Vector3d.UnitZ);
        //                GL.Vertex3(velocity + force + Vector3d.UnitZ);
        //                #endregion

        //                //#region Contact to circle
        //                //GL.Color4(1.0f, 1.0f, 1.0f, 0.2f);
        //                //GL.Vertex3(Position);
        //                //var position_on_y = new Vector3d(Position.X, 0, Position.Z);
        //                //GL.Vertex3(position_on_y);

        //                //double radius = 100;
        //                //Vector3d center = Vector3d.Zero;
        //                //Vector3d difference = position_on_y - center;
        //                //Vector3d contact = center + difference / difference.Length * radius;

        //                //GL.Vertex3(position_on_y);
        //                //GL.Vertex3(contact);
        //                //#endregion
        //            }
        //            GL.End();
        //        }

        //        public virtual void RenderAngularVelocity(double delta_time, RenderContext render_context)
        //        {
        //            var P = render_context.Projection;
        //            var V = render_context.ViewModel;
        //            GL.MatrixMode(MatrixMode.Projection);
        //            GL.LoadIdentity();
        //            GL.LoadMatrix(ref P);
        //            GL.MatrixMode(MatrixMode.Modelview);
        //            GL.LoadIdentity();
        //            GL.LoadMatrix(ref V);

        //            GL.LineWidth(5);
        //            GL.Begin(BeginMode.Lines);
        //            {
        //                var angular_velocity_x = new Vector3d(AngularVelocity.X, 0, 0);
        //                angular_velocity_x = Vector3d.Cross(angular_velocity_x, Up);

        //                var angular_velocity_y = new Vector3d(0, AngularVelocity.Y, 0);
        //                angular_velocity_y = Vector3d.Cross(angular_velocity_y, Forward);

        //                var angular_velocity_z = new Vector3d(0, 0, AngularVelocity.Z);
        //                angular_velocity_z = Vector3d.Cross(angular_velocity_z, Right);

        //                GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);

        //                GL.Vertex3(Forward);
        //                GL.Vertex3(Forward + angular_velocity_y);

        //                GL.Color4(0.0f, 1.0f, 0.0f, 0.5f);

        //                GL.Vertex3(Up);
        //                GL.Vertex3(Up + angular_velocity_x);

        //                GL.Color4(0.0f, 0.0f, 1.0f, 0.5f);

        //                GL.Vertex3(Right);
        //                GL.Vertex3(Right + angular_velocity_z);
        //            }
        //            GL.End();
        //        }

        //        public virtual void RenderBoundingBox(double delta_time, RenderContext render_context)
        //        {
        //            var P = render_context.Projection;
        //            var V = render_context.ViewModel;
        //            GL.MatrixMode(MatrixMode.Projection);
        //            GL.LoadIdentity();
        //            GL.LoadMatrix(ref P);
        //            GL.MatrixMode(MatrixMode.Modelview);
        //            GL.LoadIdentity();
        //            GL.LoadMatrix(ref V);

        //            GL.LineWidth(1);
        //            GL.Begin(BeginMode.Lines);
        //            {
        //                GL.Color4(Mark.R, Mark.G, Mark.B, Mark.A);
        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Max.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z);

        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Max.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z);

        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Max.Z);
        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Max.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Min.Z);
        //                GL.Vertex3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z);
        //            }
        //            GL.End();
        //        }
        //#endif
    }
}
