using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Graphics;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    public abstract class Drawable : Entity
    {
        public Vector3d Scale = new Vector3d(1, 1, 1);

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

        public Graphic Graphic;

        public Drawable(ResourceManager resources)
            : base(resources)
        {
            Graphic = new Graphic(this);
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            base.Update(delta_time, render_context);
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;

            if (Graphic.Model != null)
                Graphic.Render(delta_time, render_context);

            base.Render(delta_time, render_context);
        }
    }
}
