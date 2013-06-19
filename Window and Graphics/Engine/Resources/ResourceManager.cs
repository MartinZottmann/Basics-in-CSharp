using MartinZottmann.Engine.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine.Resources
{
    public class ResourceManager : IDisposable
    {
        public Programs Programs { get; set; }

        public Shaders Shaders { get; set; }

        public Textures Textures { get; set; }

        public ResourceManager()
        {
            Programs = new Programs(this);
            Shaders = new Shaders(this);
            Textures = new Textures(this);
        }

        public void Dispose()
        {
            Programs.Dispose();
            Shaders.Dispose();
            Textures.Dispose();
        }
    }
}
