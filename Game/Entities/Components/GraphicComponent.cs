using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics.OpenGL;
using System;
using System.Xml.Serialization;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class GraphicComponent : IComponent
    {
        public string ModelName;

        public string ProgramName;

        public string TextureName;

        [NonSerialized]
        [XmlIgnore]
        public Model Model;
    }
}
