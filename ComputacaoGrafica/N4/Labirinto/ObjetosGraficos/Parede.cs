using JogoLabirinto.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace JogoLabirinto.ObjetosGraficos
{
    public class Parede : ComponenteTabuleiro
    {
        public Parede(Vector3d posicao)
            : base(posicao)
        {
            Wireframe = true;
            GraphicUtils.TransformarEmCubo(this);
            AntesDeDesenhar(() => GL.Color3(Color.DimGray));
        }

        protected override void DesenharObjeto()
        {
        }

        public override void Atualizar()
        {
        }
    }
}