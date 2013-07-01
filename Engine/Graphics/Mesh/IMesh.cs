using MartinZottmann.Engine.Physics;
using OpenTK;

namespace MartinZottmann.Engine.Graphics.Mesh
{
    public interface IMesh
    {
        int VerticesLength { get; }

        int IndicesLength { get; }

        AABB3d BoundingBox { get; }

        Sphere3d BoundingSphere { get; }

        void Translate(Matrix4 matrix);
    }
}
