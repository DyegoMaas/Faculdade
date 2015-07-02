using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace JogoLabirinto.ObjetosGraficos
{

    public static class ContentManager
    {
        public static int[] TexturasSkybox;
        public static int[] TexturasCaixa;

        public static void CarregarTexturas()
        {
            var diretorioTexturas = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens", "Skybox"));
            TexturasSkybox = new int[6];
            TexturasSkybox[0] = CarregarTextura(Path.Combine(diretorioTexturas.FullName, "Night_01A_front.png"));
            TexturasSkybox[1] = CarregarTextura(Path.Combine(diretorioTexturas.FullName, "Night_01A_left.png"));
            TexturasSkybox[2] = CarregarTextura(Path.Combine(diretorioTexturas.FullName, "Night_01A_back.png"));
            TexturasSkybox[3] = CarregarTextura(Path.Combine(diretorioTexturas.FullName, "Night_01A_right.png"));
            TexturasSkybox[4] = CarregarTextura(Path.Combine(diretorioTexturas.FullName, "Night_01A_up.png"));
            TexturasSkybox[5] = CarregarTextura(Path.Combine(diretorioTexturas.FullName, "Night_01A_down.png"));

            diretorioTexturas = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens", "Caixa"));
            TexturasCaixa = new int[2];
            TexturasCaixa[0] = CarregarTextura(Path.Combine(diretorioTexturas.FullName, "woodBox_12.jpg"));
            TexturasCaixa[0] = CarregarTextura(Path.Combine(diretorioTexturas.FullName, "woodBox_12_nm.jpg"));
        }

        private static int CarregarTextura(string caminhoArquivo)
        {
            if (String.IsNullOrEmpty(caminhoArquivo))
                throw new ArgumentException(caminhoArquivo);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Bitmap bmp = new Bitmap(caminhoArquivo);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            return id;
        }
    }
    //https://sidvind.com/wiki/Skybox_tutorial
    public class Skybox : ObjetoGrafico
    {
        protected override void DesenharObjeto()
        {
            GL.Translate(-500, -1400, -500);
            GL.PushMatrix();

            GL.Scale(3000,3000,3000);

            GL.PushAttrib(AttribMask.EnableBit);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);

            GL.Color4(Color4.White);

            var texturas = ContentManager.TexturasSkybox;

            // Front
            GL.BindTexture(TextureTarget.Texture2D, texturas[0]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(0.0, 0.0, -1.0);

                GL.TexCoord2(0, 1);
                GL.Vertex3(0.5f, -0.5f, -0.5f);

                GL.TexCoord2(1, 1);
                GL.Vertex3(-0.5f, -0.5f, -0.5f);

                GL.TexCoord2(1, 0);
                GL.Vertex3(-0.5f, 0.5f, -0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(0.5f, 0.5f, -0.5f);
            }
            GL.End();

            // Left
            GL.BindTexture(TextureTarget.Texture2D, texturas[1]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(1, 0, 0);

                GL.TexCoord2(1, 1);
                GL.Vertex3(0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 1);
                GL.Vertex3(0.5f, -0.5f, -0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(0.5f, 0.5f, -0.5f);

                GL.TexCoord2(1, 0);
                GL.Vertex3(0.5f, 0.5f, 0.5f);
            }
            GL.End();

            // Back
            GL.BindTexture(TextureTarget.Texture2D, texturas[2]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(0, 0, 1);

                GL.TexCoord2(1, 1);
                GL.Vertex3(-0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 1);
                GL.Vertex3(0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(0.5f, 0.5f, 0.5f);

                GL.TexCoord2(1, 0);
                GL.Vertex3(-0.5f, 0.5f, 0.5f);
            }
            GL.End();

            // Right
            GL.BindTexture(TextureTarget.Texture2D, texturas[3]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(-1, 0, 0);

                GL.TexCoord2(1, 1);
                GL.Vertex3(-0.5f, -0.5f, -0.5f);

                GL.TexCoord2(0, 1);
                GL.Vertex3(-0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(-0.5f, 0.5f, 0.5f);

                GL.TexCoord2(1, 0);
                GL.Vertex3(-0.5f, 0.5f, -0.5f);
            }
            GL.End();

            // Top
            GL.BindTexture(TextureTarget.Texture2D, texturas[4]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(0, -1, 0);

                GL.TexCoord2(1, 0);
                GL.Vertex3(-0.5f, 0.5f, -0.5f);

                GL.TexCoord2(1, 1);
                GL.Vertex3(-0.5f, 0.5f, 0.5f);

                GL.TexCoord2(0, 1);
                GL.Vertex3(0.5f, 0.5f, 0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(0.5f, 0.5f, -0.5f);
            }
            GL.End();

            // Botton
            GL.BindTexture(TextureTarget.Texture2D, texturas[5]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(1, 1, 0);

                GL.TexCoord2(1, 1);
                GL.Vertex3(-0.5f, -0.5f, -0.5f);

                GL.TexCoord2(1, 0);
                GL.Vertex3(-0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 1);
                GL.Vertex3(0.5f, -0.5f, -0.5f);
            }
            GL.End();

            // Restore enable bits and matrix
            GL.PopAttrib();
            GL.PopMatrix();
        }
    }
}