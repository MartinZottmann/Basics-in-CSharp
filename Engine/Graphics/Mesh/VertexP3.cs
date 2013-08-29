using OpenTK;

namespace MartinZottmann.Engine.Graphics.Mesh
{
    public struct VertexP3 : IVertex
    {
        public Vector3 Position;

        public Vector3 PositionXyz { get { return Position; } }

        public VertexP3(Vector3 position)
        {
            Position = position;
        }

        public VertexP3(float x, float y, float z) : this(new Vector3(x, y, z)) { }

        public void Transform(Matrix4 matrix)
        {
            Vector3.Transform(ref Position, ref matrix, out Position);
        }
    }
}
