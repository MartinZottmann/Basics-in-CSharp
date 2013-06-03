using MartinZottmann.Math;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace MartinZottmann.Entities
{
    class Textured : Physical
    {
        public Quad quad = Quad.Zero;

        public string texture_filename = "res/textures/pointer.png";

        public int texture_id;

        public Textured()
            : base()
        {
            texture_id = LoadTexture(texture_filename, false);
        }

        public int LoadTexture(string filename, bool mipmaped)
        {
            if (String.IsNullOrEmpty(filename))
            {
                throw new ArgumentException(filename);
            }
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(filename);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
            bmp.UnlockBits(bmp_data);

            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            if (mipmaped)
            {
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            }
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            }
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            return id;
        }

        public override void Render(double delta_time)
        {
            GL.PushMatrix();
            GL.Translate(Position.X, Position.Y, 0);
            GL.Rotate(Angle, Vector3d.UnitZ);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture_id);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex2(quad[0]);

            GL.TexCoord2(1, 0);
            GL.Vertex2(quad[1]);

            GL.TexCoord2(1, 1);
            GL.Vertex2(quad[2]);

            GL.TexCoord2(0, 1);
            GL.Vertex2(quad[3]);

            GL.End();
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();

#if DEBUG
            RenderVelocity(delta_time);
#endif
        }
    }
}
