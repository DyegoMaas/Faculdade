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
        private const int NumeroPontosCirculo = 120;

        private bool circuloMenorEstahDentroQuadrado = true;
        
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
                    circuloMaior = new Circulo(new Vector2(200f), hipotenusa);
                    circuloMenor = new Circulo(new Vector2(200f), 50f);
                };
                
                var estadoInicialMouse = new MouseState();
                var estadoAntigoMouse = new MouseState();
                gameWindow.UpdateFrame += (sender, e) =>
                {
                    camera.Update();
                    var estadoAtualMouse = Mouse.GetState();
                    if (estadoAtualMouse != estadoInicialMouse && estadoAtualMouse != estadoInicialMouse)
                    {
                        var deltaX = (estadoAtualMouse.X - estadoInicialMouse.X) / 50f;
                        var deltaY = -(estadoAtualMouse.Y - estadoInicialMouse.Y) / 50f;
                        
                        var novoCentro = circuloMenor.Centro.Deslocar(deltaX, deltaY);

                        circuloMenorEstahDentroQuadrado = quadrado.Contem(novoCentro);
                        if (circuloMenorEstahDentroQuadrado)
                        {
                            circuloMenor = circuloMenor.Deslocar(deltaX, deltaY);
                        }
                        else
                        {
                            if (novoCentro.EstahDentro(circuloMaior))
                            {
                                circuloMenor = circuloMenor.Deslocar(deltaX, deltaY);
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
            var corQuadrado = circuloMenorEstahDentroQuadrado 
                ? Color.LightPink 
                : Color.Yellow;
            DesenharComCor(corQuadrado, () => DesenharQuadrado(quadrado));

            DesenharComCor(Color.Black, () =>
            {
                DesenharCirculo(circuloMaior);
                DesenharCirculoMenor(circuloMenor);    
            });
        }

        private void DesenharComCor(Color cor, Action acaoDesenho)
        {
            GL.Color3(cor);
            acaoDesenho.Invoke();
        }

        private void DesenharQuadrado(Quadrado quadrado)
        {
            GL.Begin(PrimitiveType.LineLoop);
            {
                GL.Vertex2(quadrado.PontoInferiorEsquerdo);
                GL.Vertex2(quadrado.PontoInferiorDireito);
                GL.Vertex2(quadrado.PontoSuperiorDireito);
                GL.Vertex2(quadrado.PontoSuperiorEsquerdo);
            }
            GL.End();
        }

        private void DesenharCirculoMenor(Circulo circulo)
        {
            DesenharCirculo(circulo);

            GL.PointSize(5f);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex2(circulo.Centro);
            }
            GL.End();
        }

        private void DesenharCirculo(Circulo circulo)
        {
            GL.Begin(PrimitiveType.LineLoop);
            {
                for (var ponto = 0; ponto < NumeroPontosCirculo; ponto++)
                {
                    GL.Vertex2(circulo.ObterPonto(ponto, NumeroPontosCirculo));
                }
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

        public bool Contem(float x, float y)
        {
            return x > PontoInferiorEsquerdo.X && x < PontoSuperiorDireito.X && 
                y > PontoInferiorEsquerdo.Y && y < PontoSuperiorDireito.Y;
        }

        public bool Contem(Vector2 posicao)
        {
            return Contem(posicao.X, posicao.Y);
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
}
