using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace JogoLabirinto.ObjetosGraficos
{
    public class LinhasReferencia : ObjetoGrafico
    {
        private readonly int numeroLinhas;
        private readonly int distanciaEntreLinhas;

        public LinhasReferencia(int numeroLinhas, int distanciaEntreLinhas)
        {
            this.numeroLinhas = numeroLinhas;
            this.distanciaEntreLinhas = distanciaEntreLinhas;
        }

        protected override void DesenharObjeto()
        {
            GL.Color3(Color.Black);
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            {
                for (var i = -numeroLinhas / 2; i < numeroLinhas / 2; i += distanciaEntreLinhas)
                {
                    GL.Vertex3(-1000, -50, i);
                    GL.Vertex3(1000, -50, i);

                    GL.Vertex3(i, -50, -1000);
                    GL.Vertex3(i, -50, 1000);
                }
            }
            GL.End();
        }
    }
}