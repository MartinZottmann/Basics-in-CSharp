using MartinZottmann.Engine.Resources;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MartinZottmann.Game.Resources
{
    public class ResourceLoader
    {
        public ResourceManager Manager { get; set; }

        public ResourceLoader() : this(new ResourceManager()) { }

        public ResourceLoader(ResourceManager manager)
        {
            Manager = manager;
        }

        public void LoadPrograms(string path, string pattern)
        {
            var map = new Dictionary<string, ShaderType>() {
                { "vs", ShaderType.VertexShader },
                { "gs", ShaderType.GeometryShader },
                { "fs", ShaderType.FragmentShader }
            };

            Directory
                .GetFiles(path, pattern)
                .Select(s => new { filename = s, chunks = s.Split(new char[] { '/', '.' }) })
                .Select(s => new { key = s.chunks[s.chunks.Length - 3], value = Manager.Shaders.Load(map[s.chunks[s.chunks.Length - 2]], s.filename) })
                .GroupBy(s => s.key)
                .Select(s => Manager.Programs.Load(s.Key, s.Select(t => t.value).ToArray()))
                .ToList();
        }

        public void LoadTextures(string path, string pattern)
        {
            foreach (var filename in Directory.GetFiles("Resources/Textures/", "*.png"))
                if (filename.Contains("/debug-"))
                    Manager.Textures.Load(filename);
                else
                    Manager.Textures.Load(filename, true, TextureTarget.Texture2D, TextureMinFilter.Linear, TextureMagFilter.Linear);
        }
    }
}
