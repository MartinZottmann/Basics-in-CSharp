using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace MartinZottmann.Graphics.OpenGL
{
    public class Texture : IBindable, IDisposable
    {
        public int id;

        public TextureTarget target;

        public Texture(String text, Font font, Color textColor, Color backColor, bool mipmapped, out SizeF size)
        {
            using (var img = new Bitmap(1, 1))
            using (var g = System.Drawing.Graphics.FromImage(img))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                size = g.MeasureString(text, font);
            }

            var bmp = new Bitmap((int)size.Width, (int)size.Height);
            using (var g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.Clear(backColor);
                using (var textBrush = new SolidBrush(textColor))
                {
                    g.DrawString(text, font, textBrush, 0, 0);
                    g.Save();
                }
            }

            Init(bmp, mipmapped, TextureTarget.Texture2D);
        }

        public Texture(string filename, bool mipmapped, TextureTarget target = TextureTarget.Texture2D)
        {
            Init(new Bitmap(filename), mipmapped, target);
        }

        public Texture(Bitmap bmp, bool mipmapped = false, TextureTarget target = TextureTarget.Texture2D)
        {
            Init(bmp, mipmapped, target);
        }

        protected void Init(Bitmap bmp, bool mipmapped, TextureTarget target)
        {
            id = GL.GenTexture();
            this.target = target;

            using (new Bind(this))
            {
                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
                bmp.UnlockBits(bmp_data);

                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                if (mipmapped)
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
            }
        }

        public void Bind()
        {
            GL.BindTexture(target, id);
        }

        public void UnBind()
        {
            GL.BindTexture(target, 0);
        }

        public void Dispose()
        {
            GL.DeleteTexture(id);
        }
    }
}
