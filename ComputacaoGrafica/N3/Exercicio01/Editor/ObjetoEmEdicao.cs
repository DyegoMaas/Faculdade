using System.Linq;
using Exercicio01.EngineGrafica;

namespace Exercicio01.Editor
{
    public sealed class ObjetoEmEdicao
    {
        public readonly ObjetoGrafico ObjetoGrafico;
        private readonly InputManager input;

        public ObjetoEmEdicao(ObjetoGrafico objetoGrafico, InputManager input)
        {
            ObjetoGrafico = objetoGrafico;
            this.input = input;
        }
        
        public void Mover(double x, double y, double z)
        {
            ObjetoGrafico.Mover(x, y, z);
        }

        public void Redimensionar(double escalaX, double escalaY)
        {
            var xTemp = ObjetoGrafico.BoundaryBox.MinX;
            var yTemp = ObjetoGrafico.BoundaryBox.MinY;
            Mover(-xTemp, -yTemp, 0);
            ObjetoGrafico.Redimensionar(escalaX, escalaY);
            Mover(xTemp, yTemp, 0);
        }

        public void AdicionarVertice()
        {
            var posicaoMouse = input.ObterPosicaoMouseNaTela();
            ObjetoGrafico.AdicionarVertice(new Ponto4D(posicaoMouse.X, posicaoMouse.Y));
        }

        public void RemoverVertice()
        {
            var vertice = ObjetoGrafico.Vertices.LastOrDefault();
            if(vertice != null)
            {
                ObjetoGrafico.RemoverVertice(vertice);
            }
        }
    }
}