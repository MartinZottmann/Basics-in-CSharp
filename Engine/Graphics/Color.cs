namespace MartinZottmann.Engine.Graphics
{
    public struct Color4
    {
        public float R, G, B, A;

        public Color4(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color4 Inverted()
        {
            return new Color4(1.0f - R, 1.0f - G, 1.0f - B, A);
        }
    }
}
