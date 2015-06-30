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
        private LinhasReferencia linhasReferencia;

        private bool ligarLuzes = true;

        public Jogo()
        {
            Location = new Point(50, 50);
            Title = "Labyrinth";

            Load += (sender, e) =>
            {
                WindowState = WindowState.Fullscreen;

                Glut.glutInit();
                GL.ClearColor(Color.CornflowerBlue);

                var corLuzAmbiente = new[] { 0.4f, 0.4f, 0.4f, 1.0f };
                GL.LightModel(LightModelParameter.LightModelAmbient, corLuzAmbiente);

                var lightColor0 = new []{ .8f, .8f, .8f, 1.0f };
                var lightPos0 = new[] { 5.0f, 10f, 10.0f, 0.0f };
                GL.Light(LightName.Light0, LightParameter.Diffuse, lightColor0);
                GL.Light(LightName.Light0, LightParameter.Position, lightPos0);
	            GL.Enable(EnableCap.Light0);

                var lightColor1 = new[] { .8f, .8f, .8f, 1.0f };
                var lightPos1 = new[] { -10.0f, 10f, 10.0f, 0.0f };
                GL.Light(LightName.Light1, LightParameter.Diffuse, lightColor1);
                GL.Light(LightName.Light1, LightParameter.Position, lightPos1);
                GL.Enable(EnableCap.Light1);

                GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.AmbientAndDiffuse); // depende do GL_COLOR_MATERIAL

                GL.Enable(EnableCap.CullFace);
                GL.Enable(EnableCap.DepthTest);

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
                {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'j', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'p', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'c', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'p', 'p', 'c', 'c', 'c', 'c', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'c', 'p', 'p', 'c', 'b', 'b', 'p', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'p', 'p', 'c', 'c', 'c', 'p', 'p', 'c', 'c', 'b', 'b', 'c', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'p', 'p', 'c', 'c', 'p', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'p', 'p', 'p', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'}
            },
            escala: 2,
            tamanhoParede: new Vector3d(1, 1, 1));

            tabuleiro = GeradorCenario.GerarCenario(configuracaoLabirinto);
            linhasReferencia = new LinhasReferencia(numeroLinhas:1000, distanciaEntreLinhas:10);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            if (ligarLuzes)
            {
                GL.Enable(EnableCap.Lighting);
                GL.Enable(EnableCap.ColorMaterial); // https://www.opengl.org/sdk/docs/man2/xhtml/glColorMaterial.xml
                GL.Color3(Color.White);
            }
            else
            {
                //if (colorMaterial)
                //{
                //    gl.glEnable(GL.GL_COLOR_MATERIAL); // https://www.opengl.org/sdk/docs/man2/xhtml/glColorMaterial.xml
                //    gl.glColor3f(cor[0], cor[1], cor[2]);
                //}
                //else
                //{
                GL.Material(MaterialFace.Front, MaterialParameter.AmbientAndDiffuse, Color4.White);
                    // https://www.opengl.org/sdk/docs/man2/xhtml/glMaterial.xml  	
                //}
            }

            Glu.gluLookAt(
                30, 40, 30,
                tabuleiro.Esfera.Posicao.X, tabuleiro.Esfera.Posicao.Y, tabuleiro.Esfera.Posicao.Z,
                0d, 1d, 0d);

            DesenharEixos();
            linhasReferencia.Desenhar();
            tabuleiro.Desenhar();

            GL.Disable(EnableCap.Lighting);
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

            if (e.Key == Key.L)
                ligarLuzes = !ligarLuzes;

            if (e.Key == Key.R)
            {
                rodando = false;
                ConfigurarCena();
            }
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
            MotorColisoes.Reiniciar();

            var matrizConfiguracao = configuracao.MatrizConfiguracao;
            var numeroBlocosEmX = matrizConfiguracao.GetLength(1);
            var numeroBlocosEmZ = matrizConfiguracao.GetLength(0);
            var objetosCenario = new Tabuleiro(new SizeD(numeroBlocosEmX, numeroBlocosEmZ), configuracao.Escala);

            int nTochas = 0;
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
                            var cacapa = new Cacapa(posicaoInicial);
                            objetosCenario.Cacapas.Add(cacapa);
                            MotorColisoes.Alvos.Add(cacapa);
                            break;
                        case TipoConteudoCasaTabuleiro.Chao:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));
                            break;

                        case TipoConteudoCasaTabuleiro.ChaoComEsfera:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));

                            var posicaoEsfera = new Vector3d(posicaoInicial.X, posicaoInicial.Y + 1, posicaoInicial.Z);

                            var esfera = new Esfera(posicaoEsfera)
                            {
                                //BoundingBox = new BoundingBox()
                            };
                            objetosCenario.Esfera = esfera;
                            MotorColisoes.Esfera = esfera;
                            break;

                        case TipoConteudoCasaTabuleiro.ChaoComParede:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));
                            AdicionarParede(posicaoInicial, objetosCenario);
                            break;

                        case TipoConteudoCasaTabuleiro.ChaoComParedeETocha:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));
                            AdicionarParede(posicaoInicial, objetosCenario);
                            AdicionarTocha(posicaoInicial, objetosCenario, nTochas++);

                            break;
                    }
                }
            }

            return objetosCenario;
        }

        private static void AdicionarParede(Vector3d posicaoInicial, Tabuleiro objetosCenario)
        {
            var posicaoParede = new Vector3d(posicaoInicial.X, posicaoInicial.Y + 1, posicaoInicial.Z);
            var parede = new Parede(posicaoParede)
            {
                //BoundingBox = new BoundingBox()
            };
            objetosCenario.Paredes.Add(parede);
            MotorColisoes.Paredes.Add(parede);
        }

        private static void AdicionarTocha(Vector3d posicaoInicial, Tabuleiro objetosCenario, int indiceTocha)
        {
            var posicaoParede = new Vector3d(posicaoInicial.X, posicaoInicial.Y + 2, posicaoInicial.Z);
            var parede = new Tocha(posicaoParede, indiceTocha);
            objetosCenario.Tochas.Add(parede);
        }

        private static TipoConteudoCasaTabuleiro TipoConteudo(char config)
        {
            switch (config)
            {
                case 'c': return TipoConteudoCasaTabuleiro.Chao;
                case 'p': return TipoConteudoCasaTabuleiro.ChaoComParede;
                case 'j': return TipoConteudoCasaTabuleiro.ChaoComEsfera;
                case 'f': return TipoConteudoCasaTabuleiro.ChaoComParedeETocha;
                default: return TipoConteudoCasaTabuleiro.Cacapa;
            }
        }

        private enum TipoConteudoCasaTabuleiro
        {
            Chao = 1,
            ChaoComParede = 2,
            ChaoComEsfera = 3,
            ChaoComParedeETocha = 4,
            Cacapa = 5
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

    internal class Tocha : ComponenteTabuleiro
    {
        public int IndiceTocha { get; private set; }

        public Tocha(Vector3d posicao, int indiceTocha) : base(posicao)
        {
            IndiceTocha = indiceTocha;
            GraphicUtils.TransformarEmCubo(this, x:.2f, y: .3f, z:.2f);
            AntesDeDesenhar(() =>
            {
                GL.Color3(Color.Yellow);

                var luz = (LightName)((int) LightName.Light1 + IndiceTocha);
                var luzCap = (EnableCap)((int)EnableCap.Light1 + IndiceTocha);
                var lightPos1 = new[] { (float)posicao.X, (float)posicao.Y + 3, (float)posicao.Z, 0.0f };
                GL.Enable(luzCap);
                GL.Light(luz, LightParameter.Position, lightPos1);
                GL.Light(luz, LightParameter.Diffuse, Color.White);
                //GL.Light(luz, LightParameter.SpotExponent, .9f);
                GL.Light(luz, LightParameter.SpotCutoff, 30);
                GL.Light(luz, LightParameter.SpotDirection, new Vector4(0, -1, 0, 0));
                GL.Light(luz, LightParameter.Specular, new Vector4(3, 3, 3, 1));
                //GL.Light(luz, LightParameter.LinearAttenuation, 1);
                //GL.Light(luz, LightParameter.QuadraticAttenuation, 1);
                GL.Light(luz, LightParameter.ConstantAttenuation, 1);
            });
        }

        protected override void DesenharObjeto()
        {
        }

        public override void Atualizar()
        {
        }
    }

    public class Parede : ComponenteTabuleiro
    {
        public Parede(Vector3d posicao)
            : base(posicao)
        {
            Wireframe = true;
            GraphicUtils.TransformarEmCubo(this);
            AntesDeDesenhar(() => GL.Color3(Color.DimGray));
        }

        protected override void DesenharObjeto()
        {
        }

        public override void Atualizar()
        {
        }
    }

    public class Chao : ComponenteTabuleiro
    {
        public Chao(Vector3d posicao) : base(posicao)
        {
            Wireframe = true;
            GraphicUtils.TransformarEmCubo(this);
            AntesDeDesenhar(() => GL.Color3(Color.SaddleBrown));
        }

        protected override void DesenharObjeto()
        {
        }

        public override void Atualizar()
        {
            
        }
    }

    public class Esfera : ComponenteTabuleiro
    {
        private const double FatorQueda = .03;
        public Vector3d Velocidade { get; set; }
        private bool visivel = true;
        
        public Esfera(Vector3d posicao)
            : base(posicao)
        {
            AntesDeDesenhar(() =>
            {
                if (!visivel)
                    return;

                GL.Color3(Color.Red);
                Glut.glutSolidSphere(.5f, 10, 10);
            });
        }

        protected override void DesenharObjeto()
        {
            if (visivel)
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

        private bool encontrouAlvo;
        public override void Atualizar()
        {
            if (encontrouAlvo)
            {
                if (Posicao.Y < -1)
                    visivel = false;
                else
                    Posicao -= Vector3d.UnitY * FatorQueda;
                return;
            }

            Posicao = MotorColisoes.NovaPosicaoDaEsfera(Posicao, Velocidade);

            if (MotorColisoes.EncontrouOAlvo(Posicao))
            {
                Console.WriteLine("Encontrou o alvo");
                encontrouAlvo = true;
            }
        }
    }

    public class MotorColisoes
    {
        public static Esfera Esfera { get; set; }
        public static IList<Parede> Paredes { get; set; }
        public static IList<Cacapa> Alvos { get; set; }

        static MotorColisoes()
        {
            Paredes = new List<Parede>();
            Alvos = new List<Cacapa>();
        }

        public static void Reiniciar()
        {
            Esfera = null;
            Paredes.Clear();
            Alvos.Clear();
        }

        public static Vector3d NovaPosicaoDaEsfera(Vector3d posicaoAtual, Vector3d velocidade)
        {
            var novaPosicaoDaEsfera = posicaoAtual + velocidade;
            for (int i = 0; i < Paredes.Count; i++)
            {
                var parede = Paredes[i];

                var count = 0;
                calculoColisao:
                var distancia = novaPosicaoDaEsfera - parede.Posicao;
                var distanciaAbs = new Vector3d(Math.Abs(distancia.X), Math.Abs(distancia.Y), Math.Abs(distancia.Z));
                
                if (distanciaAbs.X < 1 && distanciaAbs.Z < 1) //entrou no objeto
                {
                    Console.WriteLine("atravessou a parede {0} por {1}", i, distanciaAbs);

                    if (distanciaAbs.X > distanciaAbs.Z)
                        velocidade.X = 0;
                    if (distanciaAbs.X < distanciaAbs.Z)
                        velocidade.Z = 0;

                    novaPosicaoDaEsfera = posicaoAtual + velocidade;
                    if (++count > 1)
                        return posicaoAtual;

                    goto calculoColisao;
                }
            }
            return novaPosicaoDaEsfera;
        }

        public static bool EncontrouOAlvo(Vector3d posicaoAtual)
        {
            for (int i = 0; i < Alvos.Count; i++)
            {
                var alvo = Alvos[i];

                var distancia = posicaoAtual - alvo.Posicao;
                var distanciaAbs = new Vector3d(Math.Abs(distancia.X), Math.Abs(distancia.Y), Math.Abs(distancia.Z));

                if (distanciaAbs.X < .5 && distanciaAbs.Z < .5) //entrou no objeto
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Cacapa : ComponenteTabuleiro
    {
        public Cacapa(Vector3d posicao)
            : base(posicao)
        {
            GraphicUtils.TransformarEmCubo(this, y: .25f);
            AntesDeDesenhar(() => GL.Color3(Color.Black));
        }

        protected override void DesenharObjeto()
        {
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
                    GL.Vertex3(-1000, -50, i);
                    GL.Vertex3(1000, -50, i);

                    GL.Vertex3(i, -50, -1000);
                    GL.Vertex3(i, -50, 1000);
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
        public readonly List<IObjetoGrafico> Cacapas = new List<IObjetoGrafico>();
        public readonly List<IObjetoGrafico> Tochas = new List<IObjetoGrafico>();
        private double escala;
        public Esfera Esfera { get; set; }

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
                Cacapas.ForEach(c => c.Desenhar());
                BlocosChao.ForEach(b => b.Desenhar());
                Paredes.ForEach(p => p.Desenhar());
                Tochas.ForEach(p => p.Desenhar());
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
                    GraphicUtils.DesenharFace(face);
                }

                DesenharObjeto();

                if (Wireframe)
                {
                    GL.Color3(Color.Black);
                    foreach (var face in Faces)
                    {
                        GraphicUtils.DesenharWireframe(face);
                    }
                }
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

            GL.Color3(Color.Black);
            if (adicionarContornos)
            {
                Glut.glutWireCube(1);
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

        public static void DesenharFace(Face face)
        {
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(face.Normal);
                foreach (var vertice in face.Vertices)
                {
                    GL.Vertex3(vertice);
                }    
            }
            GL.End();
        }

        public static void DesenharWireframe(Face face)
        {
            GL.Begin(PrimitiveType.Lines);
            {
                foreach (var vertice in face.Vertices)
                {
                    GL.Vertex3(vertice);
                }
            }
            GL.End();
        }

        public static void TransformarEmCubo(ObjetoGrafico objetoGrafico, float x = .5f, float y = .5f, float z = .5f)
        {
            // Front Face
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

            // Left face
            objetoGrafico.Faces.Add(new Face(new Vector3d(1, 0, 0), new[]
            {
                new Vector3d(-x, -y, -z),
                new Vector3d(-x, -y, z),
                new Vector3d(-x, y, z),
                new Vector3d(-x, y, -z)
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