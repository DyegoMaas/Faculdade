using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace N2_Exercicio04
{
    class Program
    {
        private static float raio = 100f;
        private static float angulo = 45f;
        private static float xRef = 0f;
        private static float yRef = 0f;
        private static PointF pontoCentral;
        private static PointF pontoDistante;

        private static void AtualizarLinha()
        {
            var theta = 2 * (float)Math.PI * angulo / 360;
            var x = xRef + raio * (float)Math.Cos(theta);
            var y = yRef + raio * (float)Math.Sin(theta);
            pontoDistante = new PointF(x, y);
            pontoCentral = new PointF(xRef, yRef);
        }
        
        static void Main(string[] args)
        {
            using (var gameWindow = new GameWindow(400, 400))
            {
                gameWindow.Load += (sender, e) =>
                {
                    GL.ClearColor(Color.LightGray);
                    AtualizarLinha();
                };

                gameWindow.UpdateFrame += (sender, e) =>
                {
                    var state = Keyboard.GetState();

                    if (state.IsKeyDown(Key.Q))
                    {
                        xRef--;
                    }
                    if (state.IsKeyDown(Key.W))
                    {
                        xRef++;
                    }
                    if (state.IsKeyDown(Key.A))
                    {
                        raio--;
                    }
                    if (state.IsKeyDown(Key.S))
                    {
                        raio++;
                    }
                    if (state.IsKeyDown(Key.Z))
                    {
                        angulo--;
                    }
                    if (state.IsKeyDown(Key.X))
                    {
                        angulo++;
                    }

                    AtualizarLinha();
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

                    SRU();
                    DesenharLinha();

                    gameWindow.SwapBuffers();
                };

                gameWindow.Run();
            }
        }

        private static void SRU()
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
        }

        private static void DesenharLinha()
        {
            GL.PointSize(3);
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex2(pontoCentral.X, pontoCentral.Y);
                GL.Vertex2(pontoDistante.X, pontoDistante.Y);
            }
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(pontoCentral.X, pontoCentral.Y);
                GL.Vertex2(pontoDistante.X, pontoDistante.Y);
            }
            GL.End();
        }
    }
}
