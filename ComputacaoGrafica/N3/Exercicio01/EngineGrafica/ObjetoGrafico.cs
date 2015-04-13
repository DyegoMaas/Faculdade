using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Exercicio01.EngineGrafica
{
    public class ObjetoGrafico : NoArvoreObjetosGraficos
    {
        public Transformacao Transformacao { get; set; }

        public float LarguraLinha { get; set; }
        public Color Cor { get; set; }
        public PrimitiveType Primitiva { get; set; }
        public IList<Vector4d> Pontos { get; set; }

        public void Desenhar()
        {
            GL.Color3(Cor);
            GL.LineWidth(LarguraLinha);

            GL.Begin(Primitiva);
            {
                for (int i = 0; i < Pontos.Count; i++)
                {
                    GL.Vertex3(Pontos[i].X, Pontos[i].Y, Pontos[i].Z);
                }    
            }
            GL.End();
        }
    }
}