using OpenTK;

namespace MartinZottmann.Engine.Graphics
{
    public class RenderContext : RenderContext<RenderContext> { }

    public class RenderContext<T> where T : RenderContext<T>, new()
    {
        public T Parent;

        public GameWindow Window;

        protected Matrix4d projection;

        /// <summary>
        /// Set with Camera
        /// </summary>
        public Matrix4d Projection { get { return Parent == null ? projection : projection * Parent.Projection; } set { projection = value; } }

        /// <summary>
        /// Calculated: P^-1
        /// </summary>
        public Matrix4d InvertedProjection { get { return Matrix4d.Invert(Projection); } }

        protected Matrix4d view;

        /// <summary>
        /// Set with Camera
        /// </summary>
        public Matrix4d View { get { return Parent == null ? view : view * Parent.View; } set { view = value; } }

        /// <summary>
        /// Calculated: V^-1
        /// </summary>
        public Matrix4d InvertedView { get { return Matrix4d.Invert(View); } }

        protected Matrix4d model;

        /// <summary>
        /// Set with Model/Entity
        /// </summary>
        public Matrix4d Model { get { return Parent == null ? model : model * Parent.Model; } set { model = value; } }

        /// <summary>
        /// Calculated: M^-1
        /// </summary>
        public Matrix4d InvertedModel { get { return Matrix4d.Invert(Model); } }

        /// <summary>
        /// Calculated: P * V * M = M' * V' * P'
        /// </summary>
        public Matrix4d ProjectionViewModel { get { return Model * View * Projection; } }

        /// <summary>
        /// Calculated: P * V = V' * P'
        /// </summary>
        public Matrix4d ProjectionView { get { return View * Projection; } }

        /// <summary>
        /// Calculated: V * M = M' * V'
        /// </summary>
        public Matrix4d ViewModel { get { return Model * View; } }

        /// <summary>
        /// Calculated: [[V * M]^-1]^T = [[M' * V']^-1]^T
        /// </summary>
        public Matrix4d Normal { get { return Matrix4d.Transpose(Matrix4d.Invert(Model * View)); } }

        public virtual T Push()
        {
            return new T()
            {
                Parent = (T)this,
                Window = Window,
                Projection = Matrix4d.Identity,
                View = Matrix4d.Identity,
                Model = Matrix4d.Identity
            };
        }

        public virtual T Pop()
        {
            return Parent;
        }
    }
}
