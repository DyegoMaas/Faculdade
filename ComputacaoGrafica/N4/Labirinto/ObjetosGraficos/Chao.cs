using JogoLabirinto.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace JogoLabirinto.ObjetosGraficos
{
    public class Chao : ComponenteTabuleiro
    {
        public Chao(Vector3d posicao) : base(posicao)
        {
            Wireframe = true;
            GraphicUtils.TransformarEmCubo(this);
            AntesDeDesenhar(() => GL.Color3(Color.SaddleBrown));
        }

        protected override void DesenharObjeto()
        {
        }

        public override void Atualizar()
        {
            
        }
    }
}