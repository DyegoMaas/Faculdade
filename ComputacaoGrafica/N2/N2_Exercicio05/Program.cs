using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using OpenTK.Input;

namespace N2_Exercicio04
{
    public class Program
    {
        private const float Ortho2DMinX = -400f;
        private const float Ortho2DMaxX = 400f;
        private const float Ortho2DMinY = -400f;
        private const float Ortho2DMaxY = 400f;

        private readonly Point posicaoInicialJanela = new Point(50, 50);
        private readonly Size tamanhoInicialJanela = new Size(400, 400);
        private readonly Primitivas primitivas = new Primitivas();
        
        /// <summary>
        /// Para evitar 100% de uso da CPU http://www.opentk.com/doc/intro/cpu-usage
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            var program = new Program();
            program.Executar();
        }

        private void Executar()
        {
            var primitivaSelecionada = primitivas.Proxima();

            // Creates a 3.0-compatible GraphicsContext with 32bpp color, 24bpp depth
            // 8bpp stencil
            using (var gameWindow = new GameWindow(tamanhoInicialJanela.Width, tamanhoInicialJanela.Height, new GraphicsMode(32, 24, 8, 0)))
            {
                gameWindow.Location = posicaoInicialJanela;
                gameWindow.Title = "CG-Respostas-N2_exe04";

                gameWindow.Load += (sender, e) =>
                {
                    GL.ClearColor(Color.White);
                };

                gameWindow.Resize += (sender, e) =>
                {

                };

                gameWindow.UpdateFrame += (sender, e) =>
                {
                };

                gameWindow.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Space)
                    {
                        primitivaSelecionada = primitivas.Proxima();
                    }
                };

                gameWindow.RenderFrame += (sender, e) =>
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();

                    var matriz = Matrix4.CreateOrthographicOffCenter(Ortho2DMinX, Ortho2DMaxX, Ortho2DMinY, Ortho2DMaxY, 0, 1);
                    GL.LoadMatrix(ref matriz);

                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    SRU();
                    DesenharCoisas(primitivaSelecionada);

                    gameWindow.SwapBuffers();
                };

                gameWindow.Run(120, 60);
            }
        }

        private void DesenharCoisas(PrimitiveType primitiva)
        {
            GL.PointSize(4f);
            GL.LineWidth(4f);

            GL.Begin(primitiva);
            {
                GL.Color3(Color.Red);
                GL.Vertex2(200, -200);

                GL.Color3(Color.FromArgb(0, 255, 0));
                GL.Vertex2(200, 200);

                GL.Color3(Color.Blue);
                GL.Vertex2(-200, 200);

                GL.Color3(Color.DeepPink);
                GL.Vertex2(-200, -200);
            }
            GL.End();
        }

        private void SRU()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.Disable(EnableCap.Lighting);
            
            GL.LineWidth(1f);

            // eixo x
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(-200, 0);
                GL.Vertex2(200, 0);
            }
            GL.End();

            // eixo y
            GL.Color3(Color.FromArgb(0, 255, 0));
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(0, -200);
                GL.Vertex2(0, 200);
            }
            GL.End();
        }

        private class Primitivas
        {
            private readonly PrimitiveType[] primitivas =
            {
                PrimitiveType.Points,
                PrimitiveType.Lines,
                PrimitiveType.LineLoop,
                PrimitiveType.LineStrip,
                PrimitiveType.Triangles,
                PrimitiveType.TriangleFan,
                PrimitiveType.TriangleStrip,
                PrimitiveType.Quads,
                PrimitiveType.QuadStrip,
                PrimitiveType.Polygon
            };

            private int indice = 0;

            public PrimitiveType Proxima()
            {
                var tipo = primitivas[indice];
                indice = (indice + 1) % primitivas.Length;

                return tipo;
            }
        }
    }
}
