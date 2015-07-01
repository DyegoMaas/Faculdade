using OpenTK;
using OpenTK.Graphics.OpenGL;
using Tao.OpenGl;

namespace JogoLabirinto
{
    public class Camera
    {
        private double fatorZoom = 1f;

        public double FatorZoom
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

            var perspectiva = Matrix4.CreatePerspectiveFieldOfView(1.04f, largura / (float)altura, 1f, 10000f);
            GL.LoadMatrix(ref perspectiva);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        public void OlharPara(double eyeX, double eyeY, double eyeZ, double targetX, double targetY, double targetZ)
        {
            Glu.gluLookAt(
                eyeX, eyeY, eyeZ,
                targetX, targetY, targetZ,
                0d, 1d, 0d);
        }
    }
}