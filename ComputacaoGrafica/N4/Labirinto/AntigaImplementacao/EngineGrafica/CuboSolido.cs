using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Tao.FreeGlut;

namespace JogoLabirinto.AntigaImplementacao.EngineGrafica
{
    public class CuboSolido : ObjetoGrafico
    {
        private Color cor;

        public CuboSolido(Color cor)
        {
            this.cor = cor;
        }

        protected override void Desenhar()
        {
            GL.Color3(cor);
            Glut.glutSolidCube(1f);
        }
    }
}