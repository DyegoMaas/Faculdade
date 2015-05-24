using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Tao.FreeGlut;

namespace JogoLabirinto.EngineGrafica
{
    internal class EsferaSolida : ObjetoGrafico
    {
        private Color cor;

        public EsferaSolida(Color cor)
        {
            this.cor = cor;
        }

        protected override void Desenhar()
        {
            GL.Color3(cor);
            Glut.glutSolidSphere(.5, 100, 10);
        }
    }
}