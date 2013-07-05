using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using Color4 = MartinZottmann.Engine.Graphics.Color4;

namespace MartinZottmann.Game.Entities
{
    public abstract class Drawable : Entity
    {
        public Vector3d Scale = new Vector3d(1, 1, 1);

        public Quaterniond Orientation = Quaterniond.Identity;

        public Vector3d Forward = -Vector3d.UnitZ;

        public Vector3d ForwardRelative { get { return Vector3d.Transform(Forward, Orientation); } }

        public Vector3d Up = Vector3d.UnitY;

        public Vector3d UpRelative { get { return Vector3d.Transform(Up, Orientation); } }

        public Vector3d Right = Vector3d.UnitX;

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

#if DEBUG
        protected Engine.Graphics.OpenGL.Entity orientation_graphic;
#endif

        public Drawable(ResourceManager resources)
            : base(resources)
        {
#if DEBUG
            var forward = new Color4(1.0f, 0.0f, 0.0f, 0.5f);
            var up = new Color4(0.0f, 1.0f, 0.0f, 0.5f);
            var right = new Color4(0.0f, 0.0f, 1.0f, 0.5f);
            orientation_graphic = new Engine.Graphics.OpenGL.Entity();
            orientation_graphic.Mesh(
                new Mesh<VertexP3C4, uint>()
                {
                    Vertices = new VertexP3C4[] {
                        new VertexP3C4(Vector3.Zero, forward),
                        new VertexP3C4((Vector3)Forward, forward),
                        new VertexP3C4(Vector3.Zero, up),
                        new VertexP3C4((Vector3)Up, up),
                        new VertexP3C4(Vector3.Zero, right),
                        new VertexP3C4((Vector3)Right, right),
                    },
                    Indices = new uint[] {
                        0, 1, 2, 3, 4, 5
                    }
                }
            );
            orientation_graphic.Mode = BeginMode.Lines;
            orientation_graphic.Program = Resources.Programs["normal"];
#endif
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            base.Update(delta_time, render_context);
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;

            if (graphic != null)
            {
                foreach (var s in graphic.Program.UniformLocations)
                    switch (s.Key)
                    {
                        case "alpha_cutoff": s.Value.Set(render_context.alpha_cutoff); break;
                    }
                graphic.Draw();
            }

            base.Render(delta_time, render_context);
#if DEBUG
            if (render_context.Debug)
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
            GL.LineWidth(5);
            orientation_graphic.Program.UniformLocations["PVM"].Set(render_context.ProjectionViewModel);
            orientation_graphic.Draw();
        }
#endif
    }
}
