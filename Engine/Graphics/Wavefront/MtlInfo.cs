using System;

namespace MartinZottmann.Engine.Graphics.Wavefront
{
    public struct MtlInfo : IDisposable
    {
        public string newmtl;

        public float? Ns;

        public float[] Ka;

        public float[] Kd;

        public float[] Ks;

        public float? Ni;

        public float? d;

        public byte? illum;

        /// <summary>
        /// Diffuse color texture map.
        /// </summary>
        public TextureMapFile? map_Kd;

        /// <summary>
        /// Specular color texture map.
        /// </summary>
        public TextureMapFile? map_Ks;

        /// <summary>
        /// Ambient color texture map.
        /// </summary>
        public TextureMapFile? map_Ka;

        /// <summary>
        /// Bump texture map.
        /// </summary>
        public TextureMapFile? map_Bump;

        /// <summary>
        /// Opacity texture map.
        /// </summary>
        public TextureMapFile? map_d;

        public MtlInfo(string name)
        {
            newmtl = name;
            Ns = null;
            Ka = null;
            Kd = null;
            Ks = null;
            Ni = null;
            d = null;
            illum = null;
            map_Kd = null;
            map_Ks = null;
            map_Ka = null;
            map_Bump = null;
            map_d = null;
        }

        public void Dispose()
        {
            if (null != map_Bump)
                map_Bump.Value.Dispose();
            if (null != map_d)
                map_d.Value.Dispose();
            if (null != map_Ka)
                map_Ka.Value.Dispose();
            if (null != map_Kd)
                map_Kd.Value.Dispose();
            if (null != map_Ks)
                map_Ks.Value.Dispose();
        }
    }
}
