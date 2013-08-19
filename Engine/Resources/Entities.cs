using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Wavefront;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace MartinZottmann.Engine.Resources
{
    public class Entities : Resource<Entity>
    {
        public Entities(ResourceManager resources) : base(resources) { }

        public Entity Load(string filename, Matrix4? transformation = null)
        {
            var key = transformation == null
                ? filename
                : String.Format("{0} as {1}", filename, transformation.Value);

            if (!resources.ContainsKey(key))
                this[key] = Entity(Resources.WavefrontObjFiles.Load(filename), transformation);

            return this[key];
        }

        public Entity Entity(ObjFile obj_file, Matrix4? transformation = null)
        {
            var n = obj_file.f.Count;
            var m = obj_file.f[0].v.Length;
            var vertices = new VertexP3N3T2[n * m];
            var indices = new uint[n * m];
            for (var i = 0; i < n; i++)
            {
                var fi = obj_file.f[i];
                Debug.Assert(fi.v.Length == fi.vn.Length);
                Debug.Assert(fi.vn.Length == fi.vt.Length);
                for (var j = 0; j < fi.v.Length; j++)
                {
                    var vj = (int)fi.v[j] - 1;
                    var vnj = (int)fi.vn[j] - 1;
                    var vtj = (int)fi.vt[j] - 1;
                    vertices[i * m + j] = new VertexP3N3T2(obj_file.v[vj][0], obj_file.v[vj][1], obj_file.v[vj][2], obj_file.vn[vnj][0], obj_file.vn[vnj][1], obj_file.vn[vnj][2], obj_file.vt[vtj][0], obj_file.vt[vtj][1]);
                    indices[i * m + j] = (uint)(i * m + j);
                }
            }
            var mesh = new Mesh<VertexP3N3T2, uint>(vertices, indices);
            if (transformation != null)
                mesh.Translate(transformation.Value);

            var entity = new Entity();
            entity.Mesh(mesh);
            switch (obj_file.f[0].v.Length)
            {
                case 3:
                    entity.Mode = BeginMode.Triangles;
                    break;
                case 4:
                    entity.Mode = BeginMode.Quads;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return entity;
        }
    }
}
