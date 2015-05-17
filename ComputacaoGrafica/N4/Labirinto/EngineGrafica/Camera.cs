using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Labirinto.EngineGrafica
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
        private float panX;
        private float panY;

        public float FatorZoom
        {
            get { return fatorZoom; }
            set 
            {
                fatorZoom = value;
                CarregarMatrizOrtografica();
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
            panX += deslocamentoX;
            panY += deslocamentoY;

            CarregarMatrizOrtografica();
        }
        
        public void CarregarMatrizOrtografica()
        {
            var fatorZoomAplicado = fatorZoom > 0 ? fatorZoom : 1;
            xMinOrtho2D = (xMinOrtho2DOriginal + panX) / fatorZoomAplicado;
            xMaxOrtho2D = (xMaxOrtho2DOriginal + panX) / fatorZoomAplicado;
            yMinOrtho2D = (yMinOrtho2DOriginal + panY) / fatorZoomAplicado;
            yMaxOrtho2D = (yMaxOrtho2DOriginal + panY) / fatorZoomAplicado;

            var matriz = Matrix4.CreateOrthographicOffCenter(xMinOrtho2D, xMaxOrtho2D, yMinOrtho2D, yMaxOrtho2D, 0, 1);
            GL.LoadMatrix(ref matriz);
        }
    }
}