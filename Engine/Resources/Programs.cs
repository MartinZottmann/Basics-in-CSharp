using MartinZottmann.Engine.Graphics.OpenGL;

namespace MartinZottmann.Engine.Resources
{
    public class Programs : Resource<Program>
    {
        public Programs(ResourceManager resources) : base(resources) { }

        public Program Load(string name, string[] shader_files)
        {
            if (!resources.ContainsKey(name))
            {
                var n = shader_files.Length;
                var shaders = new Shader[n];
                for (int i = 0; i < n; i++)
                    shaders[i] = Resources.Shaders[shader_files[i]];

                this[name] = new Program(shaders);
            }

            return this[name];
        }

        public Program Load(string name, Shader[] shaders)
        {
            if (!resources.ContainsKey(name))
                this[name] = new Program(shaders);

            return this[name];
        }
    }
}
