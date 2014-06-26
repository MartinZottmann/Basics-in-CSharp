using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities.GUI
{
    public class Crosshair : IGUIElement
    {
        public const uint SIZE = 10;

        public Matrix4d ModelMatrix { get { return Matrix4d.CreateTranslation(0.5, 0.5, 0.0); } }

        public Model Model { get; set; }

        protected ResourceManager resource_manager;

        public void Bind(ResourceManager resource_manager, FontMeshBuilder font_mesh_builder)
        {
            this.resource_manager = resource_manager;

            InitModel();
        }

        public void Update(double delta_time) { }

        public void Render(double delta_time)
        {
            Model.Draw();
        }

        protected void InitModel()
        {
            var mesh = new Mesh<VertexP3C4, uint>(
                new VertexP3C4[] {
                    new VertexP3C4(SIZE, 0, 0, 1, 1, 1, 1),
                    new VertexP3C4(-SIZE, 0, 0, 1, 1, 1, 1),
                    new VertexP3C4(0, SIZE, 0, 1, 1, 1, 1),
                    new VertexP3C4(0, -SIZE, 0, 1, 1, 1, 1),
                }
            );

            Model = new Model();
            Model.Mode = PrimitiveType.Lines;
            Model.Mesh(mesh);
            Model.Program = resource_manager.Programs["normal"];
            Model.Texture = null;
        }
    }
}
