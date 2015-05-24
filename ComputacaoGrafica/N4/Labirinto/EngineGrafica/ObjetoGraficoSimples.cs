using OpenTK.Graphics.OpenGL;
using System;

namespace Labirinto.EngineGrafica
{
    public abstract class ObjetoGraficoSimples : NoGrafoCenaSimples
    {
        public Transformacao4D Transformacao { get; private set; }

        private static readonly Transformacao4D MatrizTmpTranslacao = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpTranslacaoInversa = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpEscala = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpRotacaoZ = new Transformacao4D();
        private static Transformacao4D matrizGlobal = new Transformacao4D();

        public Ponto4D Posicao
        {
            get { return Transformacao.TransformarPonto(new Ponto4D()); }
        }

        protected ObjetoGraficoSimples()
        {
            Transformacao = new Transformacao4D();
        }

        public void DesenharObjetoGrafico()
        {
            //GL.Color3(Cor);
            //GL.PointSize(TamanhoPonto);
            //GL.LineWidth(LarguraLinha);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.MultMatrix(Transformacao.Data);
                Desenhar();

                foreach (var objetoGrafico in ObjetosGraficos)
                {
                    objetoGrafico.DesenharObjetoGrafico();
                }
            }
            GL.PopMatrix();
        }

        protected abstract void Desenhar();

        /// <summary>
        /// Move o objeto
        /// </summary>
        /// <param name="x">Dist�ncia em que o objeto ser� movido no eixo X</param>
        /// <param name="y">Dist�ncia em que o objeto ser� movido no eixo Y</param>
        /// <param name="z">Dist�ncia em que o objeto ser� movido no eixo Z</param>
        public void Mover(double x, double y, double z)
        {
            var matrizTranslacao = new Transformacao4D();
            matrizTranslacao.AtribuirTranslacao(x, y, z);

            Transformacao = matrizTranslacao.TransformarMatriz(Transformacao);
            RecalcularBBox();
        }

        /// <summary>
        /// Redimensiona o objeto em rela��o ao piv�
        /// </summary>
        /// <param name="escala">A escala pela qual o objeto ser� redimensionado</param>
        /// <param name="pivo">Ponto ao redor do qual o objeto ser� redimensionado</param>
        public void Redimensionar(double escala, Ponto4D pivo)
        {
            matrizGlobal.AtribuirIdentidade();

            ExecutarEmRelacaoAoPivo(pivo, () =>
            {
                MatrizTmpEscala.AtribuirEscala(escala, escala, 1.0);
                matrizGlobal = MatrizTmpEscala.TransformarMatriz(matrizGlobal);
            });

            Transformacao = matrizGlobal.TransformarMatriz(Transformacao);
            RecalcularBBox();
        }

        /// <summary>
        /// Rotaciona o objeto em rela��o ao piv�
        /// </summary>
        /// <param name="angulo">�ngulo em graus</param>
        /// <param name="pivo">Ponto ao redor do qual o objeto ser� rotacionado</param>
        public void RotacionarNoEixoZ(double angulo, Ponto4D pivo)
        {
            matrizGlobal.AtribuirIdentidade();

            ExecutarEmRelacaoAoPivo(pivo, () =>
            {
                MatrizTmpRotacaoZ.AtribuirRotacaoZ(angulo * Transformacao4D.DegToRad);
                matrizGlobal = MatrizTmpRotacaoZ.TransformarMatriz(matrizGlobal);
            });

            Transformacao = matrizGlobal.TransformarMatriz(Transformacao);
            RecalcularBBox();
        }

        private void ExecutarEmRelacaoAoPivo(Ponto4D pivo, Action acao)
        {
            MatrizTmpTranslacao.AtribuirTranslacao(pivo.X, pivo.Y, pivo.Z);
            matrizGlobal = MatrizTmpTranslacao.TransformarMatriz(matrizGlobal);

            acao.Invoke();

            pivo.InverterSinal();
            MatrizTmpTranslacaoInversa.AtribuirTranslacao(pivo.X, pivo.Y, pivo.Z);
            matrizGlobal = MatrizTmpTranslacaoInversa.TransformarMatriz(matrizGlobal);
        }

        /// <summary>
        /// Recalcula a BBox aplicando as transforma��es da hierarquia do objeto no grafo de cena
        /// </summary>
        public void RecalcularBBox()
        {
            //var verticesAjustados = Vertices.Select(TransformarPontoParaEsteObjetoGrafico);
            //BoundaryBox.RecalcularPara(verticesAjustados);
        }
    }
}