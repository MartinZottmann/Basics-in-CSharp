using OpenTK;

namespace MartinZottmann.Engine.Graphics.Mesh
{
    public struct VertexP3C4 : IVertex
    {
        public Vector3 Position;

        public Vector3 PositionXyz { get { return Position; } }

        public Color4 Color;

        public VertexP3C4(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }

        public VertexP3C4(float x, float y, float z, float r, float g, float b, float a) : this(new Vector3(x, y, z), new Color4(r, g, b, a)) { }

        public void Transform(Matrix4 matrix)
        {
            Vector3.Transform(ref Position, ref matrix, out Position);
        }

        public string[] GetAttributeLayout()
        {
            return new string[] {
                "in_Position",
                "in_Color"
            };
        }

        //public static readonly int Stride = Marshal.SizeOf(default(VertexData));
    }
}
