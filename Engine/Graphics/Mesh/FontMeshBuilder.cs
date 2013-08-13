using FontStructure = System.Collections.Generic.Dictionary<char, System.Drawing.RectangleF>;
using MeshStructure = MartinZottmann.Engine.Graphics.Mesh.Mesh<MartinZottmann.Engine.Graphics.Mesh.VertexP3N3T2, uint>;

namespace MartinZottmann.Engine.Graphics.Mesh
{
    public class FontMeshBuilder
    {
        public FontStructure FontStructure { get; set; }

        public FontMeshBuilder(FontStructure font_structure)
        {
            FontStructure = font_structure;
        }

        public MeshStructure FromString(string @string)
        {
            const uint nv = 4;
            const uint ni = 6;
            int nc = FontStructure.Count / 2;
            var mesh = new MeshStructure(
                new VertexP3N3T2[@string.Length * nv],
                new uint[@string.Length * ni]
            );
            var offset_x = 0.0f;
            for (var i = 0; i < @string.Length; i++)
            {
                var character = FontStructure[@string[i]];
                var x = character.X;
                var y = character.Y;
                var w = character.Width;
                var h = character.Height;

                mesh.Vertices[i * nv + 0] = new VertexP3N3T2(0 + offset_x, 0, 0, 0, 0, 1, x, y + h);
                mesh.Vertices[i * nv + 1] = new VertexP3N3T2(w + offset_x, 0, 0, 0, 0, 1, x + w, y + h);
                mesh.Vertices[i * nv + 2] = new VertexP3N3T2(w + offset_x, h / nc, 0, 0, 0, 1, x + w, y);
                mesh.Vertices[i * nv + 3] = new VertexP3N3T2(0 + offset_x, h / nc, 0, 0, 0, 1, x, y);
                
                mesh.Indices[i * ni + 0] = 0 + (uint)(i * nv);
                mesh.Indices[i * ni + 1] = 1 + (uint)(i * nv);
                mesh.Indices[i * ni + 2] = 2 + (uint)(i * nv);
                mesh.Indices[i * ni + 3] = 2 + (uint)(i * nv);
                mesh.Indices[i * ni + 4] = 3 + (uint)(i * nv);
                mesh.Indices[i * ni + 5] = 0 + (uint)(i * nv);

                offset_x = w + offset_x;
            }
            return mesh;
        }
    }
}
