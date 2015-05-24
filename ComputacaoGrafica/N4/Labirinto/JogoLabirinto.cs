using Labirinto.EngineGrafica;
using Labirinto.Regras;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace Labirinto
{
    public class JogoLabirinto : GameWindow
    {
        private readonly Mundo mundo = new Mundo(new Camera());
        private Regras.Labirinto labirinto;
        private readonly InputManager input;

        public JogoLabirinto()
            : base(800, 800, new GraphicsMode(32, 24, 8, 0))
        {
            input = new InputManager(this);
            Location = new Point(50, 50);
            Title = "Labyrinth";

            Load += (sender, e) =>
            {
                Glut.glutInit();

                GL.ClearColor(Color.CornflowerBlue);
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
            var configuracaoLabirinto = new ConfiguracaoLabirinto(new[,]
            {
                {'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c'},
                {'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c'},
                {'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c'},
                {'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c'},
                {'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c'},
                {'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c'},
                {'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c'},
                {'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c'},
                {'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c'},
                {'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c'}
            });


            labirinto = new Regras.Labirinto(configuracaoLabirinto);
            labirinto.Mover(40, 0, 0);
            mundo.AdicionarObjetoGrafico(labirinto);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            var alvo = labirinto.Posicao;
            Glu.gluLookAt(0, 0, 200, alvo.X, alvo.Y, alvo.Z, 0, 1, 0);

            foreach (var objetoGrafico in mundo.ObjetosGraficos)
            {
                objetoGrafico.DesenharObjetoGrafico();
            }

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
    }
}