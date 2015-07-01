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

        public Camera(Vector3d posicaoInicial)
        {
            Comportamento = ComportamentoCamera.Estatico;
            Reposicionar(posicaoInicial);
        }

        public void Reshape(int largura, int altura)
        {
            GL.Viewport(0, 0, largura, altura);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            var perspectiva = Matrix4.CreatePerspectiveFieldOfView(1.04f, largura / (float)altura, 1f, 10000f);
            GL.LoadMatrix(ref perspectiva);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        private Vector3d alvo = Vector3d.Zero;
        private double raioRotacao = 0;
        public void OlharPara(double targetX, double targetY, double targetZ)
        {
            Glu.gluLookAt(
                posicao.X, posicao.Y, posicao.Z,
                targetX, targetY, targetZ,
                0d, 1d, 0d);
            raioRotacao = (targetX + targetZ) / 2;

            var novaPosicaoAlvo = new Vector3d(targetX, targetY, targetZ);
            if (alvo != Vector3d.Zero)
            {
                posicao += (novaPosicaoAlvo - alvo);
            }
            alvo = novaPosicaoAlvo;
        }

        private const double PontosCircunferencia = 720;
        private double anguloAtual = 0;
        private const double IncrementoAngular = .5d;
        public void Atualizar()
        {
            //TODO zoom out enquanto se aproxima

            if (Comportamento == ComportamentoCamera.RotacionarAoRedor)
            {
                var theta = 2 * Math.PI * anguloAtual / PontosCircunferencia;

                posicao.X = raioRotacao * Math.Cos(theta);
                posicao.Z = raioRotacao * Math.Sin(theta);

                anguloAtual += IncrementoAngular;
            }
        }

        public void Reposicionar(Vector3d novaPosicao)
        {
            posicao = novaPosicao;
        }
    }

    public enum ComportamentoCamera
    {
        Estatico,
        RotacionarAoRedor
    }
}