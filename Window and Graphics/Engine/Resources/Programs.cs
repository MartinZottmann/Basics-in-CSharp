using MartinZottmann.Engine.Graphics.OpenGL;

namespace MartinZottmann.Engine.Resources
{
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
}
