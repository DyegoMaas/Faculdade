using Labirinto.Editor;
using Labirinto.EngineGrafica;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using Tao.FreeGlut;

namespace Labirinto
{
    public class Jogo : GameWindow
    {
        private readonly Mundo2 mundo = new Mundo2(new Camera());
        private readonly InputManager input;

        public Jogo()
            : base(800, 800, new GraphicsMode(32, 24, 8, 0))
        {
            input = new InputManager(this);
            Location = new Point(50, 50);
            Title = "Labyrinth";

            Load += (sender, e) =>
            {
                Glut.glutInit();

                GL.ClearColor(Color.Black);
                ConfigurarCena();
            };
            Resize += (sender, e) => mundo.Camera.Reshape(ClientSize.Width, ClientSize.Height);
            UpdateFrame += OnUpdateFrame;
            RenderFrame += OnRenderFrame;
            KeyDown += OnKeyDown;
            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
        }

        private void ConfigurarCena()
        {
            var tabuleiro = new Tabuleiro();
            var tabuleiroGrafico = tabuleiro.Chao;

            mundo.AdicionarObjetoGrafico(tabuleiroGrafico);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //SRU();
            //DesenharGrafoCena();
            //DesenharVerticeSelecionado();
            foreach (var objetoGrafico in mundo.ObjetosGraficos)
            {
                objetoGrafico.Desenhar();
            }
            //if (objetoEmEdicao != null)
            //{
            //    objetoEmEdicao.ObjetoGrafico.DesenharBBox();
            //}

            SwapBuffers();
        }

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }
        
        private Ponto4D posicaoAnterior;
        void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            var posicaoMouseNaTela = input.ObterPosicaoMouseNaTela();
           

            posicaoAnterior = posicaoMouseNaTela;
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            var teclado = OpenTK.Input.Keyboard.GetState();

            Zoom(teclado);
            //if (operacao == OperacaoSobreObjeto.Pan)
            //{
            //    Pan(teclado);
            //}
        }

        void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
           
        }

        private void Zoom(KeyboardState teclado)
        {
            if (teclado.IsKeyDown(Key.I))
            {
                mundo.Camera.FatorZoom += .01f;
            }
            if (teclado.IsKeyDown(Key.O))
            {
                mundo.Camera.FatorZoom -= .01f;
            }
        }

        private void Pan(KeyboardState teclado)
        {
            float panX = 0f, panY = 0f;
            // pan x
            if (teclado.IsKeyDown(Key.Left))
            {
                panX += 1;
            }
            if (teclado.IsKeyDown(Key.Right))
            {
                panX -= 1;
            }
            // pan y
            if (teclado.IsKeyDown(Key.Up))
            {
                panY -= 1;
            }
            if (teclado.IsKeyDown(Key.Down))
            {
                panY += 1;
            }
            mundo.Camera.Pan(panX, panY);
        }
    }
}