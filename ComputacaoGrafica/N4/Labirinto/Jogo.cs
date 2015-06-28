﻿using OpenTK;
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
            linhasReferencia = new LinhasReferencia(numeroLinhas:400, distanciaEntreLinhas:2);
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
            rotacaoX += (e.XDelta * SensibilidadeMouse);
            rotacaoX = rotacaoX.Clamp(-AnguloLimiteRotacao, AnguloLimiteRotacao);

            rotacaoZ += (e.YDelta * SensibilidadeMouse);
            rotacaoZ = rotacaoZ.Clamp(-AnguloLimiteRotacao, AnguloLimiteRotacao);

            tabuleiro.RotacaoX = rotacaoX;
            tabuleiro.RotacaoZ = rotacaoZ;
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            tabuleiro.Atualizar();
        }

        void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
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
            var numeroBlocosEmX = matrizConfiguracao.GetLength(1);
            var numeroBlocosEmZ = matrizConfiguracao.GetLength(0);
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

                            //var posicaoEsfera = new Vector3d(posicaoInicial.X, posicaoInicial.Y + 1, posicaoInicial.Z);
                            var posicaoEsfera = new Vector3d(posicaoInicial.X, posicaoInicial.Y + 1, posicaoInicial.Z);

                            objetosCenario.Esfera = new Esfera(posicaoEsfera)
                            {
                                BoundingBox = new BoundingBox(posicaoEsfera)

                                //BoundingBox = new BoundingBox(posicaoEsfera - new Vector3d(1.5f), posicaoEsfera - new Vector3d(.5f))
                                //BoundingBox = new BoundingBox(posicaoEsfera - new Vector3d(1f), posicaoEsfera + new Vector3d(1f))
                            };
                            break;

                        case TipoConteudoCasaTabuleiro.ChaoComParede:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));

                            var posicaoParede = new Vector3d(posicaoInicial.X, posicaoInicial.Y + 1, posicaoInicial.Z);
                            //var posicaoBBox = new Vector3d(x/2, posicaoInicial.Y + 1, z/2);
                            var min = new Vector3d(x -1, 0, z) - new Vector3d(.5d);
                            var max = new Vector3d(x -1, 0, z) + new Vector3d(.5d);
                            //var min = new Vector3d(posicaoInicial.X - escala / 2, posicaoInicial.Y - escala / 2, posicaoInicial.Z - escala / 2);
                            //var max = new Vector3d(posicaoInicial.X + escala / 2, posicaoInicial.Y + escala / 2, posicaoInicial.Z + escala / 2);
                            objetosCenario.Paredes.Add(new Parede(posicaoParede)
                            {
                                BoundingBox = new BoundingBox(posicaoParede)
                                //BoundingBox = new BoundingBox(min, max)
                                //BoundingBox = new BoundingBox(posicaoBBox - new Vector3d(1f), posicaoBBox - new Vector3d(.5f))
                                //BoundingBox = new BoundingBox(posicaoParede - new Vector3d(1.5f), posicaoParede - new Vector3d(.5f))
                                //BoundingBox = new BoundingBox(posicaoParede - new Vector3d(2f), posicaoParede + new Vector3d(1f))
                                //BoundingBox = new BoundingBox(new Vector3d(posicaoParede.X/escala) - new Vector3d(.5f, .5f, .5f), posicaoParede + new Vector3d(.5f, .5f, .5f))
                            });
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
        protected Vector3d Posicao;
        public BoundingBox BoundingBox { get; set; }

        protected ComponenteTabuleiro(Vector3d posicao)
        {
            Posicao = posicao;
            
            //AplicarTransformacao(translacao);
            AntesDeDesenhar(() =>
            {
                var translacao = Matrix4d.CreateTranslation(Posicao);
                //GL.Translate(Posicao);
                GL.MultMatrix(ref translacao);

                if(BoundingBox != null)
                    BoundingBox.Desenhar();
                    //GraphicUtils.DesenharBoundingBox(BoundingBox);
            });
        }
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
    }

    public class Esfera : ComponenteTabuleiro, IObjetoInteligente
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
                GL.Color3(Color.DarkBlue);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(direcao);
            }
            GL.End();
        }

        public void Atualizar()
        {
            Posicao = Posicao + Velocidade;
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

            var rotacaoX = Matrix4d.CreateRotationX(RotacaoX);
            var rotacaoZ = Matrix4d.CreateRotationZ(RotacaoZ);
            var translacao = Matrix4d.CreateTranslation(-Tamanho.Comprimento / 2, 0, -Tamanho.Largura / 2);
            var matrizEscala = Matrix4d.Scale(escala, escala, escala);
            var resultado = rotacaoX * rotacaoZ * translacao * matrizEscala;
            //var resultado = translacao * matrizEscala * rotacaoX * rotacaoZ;
            //AplicarTransformacao(resultado);
        }

        public void Atualizar()
        {
            Esfera.Velocidade = CalcularVelocidadeEsfera();
            Esfera.Atualizar();
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
                //var rotacaoX = Matrix4d.CreateRotationZ(RotacaoX);
                //var rotacaoZ = Matrix4d.CreateRotationZ(RotacaoZ);
                //var translacao = Matrix4d.CreateTranslation(-Tamanho.Comprimento / 2, 0, -Tamanho.Largura / 2);
                //var matrizEscala = Matrix4d.Scale(escala);
                //MatrizTransformacao = rotacaoX * rotacaoZ * translacao * matrizEscala;

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

        private Matrix4d matrizTransformacao;

        protected ObjetoGrafico()
        {
            matrizTransformacao = Matrix4d.Identity;
        }

        protected void AplicarTransformacao(Matrix4d novaMatrizTransformacao)
        {
            matrizTransformacao = novaMatrizTransformacao;
        }

        public void Desenhar()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                //GL.MultMatrix(ref matrizTransformacao);

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
        //public static void DesenharCubo(float escalaX = .5f, float escalaY = .5f, float escalaZ = .5f, bool adicionarContornos = false)
        //{
        //    float x = escalaX,
        //        y = escalaY,
        //        z = escalaZ;

        //    GL.Begin(PrimitiveType.Quads);
        //    {
        //        // Front Face
        //        GL.Normal3(0, 0, 1);
        //        GL.Vertex3(-x, -y, z);
        //        GL.Vertex3(x, -y, z);
        //        GL.Vertex3(x, y, z);
        //        GL.Vertex3(-x, y, z);

        //        // Back Face
        //        GL.Normal3(0, 0, -1);
        //        GL.Vertex3(-x, -y, -z);
        //        GL.Vertex3(-x, y, -z);
        //        GL.Vertex3(x, y, -z);
        //        GL.Vertex3(x, -y, -z);

        //        // Top Face
        //        GL.Normal3(0, 1, 0);
        //        GL.Vertex3(-x, y, -z);
        //        GL.Vertex3(-x, y, z);
        //        GL.Vertex3(x, y, z);
        //        GL.Vertex3(x, y, -z);

        //        // Bottom Face
        //        GL.Normal3(0, -1, 0);
        //        GL.Vertex3(-x, -y, -z);
        //        GL.Vertex3(x, -y, -z);
        //        GL.Vertex3(x, -y, z);
        //        GL.Vertex3(-x, -y, z);

        //        // Right face
        //        GL.Normal3(1, 0, 0);
        //        GL.Vertex3(x, -y, -z);
        //        GL.Vertex3(x, y, -z);
        //        GL.Vertex3(x, y, z);
        //        GL.Vertex3(x, -y, z);

        //        // Left Face
        //        GL.Normal3(-1, 0, 0);
        //        GL.Vertex3(-x, -y, -z);
        //        GL.Vertex3(-x, -y, z);
        //        GL.Vertex3(-x, y, z);
        //        GL.Vertex3(-x, y, -z);
        //    }
        //    GL.End();

        //    GL.LineWidth(1);
        //    GL.Color3(Color.Black);
        //    GL.Begin(PrimitiveType.Lines);
        //    {
        //        //TODO remover arestas duplicadas

        //        // Front Face
        //        GL.Vertex3(-x, -y, z);
        //        GL.Vertex3(x, -y, z);
        //        GL.Vertex3(x, y, z);
        //        GL.Vertex3(-x, y, z);

        //        // Back Face
        //        GL.Vertex3(-x, -y, -z);
        //        GL.Vertex3(-x, y, -z);
        //        GL.Vertex3(x, y, -z);
        //        GL.Vertex3(x, -y, -z);

        //        // Top Face
        //        GL.Vertex3(-x, y, -z);
        //        GL.Vertex3(-x, y, z);
        //        GL.Vertex3(x, y, z);
        //        GL.Vertex3(x, y, -z);

        //        // Bottom Face
        //        GL.Vertex3(-x, -y, -z);
        //        GL.Vertex3(x, -y, -z);
        //        GL.Vertex3(x, -y, z);
        //        GL.Vertex3(-x, -y, z);

        //        // Right face
        //        GL.Vertex3(x, -y, -z);
        //        GL.Vertex3(x, y, -z);
        //        GL.Vertex3(x, y, z);
        //        GL.Vertex3(x, -y, z);

        //        // Left Face
        //        GL.Vertex3(-x, -y, -z);
        //        GL.Vertex3(-x, -y, z);
        //        GL.Vertex3(-x, y, z);
        //        GL.Vertex3(-x, y, -z);
        //    }
        //    GL.End();
        //}

        public static void DesenharBoundingBox(BoundingBox bBox)
        {
            var min = bBox.Min;
            var max = bBox.Max;
            GL.LineWidth(1);
            GL.Color3(Color.BurlyWood);

            //float x = .5f, 
            //    y = .5f, 
            //    z = .5f;
            //GL.Begin(PrimitiveType.Lines);
            //{
            //    //TODO remover arestas duplicadas

            //    // Front Face
            //    GL.Vertex3(-x, -y, z);
            //    GL.Vertex3(x, -y, z);
            //    GL.Vertex3(x, y, z);
            //    GL.Vertex3(-x, y, z);

            //    // Back Face
            //    GL.Vertex3(-x, -y, -z);
            //    GL.Vertex3(-x, y, -z);
            //    GL.Vertex3(x, y, -z);
            //    GL.Vertex3(x, -y, -z);

            //    // Top Face
            //    GL.Vertex3(-x, y, -z);
            //    GL.Vertex3(-x, y, z);
            //    GL.Vertex3(x, y, z);
            //    GL.Vertex3(x, y, -z);

            //    // Bottom Face
            //    GL.Vertex3(-x, -y, -z);
            //    GL.Vertex3(x, -y, -z);
            //    GL.Vertex3(x, -y, z);
            //    GL.Vertex3(-x, -y, z);

            //    // Right face
            //    GL.Vertex3(x, -y, -z);
            //    GL.Vertex3(x, y, -z);
            //    GL.Vertex3(x, y, z);
            //    GL.Vertex3(x, -y, z);

            //    // Left Face
            //    GL.Vertex3(-x, -y, -z);
            //    GL.Vertex3(-x, -y, z);
            //    GL.Vertex3(-x, y, z);
            //    GL.Vertex3(-x, y, -z);
            //}
            //GL.End();

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

        public BoundingBox(Vector3d posicao)
            : base()
        {
            new Matrix4d();
            GraphicUtils.TransformarEmCubo(this);
            Wireframe = true;
            AntesDeDesenhar(() => GL.Color3(Color.DarkKhaki));
        }

        public BoundingBox(Vector3d posicao, BoundingBox box) 
            : this(posicao)
        {
            min = box.Min;
            max = box.Max;
        }

        public BoundingBox(Vector3d posicao, Vector3d min, Vector3d max)
            : this(posicao)
        {
            this.min = min;
            this.max = max;
        }

        //public BoundingBox(IMesh mesh)
        //{
        //    min = new Vector3d(9999.0f, 9999.0f, 9999.0f); //...
        //    max = -min;

        //    foreach (var vertice in mesh.Faces)
        //    {
        //        if (max.X < vertice.X)
        //            max.X = vertice.X;
               
        //        if (max.Z < vertice.Z)
        //            max.Z = vertice.Z;

        //        if (min.X > vertice.X)
        //            min.X = vertice.X;
                
        //        if (min.Z > vertice.Z)
        //            min.Z = vertice.Z;
        //    }
        //}
        protected override void DesenharObjeto()
        {
            //TODO desenhar
        }
    }

    ////TODO precisa??
    //public interface IMesh
    //{
    //    Vector3d[] Faces { get; set; }
    //}
}