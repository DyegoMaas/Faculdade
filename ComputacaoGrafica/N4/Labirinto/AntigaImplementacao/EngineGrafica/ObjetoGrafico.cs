using OpenTK.Graphics.OpenGL;
using System;

namespace JogoLabirinto.EngineGrafica
{
    public abstract class ObjetoGrafico : NoGrafoCena
    {
        public Transformacao4D Transformacao { get; private set; }

        private static readonly Transformacao4D MatrizTmpTranslacao = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpTranslacaoInversa = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpEscala = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpRotacao = new Transformacao4D();
        private static Transformacao4D matrizGlobal = new Transformacao4D();

        public Ponto4D Posicao
        {
            get { return Transformacao.TransformarPonto(new Ponto4D()); }
        }

        protected ObjetoGrafico()
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
        /// <param name="x">Distância em que o objeto será movido no eixo X</param>
        /// <param name="y">Distância em que o objeto será movido no eixo Y</param>
        /// <param name="z">Distância em que o objeto será movido no eixo Z</param>
        public void Mover(double x, double y, double z)
        {
            var matrizTranslacao = new Transformacao4D();
            matrizTranslacao.AtribuirTranslacao(x, y, z);

            Transformacao = matrizTranslacao.TransformarMatriz(Transformacao);
            RecalcularBBox();
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
        /// <param name="escala">A escala pela qual o objeto será redimensionado</param>
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
            RecalcularBBox();
        }

        public virtual void RotacionarNoEixoX(double angulo, Ponto4D pivo)
        {
            matrizGlobal.AtribuirIdentidade();

            ExecutarEmRelacaoAoPivo(pivo, () =>
            {
                MatrizTmpRotacao.AtribuirRotacaoX(angulo * Transformacao4D.DegToRad);
                matrizGlobal = MatrizTmpRotacao.TransformarMatriz(matrizGlobal);
            });

            Transformacao = matrizGlobal.TransformarMatriz(Transformacao);
            RecalcularBBox();
        }

        public virtual void RotacionarNoEixoY(double angulo, Ponto4D pivo)
        {
            matrizGlobal.AtribuirIdentidade();

            ExecutarEmRelacaoAoPivo(pivo, () =>
            {
                MatrizTmpRotacao.AtribuirRotacaoY(angulo * Transformacao4D.DegToRad);
                matrizGlobal = MatrizTmpRotacao.TransformarMatriz(matrizGlobal);
            });

            Transformacao = matrizGlobal.TransformarMatriz(Transformacao);
            RecalcularBBox();
        }

        /// <summary>
        /// Rotaciona o objeto em relação ao pivô
        /// </summary>
        /// <param name="angulo">Ângulo em graus</param>
        /// <param name="pivo">Ponto ao redor do qual o objeto será rotacionado</param>
        public virtual void RotacionarNoEixoZ(double angulo, Ponto4D pivo)
        {
            matrizGlobal.AtribuirIdentidade();

            ExecutarEmRelacaoAoPivo(pivo, () =>
            {
                MatrizTmpRotacao.AtribuirRotacaoZ(angulo * Transformacao4D.DegToRad);
                matrizGlobal = MatrizTmpRotacao.TransformarMatriz(matrizGlobal);
            });

            Transformacao = matrizGlobal.TransformarMatriz(Transformacao);
            RecalcularBBox();
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

        /// <summary>
        /// Recalcula a BBox aplicando as transformações da hierarquia do objeto no grafo de cena
        /// </summary>
        public void RecalcularBBox()
        {
            //var verticesAjustados = Vertices.Select(TransformarPontoParaEsteObjetoGrafico);
            //BoundaryBox.RecalcularPara(verticesAjustados);
        }
    }
}