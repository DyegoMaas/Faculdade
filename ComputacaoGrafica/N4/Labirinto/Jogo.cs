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
        private readonly Mundo mundo = new Mundo(new Camera());
        private Tabuleiro tabuleiro;
        //private Labirinto labirinto;
        private CuboSolido cuboSolido;

        private Ponto4D centroTabuleiro;
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

            //labirinto = new Labirinto(configuracaoLabirinto);
            //centroTabuleiro = labirinto.Centro;
            ////labirinto.Mover(-50,0,-150);
            //mundo.AdicionarObjetoGrafico(labirinto);


            //cuboSolido = new CuboSolido(Color.LawnGreen);
            //cuboSolido.Redimensionar(20d, new Ponto4D());
            //mundo.AdicionarObjetoGrafico(cuboSolido);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();


            //var alvo = labirinto.Posicao;
            ////a transformação do lookAt parece estar interferindo na rotação do cenário.
            Glu.gluLookAt(
                30, 30, 0,
                0, 0, 0,
                //alvo.X, alvo.Y, alvo.Z,
                0d, 1d, 0d);

            DesenharEixoY();
            tabuleiro.Desenhar();



            SwapBuffers();
        }

        private void DesenharEixoY()
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
            //if (e.XDelta >  1) rotacaoX += .5f;
            //if (e.XDelta < -1) rotacaoX -= .5f;
            //if (e.YDelta >  1) rotacaoZ += .5f;
            //if (e.YDelta < -1) rotacaoZ -= .5f;

            rotacaoX += (e.XDelta * SensibilidadeMouse);
            rotacaoX = rotacaoX.Clamp(-25, 25);
            tabuleiro.RotacaoX = rotacaoX;
            tabuleiro.RotacaoZ = rotacaoZ;
            //var pivoLabirinto = new Ponto4D(200, 100, 200).InverterSinal();
            //var pivoLabirinto = labirinto.Centro.InverterSinal();
            //ponto = pivoLabirinto.InverterSinal();
            //labirinto.RotacionarNoEixoY(-e.YDelta * SensibilidadeMouse, centroTabuleiro);
            //labirinto.RotacionarNoEixoY(-e.YDelta * SensibilidadeMouse, Ponto4D.Zero);


            //labirinto.RotacionarNoEixoX(-e.YDelta * SensibilidadeMouse, centroTabuleiro);
            //labirinto.RotacionarNoEixoZ(e.XDelta * SensibilidadeMouse, centroTabuleiro);

            //var pivoCubo = cuboSolido.Posicao.InverterSinal();
            //cuboSolido.RotacionarNoEixoX(-e.YDelta * SensibilidadeMouse, pivoCubo);
            //cuboSolido.RotacionarNoEixoZ(e.XDelta * SensibilidadeMouse, pivoCubo);
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

                    var posicaoInicial = new Ponto4D(escala * x, 0, escala * z);
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
                            objetosCenario.Esfera = new Esfera(new Ponto4D(posicaoInicial.X, posicaoInicial.Y + 2, posicaoInicial.Z));
                            break;

                        case TipoConteudoCasaTabuleiro.ChaoComParede:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));
                            objetosCenario.Paredes.Add(new Parede(new Ponto4D(posicaoInicial.X, posicaoInicial.Y + 2, posicaoInicial.Z)));
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
        protected readonly Ponto4D Posicao;

        protected ComponenteTabuleiro(Ponto4D posicao)
        {
            Posicao = posicao;
            AntesDeDesenhar(() => GL.Translate(posicao.X, posicao.Y, posicao.Z));
        }
    }

    public class Parede : ComponenteTabuleiro //TODO fazer um desenho próprio
    {
        public Parede(Ponto4D posicao)
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
        public Esfera(Ponto4D posicao) : base(posicao)
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
        public Cacapa(Ponto4D posicao) : base(posicao)
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
        public Chao(Ponto4D posicao) : base(posicao)
        {
        }

        protected override void DesenharObjeto()
        {
            GL.Color3(Color.SaddleBrown);
            GraphicUtils.DesenharCubo(adicionarContornos:true);
        }
    }

    public class Tabuleiro : ObjetoGrafico
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
    }
}