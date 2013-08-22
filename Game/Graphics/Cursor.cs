using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Graphics
{
    public class Cursor : Model
    {
        public Cursor()
        {
            Mesh(
                new Mesh<VertexP3C4, uint>(
                    new VertexP3C4[] {
                        new VertexP3C4(-1, 0, 0, 1, 1, 1, 1),
                        new VertexP3C4(0, 0, 1, 1, 0, 1, 1),
                        new VertexP3C4(1, 0, 0, 1, 1, 1, 1),
                        new VertexP3C4(0, 0, -1, 0, 1, 1, 1)
                    },
                    new uint[] { 0, 1, 2, 3 }
                )
            );
            Mode = BeginMode.LineLoop;
        }
    }
}
