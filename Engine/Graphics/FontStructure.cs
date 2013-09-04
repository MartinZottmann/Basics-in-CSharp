using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace MartinZottmann.Engine.Graphics
{
    [Serializable]
    public class FontStructure : Dictionary<char, RectangleF>, ISerializable
    {
        public int ImageWidth;

        public int ImageHeight;

        public float LineSpacing;

        public FontStructure() : base() { }

        protected FontStructure(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
