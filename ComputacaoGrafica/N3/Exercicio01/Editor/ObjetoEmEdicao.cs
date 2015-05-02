using Exercicio01.EngineGrafica;
using System.Linq;

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

        public void RedimensionarEmRelacaoAoCentroDoObjeto(double escala)
        {
            var ponto4D = ObjetoGrafico.BoundaryBox.Centro.InverterSinal();
            ObjetoGrafico.Redimensionar(escala, ponto4D);
        }

        public void RotacionarEmRelacaoAoCentroDoObjeto(double angulo)
        {
            var ponto4D = ObjetoGrafico.BoundaryBox.Centro.InverterSinal();
            ObjetoGrafico.RotacionarNoEixoZ(angulo, ponto4D);
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