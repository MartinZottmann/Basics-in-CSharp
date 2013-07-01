using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.Graphics.Wavefront
{
    public class MtlFile : IDisposable
    {
        public string Filename;

        public Dictionary<string, MtlInfo> Materials;

        public MtlFile(string filename)
        {
            Filename = filename;
            Materials = new Dictionary<string, MtlInfo>();
        }

        public void Dispose()
        {
            foreach (var material in Materials)
                material.Value.Dispose();
        }
    }
}
