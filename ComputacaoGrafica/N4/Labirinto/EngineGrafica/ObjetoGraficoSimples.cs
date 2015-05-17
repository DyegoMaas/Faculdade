using OpenTK.Graphics.OpenGL;

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

        public abstract void Desenhar();
    }
}