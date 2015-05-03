using System.Drawing;
using Exercicio01.EngineGrafica;
using System.Linq;

namespace Exercicio01.Editor
{
    public sealed class ObjetoEmEdicao
    {
        public static InputManager Input { get; set; }
        public readonly ObjetoGrafico ObjetoGrafico;

        public ObjetoEmEdicao(NoGrafoCena pai, string nome)
        {
            var verticeInicial = CriarVerticeNaPosicaoAtualDoMouse();
            ObjetoGrafico = new ObjetoGrafico(pai, nome, verticeInicial);
        }

        public ObjetoEmEdicao(NoGrafoCena pai, string nome, Ponto4D verticeInicial, params Ponto4D[] vertices)
        {
            ObjetoGrafico = new ObjetoGrafico(pai, nome, verticeInicial, vertices);
        }

        public void DefinirNome(string nome)
        {
            ObjetoGrafico.Nome = nome;
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

        public void AdicionarVerticeNaPosicaoAtual()
        {
            var vertice = CriarVerticeNaPosicaoAtualDoMouse();
            ObjetoGrafico.AdicionarVertice(vertice);
        }

        public void AdicionarVertice(Ponto4D vertice)
        {
            ObjetoGrafico.AdicionarVertice(vertice);
        }

        private Ponto4D CriarVerticeNaPosicaoAtualDoMouse()
        {
            return Input.ObterPosicaoMouseNaTela();
        }

        public void RemoverVertice()
        {
            var vertice = ObjetoGrafico.Vertices.LastOrDefault();
            if(vertice != null)
            {
                ObjetoGrafico.RemoverVertice(vertice);
            }
        }

        public void DefinirCor(Color cor)
        {
            ObjetoGrafico.Cor = cor;
        }
    }
}