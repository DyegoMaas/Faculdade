using System.Drawing.Drawing2D;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using Exercicio01.Editor;

namespace Exercicio01.EngineGrafica
{
    public class ObjetoGrafico : NoGrafoCena
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
        public string Nome { get; set; }

        /// <summary>
        /// Cria um objeto gráfico.
        /// </summary>
        /// <param name="verticeInicial">Primeiro vértice. É necessário informar um vértice para garantir a consistência da Boudary Box</param>
        public ObjetoGrafico(NoGrafoCena pai, string nome, Ponto4D verticeInicial, params Ponto4D[] vertices)
        {
            Nome = nome;
            TamanhoPonto = 1;
            LarguraLinha = 1;
            Primitiva = PrimitiveType.LineStrip;
            this.vertices = new List<Ponto4D>();
            Transformacao = new Transformacao4D();
            
            if(pai != null)
                pai.AdicionarObjetoGrafico(this);

            AdicionarVertice(verticeInicial);
            for (int i = 0; i < vertices.Length; i++)
            {
                AdicionarVertice(vertices[i]);
            }
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

                foreach (var objetoGrafico in ObjetosGraficos)
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

            var verticeAjustado = TransformarPontoParaEsteObjetoGrafico(vertice);
            if (BoundaryBox == null)
            {
                BoundaryBox = new BBox(verticeAjustado);
            }
            else
            {
                BoundaryBox.AtualizarCom(verticeAjustado);
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
            RecalcularBBox();
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
            RecalcularBBox();
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
            RecalcularBBox();
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

        // O problema aqui é que a escala e a rotação são feitas em relação ao centro da bbox, e não conseguimos aplicar 
        // transformação equivalente no ponto porque não temos como saber a ordem em que as operações foram feitas
        private const double Tolerancia = 20;//4;
        public VerticeSelecionado ProcurarVertice(Ponto4D ponto)
        {
            //ponto = Transformacao.TransformarPonto(ponto);
            ponto = Transformacao.TransformarPonto(ponto);
            if (BoundaryBox.Contem(ponto, 10d))
            {
                foreach (var vertice in vertices)
                {
                    //// O processo seria mais ou menos esse...
                    //var d = Transformacao.Data;
                    //var matrix4 = new Matrix4(
                    //    (float)d[0], (float)d[1], (float)d[2], (float)d[3], 
                    //    (float)d[4], (float)d[5], (float)d[6], (float)d[7], 
                    //    (float)d[8], (float)d[9], (float)d[10], (float)d[11], 
                    //    (float)d[12], (float)d[13], (float)d[14], (float)d[15])
                    //    .Inverted();

                    //var transformacao4D = new Transformacao4D();
                    //transformacao4D.SetElement(0, matrix4.M11);
                    //transformacao4D.SetElement(1, matrix4.M12);
                    //transformacao4D.SetElement(2, matrix4.M13);
                    //transformacao4D.SetElement(3, matrix4.M14);
                    //transformacao4D.SetElement(4, matrix4.M21);
                    //transformacao4D.SetElement(5, matrix4.M22);
                    //transformacao4D.SetElement(6, matrix4.M23);
                    //transformacao4D.SetElement(7, matrix4.M24);
                    //transformacao4D.SetElement(8, matrix4.M31);
                    //transformacao4D.SetElement(9, matrix4.M32);
                    //transformacao4D.SetElement(10, matrix4.M33);
                    //transformacao4D.SetElement(11, matrix4.M34);
                    //transformacao4D.SetElement(12, matrix4.M41);
                    //transformacao4D.SetElement(13, matrix4.M42);
                    //transformacao4D.SetElement(14, matrix4.M43);
                    //transformacao4D.SetElement(15, matrix4.M44);
                    //var verticeAjustado = transformacao4D.TransformarPonto(vertice);

                    var verticeAjustado = TransformarPontoParaEsteObjetoGrafico(vertice);
                    if (verticeAjustado.EstaProximo(ponto, Tolerancia))
                    {
                        return new VerticeSelecionado(vertice, this);
                    }
                }
            }

            foreach (var objetosGrafico in ObjetosGraficos)
            {
                var verticeEncontrado = objetosGrafico.ProcurarVertice(ponto);
                if (verticeEncontrado != null)
                {
                    return verticeEncontrado;
                }
            }

            return null;
        }

        /// <summary>
        /// Tenta buscar objeto selecionado através da BBox e depois do ScanLina
        /// </summary>
        public ObjetoGrafico BuscarObjetoSelecionado(Ponto4D ponto)
        {
            ponto = Transformacao.TransformarPonto(ponto);
            if (BoundaryBox.Contem(ponto))
            {
                if (VerificarScanLine(ponto.X, ponto.Y))
                {
                    return this;
                }
            }

            return null;
        }


        /// <summary>
        /// Aplica as transformações da hierarquia do objeto ao ponto (a transformação, da raiz para o filho do filho do filho ate chegar ao objeto atual
        /// </summary>
        /// <param name="vertice">Ponto para transformar</param>
        /// <returns>Ponto transformado</returns>
        public Ponto4D TransformarPontoParaEsteObjetoGrafico(Ponto4D vertice)
        {
            var transformacoes = new Stack<Transformacao4D>();

            NoGrafoCena no = this;
            while (no != null)
            {
                var objeto = no as ObjetoGrafico;
                if (objeto != null)
                {
                    transformacoes.Push(objeto.Transformacao);
                }
                no = no.Pai;
            }

            while (transformacoes.Count > 0)
            {
                var transformacao = transformacoes.Pop();
                vertice = transformacao.TransformarPonto(vertice);
            }

            return vertice;
        }

        /// <summary>
        /// Recalcula a BBox aplicando as transformações da hierarquia do objeto no grafo de cena
        /// </summary>
        public void RecalcularBBox()
        {
            var verticesAjustados = Vertices.Select(TransformarPontoParaEsteObjetoGrafico);
            BoundaryBox.RecalcularPara(verticesAjustados);
        }

        public void ExcluirVertice(Ponto4D vertice)
        {
            vertices.Remove(vertice);
            RecalcularBBox();
        }

        /// <summary>
        /// Verificação ScanLine
        /// </summary>
        /// <param name="x">Cordenada x a ser testada</param>
        /// <param name="y">Cordenada y</param>
        public bool VerificarScanLine(double x, double y)
        {
            int cont = 0;

            for (int i = 0; i < Vertices.Count(); i++)
            {
                Ponto4D verticeA = vertices[i];
                Ponto4D verticeB = vertices[(i + 1) % vertices.Count()];

                double fi = (y - verticeA.Y) / (verticeB.Y - verticeA.Y);
                double xi = verticeA.X + (verticeB.X - verticeA.X) * fi;

                if (fi >= 0 && fi <= 1 && xi >= x)
                {
                    cont++;
                }
            }

            return cont % 2 == 1;
        }
    }
}