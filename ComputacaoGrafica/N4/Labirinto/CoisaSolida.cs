using System;

namespace JogoLabirinto
{
    public abstract class CoisaSolida
    {
        public Transformacao4D Transformacao { get; private set; }
        private static readonly Transformacao4D MatrizTmpTranslacao = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpEscala = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpTranslacaoInversa = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpRotacao = new Transformacao4D();
        private static Transformacao4D matrizGlobal = new Transformacao4D();

        public Ponto4D Posicao
        {
            get { return Transformacao.TransformarPonto(new Ponto4D()); }
        }

        /// <summary>
        /// Move o objeto
        /// </summary>
        /// <param name="x">Distância em que o objeto será movido no eixo X</param>
        /// <param name="y">Distância em que o objeto será movido no eixo Y</param>
        /// <param name="z">Distância em que o objeto será movido no eixo Z</param>
        public void Mover(double x, double y, double z)
        {
            var matrizTranslacao = new Transformacao4D();
            matrizTranslacao.AtribuirTranslacao(x, y, z);

            Transformacao = matrizTranslacao.TransformarMatriz(Transformacao);
        }

        /// <summary>
        /// Redimensiona o objeto em relação ao pivô
        /// </summary>
        /// <param name="escala">A escala pela qual o objeto será redimensionado</param>
        /// <param name="pivo">Ponto ao redor do qual o objeto será redimensionado</param>
        public void Redimensionar(double escala, Ponto4D pivo)
        {
            Redimensionar(escala, escala, escala, pivo);
        }

        /// <summary>
        /// Redimensiona o objeto em relação ao pivô
        /// </summary>
        /// <param name="pivo">Ponto ao redor do qual o objeto será redimensionado</param>
        public void Redimensionar(double escalaX, double escalaY, double escalaZ, Ponto4D pivo)
        {
            matrizGlobal.AtribuirIdentidade();

            ExecutarEmRelacaoAoPivo(pivo, () =>
            {
                MatrizTmpEscala.AtribuirEscala(escalaX, escalaY, escalaZ);
                matrizGlobal = MatrizTmpEscala.TransformarMatriz(matrizGlobal);
            });

            Transformacao = matrizGlobal.TransformarMatriz(Transformacao);
        }

        private void ExecutarEmRelacaoAoPivo(Ponto4D pivo, Action acao)
        {
            pivo.InverterSinal();
            MatrizTmpTranslacao.AtribuirTranslacao(pivo.X, pivo.Y, pivo.Z);
            matrizGlobal = MatrizTmpTranslacao.TransformarMatriz(matrizGlobal);

            acao.Invoke();

            pivo.InverterSinal();
            MatrizTmpTranslacaoInversa.AtribuirTranslacao(pivo.X, pivo.Y, pivo.Z);
            matrizGlobal = MatrizTmpTranslacaoInversa.TransformarMatriz(matrizGlobal);
        }
    }
}