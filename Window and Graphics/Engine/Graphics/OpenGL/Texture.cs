using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class Texture : IBindable, IDisposable
    {
        public int id;

        public TextureTarget target;

        public Texture(String text, Font font, Color textColor, Color backColor, bool mipmapped, SizeF size)
        {
            using (var img = new Bitmap((int)size.Width, (int)size.Height))
            using (var g = System.Drawing.Graphics.FromImage(img))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.Clear(backColor);
                using (var textBrush = new SolidBrush(textColor))
                {
                    g.DrawString(text, font, textBrush, 0, 0);
                    g.Save();
                }
                Init(img, mipmapped, TextureTarget.Texture2D);
            }
        }

        public Texture(String text, Font font, Color textColor, Color backColor, bool mipmapped, out SizeF size)
        {
            using (var img = new Bitmap(1, 1))
            using (var g = System.Drawing.Graphics.FromImage(img))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                size = g.MeasureString(text, font);
            }

            using (var img = new Bitmap((int)size.Width, (int)size.Height))
            using (var g = System.Drawing.Graphics.FromImage(img))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.Clear(backColor);
                using (var textBrush = new SolidBrush(textColor))
                {
                    g.DrawString(text, font, textBrush, 0, 0);
                    g.Save();
                }
                Init(img, mipmapped, TextureTarget.Texture2D);
            }
        }

        public Texture(string filename, bool mipmapped, TextureTarget target = TextureTarget.Texture2D)
        {
            using (var img = new Bitmap(filename))
                Init(img, mipmapped, target);
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
                OpenTK.Graphics.OpenGL.PixelInternalFormat bmp_if;
                OpenTK.Graphics.OpenGL.PixelFormat bmp_pf;
                OpenTK.Graphics.OpenGL.PixelType bmp_pt;

                switch (bmp.PixelFormat)
                {
                    case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                        bmp_if = OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgba;
                        bmp_pf = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
                        bmp_pt = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;
                        break;
                    default:
                        throw new NotImplementedException();

                }
                BitmapData bmp_data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadOnly, bmp.PixelFormat);
                switch (target)
                {
                    case TextureTarget.Texture2D:
                        GL.TexImage2D(target, 0, bmp_if, bmp_data.Width, bmp_data.Height, 0, bmp_pf, bmp_pt, bmp_data.Scan0);
                        break;
                    case TextureTarget.TextureCubeMap:
                        GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, bmp_if, bmp_data.Width, bmp_data.Height, 0, bmp_pf, bmp_pt, bmp_data.Scan0);
                        GL.TexImage2D(TextureTarget.TextureCubeMapNegativeX, 0, bmp_if, bmp_data.Width, bmp_data.Height, 0, bmp_pf, bmp_pt, bmp_data.Scan0);
                        GL.TexImage2D(TextureTarget.TextureCubeMapPositiveY, 0, bmp_if, bmp_data.Width, bmp_data.Height, 0, bmp_pf, bmp_pt, bmp_data.Scan0);
                        GL.TexImage2D(TextureTarget.TextureCubeMapNegativeY, 0, bmp_if, bmp_data.Width, bmp_data.Height, 0, bmp_pf, bmp_pt, bmp_data.Scan0);
                        GL.TexImage2D(TextureTarget.TextureCubeMapPositiveZ, 0, bmp_if, bmp_data.Width, bmp_data.Height, 0, bmp_pf, bmp_pt, bmp_data.Scan0);
                        GL.TexImage2D(TextureTarget.TextureCubeMapNegativeZ, 0, bmp_if, bmp_data.Width, bmp_data.Height, 0, bmp_pf, bmp_pt, bmp_data.Scan0);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                bmp.UnlockBits(bmp_data);

                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                if (mipmapped)
                {
                    GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                    GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                }
                else
                {
                    GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                }
                GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

                GL.TexParameter(target, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(target, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            }
        }

        public void Bind()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(target, id);
        }

        public void UnBind()
        {
            GL.BindTexture(target, 0);
            GL.Disable(EnableCap.Texture2D);
        }

        public void Dispose()
        {
            GL.DeleteTexture(id);
        }
    }
}
