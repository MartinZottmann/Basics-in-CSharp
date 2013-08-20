using MartinZottmann.Engine.Graphics.OpenGL;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Engine.Resources
{
    public class Shaders : Resource<Shader>
    {
        public Shaders(ResourceManager resource_manager) : base(resource_manager) { }

        public Shader Load(ShaderType type, string filename)
        {
            if (!resources.ContainsKey(filename))
                this[filename] = new Shader(type, filename);

            return this[filename];
        }

        public Shader[] Load(ShaderType type, string[] filenames)
        {
            var sharders = new Shader[filenames.Length];

            foreach (var filename in filenames)
                sharders[sharders.Length] = Load(type, filename);

            return sharders;
        }
    }
}
