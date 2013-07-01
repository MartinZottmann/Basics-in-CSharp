using MartinZottmann.Engine.Graphics.Wavefront;

namespace MartinZottmann.Engine.Resources
{
    public class WavefrontObjFiles : Resource<ObjFile>
    {
        public ObjLoader ObjLoader;

        public WavefrontObjFiles(ResourceManager resources, ObjLoader obj_loader = null)
            : base(resources)
        {
            ObjLoader = obj_loader == null
                ? new ObjLoader()
                : obj_loader;
        }

        public ObjFile Load(string name)
        {
            if (!resources.ContainsKey(name))
                this[name] = ObjLoader.Load(name);

            return this[name];
        }
    }
}
