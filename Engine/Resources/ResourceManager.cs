using System;

namespace MartinZottmann.Engine.Resources
{
    public class ResourceManager
    {
        public Models Models { get; set; }

        public Programs Programs { get; set; }

        public Shaders Shaders { get; set; }

        public Textures Textures { get; set; }

        public WavefrontObjFiles WavefrontObjFiles { get; set; }

        public ResourceManager()
        {
            Models = new Models(this);
            Programs = new Programs(this);
            Shaders = new Shaders(this);
            Textures = new Textures(this);
            WavefrontObjFiles = new WavefrontObjFiles(this);
        }

        public void Clear()
        {
            Models.Clear();
            Programs.Clear();
            Shaders.Clear();
            Textures.Clear();
            WavefrontObjFiles.Clear();
        }
    }
}
