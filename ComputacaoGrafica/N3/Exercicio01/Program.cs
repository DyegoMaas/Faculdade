using Exercicio01.EngineGrafica;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace Exercicio01
{
    class Program
    {
        private readonly Point posicaoInicialJanela = new Point(50, 50);
        private readonly Size tamanhoInicialJanela = new Size(400, 400);
        private readonly Mundo mundo = new Mundo(new Camera(-400f, 400f, -400f, 400f));

        private float fatorZoom = 1f;

        public static void Main(string[] args)
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
                gameWindow.Title = "N3-Exercicio01";

                gameWindow.Load += (sender, e) =>
                {
                    GL.ClearColor(Color.White);
                };

                gameWindow.UpdateFrame += (sender, e) =>
                {
                    var teclado = Keyboard.GetState();

                    float panX = 0f, panY = 0f;
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
                    mundo.Camera.Pan(panX, panY);

                    var zoom = 0;
                    if (teclado.IsKeyDown(Key.I))
                    {
                        zoom += 1;
                    }
                    if (teclado.IsKeyDown(Key.O))
                    {
                        zoom -= 1;
                    }
                    mundo.Camera.Zoom(zoom);
                };
                
                gameWindow.RenderFrame += (sender, e) =>
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    mundo.Camera.CarregarMatrizOrtografica();
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    SRU();
                    foreach (var objetoGrafico in mundo.ObjetosGraficos)
                    {
                        objetoGrafico.Desenhar();
                    }

                    gameWindow.SwapBuffers();
                };

                gameWindow.Run(120, 60);
            }
        }

        private void SRU()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.Disable(EnableCap.Lighting);

            GL.LineWidth(1f);

            // eixo centroX
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(-200, 0);
                GL.Vertex2(200, 0);
            }
            GL.End();

            // eixo centroY
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
