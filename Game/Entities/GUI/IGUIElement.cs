using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;

namespace MartinZottmann.Game.Entities.GUI
{
    public interface IGUIElement
    {
        //Vector3d Position { get; set; }

        //Texture FontTexture { get; set; }

        //FontMeshBuilder FontMeshBuilder { get; set; }

        //Model Model { get; set; }

        Matrix4d ModelMatrix { get; }

        void Start(ResourceManager resource_manager);

        void Update(double delta_time);

        void Render(double delta_time);
    }
}
