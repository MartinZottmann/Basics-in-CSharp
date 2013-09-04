using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.Graphics.Wavefront
{
    public class MtlFile
    {
        public string Filename;

        public Dictionary<string, MtlInfo> Materials;

        public MtlFile(string filename)
        {
            Filename = filename;
            Materials = new Dictionary<string, MtlInfo>();
        }
    }
}
