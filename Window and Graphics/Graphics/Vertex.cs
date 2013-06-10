using OpenTK;

namespace MartinZottmann.Graphics
{
    public interface IStruct<T> where T : struct
    {
        T GetStruct();

        string[] GetAttributeLayout();
    }

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

        public string[] GetAttributeLayout()
        {
            return new string[] {
                "in_Position",
                "in_Color"
            };
        }
    }

    public struct VertexP3N3T2 : IStruct<VertexP3N3T2>
    {
        public Vector3 position;

        public Vector3 normal;

        public Vector2 texcoord;

        public VertexP3N3T2(Vector3 position, Vector3 normal, Vector2 texcoord)
        {
            this.position = position;
            this.normal = normal;
            this.texcoord = texcoord;
        }

        public VertexP3N3T2(float px, float py, float pz, float nx, float ny, float nz, float s, float t) : this(new Vector3(px, py, pz), new Vector3(nx, ny, nz), new Vector2(s, t)) { }

        public VertexP3N3T2 GetStruct() { return this; }

        public string[] GetAttributeLayout()
        {
            return new string[] {
                "in_Position",
                "in_Normal",
                "in_TexCoord"
            };
        }
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
