using MartinZottmann.Engine.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine.Resources
{
    public class Resources : IDisposable
    {
        public Programs Programs { get; protected set; }

        public Shaders Shaders { get; protected set; }

        public Textures Textures { get; protected set; }

        public Resources()
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
