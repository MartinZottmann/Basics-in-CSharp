using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics.OpenGL;
using System;
using System.Xml.Serialization;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class GraphicComponent : IComponent, IDisposable
    {
        public string ModelName;

        public string ProgramName;

        public string TextureName;

        [XmlIgnore]
        public Model Model;

#if DEBUG
        [XmlIgnore]
        public Model DebugModel;
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
