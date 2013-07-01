using OpenTK;

namespace MartinZottmann.Engine.Graphics.Mesh
{
    public struct VertexP3N3T2 : IVertex
    {
        public Vector3 Position;

        public Vector3 PositionXyz { get { return Position; } }

        public Vector3 Normal;

        public Vector2 Texcoord;

        public VertexP3N3T2(Vector3 position, Vector3 normal, Vector2 texcoord)
        {
            Position = position;
            Normal = normal;
            Texcoord = texcoord;
        }

        public VertexP3N3T2(float px, float py, float pz, float nx, float ny, float nz, float s, float t) : this(new Vector3(px, py, pz), new Vector3(nx, ny, nz), new Vector2(s, t)) { }

        public void Transform(Matrix4 matrix)
        {
            Vector3.Transform(ref Position, ref matrix, out Position);
        }

        public string[] GetAttributeLayout()
        {
            return new string[] {
                "in_Position",
                "in_Normal",
                "in_TexCoord"
            };
        }
    }
}
