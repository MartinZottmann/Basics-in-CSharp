using OpenTK;

namespace MartinZottmann.Graphics.Shapes
{
    public class Cube : Mesh<VertexP3N3T2, uint>
    {
        public Cube()
        {
            Vertices = new VertexP3N3T2[] {
                // Front face
                new VertexP3N3T2(-1.0f, -1.0f,  1.0f,  0f,  0f,  1f, 0, 1),
                new VertexP3N3T2( 1.0f, -1.0f,  1.0f,  0f,  0f,  1f, 1, 1),
                new VertexP3N3T2( 1.0f,  1.0f,  1.0f,  0f,  0f,  1f, 1, 0),
                new VertexP3N3T2(-1.0f,  1.0f,  1.0f,  0f,  0f,  1f, 0, 0),
                // Right face
                new VertexP3N3T2( 1.0f, -1.0f,  1.0f,  1f,  0f,  0f, 0, 1),
                new VertexP3N3T2( 1.0f, -1.0f, -1.0f,  1f,  0f,  0f, 1, 1),
                new VertexP3N3T2( 1.0f,  1.0f, -1.0f,  1f,  0f,  0f, 1, 0),
                new VertexP3N3T2( 1.0f,  1.0f,  1.0f,  1f,  0f,  0f, 0, 0),
                // Back face
                new VertexP3N3T2( 1.0f, -1.0f, -1.0f,  0f,  0f, -1f, 0, 1),
                new VertexP3N3T2(-1.0f, -1.0f, -1.0f,  0f,  0f, -1f, 1, 1),
                new VertexP3N3T2(-1.0f,  1.0f, -1.0f,  0f,  0f, -1f, 1, 0),
                new VertexP3N3T2( 1.0f,  1.0f, -1.0f,  0f,  0f, -1f, 0, 0),
                // Left face
                new VertexP3N3T2(-1.0f, -1.0f, -1.0f, -1f,  0f,  0f, 0, 1),
                new VertexP3N3T2(-1.0f, -1.0f,  1.0f, -1f,  0f,  0f, 1, 1),
                new VertexP3N3T2(-1.0f,  1.0f,  1.0f, -1f,  0f,  0f, 1, 0),
                new VertexP3N3T2(-1.0f,  1.0f, -1.0f, -1f,  0f,  0f, 0, 0),
                // Top face
                new VertexP3N3T2(-1.0f,  1.0f,  1.0f,  0f,  1f,  0f, 0, 1),
                new VertexP3N3T2( 1.0f,  1.0f,  1.0f,  0f,  1f,  0f, 1, 1),
                new VertexP3N3T2( 1.0f,  1.0f, -1.0f,  0f,  1f,  0f, 1, 0),
                new VertexP3N3T2(-1.0f,  1.0f, -1.0f,  0f,  1f,  0f, 0, 0),
                // Bottom face
                new VertexP3N3T2( 1.0f, -1.0f,  1.0f,  0f, -1f,  0f, 0, 1),
                new VertexP3N3T2(-1.0f, -1.0f,  1.0f,  0f, -1f,  0f, 1, 1),
                new VertexP3N3T2(-1.0f, -1.0f, -1.0f,  0f, -1f,  0f, 1, 0),
                new VertexP3N3T2( 1.0f, -1.0f, -1.0f,  0f, -1f,  0f, 0, 0)
            };

            Indices = new uint[] {
                // Font face
                0, 1, 2, 2, 3, 0,
                // Right face
                7, 6, 5, 5, 4, 7,
                // Back face
                11, 10, 9, 9, 8, 11,
                // Left face
                15, 14, 13, 13, 12, 15,
                // Top face
                19, 18, 17, 17, 16, 19,
                // Bottom face
                23, 22, 21, 21, 20, 23
            };
        }
    }
}
