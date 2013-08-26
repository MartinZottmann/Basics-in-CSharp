using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics.OpenGL;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class GraphicComponent : IComponent, IDisposable
    {
        public string ModelName;

        public string ProgramName;

        public string TextureName;

        [NonSerialized]
        internal Model Model;

#if DEBUG
        [NonSerialized]
        internal Model DebugModel;
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
