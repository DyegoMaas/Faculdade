using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace N2
{
    class Program
    {
        private class SistemaReferenciaUniverso
        {
            public PointF[] PontosCirculo { get; private set; }

            public SistemaReferenciaUniverso()
            {
                const int numeroPontos = 72;
                const int raio = 50;

                PontosCirculo = new PointF[numeroPontos];
                for (var i = 0; i < numeroPontos; i++)
                {
                    var theta = 2 * (float) Math.PI * i / numeroPontos;

                    var x = raio * (float)Math.Cos(theta);
                    var y = raio * (float)Math.Sin(theta);

                    PontosCirculo[i] = new PointF(x, y);
                }
            }
        }

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
            var tamanhoInicialJanela = new Size(800, 800);

            var sistemaReferenciaUniverso = new SistemaReferenciaUniverso();

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
                    
                    SRU(sistemaReferenciaUniverso);

                    gameWindow.SwapBuffers();
                };

                gameWindow.Run(120, 60);
            }
        }

        private static void SRU(SistemaReferenciaUniverso sru)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.Disable(EnableCap.Lighting);

            GL.Color3(Color.Blue);
            GL.PointSize(1.5f);
            GL.Begin(PrimitiveType.Points);
            {
                foreach (var ponto in sru.PontosCirculo)
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
                GL.Vertex2(-100, 0);
                GL.Vertex2(100, 0);
            }
            GL.End();

            // eixo y
            GL.Color3(Color.FromArgb(0, 255, 0));
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(0, -100);
                GL.Vertex2(0, 100);
            }
            GL.End();
        }
    }
}
