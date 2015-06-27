using OpenTK;
using OpenTK.Graphics;
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

        public Jogo()
            : base(800, 800, new GraphicsMode(32, 24, 8, 0))
        {
            Location = new Point(50, 50);
            Title = "Labyrinth";

            Load += (sender, e) =>
            {
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
                {'p', 'c', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'j', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'b', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'c', 'p'}
            },
            escala: 2,
            tamanhoParede: new Vector3d(1, 1, 1));

            tabuleiro = GeradorCenario.GerarCenario(configuracaoLabirinto);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            Glu.gluLookAt(
                20, 25, 20,
                0, 0, 0,
                0d, 1d, 0d);

            DesenharEixos();
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
            rotacaoX += (e.XDelta * SensibilidadeMouse);
            rotacaoX = rotacaoX.Clamp(-AnguloLimiteRotacao, AnguloLimiteRotacao);

            rotacaoZ += (e.YDelta * SensibilidadeMouse);
            rotacaoZ = rotacaoZ.Clamp(-AnguloLimiteRotacao, AnguloLimiteRotacao);

            tabuleiro.RotacaoX = rotacaoX;
            tabuleiro.RotacaoZ = rotacaoZ;
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
            var escala = configuracao.Escala;
            var numeroBlocosEmX = matrizConfiguracao.GetLength(1);
            var numeroBlocosEmZ = matrizConfiguracao.GetLength(0);
            var objetosCenario = new Tabuleiro(new SizeD(numeroBlocosEmX * escala, numeroBlocosEmZ * escala));
            
            for (var x = 0; x < numeroBlocosEmX; x++)
            {
                for (var z = 0; z < numeroBlocosEmZ; z++)
                {
                    var config = matrizConfiguracao[x, z];
                    var tipoConteudo = TipoConteudo(config);

                    var posicaoInicial = new Vector3d(escala * x, 0, escala * z);
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
                            objetosCenario.Esfera = new Esfera(new Vector3d(posicaoInicial.X, posicaoInicial.Y + 2, posicaoInicial.Z));
                            break;

                        case TipoConteudoCasaTabuleiro.ChaoComParede:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));
                            objetosCenario.Paredes.Add(new Parede(new Vector3d(posicaoInicial.X, posicaoInicial.Y + 2, posicaoInicial.Z)));
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


    public abstract class ComponenteTabuleiro : ObjetoGrafico
    {
        protected readonly Vector3d Posicao;

        protected ComponenteTabuleiro(Vector3d posicao)
        {
            Posicao = posicao;
            AntesDeDesenhar(() => GL.Translate(posicao.X, posicao.Y, posicao.Z));
        }
    }

    public class Parede : ComponenteTabuleiro //TODO fazer um desenho próprio
    {
        public Parede(Vector3d posicao)
            : base(posicao)
        {
        }

        protected override void DesenharObjeto()
        {
            GL.Color3(Color.DimGray);
            GraphicUtils.DesenharCubo(adicionarContornos: true);
        }
    }

    public class Esfera : ComponenteTabuleiro
    {
        public Esfera(Vector3d posicao)
            : base(posicao)
        {
        }

        protected override void DesenharObjeto()
        {
            //TODO desenhar uma esfera
            GL.Color3(Color.Aqua);
            GraphicUtils.DesenharCubo();
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
            GraphicUtils.DesenharCubo(escalaY:.1f);
        }
    }

    public class Chao : ComponenteTabuleiro
    {
        public Chao(Vector3d posicao) : base(posicao)
        {
        }

        protected override void DesenharObjeto()
        {
            GL.Color3(Color.SaddleBrown);
            GraphicUtils.DesenharCubo(adicionarContornos:true);
        }
    }

    public class Tabuleiro : ObjetoGrafico, IObjetoInteligente
    {
        public SizeD Tamanho { get; private set; }
        public double RotacaoX { get; set; }
        public double RotacaoZ { get; set; }

        public readonly List<IObjetoGrafico> BlocosChao = new List<IObjetoGrafico>();
        public readonly List<IObjetoGrafico> Paredes = new List<IObjetoGrafico>();
        public IObjetoGrafico Esfera { get; set; }
        public IObjetoGrafico Cacapa { get; set; }

        public Tabuleiro(SizeD tamanho)
        {
            Tamanho = tamanho;
        }

        public void Atualizar()
        {
            throw new NotImplementedException();
        }

        protected override void DesenharObjeto()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.Rotate(RotacaoX, Vector3d.UnitX);
                GL.Rotate(RotacaoZ, Vector3d.UnitZ);
                GL.Translate(-Tamanho.Comprimento / 2, 0, -Tamanho.Largura / 2);
            
                //TODO adornos no tabuleiro???
                Cacapa.Desenhar();
                BlocosChao.ForEach(b => b.Desenhar());
                Paredes.ForEach(p => p.Desenhar());
                Esfera.Desenhar();
            }
            GL.PopMatrix();
        }
    }

    public abstract class ObjetoGrafico : IObjetoGrafico
    {
        private readonly List<Action> acoes = new List<Action>(); 
        public void Desenhar()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                acoes.ForEach(acao => acao.Invoke());
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
        public static void DesenharCubo(float escalaX = 1f, float escalaY = 1f, float escalaZ = 1f, bool adicionarContornos = false)
        {
            float x = escalaX,
                y = escalaY,
                z = escalaZ;

            GL.Begin(PrimitiveType.Quads);
            {
                // Front Face
                GL.Normal3(0, 0, 1);
                GL.Vertex3(-x, -y, z);
                GL.Vertex3(x, -y, z);
                GL.Vertex3(x, y, z);
                GL.Vertex3(-x, y, z);

                // Back Face
                GL.Normal3(0, 0, -1);
                GL.Vertex3(-x, -y, -z);
                GL.Vertex3(-x, y, -z);
                GL.Vertex3(x, y, -z);
                GL.Vertex3(x, -y, -z);

                // Top Face
                GL.Normal3(0, 1, 0);
                GL.Vertex3(-x, y, -z);
                GL.Vertex3(-x, y, z);
                GL.Vertex3(x, y, z);
                GL.Vertex3(x, y, -z);

                // Bottom Face
                GL.Normal3(0, -1, 0);
                GL.Vertex3(-x, -y, -z);
                GL.Vertex3(x, -y, -z);
                GL.Vertex3(x, -y, z);
                GL.Vertex3(-x, -y, z);

                // Right face
                GL.Normal3(1, 0, 0);
                GL.Vertex3(x, -y, -z);
                GL.Vertex3(x, y, -z);
                GL.Vertex3(x, y, z);
                GL.Vertex3(x, -y, z);

                // Left Face
                GL.Normal3(-1, 0, 0);
                GL.Vertex3(-x, -y, -z);
                GL.Vertex3(-x, -y, z);
                GL.Vertex3(-x, y, z);
                GL.Vertex3(-x, y, -z);
            }
            GL.End();

            GL.LineWidth(1);
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Lines);
            {
               // Front Face
                GL.Vertex3(-x, -y, z);
                GL.Vertex3(x, -y, z);
                GL.Vertex3(x, y, z);
                GL.Vertex3(-x, y, z);

                // Back Face
                GL.Vertex3(-x, -y, -z);
                GL.Vertex3(-x, y, -z);
                GL.Vertex3(x, y, -z);
                GL.Vertex3(x, -y, -z);

                // Top Face
                GL.Vertex3(-x, y, -z);
                GL.Vertex3(-x, y, z);
                GL.Vertex3(x, y, z);
                GL.Vertex3(x, y, -z);

                // Bottom Face
                GL.Vertex3(-x, -y, -z);
                GL.Vertex3(x, -y, -z);
                GL.Vertex3(x, -y, z);
                GL.Vertex3(-x, -y, z);

                // Right face
                GL.Vertex3(x, -y, -z);
                GL.Vertex3(x, y, -z);
                GL.Vertex3(x, y, z);
                GL.Vertex3(x, -y, z);

                // Left Face
                GL.Vertex3(-x, -y, -z);
                GL.Vertex3(-x, -y, z);
                GL.Vertex3(-x, y, z);
                GL.Vertex3(-x, y, -z);
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
    }
}