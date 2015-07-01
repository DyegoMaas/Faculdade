﻿using JogoLabirinto.Jogo.GeracaoCenarios;
using JogoLabirinto.ObjetosGraficos;
using JogoLabirinto.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using Tao.FreeGlut;

namespace JogoLabirinto
{
    public class JogoLabirinto : GameWindow
    {
        private const double SensibilidadeMouse = .25d;
        private const int AnguloLimiteRotacao = 15;
        private readonly Camera camera = new Camera();
        private Tabuleiro tabuleiro;
        private LinhasReferencia linhasReferencia;

        private bool ligarLuzes = true;

        public JogoLabirinto()
        {
            Location = new Point(50, 50);
            Title = "Labyrinth";

            Load += (sender, e) =>
            {
                WindowState = WindowState.Fullscreen;

                Glut.glutInit();
                GL.ClearColor(Color.CornflowerBlue);

                var corLuzAmbiente = new[] { 0.4f, 0.4f, 0.4f, 1.0f };
                GL.LightModel(LightModelParameter.LightModelAmbient, corLuzAmbiente);

                var lightColor0 = new []{ .8f, .8f, .8f, 1.0f };
                var lightPos0 = new[] { 5.0f, 10f, 10.0f, 0.0f };
                GL.Light(LightName.Light0, LightParameter.Diffuse, lightColor0);
                GL.Light(LightName.Light0, LightParameter.Position, lightPos0);
	            GL.Enable(EnableCap.Light0);

                var lightColor1 = new[] { .8f, .8f, .8f, 1.0f };
                var lightPos1 = new[] { -10.0f, 10f, 10.0f, 0.0f };
                GL.Light(LightName.Light1, LightParameter.Diffuse, lightColor1);
                GL.Light(LightName.Light1, LightParameter.Position, lightPos1);
                GL.Enable(EnableCap.Light1);

                GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.AmbientAndDiffuse); // depende do GL_COLOR_MATERIAL

                GL.Enable(EnableCap.CullFace);
                GL.Enable(EnableCap.DepthTest);

                ConfigurarCena();
            };
            Resize += (sender, e) => camera.Reshape(ClientSize.Width, ClientSize.Height);
            UpdateFrame += OnUpdateFrame;
            RenderFrame += OnRenderFrame;
            KeyDown += OnKeyDown;
            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
        }

        /*  
         * c = chão
         * p = parede
         * j = esfera
         * b = buraco/caçapa
         */
        private void ConfigurarCena()
        {
            var configuracaoLabirinto = new ConfiguracaoLabirinto(new[,]
            {
                {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'j', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'p', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'c', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'p', 'p', 'c', 'c', 'c', 'c', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'c', 'p', 'p', 'c', 'b', 'b', 'p', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'p', 'p', 'c', 'c', 'c', 'p', 'p', 'c', 'c', 'b', 'b', 'c', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'p', 'p', 'c', 'c', 'p', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'p', 'p', 'p', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'}
            },
            escala: 2,
            tamanhoParede: new Vector3d(1, 1, 1));

            tabuleiro = GeradorCenario.GerarCenario(configuracaoLabirinto);
            linhasReferencia = new LinhasReferencia(numeroLinhas:1000, distanciaEntreLinhas:10);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            if (ligarLuzes)
            {
                GL.Enable(EnableCap.Lighting);
                GL.Enable(EnableCap.ColorMaterial); // https://www.opengl.org/sdk/docs/man2/xhtml/glColorMaterial.xml
                GL.Color3(Color.White);
            }
            else
            {
                //if (colorMaterial)
                //{
                //    gl.glEnable(GL.GL_COLOR_MATERIAL); // https://www.opengl.org/sdk/docs/man2/xhtml/glColorMaterial.xml
                //    gl.glColor3f(cor[0], cor[1], cor[2]);
                //}
                //else
                //{
                GL.Material(MaterialFace.Front, MaterialParameter.AmbientAndDiffuse, Color4.White);
                    // https://www.opengl.org/sdk/docs/man2/xhtml/glMaterial.xml  	
                //}
            }

            camera.OlharPara(30, 40, 30,
                tabuleiro.Esfera.Posicao.X, tabuleiro.Esfera.Posicao.Y, tabuleiro.Esfera.Posicao.Z);

            DesenharEixos();
            linhasReferencia.Desenhar();
            tabuleiro.Desenhar();

            GL.Disable(EnableCap.Lighting);
            SwapBuffers();
        }

        private void DesenharEixos()
        {
            GL.LineWidth(5);
            GL.Begin(PrimitiveType.Lines);
            {
                //x
                GL.Color3(Color.Red);
                GL.Vertex3(-10, 0, 0);
                GL.Vertex3(10, 0, 0);

                //y
                GL.Color3(Color.LawnGreen);
                GL.Vertex3(0, -10, 0);
                GL.Vertex3(0, 10, 0);

                //x
                GL.Color3(Color.Blue);
                GL.Vertex3(0, 0, -10);
                GL.Vertex3(0, 0, 10);
            }
            GL.End();
        }

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private double rotacaoX, rotacaoZ;

        void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            if (!rodando)
                return;

            rotacaoX += (e.XDelta * SensibilidadeMouse);
            rotacaoX = rotacaoX.Clamp(-AnguloLimiteRotacao, AnguloLimiteRotacao);

            rotacaoZ += (e.YDelta * SensibilidadeMouse);
            rotacaoZ = rotacaoZ.Clamp(-AnguloLimiteRotacao, AnguloLimiteRotacao);

            tabuleiro.RotacaoX = rotacaoX;
            tabuleiro.RotacaoZ = rotacaoZ;
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            if (!rodando)
                return;

            tabuleiro.Atualizar();
        }

        private bool rodando = false;
        void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
                rodando = !rodando;

            if(e.Key == Key.Escape)
                Exit();

            if (e.Key == Key.L)
                ligarLuzes = !ligarLuzes;

            if (e.Key == Key.R)
            {
                rodando = false;
                ConfigurarCena();
            }
        }

        private void Zoom(KeyboardState teclado)
        {
            if (teclado.IsKeyDown(Key.I))
            {
                camera.FatorZoom += .01f;
            }
            if (teclado.IsKeyDown(Key.O))
            {
                camera.FatorZoom -= .01f;
            }
        }
    }
}