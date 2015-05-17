using System.Drawing;
using System.Linq;
using Labirinto.Editor;
using Labirinto.EngineGrafica;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Labirinto
{
    /// <summary>
    /// TECLAS ATALHO:
    /// 
    /// F1 - Ativar modo de criação
    /// F2 - Ativar modo de edição
    /// 
    /// Um indicador do modo de operação é exibido no canto superior direito da tela
    /// 
    /// -----------------------------------------------------------------------------
    /// Modo de Criação:
    /// N - Alterar modo de polígono para fechado
    /// M - Alterar modo de polígono para aberto
    /// 
    /// R - Mudar cor do objeto para vermelho
    /// G - Mudar cor do objeto para verde
    /// B - Mudar cor do objeto para preta
    /// 
    /// -----------------------------------------------------------------------------
    /// Modo de Edição:
    /// 
    /// Q - Ativar a operação Pan
    /// A câmera pode ser controlada com as SETAS. Segurar Shift faz mover mais rápido
    /// 
    /// W - Ativar a operação Translação
    /// O objeto selecionado pode ser movido com as SETAS. 
    /// Segurar Shift faz com que o objeto seja movido mais rapidamento.
    /// 
    /// E - Ativar a operação Escala
    /// O objeto selecionado pode ser redimensionado utilizando as SETAS CIMA e BAIXO. 
    /// Segurar Shift faz com que o objeto seja redimensionado mais rapidamente.
    /// 
    /// R - Ativar a operação Rotação
    /// O objeto selecionado pode ser rotacionado utilizando as SETAS ESQUERDA e DIREITA.
    /// Segurar Shift faz com que o objeto seja rotacionado mais rapidamente.
    /// 
    /// Insert - Inserir um filho no objeto selecionado
    /// Se houver um objeto selecionado, será criado um filho.
    /// 
    /// -----------------------------------------------------------------------------
    /// Seleção de objetos no grafo de cena (teclado numérico):
    /// Keypd8 ou Keypad6 - Selecionar o próximo objeto gráfico
    /// Keypd4 ou Keypad2 - Selecionar o objeto gráfico anterior
    /// 
    /// </summary>
    public class Jogo : GameWindow
    {
        private const double VelocidadeTranslacao = 1d;
        private const double VelocidadeEscala = 1.005d;
        private const double VelocidadeRotacao = 1d;

        private readonly Mundo mundo = new Mundo(new Camera(0, 800, 0, 800));
        private readonly InputManager input;

        private ObjetoEmEdicao objetoEmEdicao = null;
        private ObjetoGrafico objetoSelecionado = null;

        private VerticeSelecionado verticeSelecionado = null;

        private ModoExecucao modoExecucao = ModoExecucao.Criacao;
        private OperacaoSobreObjeto operacao = OperacaoSobreObjeto.Translacao;

        public Jogo()
            : base(800, 800, new GraphicsMode(32, 24, 8, 0))
        {
            input = new InputManager(this);
            ObjetoEmEdicao.Input = input;
            Location = new Point(50, 50);
            Title = "Labyrinth";

            Load += (sender, e) =>
            {
                GL.ClearColor(Color.White);

                ////teste do grafo de cena
                //var quadrado1 = new ObjetoEmEdicao(mundo, "pai",
                //    new Ponto4D(100, 100),
                //    new Ponto4D(100, 300),
                //    new Ponto4D(300, 300),
                //    new Ponto4D(300, 100));
                //quadrado1.DefinirCor(Color.BlueViolet);
                //quadrado1.RedimensionarEmRelacaoAoCentroDoObjeto(2);
                //quadrado1.RotacionarEmRelacaoAoCentroDoObjeto(45);

                //var quadrado2 = new ObjetoEmEdicao(quadrado1.ObjetoGrafico, "filho",
                //    new Ponto4D(200, 200),
                //    new Ponto4D(200, 300),
                //    new Ponto4D(300, 300),
                //    new Ponto4D(300, 200));
                //quadrado2.RotacionarEmRelacaoAoCentroDoObjeto(20);
                //quadrado2.DefinirCor(Color.CadetBlue);

                //objetoEmEdicao = quadrado1;
            };
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
            DesenharGrafoCena();
            DesenharVerticeSelecionado();
            foreach (var objetoGrafico in mundo.ObjetosGraficos)
            {
                objetoGrafico.Desenhar();
            }
            if (objetoEmEdicao != null)
            {
                objetoEmEdicao.ObjetoGrafico.DesenharBBox();
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
                        objetoEmEdicao = CriarObjetoEmEdicao(pai:null);
                        mundo.AdicionarObjetoGrafico(objetoEmEdicao.ObjetoGrafico);
                    }
                    objetoEmEdicao.AdicionarVerticeNaPosicaoAtual();
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

                if (verticeSelecionado == null)
                {
                    if (objetoSelecionado == null)
                    {
                        objetoSelecionado = mundo.BuscarObjetoSelecionado(ponto.X, ponto.Y);
                        if (objetoSelecionado != null)
                        {
                            objetoEmEdicao = ObjetoEmEdicao.Editar(objetoSelecionado);
                        }
                    }
                    else
                    {
                        objetoSelecionado = null;
                    }
                }
            }
        }

        private ObjetoEmEdicao CriarObjetoEmEdicao(NoGrafoCena pai)
        {
            return new ObjetoEmEdicao(pai, "NomeObjeto");
        }

        private void FinalizarObjetoEmEdicao()
        {
            objetoEmEdicao = null;
        }

        private Ponto4D posicaoAnterior;
        void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            var posicaoMouseNaTela = input.ObterPosicaoMouseNaTela();
            if (modoExecucao == ModoExecucao.Criacao && objetoEmEdicao != null)
            {
                var vertice = objetoEmEdicao.ObjetoGrafico.Vertices.LastOrDefault();
                if (vertice != null)
                {
                    var ponto = posicaoMouseNaTela;
                    vertice.X = ponto.X;
                    vertice.Y = ponto.Y;
                }
            }
            else
            {
                var ponto = posicaoMouseNaTela;
                
                if (verticeSelecionado != null)
                {
                    verticeSelecionado.Relocar(ponto.X, ponto.Y);
                }

                //if (objetoSelecionado != null)
                //{
                //    var deltaX = posicaoMouseNaTela.X - posicaoAnterior.X;
                //    var deltaY = posicaoMouseNaTela.Y - posicaoAnterior.Y;
                //    objetoSelecionado.Mover(deltaX, deltaY, 0);
                //}
            }
            posicaoAnterior = posicaoMouseNaTela;
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

            if (modoExecucao == ModoExecucao.Criacao && objetoEmEdicao != null)
            {
                //TODO remover?? ou mudar para outro ponto
                if (e.Key == Key.F9) objetoEmEdicao.RemoverVertice();
                
                switch (e.Key)
                {
                    case Key.M:
                        objetoEmEdicao.ObjetoGrafico.Primitiva = PrimitiveType.LineLoop;
                        break;
                    case Key.N:
                        objetoEmEdicao.ObjetoGrafico.Primitiva = PrimitiveType.LineStrip;
                        break;
                    case Key.R:
                        objetoEmEdicao.ObjetoGrafico.Cor = Color.Red;
                        break;
                    case Key.G:
                        objetoEmEdicao.ObjetoGrafico.Cor = Color.Green;
                        break;
                    case Key.B:
                        objetoEmEdicao.ObjetoGrafico.Cor = Color.Black;
                        break;
                }

                if (e.Key == Key.Insert)
                {
                    if (objetoEmEdicao != null)
                    {
                        var objetoGraficoPai = objetoEmEdicao.ObjetoGrafico;
                        objetoEmEdicao = CriarObjetoEmEdicao(pai: objetoGraficoPai);
                        objetoEmEdicao.AdicionarVerticeNaPosicaoAtual();
                    }
                }
            }
            else
            {
                if (e.Key == Key.Q) operacao = OperacaoSobreObjeto.Pan;
                if (e.Key == Key.W) operacao = OperacaoSobreObjeto.Translacao;
                if (e.Key == Key.E) operacao = OperacaoSobreObjeto.Escala;
                if (e.Key == Key.R) operacao = OperacaoSobreObjeto.Rotacao;

                if (e.Key == Key.Delete)
                {
                    if (verticeSelecionado != null)
                    {
                        verticeSelecionado.ExcluirDoObjetoGrafico();
                        verticeSelecionado = null;
                    }

                    if (objetoSelecionado != null)
                    {
                        objetoEmEdicao.ExcluirObjetoGrafico();
                        objetoEmEdicao = null;
                    }
                }

                if (objetoEmEdicao != null)
                switch (operacao)
                {
                    case OperacaoSobreObjeto.Translacao:
                    {
                        var velocidadeTranslacao = e.Shift ? VelocidadeTranslacao * 5 : VelocidadeTranslacao;
                        if (e.Key == Key.Right) objetoEmEdicao.Mover(velocidadeTranslacao, 0, 0);
                        if (e.Key == Key.Left) objetoEmEdicao.Mover(-velocidadeTranslacao, 0, 0);
                        if (e.Key == Key.Up) objetoEmEdicao.Mover(0, velocidadeTranslacao, 0);
                        if (e.Key == Key.Down) objetoEmEdicao.Mover(0, -velocidadeTranslacao, 0);
                        break;
                    }
                    case OperacaoSobreObjeto.Escala:
                    {
                        //TODO aumentar e diminuir em relação ao centro da bbox (mover para a origem e depois voltar)
                        var velocidadeEscala = e.Shift ? VelocidadeEscala * 1.1d : VelocidadeEscala;

                        if (e.Key == Key.Up)
                            objetoEmEdicao.RedimensionarEmRelacaoAoCentroDoObjeto(velocidadeEscala);

                        if (e.Key == Key.Down)
                        {
                            velocidadeEscala = 1 - (velocidadeEscala - 1);
                            objetoEmEdicao.RedimensionarEmRelacaoAoCentroDoObjeto(velocidadeEscala);
                        }
                        break;
                    }
                    case OperacaoSobreObjeto.Rotacao:
                    {
                        var velocidadeRotacao = e.Shift ? VelocidadeRotacao * 2 : VelocidadeRotacao;
                        if (e.Key == Key.Right) objetoEmEdicao.RotacionarEmRelacaoAoCentroDoObjeto(-velocidadeRotacao);
                        if (e.Key == Key.Left) objetoEmEdicao.RotacionarEmRelacaoAoCentroDoObjeto(velocidadeRotacao);
                        break;
                    }
                }
            }

            if (e.Key == Key.Keypad4 || e.Key == Key.Keypad8) SelecionarObjetoAnteriorNoGrafoCena();
            if (e.Key == Key.Keypad2 || e.Key == Key.Keypad6) SelecionarProximoObjetoNoGrafoCena();
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

        private const float AlturaRetangulo = 5f;

        private void DesenharGrafoCena()
        {
            var itensDesenhados = 0;

            foreach (var objetoGrafico in mundo.ObjetosGraficos)
            {
                DesenharHierarquia(objetoGrafico, ref itensDesenhados, 1);
            }
        }

        private void DesenharHierarquia(ObjetoGrafico objetoGrafico, ref int itens, int profundidade)
        {
            itens++;
            var x = profundidade * 5;
            var y = Height - itens * AlturaRetangulo - itens * 2;

            GL.Color3(objetoGrafico.Cor);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Vertex2(x, y);
                GL.Vertex2(x + 100, y);
                GL.Vertex2(x + 100, y - AlturaRetangulo);
                GL.Vertex2(x, y - AlturaRetangulo);
            }
            GL.End();

            //se for o objeto em edição, desenhar um contorno vermelho
            if (objetoEmEdicao != null)
            {
                if (objetoGrafico == objetoEmEdicao.ObjetoGrafico)
                {
                    y -= 1;
                    GL.Color3(Color.Red);
                    GL.Begin(PrimitiveType.LineLoop);
                    {
                        GL.Vertex2(x - 1, y + 1);
                        GL.Vertex2(x + 100, y + 1);
                        GL.Vertex2(x + 100, y - AlturaRetangulo);
                        GL.Vertex2(x - 1, y - AlturaRetangulo);
                    }
                    GL.End();
                }
            }

            foreach (var objetoFilho in objetoGrafico.ObjetosGraficos)
            {
                DesenharHierarquia(objetoFilho, ref itens, profundidade + 1);
            }
        }

        private void DesenharVerticeSelecionado()
        {
            if (modoExecucao == ModoExecucao.Edicao)
            {
                if (verticeSelecionado != null)
                {
                    GL.PointSize(5);
                    GL.Color3(Color.Purple);
                    GL.Begin(PrimitiveType.Points);
                    {
                        GL.Vertex2(verticeSelecionado.Ponto.X, verticeSelecionado.Ponto.Y);
                    }
                    GL.End();
                }
            }
        }

        private void SelecionarObjetoAnteriorNoGrafoCena()
        {
            if (objetoEmEdicao == null)
            {
                var ultimoObjetoDoMundo = mundo.ObjetosGraficos.LastOrDefault();
                if (ultimoObjetoDoMundo != null)
                {
                    objetoEmEdicao = ObjetoEmEdicao.Editar(ultimoObjetoDoMundo);
                }
            }

            var objetoGrafico = objetoEmEdicao.ObjetoGrafico;
            var anterior = UltimoFilhoDoAnterior(objetoGrafico) ?? objetoGrafico.Anterior ?? objetoGrafico.Pai as ObjetoGrafico;
            if (anterior != null)
            {
                objetoEmEdicao = ObjetoEmEdicao.Editar(anterior);
            }
        }

        private static ObjetoGrafico UltimoFilhoDoAnterior(ObjetoGrafico objetoGrafico)
        {
            var anterior = objetoGrafico.Anterior;
            if (anterior == null) return null;

            return anterior.UltimoFilho;
        }

        private void SelecionarProximoObjetoNoGrafoCena()
        {
            if (objetoEmEdicao == null)
            {
                var primeiroObjetoDoMundo = mundo.ObjetosGraficos.FirstOrDefault();
                if (primeiroObjetoDoMundo != null)
                {
                    objetoEmEdicao = ObjetoEmEdicao.Editar(primeiroObjetoDoMundo);
                }
            }

            var objetoGrafico = objetoEmEdicao.ObjetoGrafico;
            var proximo = objetoGrafico.PrimeiroFilho ?? objetoGrafico.Proximo ?? objetoGrafico.Pai.Proximo ?? mundo.ObjetosGraficos.FirstOrDefault();
            if (proximo != null)
            {
                objetoEmEdicao = ObjetoEmEdicao.Editar(proximo);
            }
        }
    }
}