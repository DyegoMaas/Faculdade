using System.Linq;
using Exercicio01.EngineGrafica;

namespace Exercicio01.Editor
{
    public sealed class ObjetoEmEdicao
    {
        public readonly ObjetoGrafico ObjetoGrafico;
        private readonly InputManager input;

        public ObjetoEmEdicao(ObjetoGrafico objetoGrafico, InputManager input)
        {
            ObjetoGrafico = objetoGrafico;
            this.input = input;
        }

        private double translacaoX;
        private double translacaoY;
        private double translacaoZ;

        /// <summary>
        /// Em radianos
        /// </summary>
        public double RotacaoX { get; set; }

        /// <summary>
        /// Em radianos
        /// </summary>
        public double RotacaoY { get; set; }

        /// <summary>
        /// Em radianos
        /// </summary>
        public double RotacaoZ { get; set; }

        public void Mover(double x, double y, double z)
        {
            translacaoX += x;
            translacaoY += y;
            translacaoZ += z;
            ObjetoGrafico.Transformacao.AtribuirTranslacao(translacaoX, translacaoY, translacaoZ);
        }

        public void AdicionarVertice()
        {
            var posicaoMouse = input.ObterPosicaoMouseNaTela();
            ObjetoGrafico.Vertices.Add(new Ponto4D(posicaoMouse.X, posicaoMouse.Y));
        }

        public void RemoverVertice()
        {
            if (ObjetoGrafico.Vertices.Any())
            {
                var indiceUltimoVertice = ObjetoGrafico.Vertices.Count - 1;
                ObjetoGrafico.Vertices.RemoveAt(indiceUltimoVertice);
            }
        }
    }
}