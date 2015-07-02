using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;

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
            TexturasCaixa[1] = CarregarTextura(Path.Combine(diretorioTexturas.FullName, "woodBox_12_nm.jpg"));
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
}