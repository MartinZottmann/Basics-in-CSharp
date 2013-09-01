using System.IO;
using System.Collections.Generic;
using System;

namespace MartinZottmann.Engine.Graphics.Wavefront
{
    public class ObjLoader
    {
        public ObjFile Load(string filepath)
        {
            var file_info = new FileInfo(filepath);
            return Load(file_info.DirectoryName, file_info.Name);
        }

        public ObjFile Load(string directory, string filename)
        {
            var filepath = directory + Path.DirectorySeparatorChar + filename;
            var mtl_loader = new MtlLoader();
            var stream_reader = new StreamReader(filepath);
            var obj_file = new ObjFile(filepath);

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
                    case "v":
                        obj_file.v.Add(ParseFloats(content, ' '));
                        break;
                    case "vt":
                        obj_file.vt.Add(ParseFloats(content, ' '));
                        break;
                    case "vn":
                        obj_file.vn.Add(ParseFloats(content, ' '));
                        break;
                    case "mtllib":
                        obj_file.mtllib.Add(mtl_loader.Load(directory, content));
                        break;
                    case "o":
                        // @todo
                        break;
                    case "usemtl":
                        // @todo
                        break;
                    case "s":
                        // @todo
                        break;
                    case "f":
                        obj_file.f.Add(ParseFace(content));
                        break;
                    case "g":
                        // @todo
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return obj_file;
        }

        protected float[] ParseFloats(string content, char separator)
        {
            var v = content.Split(new char[] { separator });
            var result = new float[v.Length];
            for (var i = 0; i < v.Length; i++)
                result[i] = Single.Parse(v[i]);
            return result;
        }

        protected FaceInfo ParseFace(string content)
        {
            var tokens0 = content.Split(' ');
            var result = new FaceInfo();
            result.v = new int[tokens0.Length];
            result.vt = new int[tokens0.Length];
            result.vn = new int[tokens0.Length];
            for (var i = 0; i < tokens0.Length; i++)
            {
                var tokens1 = tokens0[i].Split('/');
                switch (tokens1.Length)
                {
                    case 1:
                        result.v[i] = Int32.Parse(tokens1[0]);
                        break;
                    case 2:
                        result.v[i] = Int32.Parse(tokens1[0]);
                        result.vt[i] = Int32.Parse(tokens1[1]);
                        break;
                    case 3:
                        result.v[i] = Int32.Parse(tokens1[0]);
                        result.vt[i] = tokens1[1] == "" ? 0 : Int32.Parse(tokens1[1]);
                        result.vn[i] = Int32.Parse(tokens1[2]);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return result;
        }
    }
}
