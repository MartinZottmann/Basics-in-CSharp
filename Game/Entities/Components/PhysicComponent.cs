using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Physics;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class PhysicComponent : IComponent
    {
        public double thrust = 10.0;

        public Vector3d Torque = Vector3d.Zero;

        public Vector3d AngularVelocity = Vector3d.Zero;

        public AABB3d BoundingBox;

        public Sphere3d BoundingSphere;

        public Vector3d Force = Vector3d.Zero;

        public Vector3d Velocity = Vector3d.Zero;

        #region Mass

        protected double mass = 10.0;

        protected double inverse_mass = 1.0 / 10.0;

        public double Mass
        {
            get { return mass; }
            set
            {
                mass = value;
                inverse_mass = 1.0 / value;
            }
        }

        public double InverseMass
        {
            get { return inverse_mass; }
            set
            {
                inverse_mass = value;
                mass = 1.0 / value;
            }
        }

        #endregion

        #region Inertia

        protected Matrix4d inertia = Matrix4d.Identity;

        protected Matrix4d inverse_inertia = Matrix4d.Identity.Inverted();

        public Matrix4d Inertia
        {
            get { return inertia; }
            set
            {
                inertia = value;
                inverse_inertia = value.Inverted();
            }
        }

        public Matrix4d InverseInertia
        {
            get { return inverse_inertia; }
            set
            {
                inverse_inertia = value;
                inertia = value.Inverted();
            }
        }

        #endregion

        //#if DEBUG
        //        public void Render(double delta_time, RenderContext render_context)
        //        {
        //            if (!render_context.Debug)
        //                return;

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
        //                angular_velocity_x = Vector3d.Cross(angular_velocity_x, GameObject.Up);

        //                var angular_velocity_y = new Vector3d(0, AngularVelocity.Y, 0);
        //                angular_velocity_y = Vector3d.Cross(angular_velocity_y, GameObject.Forward);

        //                var angular_velocity_z = new Vector3d(0, 0, AngularVelocity.Z);
        //                angular_velocity_z = Vector3d.Cross(angular_velocity_z, GameObject.Right);

        //                GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);

        //                GL.Vertex3(GameObject.Forward);
        //                GL.Vertex3(GameObject.Forward + angular_velocity_y);

        //                GL.Color4(0.0f, 1.0f, 0.0f, 0.5f);

        //                GL.Vertex3(GameObject.Up);
        //                GL.Vertex3(GameObject.Up + angular_velocity_x);

        //                GL.Color4(0.0f, 0.0f, 1.0f, 0.5f);

        //                GL.Vertex3(GameObject.Right);
        //                GL.Vertex3(GameObject.Right + angular_velocity_z);
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
        //                GL.Color4(GameObject.Mark.R, GameObject.Mark.G, GameObject.Mark.B, GameObject.Mark.A);
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
