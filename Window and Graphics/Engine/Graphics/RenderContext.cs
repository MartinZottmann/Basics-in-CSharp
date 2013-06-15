using OpenTK;

namespace MartinZottmann.Engine.Graphics
{
    public class RenderContext
    {
        public GameWindow Window { get; set; }

        public Camera Camera { get; set; }

        /// <summary>
        /// Set with Camera
        /// </summary>
        public Matrix4d Projection { get; set; }

        /// <summary>
        /// Set with Camera
        /// </summary>
        public Matrix4d View { get; set; }

        /// <summary>
        /// Set with Model/Entity
        /// </summary>
        public Matrix4d Model { get; set; }

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
