using System;

namespace MartinZottmann.Engine.Graphics.Wavefront
{
    public struct TextureMapFile : IDisposable
    {
        public string Filename;

        public TextureMapFile(string filename)
        {
            Filename = filename;
        }

        public void Dispose()
        {
            // @todo
        }
    }
}
