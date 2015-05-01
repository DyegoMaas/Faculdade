using System.Linq;
using Exercicio01.Editor;
using Exercicio01.EngineGrafica;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace Exercicio01
{
    public class Exercicio3 : GameWindow
    {
        private const double VelocidadeTranslacao = 1;

        private readonly Point posicaoInicialJanela = new Point(50, 50);
        private readonly Mundo mundo = new Mundo(new Camera(0, 800, 0, 800));
        private readonly InputManager input;

        private ObjetoEmEdicao objetoEmEdicao = null;
        private ModoExecucao modoExecucao = ModoExecucao.Criacao;
        private OperacaoSobreObjeto operacao = OperacaoSobreObjeto.Translacao;

        public Exercicio3()
            : base(800, 800, new GraphicsMode(32, 24, 8, 0))
        {
            input = new InputManager(this);
            Location = posicaoInicialJanela;
            Title = "N3-Exercicio01";

            Load += (sender, e) => GL.ClearColor(Color.White);
            UpdateFrame += OnUpdateFrame;
            RenderFrame += OnRenderFrame;
            KeyDown += OnKeyDown;
            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
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

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (modoExecucao == ModoExecucao.Criacao)
            {
                if (e.Button.Equals(MouseButton.Left))
                {
                    if (objetoEmEdicao == null)
                    {
                        objetoEmEdicao = new ObjetoEmEdicao(new ObjetoGrafico(), input);
                        mundo.ObjetosGraficos.Add(objetoEmEdicao.ObjetoGrafico);

                        objetoEmEdicao.AdicionarVertice();
                    }
                    objetoEmEdicao.AdicionarVertice();
                }
                else if (e.Button.Equals(MouseButton.Right))
                {
                    objetoEmEdicao = null;
                }
            }
        }

        void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            var ponto = input.ObterPosicaoMouseNaTela();

            if (objetoEmEdicao != null)
            {
                var vertice = objetoEmEdicao.ObjetoGrafico.Vertices.LastOrDefault();
                if (vertice != null)
                {
                    vertice.X = ponto.X;
                    vertice.Y = ponto.Y;
                }
            }
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            var teclado = OpenTK.Input.Keyboard.GetState();

            Zoom(teclado);
            if (operacao == OperacaoSobreObjeto.Pan)
            {
                Pan(teclado);
            }
        }

        void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.F1) AtivarModoCriacao();
            if (e.Key == Key.F2) AtivarModoEdicao();

            if (modoExecucao == ModoExecucao.Criacao)
            {
                //TODO remover?? ou mudar para outro ponto
                if (e.Key == Key.F9) objetoEmEdicao.RemoverVertice();
            }
            else
            {
                if(e.Key == Key.Q) operacao = OperacaoSobreObjeto.Pan;
                if(e.Key == Key.W) operacao = OperacaoSobreObjeto.Translacao;
                if(e.Key == Key.E) operacao = OperacaoSobreObjeto.Rotacao;
                if(e.Key == Key.R) operacao = OperacaoSobreObjeto.Escala;

                if (operacao == OperacaoSobreObjeto.Translacao)
                {
                    if (objetoEmEdicao != null)
                    {
                        var velocidadeTranslacao = e.Shift ? VelocidadeTranslacao * 5 : VelocidadeTranslacao;
                        if (e.Key == Key.Right) objetoEmEdicao.Mover(velocidadeTranslacao, 0, 0);
                        if (e.Key == Key.Left) objetoEmEdicao.Mover(-velocidadeTranslacao, 0, 0);
                        if (e.Key == Key.Up) objetoEmEdicao.Mover(0, velocidadeTranslacao, 0);
                        if (e.Key == Key.Down) objetoEmEdicao.Mover(0, -velocidadeTranslacao, 0);
                    }
                }
            }
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

        private void AtivarModoEdicao()
        {
            modoExecucao = ModoExecucao.Edicao;
        }

        private void AtivarModoCriacao()
        {
            modoExecucao = ModoExecucao.Criacao;
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

            //modo de execução
            GL.PointSize(25);
            var corQuadrado = modoExecucao == ModoExecucao.Criacao ? Color.Black : Color.FromArgb(0, 255, 0);
            GL.Color3(corQuadrado);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex2(800, 800);
            }
            GL.End();

            if (modoExecucao == ModoExecucao.Edicao)
            {
                switch (operacao)
                {
                    case OperacaoSobreObjeto.Pan:
                        corQuadrado = Color.Black;
                        break;

                    case OperacaoSobreObjeto.Translacao:
                        corQuadrado = Color.Red;
                        break;

                    case OperacaoSobreObjeto.Rotacao:
                        corQuadrado = Color.Green;
                        break;

                    case OperacaoSobreObjeto.Escala:
                        corQuadrado = Color.Blue;
                        break;
                }
                GL.Color3(corQuadrado);
                GL.Begin(PrimitiveType.Points);
                {
                    GL.Vertex2(800, 780);
                }
                GL.End();
            }
        }
    }
}