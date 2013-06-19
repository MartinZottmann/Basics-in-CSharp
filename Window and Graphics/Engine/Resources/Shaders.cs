using MartinZottmann.Engine.Graphics.OpenGL;

namespace MartinZottmann.Engine.Resources
{
    public class Shaders : Resource<Shader>
    {
        public Shaders(ResourceManager resources) : base(resources) { }

        public Shader Load(OpenTK.Graphics.OpenGL.ShaderType type, string filename)
        {
            if (!resources.ContainsKey(filename))
                this[filename] = new Shader(type, filename);

            return this[filename];
        }
    }
}
