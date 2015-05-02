using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;

namespace Exercicio01.EngineGrafica
{
    public class ObjetoGrafico : NoArvoreObjetosGraficos
    {
        public Transformacao4D Transformacao { get; private set; }

        private readonly IList<Ponto4D> vertices = new List<Ponto4D>();
        public IEnumerable<Ponto4D> Vertices { get { return vertices; } }
        public BBox BoundaryBox { get; private set; }

        public Color Cor { get; set; }
        public PrimitiveType Primitiva { get; set; }
        public float TamanhoPonto { get; set; }
        public float LarguraLinha { get; set; }

        /// <summary>
        /// Cria um objeto gráfico.
        /// </summary>
        /// <param name="verticeInicial">Primeiro vértice. É necessário informar um vértice para garantir a consistência da Boudary Box</param>
        public ObjetoGrafico(Ponto4D verticeInicial)
        {
            TamanhoPonto = 1;
            LarguraLinha = 1;
            Primitiva = PrimitiveType.LineStrip;
            vertices = new List<Ponto4D>();
            Transformacao = new Transformacao4D();

            AdicionarVertice(verticeInicial);
        }

        public void Desenhar()
        {
            GL.Color3(Cor);
            GL.LineWidth(LarguraLinha);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.MultMatrix(Transformacao.Data);
                GL.Begin(Primitiva);
                {
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        GL.Vertex3(vertices[i].X, vertices[i].Y, vertices[i].Z);
                    }
                }
                GL.End();
            }
            GL.PopMatrix();
        }

        public void AdicionarVertice(Ponto4D vertice)
        {
            vertices.Add(vertice);
            if (BoundaryBox == null)
            {
                BoundaryBox = BBox.Calcular(this);
            }
            else
            {
                BoundaryBox.AtualizarCom(vertice);
            }
        }

        public void RemoverVertice(Ponto4D vertice)
        {
            vertices.Remove(vertice);
        }

        public void Mover(double x, double y, double z)
        {
            var matrizTranslacao = new Transformacao4D();
            matrizTranslacao.AtribuirTranslacao(x, y, z);

            Transformacao = matrizTranslacao.TransformarMatriz(Transformacao);
        }

        public void Redimensionar(double escalaX, double escalaY)
        {
            var matrizEscala = new Transformacao4D();
            matrizEscala.AtribuirEscala(escalaX, escalaY, 1.0);

            Transformacao = matrizEscala.TransformarMatriz(Transformacao);
        }
    }
}