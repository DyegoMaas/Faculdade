using System.Drawing;
using System.Linq;
using Exercicio01.EngineGrafica;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Exercicio01
{
    public class Exercicio3 : GameWindow
    {
        private readonly Point posicaoInicialJanela = new Point(50, 50);
        private readonly Mundo mundo = new Mundo(new Camera(0, 800, 0, 800));
        private Input input;

        private ObjetoGrafico objetoEmEdicao = new ObjetoGrafico();

        public Exercicio3()
            : base(800, 800, new GraphicsMode(32, 24, 8, 0))
        {
            input = new Input(this);
            Location = posicaoInicialJanela;
            Title = "N3-Exercicio01";

            Load += (sender, e) =>
            {
                GL.ClearColor(Color.White);
                mundo.ObjetosGraficos.Add(objetoEmEdicao);
            };

            UpdateFrame += OnUpdateFrame;
            KeyDown +=OnKeyDown;
            RenderFrame += OnRenderFrame;
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
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

            SwapBuffers();
        }

        void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                AdicionarVertice();
            }
            else if (e.Key == Key.F2)
            {
                RemoverVertice();
            }
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            var teclado = OpenTK.Input.Keyboard.GetState();

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
        }
    }
}