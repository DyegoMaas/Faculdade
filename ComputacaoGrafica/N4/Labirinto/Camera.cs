using JogoLabirinto.Jogo;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using Tao.OpenGl;

namespace JogoLabirinto
{
    public class Camera : IObjetoInteligente
    {
        private double fatorZoom = 1f;

        public double FatorZoom
        {
            get { return fatorZoom; }
            set { fatorZoom = value; }
        }

        private Vector3d posicao;
        public ComportamentoCamera Comportamento { get; set; }

        public Camera(Vector3d posicaoInicial, int largura, int altura)
        {
            Comportamento = ComportamentoCamera.Estatico;
            Reposicionar(posicaoInicial);
            Reshape(largura, altura);
        }

        private const float FOV60 = 1.04f; //60º
        private float FOV = FOV60; 
        private Vector2 tamanhoTela = Vector2.Zero;
        public void Reshape(int largura, int altura)
        {
            tamanhoTela = new Vector2(largura, altura);
            GL.Viewport(0, 0, largura, altura);
            ConfigurarPerspectiva();
        }

        private void ConfigurarPerspectiva()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            var perspectiva = Matrix4.CreatePerspectiveFieldOfView(FOV, tamanhoTela.X / (float)tamanhoTela.Y, 1f, 10000f);
            GL.LoadMatrix(ref perspectiva);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        private Vector3d alvo = Vector3d.Zero;
        public void OlharPara(double targetX, double targetY, double targetZ)
        {
            var posInicial = Comportamento == ComportamentoCamera.Estatico
                ? new Vector3d(raio)
                : posicao;
            Glu.gluLookAt(
                posInicial.X, posInicial.Y, posInicial.Z,
                targetX, targetY, targetZ,
                0d, 1d, 0d);

            var novaPosicaoAlvo = new Vector3d(targetX, targetY, targetZ);
            if (alvo != Vector3d.Zero)
            {
                posicao += (novaPosicaoAlvo - alvo);
            }
            alvo = novaPosicaoAlvo;
        }

        private const double PontosCircunferencia = 720;
        private const double IncrementoAngular = .5d;
        private const double RaioMaximo = 30d;
        private double anguloAtual = 0;
        private double raio = RaioMaximo;
        public void Atualizar()
        {
            if (Comportamento == ComportamentoCamera.RotacionarAoRedor)
            {
                var theta = 2 * Math.PI * anguloAtual / PontosCircunferencia;

                posicao.X = raio * Math.Cos(theta);
                posicao.Z = raio * Math.Sin(theta);

                anguloAtual += IncrementoAngular;
            }
            else
            {
                //TODO zoom out enquanto se aproxima
                if (aproximando)
                {
                    const double fatorDiminuicaoDistancia = .000002d;

                    //aproximando a câmera
                    if (zoomIn)
                    {
                        raio *= fatorDistancia;
                        fatorDistancia -= fatorDiminuicaoDistancia;

                        if (zoomOut) //Dolly effect
                        {
                            var x = raio / RaioMaximo;
                            FOV = FOV60 / (float)x;
                            ConfigurarPerspectiva();
                        }
                    }
                    else if (zoomOut)
                    {
                        //aumentando o FOV
                        fatorZoomOut += fatorDiminuicaoDistancia;
                        FOV = Math.Min((float) (FOV * fatorZoomOut), 1.6f);
                        ConfigurarPerspectiva();
                    }

                    var raioMinimo = zoomOut ? 15 : 10;
                    if (raio < raioMinimo)
                    {
                        raio = raioMinimo;
                        fatorZoomOut = 1;
                        fatorDistancia = 1;
                        aproximando = false;
                    }
                }
            }
        }

        private double fatorZoomOut = 1;
        private double fatorDistancia = 1;
        public void Reposicionar(Vector3d novaPosicao)
        {
            posicao = novaPosicao;
            raio = (novaPosicao.X + novaPosicao.Z ) / 2;
        }

        private bool aproximando, zoomIn, zoomOut;
        public void AproximarComEfeito(bool zoomIn, bool zoomOut)
        {
            aproximando = true;
            this.zoomIn = zoomIn;
            this.zoomOut = zoomOut;
        }
    }

    public enum ComportamentoCamera
    {
        Estatico,
        RotacionarAoRedor
    }
}