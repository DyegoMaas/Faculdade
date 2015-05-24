using OpenTK;
using OpenTK.Graphics.OpenGL;
using Tao.OpenGl;

namespace Labirinto.EngineGrafica
{
    public class Camera
    {
        private float fatorZoom = 1f;

        public float FatorZoom
        {
            get { return fatorZoom; }
            set 
            {
                fatorZoom = value;
            }
        }

        public void Reshape(int largura, int altura)
        {
            GL.Viewport(0, 0, largura, altura);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Gl.glViewport(0, 0, largura, altura);
            var perspectiva = Matrix4.CreatePerspectiveFieldOfView(1.04f, largura / (float)altura, 1f, 10000f);
            GL.LoadMatrix(ref perspectiva);

            GL.MatrixMode(MatrixMode.Modelview);
        }
    }
}