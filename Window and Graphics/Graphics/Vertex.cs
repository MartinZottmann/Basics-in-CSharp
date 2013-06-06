using OpenTK;

namespace MartinZottmann.Graphics
{
    public interface IVertex { }

    public struct Vertex3 : IVertex
    {
        public float x, y, z;

        //public float r, g, b, a;

        public const int PositionOffset = 0;
    }

    public struct Vertex3_PC
    {
        public float x, y, z;

        public float r, g, b, a;

        public const int PositionOffset = 0;

        public const int ColorOffset = sizeof(float) * 3;
    }
}
