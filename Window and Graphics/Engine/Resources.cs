using MartinZottmann.Engine.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine
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

    public class Programs : ResourceManager<Engine.Graphics.OpenGL.Program>
    {
        public Programs(Resources resources) : base(resources) { }

        public Engine.Graphics.OpenGL.Program Load(string name, string[] shader_files, string[] attribute_location_names)
        {
            if (!resources.ContainsKey(name))
            {
                var n = shader_files.Length;
                var shaders = new Shader[n];
                for (int i = 0; i < n; i++)
                    shaders[i] = Resources.Shaders[shader_files[i]];

                this[name] = new Engine.Graphics.OpenGL.Program(shaders, attribute_location_names);
            }

            return this[name];
        }

        public Engine.Graphics.OpenGL.Program Load(string name, Shader[] shaders, string[] attribute_location_names)
        {
            if (!resources.ContainsKey(name))
                this[name] = new Engine.Graphics.OpenGL.Program(shaders, attribute_location_names);

            return this[name];
        }
    }

    public class Shaders : ResourceManager<Shader>
    {
        public Shaders(Resources resources) : base(resources) { }

        public Shader Load(OpenTK.Graphics.OpenGL.ShaderType type, string filename)
        {
            if (!resources.ContainsKey(filename))
                this[filename] = new Shader(type, filename);

            return this[filename];
        }
    }

    public class Textures : ResourceManager<Texture>
    {
        public Textures(Resources resources) : base(resources) { }

        //public override void LoadFromFile(string filename)
        //{
        //    Load(filename);
        //}

        public Texture Load(string filename, bool mipmapped = false, OpenTK.Graphics.OpenGL.TextureTarget target = OpenTK.Graphics.OpenGL.TextureTarget.Texture2D)
        {
            if (!resources.ContainsKey(filename))
                this[filename] = new Texture(filename, mipmapped, target);

            return this[filename];
        }
    }
}
