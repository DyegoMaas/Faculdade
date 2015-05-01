using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Exercicio01.EngineGrafica
{
    public class Camera
    {
        private readonly float xMinOrtho2DOriginal;
        private readonly float xMaxOrtho2DOriginal;
        private readonly float yMinOrtho2DOriginal;
        private readonly float yMaxOrtho2DOriginal;

        private float xMinOrtho2D;
        private float xMaxOrtho2D;
        private float yMinOrtho2D;
        private float yMaxOrtho2D;

        private float fatorZoom = 1f;

        public float FatorZoom
        {
            get { return fatorZoom; }
            set 
            {
                fatorZoom = value;
                AplicarZoom(fatorZoom);
            }
        }

        public Camera(float xMinOrtho2D, float xMaxOrtho2D, float yMinOrtho2D, float yMaxOrtho2D)
        {
            xMinOrtho2DOriginal = this.xMinOrtho2D = xMinOrtho2D;
            xMaxOrtho2DOriginal = this.xMaxOrtho2D = xMaxOrtho2D;
            yMinOrtho2DOriginal = this.yMinOrtho2D = yMinOrtho2D;
            yMaxOrtho2DOriginal = this.yMaxOrtho2D = yMaxOrtho2D;
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

        private void AplicarZoom(float fator)
        {
            xMinOrtho2D = xMinOrtho2DOriginal * fator;
            xMaxOrtho2D = xMaxOrtho2DOriginal * fator;
            yMinOrtho2D = yMinOrtho2DOriginal * fator;
            yMaxOrtho2D = yMaxOrtho2DOriginal * fator;

            CarregarMatrizOrtografica();
        }

        public void CarregarMatrizOrtografica()
        {
            var matriz = Matrix4.CreateOrthographicOffCenter(xMinOrtho2D, xMaxOrtho2D, yMinOrtho2D, yMaxOrtho2D, 0, 1);
            GL.LoadMatrix(ref matriz);
        }
    }
}