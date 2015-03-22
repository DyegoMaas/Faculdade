using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace N2_Exercicio07
{
    class Program
    {
        private readonly Point posicaoInicialJanela = new Point(50, 50);
        private readonly Size tamanhoInicialJanela = new Size(400, 400);
        private readonly Camera camera = new Camera();

        private Quadrado quadrado;
        private Circulo circuloMenor;
        private Circulo circuloMaior;
        
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

                gameWindow.Load += (sender, e) =>
                {
                    GL.ClearColor(Color.White);

                    quadrado = new Quadrado(new Vector2(100f), 200f);

                    var hipotenusa = (float)Math.Sqrt(Math.Pow(100, 2) + Math.Pow(100, 2));
                    circuloMaior = new Circulo(new Vector2(200f), hipotenusa, 120);
                    circuloMenor = new Circulo(new Vector2(200f), 50f, 120);
                };

                var estadoInicialMouse = new MouseState();
                var estadoAntigoMouse = new MouseState();
                gameWindow.UpdateFrame += (sender, e) =>
                {
                    camera.Update();

                    var estadoAtualMouse = Mouse.GetState();
                    if (estadoAtualMouse != estadoAntigoMouse && estadoAntigoMouse != estadoInicialMouse)
                    {
                        if (estadoAtualMouse.IsButtonDown(MouseButton.Left))
                        {
                            if (circuloMenor.EstahDentro(gameWindow.Mouse.X, gameWindow.Mouse.Y))
                            {
                                Console.Write("dentro");


                                
                            }
                        }
                    }

                    estadoAntigoMouse = estadoAtualMouse;
                };

                gameWindow.RenderFrame += (sender, e) =>
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    camera.CarregarMatrizOrtografica();
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

        private void SRU()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.Disable(EnableCap.Lighting);

            GL.LineWidth(1f);

            // eixo centroX
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex2(-200, 0);
                GL.Vertex2(200, 0);
            }
            GL.End();

            // eixo centroY
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
            DesenharQuadrado(quadrado);

            DesenharCirculoMaior(circuloMaior);
            DesenharCirculoMenor(circuloMenor);

            //TODO centro do círculo
            //var centroX = esquerdaInferiorQuadrado.X + largura / 2f;
            //var centroY = esquerdaInferiorQuadrado.Y + largura / 2f;
            //GL.Color3(Color.Black);
            //GL.PointSize(4f);
            //GL.Begin(PrimitiveType.Points);
            //{
            //    GL.Vertex2(centroX, centroY);
            //}
            //GL.End();

        }

        private void DesenharQuadrado(Quadrado quadrado)
        {
            GL.Color3(Color.LightPink);
            GL.Begin(PrimitiveType.LineLoop);
            {
                GL.Vertex2(quadrado.PontoInferiorEsquerdo);
                GL.Vertex2(quadrado.PontoInferiorDireito);
                GL.Vertex2(quadrado.PontoSuperiorDireito);
                GL.Vertex2(quadrado.PontoSuperiorEsquerdo);
            }
            GL.End();
        }

        private void DesenharCirculoMaior(Circulo circulo)
        {
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.LineLoop);
            {
                foreach (var ponto in circulo.Pontos)
                {
                    GL.Vertex2(ponto);
                }
            }
            GL.End();
        }

        private void DesenharCirculoMenor(Circulo circulo)
        {
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.LineLoop);
            {
                foreach (var ponto in circulo.Pontos)
                {
                    GL.Vertex2(ponto);
                }
            }
            GL.End();

            GL.PointSize(5f);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex2(circulo.Centro);
            }
            GL.End();
        }
    }

    public struct Quadrado
    {
        public Vector2 PontoInferiorEsquerdo;
        public Vector2 PontoSuperiorEsquerdo;
        public Vector2 PontoInferiorDireito;
        public Vector2 PontoSuperiorDireito;
        public float Largura;

        public Quadrado(Vector2 posicao, float largura)
            : this()
        {
            Largura = largura;
            PontoInferiorEsquerdo = posicao;
            PontoInferiorDireito = new Vector2(posicao.X + largura, posicao.Y);
            PontoSuperiorDireito = new Vector2(posicao.X + largura, posicao.Y + largura);
            PontoSuperiorEsquerdo = new Vector2(posicao.X, posicao.Y + largura);
        }
    }

    public struct QuadradoCentralizado
    {
        public Vector2 PontoInferiorEsquerdo;
        public Vector2 PontoSuperiorEsquerdo;
        public Vector2 PontoInferiorDireito;
        public Vector2 PontoSuperiorDireito;
        public float Largura;

        public QuadradoCentralizado(Vector2 centro, float largura)
        {
            var metadeLargura = largura / 2f;

            Largura = largura;
            PontoInferiorEsquerdo = new Vector2(centro.X - metadeLargura, centro.Y - metadeLargura);
            PontoInferiorDireito = new Vector2(centro.X + metadeLargura, centro.Y);
            PontoSuperiorDireito = new Vector2(centro.X + metadeLargura, centro.Y + metadeLargura);
            PontoSuperiorEsquerdo = new Vector2(centro.X - metadeLargura, centro.Y + metadeLargura);
        }
    }

    public class Circulo
    {
        public Vector2[] Pontos { get; private set; }
        public Vector2 Centro { get; private set; }
        public float Raio { get; private set; }

        public Circulo(Vector2 centro, float raio, int numeroPontos)
        {
            Raio = raio;
            Centro = centro;
            Pontos = new Vector2[numeroPontos];

            for (var i = 0; i < numeroPontos; i++)
            {
                var theta = 2 * (float)Math.PI * i / numeroPontos;
                var x = centro.X + raio * (float)Math.Cos(theta);
                var y = centro.Y + raio * (float)Math.Sin(theta);

                Pontos[i] = new Vector2(x, y);
            }
        }

        public bool EstahDentro(float x, float y)
        {
            var distancia = Distancia(x, y, Centro.X, Centro.Y);
            return distancia <= Raio;
        }

        private float Distancia(float x1, float y1, float x2, float y2)
        {
            return (float) Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
