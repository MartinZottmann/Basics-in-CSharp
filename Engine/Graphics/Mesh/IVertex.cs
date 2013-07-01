using OpenTK;

namespace MartinZottmann.Engine.Graphics.Mesh
{
    public interface IVertex
    {
        Vector3 PositionXyz { get; }

        void Transform(Matrix4 matrix);
    }
}
