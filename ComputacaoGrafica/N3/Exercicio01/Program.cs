using System.Linq;
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
        private Input input;
        //private readonly Mundo mundo = new Mundo(new Camera(0, 800, 0, -800));

        public static void Main(string[] args)
        {
            var program = new Program();
            program.Executar();
        }

        private ObjetoGrafico objetoEmEdicao = new ObjetoGrafico();

        private void Executar()
        {
            // Creates a 3.0-compatible GraphicsContext with 32bpp color, 24bpp depth
            // 8bpp stencil
            using (var gameWindow = new GameWindow(tamanhoInicialJanela.Width, tamanhoInicialJanela.Height, new GraphicsMode(32, 24, 8, 0)))
            {
                input = new Input(gameWindow);
                gameWindow.Location = posicaoInicialJanela;
                gameWindow.Title = "N3-Exercicio01";

                gameWindow.Load += (sender, e) =>
                {
                    GL.ClearColor(Color.White);

                    mundo.ObjetosGraficos.Add(objetoEmEdicao);
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

                gameWindow.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.F1)
                    {
                        AdicionarVertice();
                    }
                    else if (e.Key == Key.F2)
                    {
                        RemoverVertice();
                    }
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

        private void AdicionarVertice()
        {
            var posicaoMouse = input.ObterPosicaoMouseNaTela();
            var vertice = new Ponto4D(posicaoMouse.X, posicaoMouse.Y);
            objetoEmEdicao.Vertices.Add(vertice);
        }

        private void RemoverVertice()
        {
            if (objetoEmEdicao.Vertices.Any())
            {
                var indiceUltimoVertice = objetoEmEdicao.Vertices.Count - 1;
                objetoEmEdicao.Vertices.RemoveAt(indiceUltimoVertice);
            }
        }

        private void SRU()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.Disable(EnableCap.Lighting);
            
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

            //GL.PointSize(3);
            //GL.Begin(PrimitiveType.Points);
            //{
            //    GL.Vertex2(0, 0);
            //}
            //GL.End();

            //GL.PointSize(5);
            //GL.Begin(PrimitiveType.Points);
            //{
            //    GL.Vertex2(0, 800);
            //}
            //GL.End();

            //GL.PointSize(10);
            //GL.Begin(PrimitiveType.Points);
            //{
            //    GL.Vertex2(800, 0);
            //}
            //GL.End();

            //GL.PointSize(20);
            //GL.Begin(PrimitiveType.Points);
            //{
            //    GL.Vertex2(800, 800);
            //}
            //GL.End();
        }
    }

    public class Input
    {
        private readonly GameWindow gameWindow;

        public Input(GameWindow gameWindow)
        {
            this.gameWindow = gameWindow;
        }

        public Vector2 ObterPosicaoMouseNaTela()
        {
            return new Vector2(gameWindow.Mouse.X, gameWindow.Width - gameWindow.Mouse.Y);
        }
    }
}
