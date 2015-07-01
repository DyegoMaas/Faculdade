using JogoLabirinto.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace JogoLabirinto.ObjetosGraficos
{
    public abstract class ObjetoGrafico : IObjetoGrafico
    {
        private readonly List<Action> acoes = new List<Action>();
        public readonly IList<Face> Faces = new List<Face>();
        protected bool Wireframe { get; set; }

        public Matrix4d MatrizTransformacao;

        protected ObjetoGrafico()
        {
            MatrizTransformacao = Matrix4d.Identity;
        }

        protected void AplicarTransformacao(Matrix4d novaMatrizTransformacao)
        {
            MatrizTransformacao = novaMatrizTransformacao;
        }

        public void Desenhar()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.MultMatrix(ref MatrizTransformacao);

                acoes.ForEach(acao => acao.Invoke());

                foreach (var face in Faces)
                {
                    GraphicUtils.DesenharFace(face);
                }

                DesenharObjeto();

                if (Wireframe)
                {
                    GL.Color3(Color.Black);
                    foreach (var face in Faces)
                    {
                        GraphicUtils.DesenharWireframe(face);
                    }
                }
            }
            GL.PopMatrix();
        }

        protected abstract void DesenharObjeto();

        protected void AntesDeDesenhar(Action operacaoAntesDesenhar)
        {
            acoes.Add(operacaoAntesDesenhar);
        }
    }
}