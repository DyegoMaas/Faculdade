using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;

namespace Exercicio01.EngineGrafica
{
    public class ObjetoGrafico : NoArvoreObjetosGraficos
    {
        public Transformacao4D Transformacao { get; private set; }

        public float TamanhoPonto { get; set; }
        public float LarguraLinha { get; set; }
        public Color Cor { get; set; }
        public PrimitiveType Primitiva { get; set; }
        public IList<Ponto4D> Vertices { get; private set; }

        public ObjetoGrafico()
        {
            TamanhoPonto = 1;
            LarguraLinha = 1;
            Primitiva = PrimitiveType.LineStrip;
            Vertices = new List<Ponto4D>();
            Transformacao = new Transformacao4D();
        }

        public void Desenhar()
        {
            GL.Color3(Cor);
            GL.LineWidth(LarguraLinha);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.MultMatrix(Transformacao.Data);
            GL.Begin(Primitiva);
            {
                for (int i = 0; i < Vertices.Count; i++)
                {
                    GL.Vertex3(Vertices[i].X, Vertices[i].Y, Vertices[i].Z);
                }    
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}