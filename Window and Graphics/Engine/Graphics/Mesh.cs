namespace MartinZottmann.Engine.Graphics
{
    public interface IMesh
    {
        int VerticesLength { get; }

        int IndicesLength { get; }
    }

    public class Mesh<V, I> : IMesh, IVertices<V>, IIndices<I>
        where V : struct
        where I : struct
    {
        protected V[] vertices;

        public V[] Vertices { get { return vertices; } set { vertices = value; } }

        public int VerticesLength { get { return vertices == null ? 0 : vertices.Length; } }

        protected I[] indices;

        public I[] Indices { get { return indices; } set { indices = value; } }

        public int IndicesLength { get { return indices == null ? 0 : indices.Length; } }

        public Mesh() { }

        public Mesh(V[] vertices)
        {
            this.vertices = vertices;
        }

        public Mesh(V[] vertices, I[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;
        }
    }
}
