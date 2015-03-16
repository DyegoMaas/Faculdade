using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace N2_Exercicio03
{
    class Program
    {
        private const float Ortho2DMinX = -400f;
        private const float Ortho2DMaxX = 400f;
        private const float Ortho2DMinY = -400f;
        private const float Ortho2DMaxY = 400f;
        private const float RaioCirculos = 100;

        private readonly Point posicaoInicialJanela = new Point(50, 50);
        private readonly Size tamanhoInicialJanela = new Size(400, 400);

        private readonly Circulo[] circulos =
        {
            new Circulo(RaioCirculos, -RaioCirculos, RaioCirculos), 
            new Circulo(RaioCirculos, RaioCirculos, RaioCirculos), 
            new Circulo(RaioCirculos, 0, -RaioCirculos), 
        };

        /// <summary>
        /// Para evitar 100% de uso da CPU http://www.opentk.com/doc/intro/cpu-usage
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            var program = new Program();
            program.Executar();
        }

        private void Executar()
        {
            // Creates a 3.0-compatible GraphicsContext with 32bpp color, 24bpp depth
            // 8bpp stencil
            using (var gameWindow = new GameWindow(tamanhoInicialJanela.Width, tamanhoInicialJanela.Height, new GraphicsMode(32, 24, 8, 0)))
            {
                gameWindow.Location = posicaoInicialJanela;
                gameWindow.Title = "CG-Respostas-N2_exe03";

                gameWindow.Load += (sender, e) =>
                {
                    GL.ClearColor(Color.White);
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

                    var matriz = Matrix4.CreateOrthographicOffCenter(Ortho2DMinX, Ortho2DMaxX, Ortho2DMinY, Ortho2DMaxY, 0, 1);
                    GL.LoadMatrix(ref matriz);

                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    SRU();
                    DesenharCoisas();

                    gameWindow.SwapBuffers();
                };

                gameWindow.Run(120, 60);
            }
        }

        private void DesenharCoisas()
        {
            // círculos
            {
                GL.LineWidth(2f);
                GL.Color3(Color.Black);

                const int numeroPontos = 200;
                foreach (var circulo in circulos)
                {
                    DesenharCirculo(numeroPontos, circulo);
                }
            }

            // triângulo
            {
                GL.LineWidth(1f);
                GL.Color3(Color.Aqua);
                GL.Begin(PrimitiveType.LineLoop);
                foreach (var circulo in circulos)
                {
                    GL.Vertex2(circulo.CentroX, circulo.CentroY);
                }
                GL.End();
            }
        }

        private struct Circulo
        {
            public float CentroX { get; private set; }
            public float CentroY { get; private set; }
            public float Raio { get; private set; }

            public Circulo(float raio, float centroX, float centroY) : this()
            {
                CentroX = centroX;
                CentroY = centroY;
                Raio = raio;
            }
        }

        private static void DesenharCirculo(int numeroPontos, Circulo circulo)
        {
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < numeroPontos; i++)
            {
                var theta = 2 * (float) Math.PI * i / numeroPontos;

                var x = circulo.Raio * (float)Math.Cos(theta) + circulo.CentroX;
                var y = circulo.Raio * (float)Math.Sin(theta) + circulo.CentroY;

                GL.Vertex2(x, y);
            }
            GL.End();
        }

        private void SRU()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.Disable(EnableCap.Lighting);
            
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
