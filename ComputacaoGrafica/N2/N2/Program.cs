using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;

namespace N2
{
    class Program
    {

        /// <summary>
        /// Para evitar 100% de uso da CPU http://www.opentk.com/doc/intro/cpu-usage
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            var posicaoInicialJanela = new Point(50, 50);
            var tamanhoInicialJanela = new Size(800, 800);

            // Creates a 3.0-compatible GraphicsContext with 32bpp color, 24bpp depth
            // 8bpp stencil
            using (var gameWindow = new GameWindow(tamanhoInicialJanela.Width, tamanhoInicialJanela.Height, new GraphicsMode(32, 24, 8, 0)))
            {
                gameWindow.X = posicaoInicialJanela.X;
                gameWindow.Y = posicaoInicialJanela.Y;

                gameWindow.Load += (sender, e) =>
                {
                    GL.ClearColor(Color.CadetBlue);
                };

                gameWindow.Resize += (sender, e) =>
                {

                };

                gameWindow.UpdateFrame += (sender, e) =>
                {

                };

                gameWindow.RenderFrame += (sender, e) =>
                {
                    //configs do OpenGL
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    gameWindow.SwapBuffers();
                };

                gameWindow.Run(120, 60);
            }
        }
    }
}
