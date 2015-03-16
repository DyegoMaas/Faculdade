using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace N2_Exercicio01
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
            const float ortho2DMinx = -400f;
            const float ortho2DMaxX = 400f;
            const float ortho2DMinY = -400f;
            const float ortho2DMaxY = 400f;

            var posicaoInicialJanela = new Point(50, 50);
            var tamanhoInicialJanela = new Size(400, 400);

            var circuloReferencia = new CirculoReferencia(raio:100, numeroPontos:72);

            // Creates a 3.0-compatible GraphicsContext with 32bpp color, 24bpp depth
            // 8bpp stencil
            using (var gameWindow = new GameWindow(tamanhoInicialJanela.Width, tamanhoInicialJanela.Height, new GraphicsMode(32, 24, 8, 0)))
            {
                gameWindow.Location = posicaoInicialJanela;
                gameWindow.Title = "CG-Respostas-N2_exe01";

                gameWindow.Load += (sender, e) =>
                {
                    GL.ClearColor(Color.LightGray);
                };

                gameWindow.Resize += (sender, e) =>
                {
                };

                gameWindow.UpdateFrame += (sender, e) =>
                {
                };

                gameWindow.RenderFrame += (sender, e) =>
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();

                    var matriz = Matrix4.CreateOrthographicOffCenter(ortho2DMinx, ortho2DMaxX, ortho2DMinY, ortho2DMaxY, 0, 1);
                    GL.LoadMatrix(ref matriz);
                    
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);   
                    
                    SRU(circuloReferencia);

                    gameWindow.SwapBuffers();
                };

                gameWindow.Run(120, 60);
            }
        }

        private static void SRU(CirculoReferencia sru)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.Disable(EnableCap.Lighting);

            GL.Color3(Color.Blue);
            GL.PointSize(1.5f);
            GL.Begin(PrimitiveType.Points);
            {
                foreach (var ponto in sru.Pontos)
                {
                    GL.Vertex2(ponto.X, ponto.Y);
                    GL.Vertex3(ponto.X, ponto.Y, 0);
                }
            }
            GL.End();


            GL.LineWidth(1f);

            // eixo x
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(-200, 0);
                GL.Vertex2(200, 0);
            }
            GL.End();

            // eixo y
            GL.Color3(Color.FromArgb(0, 255, 0));
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(0, -200);
                GL.Vertex2(0, 200);
            }
            GL.End();
        }
    }
}
