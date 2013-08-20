using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics.OpenGL;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class GraphicComponent : IComponent, IDisposable
    {
        public MartinZottmann.Engine.Graphics.OpenGL.Entity Model;

#if DEBUG
        [NonSerialized]
        public MartinZottmann.Engine.Graphics.OpenGL.Entity DebugModel;
#endif

        public void Dispose()
        {
            Model.Dispose();
#if DEBUG
            DebugModel.Dispose();
#endif
        }
    }
}
