using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Tao.FreeGlut;

namespace Labirinto.EngineGrafica
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