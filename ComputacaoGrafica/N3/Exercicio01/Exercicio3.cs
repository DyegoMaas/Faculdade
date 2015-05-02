using Exercicio01.Editor;
using Exercicio01.EngineGrafica;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Linq;

namespace Exercicio01
{
    public class Exercicio3 : GameWindow
    {
        /*
         * TECLAS ATALHO:
         * 
         * F1 - Ativar modo de criação
         * F2 - Ativar modo de edição
         * 
         * Um indicador do modo de operação é exibido no canto superior direito da tela
         * -----------------------
         * 
         * Modo de Criação:
         * N - Alterar modo de polígno para fechado
         * M - Alterar modo de polígno para aberto
         * 
         * R - Mudar cor do objeto para vermelho
         * G - Mudar cor do objeto para verde
         * B - Mudar cor do objeto para preta
         * ------------------------------
         * Modo de Edição:
         * 
         * Q - Ativar a operação Pan
         * A câmera pode ser controlada com as SETAS. Segurar Shift faz mover mais rápido
         * 
         * W - Ativar a operação Translação
         * O objeto selecionado pode ser movido com as SETAS. 
         * Segurar Shift faz com que o objeto seja movido mais rapidamento.
         * 
         * E - Ativar a operação Escala
         * O objeto selecionado pode ser redimensionado utilizando as SETAS CIMA e BAIXO. 
         * Segurar Shift faz com que o objeto seja redimensionado mais rapidamente.
         * 
         * R - Ativar a operação Rotação
         * O objeto selecionado pode ser rotacionado utilizando as SETAS ESQUERDA e DIREITA.
         * Segurar Shift faz com que o objeto seja rotacionado mais rapidamente.
         */

        private const double VelocidadeTranslacao = 1d;
        private const double VelocidadeEscala = 1.005d;
        private const double VelocidadeRotacao = 1d;

        private readonly Point posicaoInicialJanela = new Point(50, 50);
        private readonly Mundo mundo = new Mundo(new Camera(0, 800, 0, 800));
        private readonly InputManager input;

        /// <summary>
        /// Acessar pela propriedade ObjetoEmEdicao
        /// </summary>
        private ObjetoEmEdicao _objetoEmEdicao = null;
        public ObjetoEmEdicao ObjetoEmEdicao
        {
            get { return _objetoEmEdicao ?? (_objetoEmEdicao = new ObjetoEmEdicao(input)); }
        }

        private ObjetoGrafico objetoSelcionado = null;
        private Ponto4D verticeSelecionado = null;
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
                    mundo.ObjetosGraficos.Add(ObjetoEmEdicao.ObjetoGrafico);
                    ObjetoEmEdicao.AdicionarVertice();
                }
                else if (e.Button.Equals(MouseButton.Right))
                {
                    FinalizarObjetoEmEdicao();
                }
            }
            else
            {
                var ponto = input.ObterPosicaoMouseNaTela();

                if (verticeSelecionado == null)
                {
                    verticeSelecionado = mundo.BuscarVerticeSelecionado(ponto.X, ponto.Y);
                }
                else
                {
                    verticeSelecionado = null;
                }
            }
        }

        private void FinalizarObjetoEmEdicao()
        {
            _objetoEmEdicao = null;
        }

        void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            if (modoExecucao == ModoExecucao.Criacao && ObjetoEmEdicao != null)
            {
                var vertice = ObjetoEmEdicao.ObjetoGrafico.Vertices.LastOrDefault();
                if (vertice != null)
                {
                    var ponto = input.ObterPosicaoMouseNaTela();
                    vertice.X = ponto.X;
                    vertice.Y = ponto.Y;
                }
            }
            else
            {
                if (verticeSelecionado != null)
                {
                    var ponto = input.ObterPosicaoMouseNaTela();

                    verticeSelecionado.X = ponto.X;
                    verticeSelecionado.Y = ponto.Y;
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
                if (e.Key == Key.F9) ObjetoEmEdicao.RemoverVertice();
                
                switch (e.Key)
                {
                    case Key.M:
                        ObjetoEmEdicao.ObjetoGrafico.Primitiva = PrimitiveType.LineLoop;
                        break;
                    case Key.N:
                        ObjetoEmEdicao.ObjetoGrafico.Primitiva = PrimitiveType.LineStrip;
                        break;
                    case Key.R:
                        ObjetoEmEdicao.ObjetoGrafico.Cor = Color.Red;
                        break;
                    case Key.G:
                        ObjetoEmEdicao.ObjetoGrafico.Cor = Color.Green;
                        break;
                    case Key.B:
                        ObjetoEmEdicao.ObjetoGrafico.Cor = Color.Black;
                        break;
                }
            }
            else
            {
                if (e.Key == Key.Q) operacao = OperacaoSobreObjeto.Pan;
                if (e.Key == Key.W) operacao = OperacaoSobreObjeto.Translacao;
                if (e.Key == Key.E) operacao = OperacaoSobreObjeto.Escala;
                if (e.Key == Key.R) operacao = OperacaoSobreObjeto.Rotacao;

                if (e.Key == Key.Delete) mundo.RemoverVerticeSelecionado(verticeSelecionado);

                if (ObjetoEmEdicao != null)
                switch (operacao)
                {
                    case OperacaoSobreObjeto.Translacao:
                    {
                        var velocidadeTranslacao = e.Shift ? VelocidadeTranslacao * 5 : VelocidadeTranslacao;
                        if (e.Key == Key.Right) ObjetoEmEdicao.Mover(velocidadeTranslacao, 0, 0);
                        if (e.Key == Key.Left) ObjetoEmEdicao.Mover(-velocidadeTranslacao, 0, 0);
                        if (e.Key == Key.Up) ObjetoEmEdicao.Mover(0, velocidadeTranslacao, 0);
                        if (e.Key == Key.Down) ObjetoEmEdicao.Mover(0, -velocidadeTranslacao, 0);
                        break;
                    }
                    case OperacaoSobreObjeto.Escala:
                    {
                        //TODO aumentar e diminuir em relação ao centro da bbox (mover para a origem e depois voltar)
                        var velocidadeEscala = e.Shift ? VelocidadeEscala * 1.1d : VelocidadeEscala;

                        if (e.Key == Key.Up)
                            ObjetoEmEdicao.RedimensionarEmRelacaoAoCentroDoObjeto(velocidadeEscala);

                        if (e.Key == Key.Down)
                        {
                            velocidadeEscala = 1 - (velocidadeEscala - 1);
                            ObjetoEmEdicao.RedimensionarEmRelacaoAoCentroDoObjeto(velocidadeEscala);
                        }
                        break;
                    }
                    case OperacaoSobreObjeto.Rotacao:
                    {
                        var velocidadeRotacao = e.Shift ? VelocidadeRotacao * 2 : VelocidadeRotacao;
                        if (e.Key == Key.Right) ObjetoEmEdicao.RotacionarEmRelacaoAoCentroDoObjeto(-velocidadeRotacao);
                        if (e.Key == Key.Left) ObjetoEmEdicao.RotacionarEmRelacaoAoCentroDoObjeto(velocidadeRotacao);
                        break;
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