namespace MartinZottmann.Engine.Graphics.Shapes
{
    public class Quad : Mesh<VertexP3N3T2, uint>
    {
        public Quad()
        {
            Vertices = new VertexP3N3T2[] {
                new VertexP3N3T2(-1, -1, 0, 0, 0, 1, 0, 1),
                new VertexP3N3T2( 1, -1, 0, 0, 0, 1, 1, 1),
                new VertexP3N3T2( 1,  1, 0, 0, 0, 1, 1, 0),
                new VertexP3N3T2(-1,  1, 0, 0, 0, 1, 0, 0)
            };

            Indices = new uint[] {
                0, 1, 2, 2, 3, 0
            };
        }
    }
}
