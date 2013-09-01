using MartinZottmann.Engine.Physics;
using OpenTK;
using System;

namespace MartinZottmann.Engine.Graphics.Mesh
{
    public class Mesh<V, I> : IMesh, IVertices<V>, IIndices<I>
        where V : struct, IVertex
        where I : struct
    {
        protected V[] vertices;

        public V[] Vertices { get { return vertices; } set { vertices = value; } }

        public int VerticesLength { get { return null == vertices ? 0 : vertices.Length; } }

        protected I[] indices;

        public I[] Indices { get { return indices; } set { indices = value; } }

        public int IndicesLength { get { return null == indices ? 0 : indices.Length; } }

        public AABB3d BoundingBox
        {
            get
            {
                var bb = new AABB3d();
                for (var i = 0; i < vertices.Length; i++)
                {
                    bb.Min.X = Math.Min(bb.Min.X, vertices[i].PositionXyz.X);
                    bb.Min.Y = Math.Min(bb.Min.Y, vertices[i].PositionXyz.Y);
                    bb.Min.Z = Math.Min(bb.Min.Z, vertices[i].PositionXyz.Z);

                    bb.Max.X = Math.Max(bb.Max.X, vertices[i].PositionXyz.X);
                    bb.Max.Y = Math.Max(bb.Max.Y, vertices[i].PositionXyz.Y);
                    bb.Max.Z = Math.Max(bb.Max.Z, vertices[i].PositionXyz.Z);
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
                    r = Math.Max(r, vertices[i].PositionXyz.Length);

                return new Sphere3d(Vector3d.Zero, r);
            }
        }

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
            if (null == vertices)
                return;

            for (var i = 0; i < vertices.Length; i++)
                vertices[i].Transform(matrix);
        }
    }
}
