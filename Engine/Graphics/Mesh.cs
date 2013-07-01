using MartinZottmann.Engine.Physics;
using OpenTK;

namespace MartinZottmann.Engine.Graphics
{
    public interface IMesh
    {
        int VerticesLength { get; }

        int IndicesLength { get; }
    }

    public class Mesh<V, I> : IMesh, IVertices<V>, IIndices<I>
        where V : struct, IVertex
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

        public void Translate(Matrix4 matrix)
        {
            if (vertices == null)
                return;

            for (var i = 0; i < vertices.Length; i++)
                vertices[i].Transform(matrix);
        }

        public AABB3d BoundingBox
        {
            get
            {
                var bb = new AABB3d();
                for (var i = 0; i < vertices.Length; i++)
                {
                    bb.Min.X = System.Math.Min(bb.Min.X, vertices[i].PositionXyz.X);
                    bb.Min.Y = System.Math.Min(bb.Min.Y, vertices[i].PositionXyz.Y);
                    bb.Min.Z = System.Math.Min(bb.Min.Z, vertices[i].PositionXyz.Z);

                    bb.Max.X = System.Math.Max(bb.Max.X, vertices[i].PositionXyz.X);
                    bb.Max.Y = System.Math.Max(bb.Max.Y, vertices[i].PositionXyz.Y);
                    bb.Max.Z = System.Math.Max(bb.Max.Z, vertices[i].PositionXyz.Z);
                }
                return bb;
            }
        }

        public Sphere3d BoundingSphere
        {
            get
            {
                float r = 0;
                for (var i = 0; i < vertices.Length; i++)
                    r = System.Math.Max(r, vertices[i].PositionXyz.Length);

                return new Sphere3d(Vector3d.Zero, r);
            }
        }
    }
}
