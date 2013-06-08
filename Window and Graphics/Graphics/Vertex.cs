namespace MartinZottmann.Graphics
{
    public struct VertexObject
    {
        public Vertex3[] vertices;

        public Color4[] colors;
    }

    public struct VertexData
    {
        public Vertex3 position;

        public Color4 color;
    }

    public struct Vertex3
    {
        public float x, y, z;
    }
}
