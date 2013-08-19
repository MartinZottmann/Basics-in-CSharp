using MartinZottmann.Engine.Graphics.OpenGL;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class Graphic : Abstract, IDisposable
    {
        public Entity Model;

#if DEBUG
        [NonSerialized]
        public Entity DebugModel;
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
