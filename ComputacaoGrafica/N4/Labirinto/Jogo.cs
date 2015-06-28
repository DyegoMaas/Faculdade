using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace JogoLabirinto
{
    public class Jogo : GameWindow
    {
        private const double SensibilidadeMouse = .25d;
        private const int AnguloLimiteRotacao = 15;
        private readonly Camera camera = new Camera();
        private Tabuleiro tabuleiro;
        private LinhasReferencia linhasReferencia;

        public Jogo()
        {
            Location = new Point(50, 50);
            Title = "Labyrinth";

            Load += (sender, e) =>
            {
                WindowState = WindowState.Fullscreen;
                Glut.glutInit();

                GL.Enable(EnableCap.CullFace);
                GL.Enable(EnableCap.DepthTest);

                GL.ClearColor(Color.CornflowerBlue);

                ConfigurarCena();
            };
            Resize += (sender, e) => camera.Reshape(ClientSize.Width, ClientSize.Height);
            UpdateFrame += OnUpdateFrame;
            RenderFrame += OnRenderFrame;
            KeyDown += OnKeyDown;
            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
        }

        /*  
         * c = chão
         * p = parede
         * j = esfera
         * b = buraco/caçapa
         */
        private void ConfigurarCena()
        {
            var configuracaoLabirinto = new ConfiguracaoLabirinto(new[,]
            {
                {'p', 'c', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'p', 'p', 'c', 'c', 'p', 'c', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'j', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'b', 'p', 'c', 'c', 'b', 'p', 'b', 'p', 'c', 'c', 'b', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p', 'c', 'p', 'c', 'c', 'c', 'p'}
            },
            escala: 2,
            tamanhoParede: new Vector3d(1, 1, 1));

            tabuleiro = GeradorCenario.GerarCenario(configuracaoLabirinto);
            linhasReferencia = new LinhasReferencia(numeroLinhas:400, distanciaEntreLinhas:2);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            Glu.gluLookAt(
                30, 40, 30,
                //0, 0, 0,
                tabuleiro.Esfera.Posicao.X, tabuleiro.Esfera.Posicao.Y, tabuleiro.Esfera.Posicao.Z,
                0d, 1d, 0d);

            DesenharEixos();
            linhasReferencia.Desenhar();
            tabuleiro.Desenhar();

            SwapBuffers();
        }

        private void DesenharEixos()
        {
            GL.LineWidth(5);
            GL.Begin(PrimitiveType.Lines);
            {
                //x
                GL.Color3(Color.Red);
                GL.Vertex3(-10, 0, 0);
                GL.Vertex3(10, 0, 0);

                //y
                GL.Color3(Color.LawnGreen);
                GL.Vertex3(0, -10, 0);
                GL.Vertex3(0, 10, 0);

                //x
                GL.Color3(Color.Blue);
                GL.Vertex3(0, 0, -10);
                GL.Vertex3(0, 0, 10);
            }
            GL.End();
        }

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private double rotacaoX, rotacaoZ;

        void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            if (!rodando)
                return;

            rotacaoX += (e.XDelta * SensibilidadeMouse);
            rotacaoX = rotacaoX.Clamp(-AnguloLimiteRotacao, AnguloLimiteRotacao);

            rotacaoZ += (e.YDelta * SensibilidadeMouse);
            rotacaoZ = rotacaoZ.Clamp(-AnguloLimiteRotacao, AnguloLimiteRotacao);

            tabuleiro.RotacaoX = rotacaoX;
            tabuleiro.RotacaoZ = rotacaoZ;
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            if (!rodando)
                return;

            tabuleiro.Atualizar();
        }

        private bool rodando = false;
        void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
                rodando = !rodando;
            if(e.Key == Key.Escape)
                Exit();
        }

        private void Zoom(KeyboardState teclado)
        {
            if (teclado.IsKeyDown(Key.I))
            {
                camera.FatorZoom += .01f;
            }
            if (teclado.IsKeyDown(Key.O))
            {
                camera.FatorZoom -= .01f;
            }
        }
    }

    public static class GeradorCenario
    {
        public static Tabuleiro GerarCenario(ConfiguracaoLabirinto configuracao)
        {
            var matrizConfiguracao = configuracao.MatrizConfiguracao;
            var numeroBlocosEmX = matrizConfiguracao.GetLength(0);
            var numeroBlocosEmZ = matrizConfiguracao.GetLength(1);
            var objetosCenario = new Tabuleiro(new SizeD(numeroBlocosEmX, numeroBlocosEmZ), configuracao.Escala);

            for (var x = 0; x < numeroBlocosEmX; x++)
            {
                for (var z = 0; z < numeroBlocosEmZ; z++)
                {
                    var config = matrizConfiguracao[x, z];
                    var tipoConteudo = TipoConteudo(config);

                    var posicaoInicial = new Vector3d(x, 0, z);
                    switch (tipoConteudo)
                    {
                        case TipoConteudoCasaTabuleiro.Cacapa:
                            objetosCenario.Cacapa = new Cacapa(posicaoInicial);
                            break;
                        case TipoConteudoCasaTabuleiro.Chao:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));
                            break;

                        case TipoConteudoCasaTabuleiro.ChaoComEsfera:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));

                            var posicaoEsfera = new Vector3d(posicaoInicial.X, posicaoInicial.Y + 1, posicaoInicial.Z);

                            var esfera = new Esfera(posicaoEsfera)
                            {
                                BoundingBox = new BoundingBox()
                            };
                            objetosCenario.Esfera = esfera;
                            MotorColisoes.Esfera = esfera;
                            break;

                        case TipoConteudoCasaTabuleiro.ChaoComParede:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));

                            var posicaoParede = new Vector3d(posicaoInicial.X, posicaoInicial.Y + 1, posicaoInicial.Z);
                            var parede = new Parede(posicaoParede)
                            {
                                BoundingBox = new BoundingBox()
                            };
                            objetosCenario.Paredes.Add(parede);
                            MotorColisoes.Paredes.Add(parede);
                            break;
                    }
                }
            }

            return objetosCenario;
        }

        private static TipoConteudoCasaTabuleiro TipoConteudo(char config)
        {
            switch (config)
            {
                case 'c': return TipoConteudoCasaTabuleiro.Chao;
                case 'p': return TipoConteudoCasaTabuleiro.ChaoComParede;
                case 'j': return TipoConteudoCasaTabuleiro.ChaoComEsfera;
                default: return TipoConteudoCasaTabuleiro.Cacapa;
            }
        }

        private enum TipoConteudoCasaTabuleiro
        {
            Chao = 1,
            ChaoComParede = 2,
            ChaoComEsfera = 3,
            Cacapa
        }
    }

    public abstract class ComponenteTabuleiro : ObjetoGrafico, IObjetoInteligente
    {
        private Vector3d posicao;
        public Vector3d Posicao
        {
            get { return posicao; }
            set
            {
                posicao = value;
                AtualizarMatrizTranslacao();
            }
        }

        public BoundingBox BoundingBox { get; set; }

        protected ComponenteTabuleiro(Vector3d posicao)
        {
            Posicao = posicao;
            AtualizarMatrizTranslacao();

            AntesDeDesenhar(() =>
            {
                if(BoundingBox != null)
                    BoundingBox.Desenhar();
            });
        }

        private void AtualizarMatrizTranslacao()
        {
            var translacao = Matrix4d.CreateTranslation(Posicao);
            AplicarTransformacao(translacao);
        }

        public abstract void Atualizar();
    }

    public class Parede : ComponenteTabuleiro
    {
        public Parede(Vector3d posicao)
            : base(posicao)
        {
            GraphicUtils.TransformarEmCubo(this);
            AntesDeDesenhar(() => GL.Color3(Color.DimGray));
        }

        protected override void DesenharObjeto()
        {
            //GL.Color3(Color.DimGray);
            //GraphicUtils.DesenharCubo(adicionarContornos: true);
        }

        public override void Atualizar()
        {
        }
    }

    public class Chao : ComponenteTabuleiro
    {
        public Chao(Vector3d posicao) : base(posicao)
        {
            GraphicUtils.TransformarEmCubo(this);
            AntesDeDesenhar(() => GL.Color3(Color.SaddleBrown));
        }

        protected override void DesenharObjeto()
        {
            //GL.Color3(Color.SaddleBrown);
            //GraphicUtils.DesenharCubo(Faces, adicionarContornos:true);
        }

        public override void Atualizar()
        {
            
        }
    }

    public class Esfera : ComponenteTabuleiro
    {
        public Vector3d Velocidade { get; set; }
        
        public Esfera(Vector3d posicao)
            : base(posicao)
        {
            GraphicUtils.TransformarEmCubo(this);
            AntesDeDesenhar(() => GL.Color3(Color.Aqua));
        }

        protected override void DesenharObjeto()
        {
            //TODO desenhar uma esfera
            //GL.Color3(Color.Aqua);
            //GraphicUtils.DesenharCubo();
            DesenharVetorVelocidade();
        }

        private void DesenharVetorVelocidade()
        {
            const int tamahoEixos = 400;

            var velX = new Vector3d(Velocidade.X * tamahoEixos, 0, 0);
            var velY = new Vector3d(0, Velocidade.Y * tamahoEixos, 0);
            var velZ = new Vector3d(0, 0, Velocidade.Z * tamahoEixos);
            var direcao = velX + velY + velZ;

            GL.LineWidth(5);
            GL.Begin(PrimitiveType.Lines);
            {
                //x
                GL.Color3(Color.Red);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(velX);

                //y
                GL.Color3(Color.LawnGreen);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(velY);

                //z
                GL.Color3(Color.Blue);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(velZ);

                //dir
                GL.Color3(Color.SlateBlue);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(direcao);
            }
            GL.End();
        }

        public override void Atualizar()
        {
            Posicao = MotorColisoes.NovaPosicaoDaEsfera(Posicao, Velocidade);
            //Posicao = Posicao + Velocidade;
        }
    }

    public static class MotorColisoes
    {
        public static Esfera Esfera { get; set; }
        public static IList<Parede> Paredes { get; set; }

        static MotorColisoes()
        {
            Paredes = new List<Parede>();
        }

        public static Vector3d NovaPosicaoDaEsfera(Vector3d posicaoAtual, Vector3d velocidade)
        {
            var novaPosicaoDaEsfera = posicaoAtual + velocidade;
            for (int i = 0; i < Paredes.Count; i++)
            {
                var parede = Paredes[i];

                var distancia = novaPosicaoDaEsfera - parede.Posicao;
                var distanciaAbs = new Vector3d(Math.Abs(distancia.X), Math.Abs(distancia.Y), Math.Abs(distancia.Z));
                
                if (distanciaAbs.X < 1 && distanciaAbs.Z < 1)
                {
                    if (distanciaAbs.X < 1)
                        novaPosicaoDaEsfera.X = posicaoAtual.X;

                    if (distanciaAbs.Z < 1)
                        novaPosicaoDaEsfera.Z = posicaoAtual.Z;
                    Console.WriteLine("atravessou a parede {0} por {1}", i, distanciaAbs);
                }
            }

            return novaPosicaoDaEsfera;
        }

        private static Vector3d multVectorByMatrix(Matrix4d matrix, Vector3d vector)
        {
            var result = new Vector3d();
            result.X = (matrix[0,0] * vector.X) +
                       (matrix[1,0] * vector.Y) + 
                       (matrix[2,0] * vector.Z) +
                        matrix[3,0];
            result.Y = (matrix[0,1] * vector.X) +
                       (matrix[1,1] * vector.Y) + 
                       (matrix[2,1] * vector.Z) +
                        matrix[3,1];
            result.Z = (matrix[0,2] * vector.X) +
                       (matrix[1,2] * vector.Y) + 
                       (matrix[2,2] * vector.Z) +
                        matrix[3,2];
            return result;
        }
    }

    public class Cacapa : ComponenteTabuleiro
    {
        public Cacapa(Vector3d posicao)
            : base(posicao)
        {
        }

        protected override void DesenharObjeto()
        {
            GL.Color3(Color.Black);
        }

        public override void Atualizar()
        {
        }
    }

    public class LinhasReferencia : ObjetoGrafico
    {
        private readonly int numeroLinhas;
        private readonly int distanciaEntreLinhas;

        public LinhasReferencia(int numeroLinhas, int distanciaEntreLinhas)
        {
            this.numeroLinhas = numeroLinhas;
            this.distanciaEntreLinhas = distanciaEntreLinhas;
        }

        protected override void DesenharObjeto()
        {
            GL.Color3(Color.Black);
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            {
                for (var i = -numeroLinhas / 2; i < numeroLinhas / 2; i += distanciaEntreLinhas)
                {
                    GL.Vertex3(-1000, -10, i);
                    GL.Vertex3(1000, -10, i);

                    GL.Vertex3(i, -10, -1000);
                    GL.Vertex3(i, -10, 1000);
                }
            }
            GL.End();
        }
    }

    public class Tabuleiro : ObjetoGrafico, IObjetoInteligente
    {
        private const double FatorAceleracaoEsfera = 0.003;

        public SizeD Tamanho { get; private set; }
        public double RotacaoX { get; set; }
        public double RotacaoZ { get; set; }

        public readonly List<IObjetoGrafico> BlocosChao = new List<IObjetoGrafico>();
        public readonly List<IObjetoGrafico> Paredes = new List<IObjetoGrafico>();
        private double escala;
        public Esfera Esfera { get; set; }
        public IObjetoGrafico Cacapa { get; set; }

        public Tabuleiro(SizeD tamanho, double escala)
        {
            this.escala = escala;
            Tamanho = tamanho;
        }

        public void Atualizar()
        {
            Esfera.Velocidade = CalcularVelocidadeEsfera();
            Esfera.Atualizar();

            //var rotacaoX = Matrix4d.CreateRotationX(RotacaoX);
            //var rotacaoZ = Matrix4d.CreateRotationZ(RotacaoZ);
            //var translacao = Matrix4d.CreateTranslation(-Tamanho.Comprimento / 2, 0, -Tamanho.Largura / 2);
            //var matrizEscala = Matrix4d.Scale(escala, escala, escala);
            ////var resultado = rotacaoX * rotacaoZ * translacao * matrizEscala;
            //AplicarTransformacao(resultado);
        }

        private Vector3d CalcularVelocidadeEsfera()
        {
            return new Vector3d(-RotacaoZ * FatorAceleracaoEsfera, 0, RotacaoX * FatorAceleracaoEsfera);
        }

        protected override void DesenharObjeto()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.Rotate(RotacaoX, Vector3d.UnitX);
                GL.Rotate(RotacaoZ, Vector3d.UnitZ);
                GL.Rotate(RotacaoZ, Vector3d.UnitZ);
                GL.Translate(-Tamanho.Comprimento / 2, 0, -Tamanho.Largura / 2);
                GL.Scale(escala, escala, escala);

                //TODO adornos no tabuleiro???
                Cacapa.Desenhar();
                BlocosChao.ForEach(b => b.Desenhar());
                Paredes.ForEach(p => p.Desenhar());
                Esfera.Desenhar();
            }
            GL.PopMatrix();
        }
    }

    public class Face
    {
        public Vector3d Normal { get; private set; }
        public IList<Vector3d> Vertices { get; private set; }

        public Face(Vector3d normal, IList<Vector3d> vertices)
        {
            Normal = normal;
            Vertices = vertices;
        }
    }

    public abstract class ObjetoGrafico : IObjetoGrafico
    {
        private readonly List<Action> acoes = new List<Action>();
        public readonly IList<Face> Faces = new List<Face>();
        protected bool Wireframe { get; set; }

        public Matrix4d MatrizTransformacao;

        protected ObjetoGrafico()
        {
            MatrizTransformacao = Matrix4d.Identity;
        }

        protected void AplicarTransformacao(Matrix4d novaMatrizTransformacao)
        {
            MatrizTransformacao = novaMatrizTransformacao;
        }

        public void Desenhar()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.MultMatrix(ref MatrizTransformacao);

                acoes.ForEach(acao => acao.Invoke());

                foreach (var face in Faces)
                {
                    GraphicUtils.DesenharFace(face, Wireframe);
                }
                DesenharObjeto();
            }
            GL.PopMatrix();
        }

        protected abstract void DesenharObjeto();

        protected void AntesDeDesenhar(Action operacaoAntesDesenhar)
        {
            acoes.Add(operacaoAntesDesenhar);
        }
    }

    public interface IObjetoGrafico
    {
        void Desenhar();
    }

    public interface IObjetoInteligente
    {
        void Atualizar();
    }

    public struct SizeD
    {
        public readonly double Comprimento;
        public readonly double Largura;

        public SizeD(double comprimento, double largura)
        {
            Comprimento = comprimento;
            Largura = largura;
        }
    }

    public static class GraphicUtils
    {
        public static void DesenharCubo(IList<Face> faces, bool adicionarContornos = false)
        {
            foreach (var face in faces)
            {
               DesenharFace(face);
            }
        }

        public static void DesenharBoundingBox(BoundingBox bBox)
        {
            var min = bBox.Min;
            var max = bBox.Max;
            GL.LineWidth(1);
            GL.Color3(Color.BurlyWood);

            GL.Begin(PrimitiveType.Lines);
            {
                //TODO remover arestas duplicadas
                // Front Face
                GL.Vertex3(min.X, min.Y, max.Z);
                GL.Vertex3(max.X, min.Y, max.Z);
                GL.Vertex3(max.X, max.Y, max.Z);
                GL.Vertex3(min.X, max.Y, max.Z);

                // Back Face
                GL.Vertex3(min.X, min.Y, min.Z);
                GL.Vertex3(min.X, max.Y, min.Z);
                GL.Vertex3(max.X, max.Y, min.Z);
                GL.Vertex3(max.X, min.Y, min.Z);

                // Top Face
                GL.Vertex3(min.X, max.Y, min.Z);
                GL.Vertex3(min.X, max.Y, max.Z);
                GL.Vertex3(max.X, max.Y, max.Z);
                GL.Vertex3(max.X, max.Y, min.Z);

                // Bottom Face
                GL.Vertex3(min.X, min.Y, min.Z);
                GL.Vertex3(max.X, min.Y, min.Z);
                GL.Vertex3(max.X, min.Y, max.Z);
                GL.Vertex3(min.X, min.Y, max.Z);

                // Right face
                GL.Vertex3(max.X, min.Y, min.Z);
                GL.Vertex3(max.X, max.Y, min.Z);
                GL.Vertex3(max.X, max.Y, max.Z);
                GL.Vertex3(max.X, min.Y, max.Z);

                // Left Face
                GL.Vertex3(min.X, min.Y, min.Z);
                GL.Vertex3(min.X, min.Y, max.Z);
                GL.Vertex3(min.X, max.Y, max.Z);
                GL.Vertex3(min.X, max.Y, min.Z);
            }
            GL.End();
        }

        public static double Clamp(this double valor, double min, double max)
        {
            if (valor < min) return min;
            if (valor > max) return max;
            return valor;
        }

        public static bool EstaProximo(this Vector3d ponto, Vector3d outroPonto, double tolerancia)
        {
            return outroPonto.X >= ponto.X - tolerancia && outroPonto.X <= ponto.X + tolerancia &&
                   outroPonto.Y >= ponto.Y - tolerancia && outroPonto.Y <= ponto.Y + tolerancia;
        }

        public static Vector3d InverterSinal(this Vector3d ponto)
        {
            return new Vector3d(-ponto.X, - ponto.Y, - ponto.Z);
        }

        public static void DesenharFace(Face face, bool desenharWireframe = false)
        {
            var primitiva = desenharWireframe 
                ? PrimitiveType.Lines 
                : PrimitiveType.Quads;

            if (primitiva == PrimitiveType.Quads)
                GL.Normal3(face.Normal);

            GL.Begin(primitiva);
            {
                foreach (var vertice in face.Vertices)
                {
                    GL.Vertex3(vertice);
                }    
            }
            GL.End();
        }

        public static void TransformarEmCubo(ObjetoGrafico objetoGrafico)
        {
            const float x = .5f;
            const float y = .5f;
            const float z = .5f;

            objetoGrafico.Faces.Add(new Face(new Vector3d(0, 0, 1), new[]
            {
                new Vector3d(-x, -y, z),
                new Vector3d(x, -y, z),
                new Vector3d(x, y, z),
                new Vector3d(-x, y, z)    
            }));

            // Back Face
            objetoGrafico.Faces.Add(new Face(new Vector3d(0, 0, -1), new[]
            {
                new Vector3d(-x, -y, -z),
                new Vector3d(-x, y, -z),
                new Vector3d(x, y, -z),
                new Vector3d(x, -y, -z)
            }));

            // Top Face
            objetoGrafico.Faces.Add(new Face(new Vector3d(0, 1, 0), new[]
            {
                new Vector3d(-x, y, -z),
                new Vector3d(-x, y, z),
                new Vector3d(x, y, z),
                new Vector3d(x, y, -z)
            }));

            // Bottom Face
            objetoGrafico.Faces.Add(new Face(new Vector3d(0, -1, 0), new[]
            {
                new Vector3d(-x, -y, -z),
                new Vector3d(x, -y, -z),
                new Vector3d(x, -y, z),
                new Vector3d(-x, -y, z)
            }));

            // Right face
            objetoGrafico.Faces.Add(new Face(new Vector3d(1, 0, 0), new[]
            {
                new Vector3d(x, -y, -z),
                new Vector3d(x, y, -z),
                new Vector3d(x, y, z),
                new Vector3d(x, -y, z)
            }));
        }
    }

    public class BoundingBox : ObjetoGrafico
    {
        Vector3d min = Vector3d.Zero, max = Vector3d.Zero;

        public Vector3d Min
        {
            set { min = value; }
            get { return min; }
        }

        public Vector3d Max
        {
            set { max = value; }
            get { return max; }
        }

        public Vector3d Centro
        {
            set
            {
                var dist = value - Centro;
                min += dist;
                max += dist;
            }
            get { return (min + max) / 2.0f; }
        }

        public Vector3d HalfSize
        {
            set
            {
                Vector3d cent = Centro;
                max = cent + value;
                min = cent - value;
            }
            get { return (max - min) / 2.0f; }
        }

        public void Scale(Vector3d scale)
        {
            var halfSize = HalfSize;

            halfSize.X *= scale.X;
            halfSize.Y *= scale.Y;
            halfSize.Z *= scale.Z;

            HalfSize = halfSize;
        }

        public BoundingBox()
        {
            GraphicUtils.TransformarEmCubo(this);
            Wireframe = true;
            AntesDeDesenhar(() => GL.Color3(Color.DarkKhaki));
        }

        public BoundingBox(BoundingBox box) 
            : this()
        {
            min = box.Min;
            max = box.Max;
        }

        public BoundingBox(Vector3d min, Vector3d max)
            : this()
        {
            this.min = min;
            this.max = max;
        }

        protected override void DesenharObjeto()
        {
            //TODO desenhar
        }
    }
}