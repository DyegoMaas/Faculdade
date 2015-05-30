using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Tao.FreeGlut;

namespace JogoLabirinto
{
    public class CuboSolido
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
}