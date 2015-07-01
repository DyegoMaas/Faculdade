using JogoLabirinto.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace JogoLabirinto.ObjetosGraficos
{
    public class Cacapa : ComponenteTabuleiro
    {
        public Cacapa(Vector3d posicao)
            : base(posicao)
        {
            GraphicUtils.TransformarEmCubo(this, y: .25f);
            AntesDeDesenhar(() => GL.Color3(Color.Black));
        }

        protected override void DesenharObjeto()
        {
        }

        public override void Atualizar()
        {
        }
    }
}