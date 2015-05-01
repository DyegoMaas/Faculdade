using System.Collections.Generic;
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
        private readonly Size tamanhoInicialJanela = new Size(800, 800);
        private readonly Mundo mundo = new Mundo(new Camera(0, 800, 0, 800));
        //private readonly Mundo mundo = new Mundo(new Camera(0, 800, 0, -800));

        public static void Main(string[] args)
        {
            var program = new Program();
            program.Executar();
        }

        private IList<Ponto4D> vertices = new List<Ponto4D>(); 

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

                    if (teclado.IsKeyDown(Key.I))
                    {
                        mundo.Camera.FatorZoom += .01f;
                    }
                    if (teclado.IsKeyDown(Key.O))
                    {
                        mundo.Camera.FatorZoom -= .01f;
                    }
                };

                gameWindow.MouseUp += (sender, e) =>
                {
                    vertices.Add(new Ponto4D(e.X, tamanhoInicialJanela.Width - e.Y));
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

                    DesenharCliques();

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
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(210, 0);
                GL.Vertex2(610, 0);
            }
            GL.End();

            // eixo centroY
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(10, 200);
                GL.Vertex2(10, 600);
            }
            GL.End();






            GL.PointSize(3);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex2(0,0);
            }
            GL.End();

            GL.PointSize(5);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex2(0, 800);
            }
            GL.End();

            GL.PointSize(10);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex2(800, 0);
            }
            GL.End();

            GL.PointSize(20);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex2(800, 800);
            }
            GL.End();




            // eixo centroX
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(200, 400);
                GL.Vertex2(600, 400);
            }
            GL.End();

            // eixo centroY
            GL.Color3(Color.FromArgb(0, 255, 0));
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(400, 200);
                GL.Vertex2(400, 600);
            }
            GL.End();
        }

        private void DesenharCliques()
        {
            GL.Color3(Color.FromArgb(160, 80, 128));
            GL.PointSize(5);
            GL.Begin(PrimitiveType.Points);
            {
                foreach (var clique in vertices)
                {
                    GL.Vertex2(clique.X, clique.Y);
                }
            }
            GL.End();
        }
    }
}
