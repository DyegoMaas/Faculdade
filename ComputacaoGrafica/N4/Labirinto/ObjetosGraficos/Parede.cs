using OpenTK;
using OpenTK.Graphics;
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
            //GraphicUtils.TransformarEmCubo(this);
            AntesDeDesenhar(() => GL.Color3(Color.DimGray));
        }

        protected override void DesenharObjeto()
        {
            var texturas = ContentManager.TexturasCaixa;
            var diffuse = texturas[0];
            var normalMap = texturas[1];

            GL.PushAttrib(AttribMask.EnableBit);
            GL.Enable(EnableCap.Texture2D);
            //GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.CullFace);

            GL.Color4(Color4.White);

            // Front
            GL.BindTexture(TextureTarget.Texture2D, diffuse);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(0.0, 0.0, 1.0);

                GL.TexCoord2(0, 1);
                GL.Vertex3(0.5f, -0.5f, -0.5f);

                GL.TexCoord2(1, 1);
                GL.Vertex3(-0.5f, -0.5f, -0.5f);

                GL.TexCoord2(1, 0);
                GL.Vertex3(-0.5f, 0.5f, -0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(0.5f, 0.5f, -0.5f);
            }
            GL.End();

            // Left
            GL.BindTexture(TextureTarget.Texture2D, diffuse);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(-1, 0, 0);

                GL.TexCoord2(1, 1);
                GL.Vertex3(0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 1);
                GL.Vertex3(0.5f, -0.5f, -0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(0.5f, 0.5f, -0.5f);

                GL.TexCoord2(1, 0);
                GL.Vertex3(0.5f, 0.5f, 0.5f);
            }
            GL.End();

            // Back
            GL.BindTexture(TextureTarget.Texture2D, diffuse);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(0, 0, -1);

                GL.TexCoord2(1, 1);
                GL.Vertex3(-0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 1);
                GL.Vertex3(0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(0.5f, 0.5f, 0.5f);

                GL.TexCoord2(1, 0);
                GL.Vertex3(-0.5f, 0.5f, 0.5f);
            }
            GL.End();

            // Right
            GL.BindTexture(TextureTarget.Texture2D, diffuse);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(1, 0, 0);

                GL.TexCoord2(1, 1);
                GL.Vertex3(-0.5f, -0.5f, -0.5f);

                GL.TexCoord2(0, 1);
                GL.Vertex3(-0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(-0.5f, 0.5f, 0.5f);

                GL.TexCoord2(1, 0);
                GL.Vertex3(-0.5f, 0.5f, -0.5f);
            }
            GL.End();

            // Top
            GL.BindTexture(TextureTarget.Texture2D, diffuse);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(0, 1, 0);

                GL.TexCoord2(1, 0);
                GL.Vertex3(-0.5f, 0.5f, -0.5f);

                GL.TexCoord2(1, 1);
                GL.Vertex3(-0.5f, 0.5f, 0.5f);

                GL.TexCoord2(0, 1);
                GL.Vertex3(0.5f, 0.5f, 0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(0.5f, 0.5f, -0.5f);
            }
            GL.End();

            //// Botton
            //GL.BindTexture(TextureTarget.Texture2D, diffuse);
            //GL.Begin(PrimitiveType.Quads);
            //{
            //    GL.Normal3(0, 1, 0);

            //    GL.TexCoord2(1, 1);
            //    GL.Vertex3(-0.5f, -0.5f, -0.5f);

            //    GL.TexCoord2(1, 0);
            //    GL.Vertex3(-0.5f, -0.5f, 0.5f);

            //    GL.TexCoord2(0, 0);
            //    GL.Vertex3(0.5f, -0.5f, 0.5f);

            //    GL.TexCoord2(0, 1);
            //    GL.Vertex3(0.5f, -0.5f, -0.5f);
            //}
            //GL.End();

            // Restore enable bits and matrix
            GL.PopAttrib();
        }

        public override void Atualizar()
        {
        }
    }
}