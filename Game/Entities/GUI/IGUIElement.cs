using MartinZottmann.Engine.Resources;
using OpenTK;

namespace MartinZottmann.Game.Entities.GUI
{
    public interface IGUIElement
    {
        Matrix4d ModelMatrix { get; }

        void Start(ResourceManager resource_manager);

        void Update(double delta_time);

        void Render(double delta_time);
    }
}
