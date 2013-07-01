using MartinZottmann.Engine.Graphics.Mesh;

namespace MartinZottmann.Engine.Graphics.Shapes
{
    public class Sphere : Mesh<VertexP3N3, uint>
    {
        public Sphere()
        {
            var lod = 10;
            var vertices = new VertexP3N3[(lod + 1) * (lod + 1) * 6];
            var indices = new uint[lod * lod * 6 * 6];
            var indices_i = 0;
            for (var x = 0; x <= lod; x++)
            {
                for (var y = 0; y <= lod; y++)
                {
                    vertices[x + (y * (lod + 1)) + (0 * (lod + 1) * (lod + 1))] = new VertexP3N3(x / (float)lod * 2.0f - 1.0f, y / (float)lod * 2.0f - 1.0f, 1, 0, 0, 1);
                    vertices[x + (y * (lod + 1)) + (1 * (lod + 1) * (lod + 1))] = new VertexP3N3(1, x / (float)lod * 2.0f - 1.0f, y / (float)lod * 2.0f - 1.0f, 0, 0, 1);
                    vertices[x + (y * (lod + 1)) + (2 * (lod + 1) * (lod + 1))] = new VertexP3N3(y / (float)lod * 2.0f - 1.0f, x / (float)lod * 2.0f - 1.0f, -1, 0, 0, 1);
                    vertices[x + (y * (lod + 1)) + (3 * (lod + 1) * (lod + 1))] = new VertexP3N3(-1, y / (float)lod * 2.0f - 1.0f, x / (float)lod * 2.0f - 1.0f, 0, 0, 1);
                    vertices[x + (y * (lod + 1)) + (4 * (lod + 1) * (lod + 1))] = new VertexP3N3(y / (float)lod * 2.0f - 1.0f, 1, x / (float)lod * 2.0f - 1.0f, 0, 0, 1);
                    vertices[x + (y * (lod + 1)) + (5 * (lod + 1) * (lod + 1))] = new VertexP3N3(x / (float)lod * 2.0f - 1.0f, -1, y / (float)lod * 2.0f - 1.0f, 0, 0, 1);
                    if (x != 0 && y != 0)
                    {
                        for (var f = 0; f < 6; f++)
                        {
                            indices[indices_i++] = (uint)(x + (y * (lod + 1)) + (f * (lod + 1) * (lod + 1)));
                            indices[indices_i++] = (uint)((x - 1) + (y * (lod + 1)) + (f * (lod + 1) * (lod + 1)));
                            indices[indices_i++] = (uint)(x + ((y - 1) * (lod + 1)) + (f * (lod + 1) * (lod + 1)));
                            indices[indices_i++] = (uint)(x + ((y - 1) * (lod + 1)) + (f * (lod + 1) * (lod + 1)));
                            indices[indices_i++] = (uint)((x - 1) + (y * (lod + 1)) + (f * (lod + 1) * (lod + 1)));
                            indices[indices_i++] = (uint)((x - 1) + ((y - 1) * (lod + 1)) + (f * (lod + 1) * (lod + 1)));
                        }
                    }
                }
            }

            for (var i = 0; i < vertices.Length; i++)
            {
                var tmp = vertices[i].Position;
                tmp /= tmp.Length;
                vertices[i] = new VertexP3N3(tmp, tmp);
            }

            Vertices = vertices;

            Indices = indices;
        }
    }
}
