using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    public abstract class Drawable : Entity
    {
        public Vector3d Scale = new Vector3d(1, 1, 1);

        public Quaterniond Orientation = Quaterniond.Identity;

        public Vector3d Forward = Vector3d.UnitZ;

        public Vector3d ForwardRelative { get { return Vector3d.Transform(Forward, Orientation); } }

        public Vector3d Up = Vector3d.UnitY;

        public Vector3d UpRelative { get { return Vector3d.Transform(Up, Orientation); } }

        public Vector3d Right = -Vector3d.UnitX;

        public Vector3d RightRelative { get { return Vector3d.Transform(Right, Orientation); } }

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

        protected Engine.Graphics.OpenGL.Entity graphic;

        public Drawable(ResourceManager resources) : base(resources) { }

        public override void Update(double delta_time, RenderContext render_context)
        {
            base.Update(delta_time, render_context);
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;

            if (graphic != null)
                graphic.Draw();

            base.Render(delta_time, render_context);
#if DEBUG
            RenderHelpers(delta_time, render_context);
#endif
        }

#if DEBUG
        public virtual void RenderHelpers(double delta_time, RenderContext render_context)
        {
            RenderOrientation(delta_time, render_context);
        }

        public virtual void RenderOrientation(double delta_time, RenderContext render_context)
        {
            var P = render_context.Projection;
            var V = render_context.View;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref P);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref V);

            GL.LineWidth(5);
            GL.Begin(BeginMode.Lines);
            {
                GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
                GL.Vertex3(Position);
                GL.Vertex3(Position + ForwardRelative);

                GL.Color4(0.0f, 1.0f, 0.0f, 0.5f);
                GL.Vertex3(Position);
                GL.Vertex3(Position + UpRelative);

                GL.Color4(0.0f, 0.0f, 1.0f, 0.5f);
                GL.Vertex3(Position);
                GL.Vertex3(Position + RightRelative);
            }
            GL.End();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
#endif
    }
}
