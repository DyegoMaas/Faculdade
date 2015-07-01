using JogoLabirinto.Jogo;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace JogoLabirinto.ObjetosGraficos
{
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
}