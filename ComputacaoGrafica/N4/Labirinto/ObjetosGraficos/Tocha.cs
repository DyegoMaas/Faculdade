using JogoLabirinto.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace JogoLabirinto.ObjetosGraficos
{
    internal class Tocha : ComponenteTabuleiro
    {
        public int IndiceTocha { get; private set; }

        public Tocha(Vector3d posicao, int indiceTocha) : base(posicao)
        {
            IndiceTocha = indiceTocha;
            GraphicUtils.TransformarEmCubo(this, x:.2f, y: .3f, z:.2f);
            AntesDeDesenhar(() =>
            {
                GL.Color3(Color.Yellow);

                var luz = (LightName)((int) LightName.Light1 + IndiceTocha);
                var luzCap = (EnableCap)((int)EnableCap.Light1 + IndiceTocha);
                var lightPos1 = new[] { (float)posicao.X, (float)posicao.Y + 3, (float)posicao.Z, 0.0f };
                GL.Enable(luzCap);
                GL.Light(luz, LightParameter.Position, lightPos1);
                GL.Light(luz, LightParameter.Diffuse, Color.White);
                //GL.Light(luz, LightParameter.SpotExponent, .9f);
                GL.Light(luz, LightParameter.SpotCutoff, 30);
                GL.Light(luz, LightParameter.SpotDirection, new Vector4(0, -1, 0, 0));
                GL.Light(luz, LightParameter.Specular, new Vector4(3, 3, 3, 1));
                //GL.Light(luz, LightParameter.LinearAttenuation, 1);
                //GL.Light(luz, LightParameter.QuadraticAttenuation, 1);
                GL.Light(luz, LightParameter.ConstantAttenuation, 1);
            });
        }

        protected override void DesenharObjeto()
        {
        }

        public override void Atualizar()
        {
        }
    }
}