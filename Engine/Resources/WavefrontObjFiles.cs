using MartinZottmann.Engine.Graphics.Wavefront;

namespace MartinZottmann.Engine.Resources
{
    public class WavefrontObjFiles : Resource<ObjFile>
    {
        public ObjLoader ObjLoader;

        public WavefrontObjFiles(ResourceManager resource_manager, ObjLoader obj_loader = null)
            : base(resource_manager)
        {
            ObjLoader = null == obj_loader
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
