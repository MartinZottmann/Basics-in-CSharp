using MartinZottmann.Engine.Graphics.OpenGL;

namespace MartinZottmann.Engine.Resources
{
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
}
