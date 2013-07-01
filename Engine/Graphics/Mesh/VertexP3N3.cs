using OpenTK;

namespace MartinZottmann.Engine.Graphics.Mesh
{
    public struct VertexP3N3 : IVertex
    {
        public Vector3 Position;

        public Vector3 PositionXyz { get { return Position; } }

        public Vector3 Normal;

        public VertexP3N3(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }

        public VertexP3N3(float px, float py, float pz, float nx, float ny, float nz) : this(new Vector3(px, py, pz), new Vector3(nx, ny, nz)) { }

        public void Transform(Matrix4 matrix)
        {
            Vector3.Transform(ref Position, ref matrix, out Position);
        }

        public string[] GetAttributeLayout()
        {
            return new string[] {
                "in_Position",
                "in_Normal"
            };
        }
    }
}
