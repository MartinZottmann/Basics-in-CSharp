using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;

namespace MartinZottmann.Game.Entities.GUI
{
    public interface IGUIElement
    {
        Matrix4d ModelMatrix { get; }

        Model Model { get; set; }

        void Bind(ResourceManager resource_manager, FontMeshBuilder font_mesh_builder);

        void Update(double delta_time);

        void Render(double delta_time);
    }
}
