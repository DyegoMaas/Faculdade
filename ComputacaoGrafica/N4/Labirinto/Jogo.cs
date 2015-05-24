using JogoLabirinto.EngineGrafica;
using JogoLabirinto.Regras;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace JogoLabirinto
{
    public class Jogo : GameWindow
    {
        private const float SensibilidadeMouse = .1f;
        private readonly Mundo mundo = new Mundo(new Camera());
        private Labirinto labirinto;
        private readonly InputManager input;
        private CuboSolido cuboSolido;

        public Jogo()
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

        /*  
         * c = chão
         * p = parede
         * e = esfera
         */
        private void ConfigurarCena()
        {
            var configuracaoLabirinto = new ConfiguracaoLabirinto(new[,]
            {
                {'p', 'c', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'e', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'c', 'p'}
            }, 
            tamanhoBlocosPiso: 10,
            tamanhoParede: new Vector3d(1, 1, 1));
            
            labirinto = new Labirinto(configuracaoLabirinto);
            //labirinto.Mover(-50, 0, -50);
            mundo.AdicionarObjetoGrafico(labirinto);


            cuboSolido = new CuboSolido(Color.LawnGreen);
            cuboSolido.Redimensionar(20, new Ponto4D());
            mundo.AdicionarObjetoGrafico(cuboSolido);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();


            var alvo = labirinto.Posicao;
            //a transformação do lookAt está interferindo na rotação do cenário.
            Glu.gluLookAt(
                150, 100, 150,
                //100,0,0,
                alvo.X, alvo.Y, alvo.Z,
                0, 1, 0);

            foreach (var objetoGrafico in mundo.ObjetosGraficos)
            {
                objetoGrafico.DesenharObjetoGrafico();
            }

            GL.Color3(Color.Blue);
            GL.PointSize(5f);
            GL.Begin(PrimitiveType.Points);
            {
                //GL.Vertex3(ponto.X, ponto.Y, ponto.Z);
                GL.Vertex3(0,0,0);
            }
            GL.End();

            

            SwapBuffers();
        }

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private Ponto4D ponto = new Ponto4D();
        void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            if (e.YDelta > 10 || e.XDelta > 10)
                return;

            //var pivoLabirinto = new Ponto4D(200, 100, 200).InverterSinal();
            var pivoLabirinto = labirinto.Centro.InverterSinal();
            ponto = pivoLabirinto.InverterSinal();
            labirinto.RotacionarNoEixoX(-e.YDelta * SensibilidadeMouse, pivoLabirinto);
            labirinto.RotacionarNoEixoZ(e.XDelta * SensibilidadeMouse, pivoLabirinto);

            var pivoCubo = cuboSolido.Posicao.InverterSinal();
            cuboSolido.RotacionarNoEixoX(-e.YDelta * SensibilidadeMouse, pivoCubo);
            cuboSolido.RotacionarNoEixoZ(e.XDelta * SensibilidadeMouse, pivoCubo);
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
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