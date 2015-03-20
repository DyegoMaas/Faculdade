using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace N2_Exercicio04
{
    class Program
    {
        private static float Raio = 100f;
        private static float Angulo = 45f;
        private static float xref = 0f;
        private static float yref = 0f;
        private static RetaInclinada retaInclinada = new RetaInclinada();

        private class RetaInclinada
        {
            public PointF PontoCentral = new PointF();
            public PointF PontoDistante = new PointF();

            public RetaInclinada()
            {
                var theta = 2 * (float)Math.PI * Angulo / 360;
                var x = xref + Raio * (float)Math.Cos(theta);
                var y = yref + Raio * (float)Math.Sin(theta);
                PontoDistante = new PointF(x, y);
                PontoCentral = new PointF(xref, yref);
            }
        }

        static void Main(string[] args)
        {
            using (var gameWindow = new GameWindow(400, 400))
            {
                gameWindow.Load += (sender, e) =>
                {
                    GL.ClearColor(Color.LightGray);
                };

                gameWindow.UpdateFrame += (sender, e) =>
                {
                    var state = Keyboard.GetState();

                    if (state.IsKeyDown(Key.Q))
                    {
                        xref--;
                    }
                    if (state.IsKeyDown(Key.W))
                    {
                        xref++;
                    }
                    if (state.IsKeyDown(Key.A))
                    {
                        Raio--;
                    }
                    if (state.IsKeyDown(Key.S))
                    {
                        Raio++;
                    }
                    if (state.IsKeyDown(Key.Z))
                    {
                        Angulo--;
                    }
                    if (state.IsKeyDown(Key.X))
                    {
                        Angulo++;
                    }

                    retaInclinada = new RetaInclinada();
                };

                gameWindow.RenderFrame += (sender, e) =>
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();

                    var matriz = Matrix4.CreateOrthographicOffCenter(-400, 400, -400, 400, 0, 1);
                    GL.LoadMatrix(ref matriz);

                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    MeuDesenho();

                    gameWindow.SwapBuffers();
                };

                gameWindow.Run();
            }
        }

        private static void MeuDesenho()
        {

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
           

            GL.PointSize(3);
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex2(retaInclinada.PontoCentral.X, retaInclinada.PontoCentral.Y);
                GL.Vertex2(retaInclinada.PontoDistante.X, retaInclinada.PontoDistante.Y);
            }
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(retaInclinada.PontoCentral.X, retaInclinada.PontoCentral.Y);
                GL.Vertex2(retaInclinada.PontoDistante.X, retaInclinada.PontoDistante.Y);
            }
            GL.End();
        }
    }
}
