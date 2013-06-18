using OpenTK;

namespace MartinZottmann.Engine.Graphics
{
    public struct RenderContext
    {
        public GameWindow Window;

        public Camera Camera;

        /// <summary>
        /// Set with Camera
        /// </summary>
        public Matrix4d Projection;

        /// <summary>
        /// Calculated: P^-1
        /// </summary>
        public Matrix4d InvertedProjection { get { return Matrix4d.Invert(Projection); } }

        /// <summary>
        /// Set with Camera
        /// </summary>
        public Matrix4d View;

        /// <summary>
        /// Calculated: V^-1
        /// </summary>
        public Matrix4d InvertedView { get { return Matrix4d.Invert(View); } }

        /// <summary>
        /// Set with Model/Entity
        /// </summary>
        public Matrix4d Model;

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
    }
}
