using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Graphics
{
    public class Grid : Model
    {
        public Grid()
        {
            var amount = 10;
            var space = 100;
            var vertices = new VertexP3C4[4 + 4 * (2 * amount + 1)];
            vertices[0] = new VertexP3C4(0, 1000, 0, 1, 0, 0, 0.5f);
            vertices[1] = new VertexP3C4(0, 0, 0, 1, 1, 1, 0.5f);
            vertices[2] = new VertexP3C4(0, 0, 0, 1, 1, 1, 0.5f);
            vertices[3] = new VertexP3C4(0, -1000, 0, 1, 0, 0, 0.5f);
            for (var i = -amount; i <= amount; i++)
            {
                vertices[4 + 4 * (i + amount) + 0] = new VertexP3C4(amount * space, 0, i * space, 1, 1, 1, 0.5f);
                vertices[4 + 4 * (i + amount) + 1] = new VertexP3C4(-amount * space, 0, i * space, 1, 1, 1, 0.5f);
                vertices[4 + 4 * (i + amount) + 2] = new VertexP3C4(i * space, 0, amount * space, 1, 1, 1, 0.5f);
                vertices[4 + 4 * (i + amount) + 3] = new VertexP3C4(i * space, 0, -amount * space, 1, 1, 1, 0.5f);
            }

            Mesh(new Mesh<VertexP3C4, uint>(vertices));
            Mode = PrimitiveType.Lines;
        }
    }
}
