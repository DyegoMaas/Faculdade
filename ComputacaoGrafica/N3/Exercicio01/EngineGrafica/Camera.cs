using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Exercicio01.EngineGrafica
{
    public class Camera
    {
        private float xMinOrtho2D;
        private float xMaxOrtho2D;
        private float yMinOrtho2D;
        private float yMaxOrtho2D;

        public Camera(float xMinOrtho2D, float xMaxOrtho2D, float yMinOrtho2D, float yMaxOrtho2D)
        {
            this.xMinOrtho2D = xMinOrtho2D;
            this.xMaxOrtho2D = xMaxOrtho2D;
            this.yMinOrtho2D = yMinOrtho2D;
            this.yMaxOrtho2D = yMaxOrtho2D;
        }

        public void Pan(Vector2 deslocamento)
        {
            Pan(deslocamento.X, deslocamento.Y);
        }

        public void Pan(float deslocamentoX, float deslocamentoY)
        {
            xMinOrtho2D += deslocamentoX;
            xMaxOrtho2D += deslocamentoX;
            yMinOrtho2D += deslocamentoY;
            yMaxOrtho2D += deslocamentoY;

            CarregarMatrizOrtografica();
        }

        public void ZoomPorFator(float fator)
        {
            xMinOrtho2D *= fator;
            xMaxOrtho2D *= fator;
            yMinOrtho2D *= fator;
            yMaxOrtho2D *= fator;

            CarregarMatrizOrtografica();
        }

        public void Zoom(int zoom)
        {
            xMinOrtho2D += zoom;
            xMaxOrtho2D -= zoom;
            yMinOrtho2D += zoom;
            yMaxOrtho2D -= zoom;

            CarregarMatrizOrtografica();
        }

        public void CarregarMatrizOrtografica()
        {
            var matriz = Matrix4.CreateOrthographicOffCenter(xMinOrtho2D, xMaxOrtho2D, yMinOrtho2D, yMaxOrtho2D, 0, 1);
            GL.LoadMatrix(ref matriz);
        }
    }
}