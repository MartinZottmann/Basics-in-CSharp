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
            int nc = FontStructure.Count;
            var mesh = new MeshStructure(
                new VertexP3N3T2[@string.Length * nv],
                new uint[@string.Length * ni]
            );
            var offset_x = 0f;
            var offset_y = 0f;
            for (var i = 0; i < @string.Length; i++)
            {
                if (@string[i] == '\n')
                {
                    offset_x = 0;
                    offset_y -= FontStructure.LineSpacing;
                    continue;
                }

                var character = FontStructure[@string[i]];
                var vx = character.X;
                var vy = character.Y;
                var vw = character.Width;
                var vh = character.Height;
                var tx = character.X / FontStructure.ImageWidth;
                var ty = character.Y / FontStructure.ImageHeight;
                var tw = character.Width / FontStructure.ImageWidth;
                var th = character.Height / FontStructure.ImageHeight;

                if (offset_y == 0f)
                    offset_y -= vh;

                mesh.Vertices[i * nv + 0] = new VertexP3N3T2(offset_x, offset_y, 0, 0, 0, 1, tx, ty + th);
                mesh.Vertices[i * nv + 1] = new VertexP3N3T2(offset_x + vw, offset_y, 0, 0, 0, 1, tx + tw, ty + th);
                mesh.Vertices[i * nv + 2] = new VertexP3N3T2(offset_x + vw, offset_y + vh, 0, 0, 0, 1, tx + tw, ty);
                mesh.Vertices[i * nv + 3] = new VertexP3N3T2(offset_x, offset_y + vh, 0, 0, 0, 1, tx, ty);

                mesh.Indices[i * ni + 0] = 0 + (uint)(i * nv);
                mesh.Indices[i * ni + 1] = 1 + (uint)(i * nv);
                mesh.Indices[i * ni + 2] = 2 + (uint)(i * nv);
                mesh.Indices[i * ni + 3] = 2 + (uint)(i * nv);
                mesh.Indices[i * ni + 4] = 3 + (uint)(i * nv);
                mesh.Indices[i * ni + 5] = 0 + (uint)(i * nv);

                offset_x += vw;
            }
            return mesh;
        }
    }
}
