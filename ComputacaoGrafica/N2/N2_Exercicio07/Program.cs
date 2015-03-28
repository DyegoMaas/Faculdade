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
}
