using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Labirinto.EngineGrafica
{
    public class Camera
    {
        private float fatorZoom = 1f;
        private float panX;
        private float panY;

        public float FatorZoom
        {
            get { return fatorZoom; }
            set 
            {
                fatorZoom = value;
                //CarregarMatrizOrtografica();
            }
        }

        public void Reshape(int largura, int altura)
        {
            GL.Viewport(0, 0, largura, altura);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-1d, 1d, -1d, 1d, 1.5d, 20d);
        }

        public void Pan(Vector2 deslocamento)
        {
            Pan(deslocamento.X, deslocamento.Y);
        }

        public void Pan(float deslocamentoX, float deslocamentoY)
        {
            panX += deslocamentoX;
            panY += deslocamentoY;

            //CarregarMatrizOrtografica();
        }
        
        //public void CarregarMatrizOrtografica()
        //{
        //    var fatorZoomAplicado = fatorZoom > 0 ? fatorZoom : 1;
        //    xMinOrtho2D = (xMinOrtho2DOriginal + panX) / fatorZoomAplicado;
        //    xMaxOrtho2D = (xMaxOrtho2DOriginal + panX) / fatorZoomAplicado;
        //    yMinOrtho2D = (yMinOrtho2DOriginal + panY) / fatorZoomAplicado;
        //    yMaxOrtho2D = (yMaxOrtho2DOriginal + panY) / fatorZoomAplicado;

        //    var matriz = Matrix4.CreateOrthographicOffCenter(xMinOrtho2D, xMaxOrtho2D, yMinOrtho2D, yMaxOrtho2D, 0, 1);
        //    GL.LoadMatrix(ref matriz);
        //}
    }
}