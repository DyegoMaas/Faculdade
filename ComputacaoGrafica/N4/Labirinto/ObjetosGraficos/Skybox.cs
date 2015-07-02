using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace JogoLabirinto.ObjetosGraficos
{
    //https://sidvind.com/wiki/Skybox_tutorial
    public class Skybox : ObjetoGrafico
    {
        protected override void DesenharObjeto()
        {
            GL.Translate(-500, -1400, -500);
            GL.PushMatrix();

            GL.Scale(3000,3000,3000);

            GL.PushAttrib(AttribMask.EnableBit);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);

            GL.Color4(Color4.White);

            var texturas = ContentManager.TexturasSkybox;

            // Front
            GL.BindTexture(TextureTarget.Texture2D, texturas[0]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(0.0, 0.0, -1.0);

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
            GL.BindTexture(TextureTarget.Texture2D, texturas[1]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(1, 0, 0);

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
            GL.BindTexture(TextureTarget.Texture2D, texturas[2]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(0, 0, 1);

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
            GL.BindTexture(TextureTarget.Texture2D, texturas[3]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(-1, 0, 0);

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
            GL.BindTexture(TextureTarget.Texture2D, texturas[4]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(0, -1, 0);

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

            // Botton
            GL.BindTexture(TextureTarget.Texture2D, texturas[5]);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(0, 1, 0);

                GL.TexCoord2(1, 1);
                GL.Vertex3(-0.5f, -0.5f, -0.5f);

                GL.TexCoord2(1, 0);
                GL.Vertex3(-0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 0);
                GL.Vertex3(0.5f, -0.5f, 0.5f);

                GL.TexCoord2(0, 1);
                GL.Vertex3(0.5f, -0.5f, -0.5f);
            }
            GL.End();

            // Restore enable bits and matrix
            GL.PopAttrib();
            GL.PopMatrix();
        }
    }
}