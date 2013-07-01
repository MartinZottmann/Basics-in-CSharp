using MartinZottmann.Engine.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine.Resources
{
    public class ResourceManager : IDisposable
    {
        public Entities Entities { get; set; }

        public Programs Programs { get; set; }

        public Shaders Shaders { get; set; }

        public Textures Textures { get; set; }

        public WavefrontObjFiles WavefrontObjFiles { get; set; }

        public ResourceManager()
        {
            Entities = new Entities(this);
            Programs = new Programs(this);
            Shaders = new Shaders(this);
            Textures = new Textures(this);
            WavefrontObjFiles = new WavefrontObjFiles(this);
        }

        public void Dispose()
        {
            Entities.Dispose();
            Programs.Dispose();
            Shaders.Dispose();
            Textures.Dispose();
            WavefrontObjFiles.Dispose();
        }
    }
}
