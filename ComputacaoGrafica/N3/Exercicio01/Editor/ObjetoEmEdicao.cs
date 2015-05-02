using System.Linq;
using Exercicio01.EngineGrafica;

namespace Exercicio01.Editor
{
    public sealed class ObjetoEmEdicao
    {
        public readonly ObjetoGrafico ObjetoGrafico;
        private readonly InputManager input;

        public ObjetoEmEdicao(InputManager input)
        {
            this.input = input;

            var verticeInicial = CriarVerticeNaPosicaoAtualDoMouse();
            ObjetoGrafico = new ObjetoGrafico(verticeInicial);
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
            var vertice = CriarVerticeNaPosicaoAtualDoMouse();
            ObjetoGrafico.AdicionarVertice(vertice);
        }

        private Ponto4D CriarVerticeNaPosicaoAtualDoMouse()
        {
            var posicaoMouse = input.ObterPosicaoMouseNaTela();
            var vertice = new Ponto4D(posicaoMouse.X, posicaoMouse.Y);
            return vertice;
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