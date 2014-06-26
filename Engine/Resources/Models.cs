using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Wavefront;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MartinZottmann.Engine.Resources
{
    public class Models : Resource<Model>
    {
        public Models(ResourceManager resource_manager) : base(resource_manager) { }

        public Model Load(string filename)
        {
            if (!resources.ContainsKey(filename))
                this[filename] = Load(ResourceManager.WavefrontObjFiles.Load(filename));

            return this[filename];
        }

        public Model Load(ObjFile obj_file)
        {
            var nv = 0;
            var ni = 0;
            var n = obj_file.f.Count;
            var m = obj_file.f[0].v.Length;
            var vertices = new VertexP3N3T2[n * m];
            var indices = new List<uint>();
            for (var i = 0; i < n; i++)
            {
                var fi = obj_file.f[i];
                Debug.Assert(fi.v.Length == fi.vn.Length);
                Debug.Assert(fi.vn.Length == fi.vt.Length);
                for (var j = 0; j < fi.v.Length; j++)
                {
                    var vj = fi.v[j] - 1;
                    if (vj < 0)
                        vj += obj_file.v.Count + 1;
                    var vnj = fi.vn[j] - 1;
                    if (vnj < 0)
                        vnj += obj_file.vn.Count + 1;
                    var vtj = fi.vt[j] - 1;
                    if (vtj < 0)
                        vtj += obj_file.vt.Count + 1;
                    vertices[nv] = new VertexP3N3T2(obj_file.v[vj][0], obj_file.v[vj][1], obj_file.v[vj][2], obj_file.vn[vnj][0], obj_file.vn[vnj][1], obj_file.vn[vnj][2], obj_file.vt[vtj][0], 1 - obj_file.vt[vtj][1]);
                    nv++;
                }
                switch (fi.v.Length)
                {
                    case 3:
                        indices.Add((uint)(ni + 0));
                        indices.Add((uint)(ni + 1));
                        indices.Add((uint)(ni + 2));
                        ni += fi.v.Length;
                        break;
                    case 4:
                        indices.Add((uint)(ni + 0));
                        indices.Add((uint)(ni + 1));
                        indices.Add((uint)(ni + 2));
                        indices.Add((uint)(ni + 2));
                        indices.Add((uint)(ni + 3));
                        indices.Add((uint)(ni + 0));
                        ni += fi.v.Length;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            var model = new Model();
            model.Mesh(new Mesh<VertexP3N3T2, uint>(vertices, indices.ToArray()));
            model.Mode = PrimitiveType.Triangles;
            return model;
        }
    }
}
