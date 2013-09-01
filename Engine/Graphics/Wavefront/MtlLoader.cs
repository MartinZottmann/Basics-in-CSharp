using System;
using System.Collections.Generic;
using System.IO;

namespace MartinZottmann.Engine.Graphics.Wavefront
{
    public class MtlLoader
    {
        public MtlFile Load(string filepath)
        {
            var file_info = new FileInfo(filepath);
            return Load(file_info.DirectoryName, file_info.Name);
        }

        public MtlFile Load(string directory, string filename_or_filepath)
        {
            var texture_map_loader = new TextureMapLoader();
            var filepath = File.Exists(filename_or_filepath) ? filename_or_filepath : directory + Path.DirectorySeparatorChar + filename_or_filepath;
            var stream_reader = new StreamReader(filepath);
            var mtl_file = new MtlFile(filepath);
            MtlInfo mtl_info = new MtlInfo();

            while (true)
            {
                var line = stream_reader.ReadLine();
                if (null == line)
                    break;

                string[] tokens = line.Split(new char[] { ' ' }, 2);
                if (tokens.Length < 2)
                    continue;

                string token = tokens[0];
                string content = tokens[1];

                switch (token)
                {
                    case "#":
                        break;
                    case "newmtl":
                        mtl_info = new MtlInfo(content);
                        mtl_file.Materials.Add(mtl_info.newmtl, mtl_info);
                        break;
                    case "Ns":
                        mtl_info.Ns = Single.Parse(content);
                        break;
                    case "Ka":
                        mtl_info.Ka = ParseFloats(content, ' ');
                        break;
                    case "Kd":
                        mtl_info.Kd = ParseFloats(content, ' ');
                        break;
                    case "Ks":
                        mtl_info.Ks = ParseFloats(content, ' ');
                        break;
                    case "Ni":
                        mtl_info.Ni = Single.Parse(content);
                        break;
                    case "d":
                        mtl_info.d = Single.Parse(content);
                        break;
                    case "illum":
                        mtl_info.illum = Byte.Parse(content);
                        break;
                    case "map_Kd":
                        mtl_info.map_Kd = texture_map_loader.Load(directory, content);
                        break;
                    case "map_Ks":
                        mtl_info.map_Ks = texture_map_loader.Load(directory, content);
                        break;
                    case "map_Ka":
                        mtl_info.map_Ka = texture_map_loader.Load(directory, content);
                        break;
                    case "bump":
                    case "map_Bump":
                        mtl_info.map_Bump = texture_map_loader.Load(directory, content);
                        break;
                    case "map_d":
                        mtl_info.map_d = texture_map_loader.Load(directory, content);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return mtl_file;
        }

        protected float[] ParseFloats(string content, char separator)
        {
            var v = content.Split(separator);
            var result = new float[v.Length];
            for (var i = 0; i < v.Length; i++)
                result[i] = Single.Parse(v[i]);
            return result;
        }
    }
}