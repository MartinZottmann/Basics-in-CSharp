using MartinZottmann.Graphics.OpenGL;

namespace MartinZottmann.Engine
{
    public class Resources
    {
        public Programs Programms { get; protected set; }

        public Shaders Shaders { get; protected set; }

        public Textures Textures { get; protected set; }

        public Resources()
        {
            Programms = new Programs(this);
            Shaders = new Shaders(this);
            Textures = new Textures(this);
        }
    }

    public class Programs : ResourceManager<MartinZottmann.Graphics.OpenGL.Program>
    {
        public Programs(Resources resources) : base(resources) { }

        public void Load(string name, string[] shader_files, string[] attribute_location_names)
        {
            if (resources.ContainsKey(name))
                return;

            var n = shader_files.Length;
            var shaders = new Shader[n];
            for (int i = 0; i < n; i++)
            {
                shaders[i] = Resources.Shaders[shader_files[i]];
            }

            this[name] = new MartinZottmann.Graphics.OpenGL.Program(shaders, attribute_location_names);
        }
    }

    public class Shaders : ResourceManager<MartinZottmann.Graphics.OpenGL.Shader>
    {
        public Shaders(Resources resources) : base(resources) { }

        public void Load(OpenTK.Graphics.OpenGL.ShaderType type, string filename)
        {
            if (resources.ContainsKey(filename))
                return;

            this[filename] = new Shader(type, filename);
        }
    }

    public class Textures : ResourceManager<Texture>
    {
        public Textures(Resources resources) : base(resources) { }

        //public override void LoadFromFile(string filename)
        //{
        //    Load(filename);
        //}

        public void Load(string filename, bool mipmapped = false, OpenTK.Graphics.OpenGL.TextureTarget target = OpenTK.Graphics.OpenGL.TextureTarget.Texture2D)
        {
            if (resources.ContainsKey(filename))
                return;

            this[filename] = new Texture(filename, mipmapped, target);
        }
    }
}
