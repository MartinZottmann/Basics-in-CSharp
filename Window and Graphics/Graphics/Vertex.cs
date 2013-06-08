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

        public VertexData(Vertex3 position, Color4 color)
        {
            this.position = position;
            this.color = color;
        }

        public VertexData(float x, float y, float z, float r, float g, float b, float a) : this(new Vertex3(x, y, z), new Color4(r, g, b, a)) { }
    }

    public struct Vertex3
    {
        public float x, y, z;

        public Vertex3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
