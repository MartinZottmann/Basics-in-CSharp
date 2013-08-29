using System.Collections.Generic;
using System.Drawing;

namespace MartinZottmann.Engine.Graphics
{
    public class FontStructure : Dictionary<char, RectangleF>
    {
        public int ImageWidth;

        public int ImageHeight;

        public float LineSpacing;
    }
}
