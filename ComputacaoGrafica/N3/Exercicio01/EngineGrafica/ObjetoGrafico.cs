using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Exercicio01.EngineGrafica
{
    public class ObjetoGrafico : NoArvoreObjetosGraficos
    {
        public Transformacao4D Transformacao { get; private set; }

        private static readonly Transformacao4D MatrizTmpTranslacao = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpTranslacaoInversa = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpEscala = new Transformacao4D();
        private static readonly Transformacao4D MatrizTmpRotacaoZ = new Transformacao4D();
        private static Transformacao4D matrizGlobal = new Transformacao4D();

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
        public ObjetoGrafico(Ponto4D verticeInicial, params Ponto4D[] vertices)
        {
            TamanhoPonto = 1;
            LarguraLinha = 1;
            Primitiva = PrimitiveType.LineStrip;
            this.vertices = new List<Ponto4D>();
            Transformacao = new Transformacao4D();

            AdicionarVertice(verticeInicial);
            for (int i = 0; i < vertices.Length; i++)
                AdicionarVertice(vertices[i]);
        }

        public void Desenhar()
        {
            GL.Color3(Cor);
            GL.PointSize(TamanhoPonto);
            GL.LineWidth(LarguraLinha);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.MultMatrix(Transformacao.Data);
                GL.Begin(Primitiva);
                {
                    foreach (var vertice in vertices)
                    {
                        GL.Vertex3(vertice.X, vertice.Y, vertice.Z);
                    }
                }
                GL.End();

                foreach (var objetoGrafico in Filhos)
                {
                    objetoGrafico.Desenhar();
                }
            }
            GL.PopMatrix();
        }

        public void DesenharBBox()
        {
            GL.Color3(Color.Bisque);
            GL.Begin(PrimitiveType.LineLoop);
            {
                GL.Vertex2(BoundaryBox.MinX, BoundaryBox.MinY);       
                GL.Vertex2(BoundaryBox.MinX, BoundaryBox.MaxY);       
                GL.Vertex2(BoundaryBox.MaxX, BoundaryBox.MaxY);       
                GL.Vertex2(BoundaryBox.MaxX, BoundaryBox.MinY);       
            }
            GL.End();
        }

        public void AdicionarVertice(Ponto4D vertice)
        {
            vertices.Add(vertice);
            if (BoundaryBox == null)
            {
                BoundaryBox = new BBox(vertice);
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

        /// <summary>
        /// Move o objeto
        /// </summary>
        /// <param name="x">Distância em que o objeto será movido no eixo X</param>
        /// <param name="y">Distância em que o objeto será movido no eixo Y</param>
        /// <param name="z">Distância em que o objeto será movido no eixo Z</param>
        public void Mover(double x, double y, double z)
        {
            var matrizTranslacao = new Transformacao4D();
            matrizTranslacao.AtribuirTranslacao(x, y, z);

            Transformacao = matrizTranslacao.TransformarMatriz(Transformacao);
        }

        /// <summary>
        /// Redimensiona o objeto em relação ao pivô
        /// </summary>
        /// <param name="escala">A escala pela qual o objeto será redimensionado</param>
        /// <param name="pivo">Ponto ao redor do qual o objeto será redimensionado</param>
        public void Redimensionar(double escala, Ponto4D pivo)
        {
            matrizGlobal.AtribuirIdentidade();

            ExecutarEmRelacaoAoPivo(pivo, () =>
            {
                MatrizTmpEscala.AtribuirEscala(escala, escala, 1.0);
                matrizGlobal = MatrizTmpEscala.TransformarMatriz(matrizGlobal);
            });

            Transformacao = matrizGlobal.TransformarMatriz(Transformacao);
        }

        /// <summary>
        /// Rotaciona o objeto em relação ao pivô
        /// </summary>
        /// <param name="angulo">Ângulo em graus</param>
        /// <param name="pivo">Ponto ao redor do qual o objeto será rotacionado</param>
        public void RotacionarNoEixoZ(double angulo, Ponto4D pivo)
        {
            matrizGlobal.AtribuirIdentidade();

            ExecutarEmRelacaoAoPivo(pivo, () =>
            {
                MatrizTmpRotacaoZ.AtribuirRotacaoZ(angulo * Transformacao4D.DegToRad);
                matrizGlobal = MatrizTmpRotacaoZ.TransformarMatriz(matrizGlobal);
            });

            Transformacao = matrizGlobal.TransformarMatriz(Transformacao);
        }

        private void ExecutarEmRelacaoAoPivo(Ponto4D pivo, Action acao)
        {
            MatrizTmpTranslacao.AtribuirTranslacao(pivo.X, pivo.Y, pivo.Z);
            matrizGlobal = MatrizTmpTranslacao.TransformarMatriz(matrizGlobal);

            acao.Invoke();

            pivo.InverterSinal();
            MatrizTmpTranslacaoInversa.AtribuirTranslacao(pivo.X, pivo.Y, pivo.Z);
            matrizGlobal = MatrizTmpTranslacaoInversa.TransformarMatriz(matrizGlobal);
        }

        public Ponto4D ProcurarVertice(double x, double y)
        {
            //TODO tem que transformar o ponto para a Transacao para que funcione depois de o objeto ter sido movido, redimensionado e rotacionado
            //TODO usar Transformacao.TransformarPonto(ponto)
            foreach(var vertice in vertices)
            {
                if (Math.Abs(vertice.X - x) < 15 && Math.Abs(vertice.Y - y) < 15)
                {
                    return vertice;
                }
            }

            return null;
        }

        internal void RemoverVerticeSelecionado(Ponto4D vertice)
        {
            vertices.Remove(vertice);
        }
    }
}