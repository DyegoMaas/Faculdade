using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;

namespace N2_Exercicio06
{
    class Program
    {
        private const float Ortho2DMinX = -400f;
        private const float Ortho2DMaxX = 400f;
        private const float Ortho2DMinY = -400f;
        private const float Ortho2DMaxY = 400f;

        private readonly Point posicaoInicialJanela = new Point(50, 50);
        private readonly Size tamanhoInicialJanela = new Size(400, 400);

        private Vector2 p1 = new Vector2(-100, -100);
        private Vector2 p2 = new Vector2(-100, 100);
        private Vector2 p3 = new Vector2(100, 100);
        private Vector2 p4 = new Vector2(100, -100);
        private PontoControleEmEdicao pontoEmEdicao = PontoControleEmEdicao.Ponto1;

        private enum PontoControleEmEdicao
        {
            Ponto1 = 1,
            Ponto2 = 2,
            Ponto3 = 3,
            Ponto4 = 4
        }
        
        static void Main(string[] args)
        {
            var program = new Program();
            program.Executar();
        }

        private void Executar()
        {
            // Creates a 3.0-compatible GraphicsContext with 32bpp color, 24bpp depth
            // 8bpp stencil
            using (var gameWindow = new GameWindow(tamanhoInicialJanela.Width, tamanhoInicialJanela.Height, new GraphicsMode(32, 24, 8, 0)))
            {
                gameWindow.Location = posicaoInicialJanela;
                gameWindow.Title = "CG-Respostas-N2_exe06";

                gameWindow.Load += (sender, e) => GL.ClearColor(Color.White);
                
                gameWindow.KeyDown += (sender, e) =>
                {
                    switch (e.Key)
                    {
                        case Key.Number1:
                            pontoEmEdicao = PontoControleEmEdicao.Ponto1;
                            break;
                        case Key.Number2:
                            pontoEmEdicao = PontoControleEmEdicao.Ponto2;
                            break;
                        case Key.Number3:
                            pontoEmEdicao = PontoControleEmEdicao.Ponto3;
                            break;
                        case Key.Number4:
                            pontoEmEdicao = PontoControleEmEdicao.Ponto4;
                            break;
                    }
                };

                var antigo = new MouseState();
                var estadoInicialMouse = new MouseState();
                gameWindow.UpdateFrame += (sender, e) =>
                {
                    var atual = Mouse.GetState();
                    if (atual != antigo && antigo != estadoInicialMouse)
                    {
                        var deltaX = atual.X - antigo.X;
                        var deltaY = -(atual.Y - antigo.Y);

                        var delta = new Vector2(deltaX, deltaY);
                        switch (pontoEmEdicao)
                        {
                            case PontoControleEmEdicao.Ponto1:
                                p1 += delta;
                                break;
                            case PontoControleEmEdicao.Ponto2:
                                p2 += delta;
                                break;
                            case PontoControleEmEdicao.Ponto3:
                                p3 += delta;
                                break;
                            case PontoControleEmEdicao.Ponto4:
                                p4 += delta;
                                break;
                        }
                    }

                    antigo = atual;
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
                    DesenharCoisas();

                    gameWindow.SwapBuffers();
                };

                gameWindow.Run(120, 60);
            }
        }

        private static void SRU()
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

        private void DesenharCoisas()
        {
            GL.Color3(Color.Red);
            GL.PointSize(5f);
            GL.Begin(PrimitiveType.Points);
            {
                var ponto = ObterPontoEmEdicao();
                GL.Vertex2(ponto);
            }
            GL.End();

            GL.LineWidth(3f);

            // poliedro de controle
            GL.Color3(Color.Cyan);
            GL.Begin(PrimitiveType.LineStrip);
            {
                GL.Vertex2(p1);
                GL.Vertex2(p2);
                GL.Vertex2(p3);
                GL.Vertex2(p4);
            }
            GL.End();
            
            // apline
            GL.Color3(Color.Yellow);
            const float segmentos = 100f;
            const float step = 1f / segmentos;
            GL.Begin(PrimitiveType.LineStrip);
            {
                for (var t = 0f; t <= 1f; t += step)
                {
                    var ponto = GerarPontoSpline(p1, p2, p3, p4, t);
                    GL.Vertex2(ponto);
               }
            }
            GL.End();
        }

        private Vector2 ObterPontoEmEdicao()
        {
            switch (pontoEmEdicao)
            {
                case PontoControleEmEdicao.Ponto2:
                    return p2;
                case PontoControleEmEdicao.Ponto3:
                    return p3;
                case PontoControleEmEdicao.Ponto4:
                    return p4;
                default:
                    return p1;
            }
        }

        private Vector2 GerarPontoSpline(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float t)
        {
            var p1p2 = new Vector2(
                p1.X + (p2.X - p1.X) * t,
                p1.Y + (p2.Y - p1.Y) * t
            );

            var p3p4 = new Vector2(
                p3.X + (p4.X - p3.X) * t,
                p3.Y + (p4.Y - p3.Y) * t
            );

            var p1p2p3p4 = new Vector2(
                p1p2.X + (p3p4.X - p1p2.X) * t,
                p1p2.Y + (p3p4.Y - p1p2.Y) * t
            );

            return p1p2p3p4;
        }

    }
}
