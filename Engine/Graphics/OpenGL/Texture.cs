using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class Texture : IBindable, IDisposable
    {
        public readonly uint Id;

#if DEBUG
        public readonly string Name;
#endif

        public uint BindId { get; protected set; }

        public readonly TextureTarget Target;

        protected readonly EnableCap gl_texture_capability;

        // @todo Implement per render contexts.
        protected static uint bind_stack = 0;

        public Texture(String text, Font font, Color textColor, Color backColor, bool mipmapped, SizeF size)
            : this(TextureTarget.Texture2D)
        {
            using (var img = new Bitmap((int)size.Width, (int)size.Height))
            {
                using (var g = System.Drawing.Graphics.FromImage(img))
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.Clear(backColor);
                    using (var textBrush = new SolidBrush(textColor))
                        g.DrawString(text, font, textBrush, 0, 0);
                    g.Save();
                }
                Init(img, mipmapped, Target, mipmapped ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear, TextureMagFilter.Linear);
            }
        }

        public Texture(String text, Font font, Color text_color, Color text_outline_color, Color background_color, bool mipmapped, out SizeF size)
            : this(TextureTarget.Texture2D)
        {
            using (var img = new Bitmap(1, 1))
            using (var g = System.Drawing.Graphics.FromImage(img))
            {
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                size = g.MeasureString(text, font);
            }

            using (var img = new Bitmap((int)size.Width, (int)size.Height))
            {
                using (var g = System.Drawing.Graphics.FromImage(img))
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.Clear(background_color);
                    using (var textBrush = new SolidBrush(text_color))
                        g.DrawString(text, font, textBrush, 0, 0);
                    g.Save();
                }
                Init(img, mipmapped, Target, mipmapped ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear, TextureMagFilter.Linear);
            }
        }

        public Texture(Font font, Color text_color, Color text_outline_color, Color background_color, bool mipmapped, out FontStructure font_map_normalized)
            : this(TextureTarget.Texture2D)
        {
#if DEBUG
            Name = "Font map";
#endif
            var font_map = new FontStructure();
            font_map_normalized = new FontStructure();
            var height = 0.0f;
            var width = 0.0f;

            using (var img = new Bitmap(1, 1))
            using (var g = System.Drawing.Graphics.FromImage(img))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                for (var i = 0; i < 128; i++)
                {
                    var size = g.MeasureString(((char)i).ToString(), font);
                    height = Math.Max(height, size.Height);
                    font_map.Add((char)i, new RectangleF(width, 0.0f, size.Width, size.Height));
                    width += size.Width;
                }
            }

            foreach (var character in font_map)
            {
                font_map_normalized.Add(
                    character.Key,
                    new RectangleF(
                        character.Value.X / width,
                        character.Value.Y / height,
                        character.Value.Width / width,
                        character.Value.Height / height
                    )
                );
            }

            using (var img = new Bitmap((int)width, (int)height))
            {
                using (var g = System.Drawing.Graphics.FromImage(img))
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.Clear(background_color);
                    using (var text_brush = new SolidBrush(text_color))
                        foreach (var character in font_map)
                        {
                            if (text_outline_color != Color.Transparent)
                                using (var text_outline_brush = new SolidBrush(text_outline_color))
                                    for (var x = -1; x <= 1; x++)
                                        for (var y = -1; y <= 1; y++)
                                            g.DrawString(character.Key.ToString(), font, text_outline_brush, x + character.Value.X, y);
                            g.DrawString(character.Key.ToString(), font, text_brush, character.Value.X, 0);
                        }
                    g.Save();
                }
                Init(img, mipmapped, Target, mipmapped ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear, TextureMagFilter.Linear);
            }
        }

        public Texture(string filename, bool mipmapped, TextureTarget target = TextureTarget.Texture2D)
            : this(target)
        {
#if DEBUG
            Name = filename;
#endif
            using (var img = new Bitmap(filename))
                Init(img, mipmapped, target);
        }

        public Texture(Bitmap bmp, bool mipmapped = false, TextureTarget target = TextureTarget.Texture2D)
            : this(target)
        {
            Init(bmp, mipmapped, target);
        }

        public Texture(TextureTarget target = TextureTarget.Texture2D)
        {
            Id = (uint)GL.GenTexture();
            Target = target;

            switch (Target)
            {
                case TextureTarget.Texture2D:
                    gl_texture_capability = EnableCap.Texture2D;
                    break;
                case TextureTarget.TextureCubeMap:
                    gl_texture_capability = EnableCap.TextureCubeMap;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected void Init(Bitmap bmp, bool mipmapped, TextureTarget target, TextureMinFilter min_filter = TextureMinFilter.Nearest, TextureMagFilter mag_filter = TextureMagFilter.Nearest)
        {
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

                if (mipmapped)
                    GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)min_filter);
                GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int)mag_filter);

                GL.TexParameter(target, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(target, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(target, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            }
        }

        public void Bind()
        {
            BindId = bind_stack;
            GL.Enable(gl_texture_capability);
            GL.ActiveTexture((TextureUnit)((uint)TextureUnit.Texture0 + bind_stack));
            GL.BindTexture(Target, Id);
            bind_stack++;
        }

        public void UnBind()
        {
            GL.BindTexture(Target, 0);
            GL.Disable(gl_texture_capability);
            bind_stack--;
            BindId = 0;
        }

        public void Dispose()
        {
            GL.DeleteTexture(Id);
        }
    }
}
