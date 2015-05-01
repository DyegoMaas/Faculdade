using Exercicio01.EngineGrafica;

namespace Exercicio01.Editor
{
    public sealed class ObjetoEmEdicao
    {
        public readonly ObjetoGrafico ObjetoGrafico;

        public ObjetoEmEdicao(ObjetoGrafico objetoGrafico)
        {
            ObjetoGrafico = objetoGrafico;
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
    }
}