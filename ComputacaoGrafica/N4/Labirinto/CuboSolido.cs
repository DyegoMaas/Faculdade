using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Tao.FreeGlut;

namespace JogoLabirinto
{
    public class CuboSolido : CoisaSolida
    {
        private Color cor;

        public CuboSolido(Color cor)
        {
            this.cor = cor;
        }

        protected void Desenhar()
        {
            GL.Color3(cor);
            Glut.glutSolidCube(1f);
        } 
    }


    public abstract class ObjetoGraficoLabirinto
    {
        public Transformacao4D Transformacao { get; private set; }
        private Transformacao4D transformacaoTransalacaoX = new Transformacao4D();
        private Transformacao4D transformacaoTransalacaoZ = new Transformacao4D();
        private Transformacao4D transformacaoTransalacaoY = new Transformacao4D();
        private static Transformacao4D matrizGlobal = new Transformacao4D();

        private static readonly Transformacao4D MatrizTmpTranslacao = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpTranslacaoInversa = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpRotacao = new Transformacao4D();

        public Ponto4D Posicao
        {
            get { return transformacaoTransalacaoX.TransformarPonto(new Ponto4D()); }
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
            matrizTranslacao.AtribuirTranslacao(x, 1, 1);
            transformacaoTransalacaoX = matrizTranslacao.TransformarMatriz(transformacaoTransalacaoX);

            matrizTranslacao = new Transformacao4D();
            matrizTranslacao.AtribuirTranslacao(1, y, 1);
            transformacaoTransalacaoY = matrizTranslacao.TransformarMatriz(transformacaoTransalacaoY);

            matrizTranslacao = new Transformacao4D();
            matrizTranslacao.AtribuirTranslacao(1, 1, z);
            transformacaoTransalacaoZ = matrizTranslacao.TransformarMatriz(transformacaoTransalacaoZ);

            Transformacao = transformacaoTransalacaoX.TransformarMatriz(Transformacao);
            Transformacao = transformacaoTransalacaoY.TransformarMatriz(Transformacao);
            Transformacao = transformacaoTransalacaoZ.TransformarMatriz(Transformacao);
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