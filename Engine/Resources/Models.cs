﻿using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Wavefront;
using OpenTK.Graphics.OpenGL;
using System;
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

            var model = new Model();
            model.Mesh(new Mesh<VertexP3N3T2, uint>(vertices, indices));
            switch (obj_file.f[0].v.Length)
            {
                case 3:
                    model.Mode = BeginMode.Triangles;
                    break;
                case 4:
                    model.Mode = BeginMode.Quads;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return model;
        }
    }
}