using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.Graphics.Wavefront
{
    public struct ObjFile : IDisposable
    {
        public string Filename;

        public List<float[]> v;

        public List<float[]> vt;

        public List<float[]> vn;

        public List<MtlFile> mtllib;

        public List<FaceInfo> f;

        public ObjFile(string filename)
        {
            Filename = filename;
            v = new List<float[]>();
            vt = new List<float[]>();
            vn = new List<float[]>();
            mtllib = new List<MtlFile>();
            f = new List<FaceInfo>();
        }

        public void Dispose()
        {
            mtllib.ForEach(s => s.Dispose());
        }
    }
}
