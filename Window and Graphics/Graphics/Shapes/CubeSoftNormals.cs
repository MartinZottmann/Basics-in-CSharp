namespace MartinZottmann.Graphics.Shapes
{
    public class CubeSoftNormals : Mesh<VertexP3N3T2, uint>
    {
        public CubeSoftNormals()
        {
            Vertices = new VertexP3N3T2[] {
                // Front face
                new VertexP3N3T2(-1, -1,  1, -1, -1,  1, 0, 1),
                new VertexP3N3T2( 1, -1,  1,  1, -1,  1, 1, 1),
                new VertexP3N3T2( 1,  1,  1,  1,  1,  1, 1, 0),
                new VertexP3N3T2(-1,  1,  1, -1,  1,  1, 0, 0),
                // Right face
                new VertexP3N3T2( 1, -1,  1,  1, -1,  1, 0, 1),
                new VertexP3N3T2( 1, -1, -1,  1, -1, -1, 1, 1),
                new VertexP3N3T2( 1,  1, -1,  1,  1, -1, 1, 0),
                new VertexP3N3T2( 1,  1,  1,  1,  1,  1, 0, 0),
                // Back face
                new VertexP3N3T2( 1, -1, -1,  1, -1, -1, 0, 1),
                new VertexP3N3T2(-1, -1, -1, -1, -1, -1, 1, 1),
                new VertexP3N3T2(-1,  1, -1, -1,  1, -1, 1, 0),
                new VertexP3N3T2( 1,  1, -1,  1,  1, -1, 0, 0),
                // Left face
                new VertexP3N3T2(-1, -1, -1, -1, -1, -1, 0, 1),
                new VertexP3N3T2(-1, -1,  1, -1, -1,  1, 1, 1),
                new VertexP3N3T2(-1,  1,  1, -1,  1,  1, 1, 0),
                new VertexP3N3T2(-1,  1, -1, -1,  1, -1, 0, 0),
                // Top face
                new VertexP3N3T2(-1,  1,  1, -1,  1,  1, 0, 1),
                new VertexP3N3T2( 1,  1,  1,  1,  1,  1, 1, 1),
                new VertexP3N3T2( 1,  1, -1,  1,  1, -1, 1, 0),
                new VertexP3N3T2(-1,  1, -1, -1,  1, -1, 0, 0),
                // Bottom face
                new VertexP3N3T2( 1, -1,  1,  1, -1,  1, 0, 1),
                new VertexP3N3T2(-1, -1,  1, -1, -1,  1, 1, 1),
                new VertexP3N3T2(-1, -1, -1, -1, -1, -1, 1, 0),
                new VertexP3N3T2( 1, -1, -1,  1, -1, -1, 0, 0)
            };

            Indices = new uint[] {
                // Font face
                0, 1, 2, 2, 3, 0,
                // Right face
                7, 5, 6, 5, 7, 4,
                // Back face
                11, 9, 10, 9, 11, 8,
                // Left face
                15, 13, 14, 13, 15, 12,
                // Top face
                19, 17, 18, 17, 19, 16,
                // Bottom face
                23, 21, 22, 21, 23, 20
            };
        }
    }
}
