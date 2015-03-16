using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace N2_Exercicio02
{
    class Program
    {
        private float ortho2DMinX = -400f;
        private float ortho2DMaxX = 400f;
        private float ortho2DMinY = -400f;
        private float ortho2DMaxY = 400f;

        private readonly Point posicaoInicialJanela = new Point(50, 50);
        private readonly Size tamanhoInicialJanela = new Size(800, 800);
        private readonly SistemaReferenciaUniverso sistemaReferenciaUniverso = new SistemaReferenciaUniverso();

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
                gameWindow.Title = "CG-Respostas-N2_exe02";

                gameWindow.Load += (sender, e) =>
                {
                    GL.ClearColor(Color.White);
                };

                gameWindow.Resize += (sender, e) =>
                {

                };

                gameWindow.UpdateFrame += (sender, e) =>
                {
                    var teclado = Keyboard.GetState();

                    Pan(teclado);
                    Zoom(teclado);
                };

                gameWindow.RenderFrame += (sender, e) =>
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    CarregarMatrizOrtografica();
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    SRU(sistemaReferenciaUniverso);

                    gameWindow.SwapBuffers();
                };

                gameWindow.Run(120, 60);
            }
        }

        private void Zoom(KeyboardState teclado)
        {
            float zoom = 0;

            if (teclado.IsKeyDown(Key.I))
            {
                zoom += 1;
            }
            if (teclado.IsKeyDown(Key.O))
            {
                zoom -= 1;
            }

            ortho2DMinX += zoom;
            ortho2DMaxX -= zoom;
            ortho2DMinY += zoom;
            ortho2DMaxY -= zoom;

            CarregarMatrizOrtografica();
        }

        private void Pan(KeyboardState teclado)
        {
            float panX = 0;
            float panY = 0;

            // pan x
            if (teclado.IsKeyDown(Key.E) || teclado.IsKeyDown(Key.A))
            {
                panX += 1;
            }
            if (teclado.IsKeyDown(Key.D))
            {
                panX -= 1;
            }

            // pan y
            if (teclado.IsKeyDown(Key.C) || teclado.IsKeyDown(Key.W))
            {
                panY -= 1;
            }
            if (teclado.IsKeyDown(Key.B) || teclado.IsKeyDown(Key.S))
            {
                panY += 1;
            }

            ortho2DMinX += panX;
            ortho2DMaxX += panX;
            ortho2DMinY += panY;
            ortho2DMaxY += panY;

            CarregarMatrizOrtografica();
        }

        private void SRU(SistemaReferenciaUniverso sru)
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

        private void CarregarMatrizOrtografica()
        {
            var matriz = Matrix4.CreateOrthographicOffCenter(ortho2DMinX, ortho2DMaxX, ortho2DMinY, ortho2DMaxY, 0, 1);
            GL.LoadMatrix(ref matriz);
        }
    }
}
