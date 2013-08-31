using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Game.Entities.Components;

namespace MartinZottmann.Game.Entities.Nodes
{
    public class GraphicNode : Node
    {
        public BaseComponent Base;

        public GraphicComponent Graphic;

#if DEBUG
        public Model ModelDebugOrientation;

        public Model ModelDebugPhysic;
#endif
    }
}
