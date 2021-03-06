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
        private Camera camera;
        private Tabuleiro tabuleiro;
        private LinhasReferencia linhasReferencia;
        private Skybox skybox;

        private bool ligarLuzes = true;

        public JogoLabirinto()
        {
            Location = new Point(50, 50);
            Title = "Labirinto 42";

            Load += (sender, e) =>
            {
                WindowState = WindowState.Fullscreen;

                Glut.glutInit();
                GL.ClearColor(Color.CornflowerBlue);

                //var corLuzAmbiente = new[] { 0.4f, 0.4f, 0.4f, 1.0f };
                //GL.LightModel(LightModelParameter.LightModelAmbient, corLuzAmbiente);

                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.White); // depende do GL_COLOR_MATERIAL

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
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'p', 'p', 'c', 'c', 'c', 'c', 'c', 'p', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'p', 'c', 'c', 'c', 'c', 'c', 'p', 'p', 'b', 'b', 'c', 'p', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'p', 'p', 'c', 'c', 'c', 'c', 'p', 'p', 'c', 'b', 'b', 'c', 'c', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'p', 'p', 'c', 'c', 'c', 'p', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'p', 'p', 'p', 'p', 'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'c', 'p'},
                {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'}
            },
           escala: 2,
           tamanhoParede: new Vector3d(1, 1, 1));

            tabuleiro = GeradorCenario.GerarCenario(configuracaoLabirinto);
            linhasReferencia = new LinhasReferencia(numeroLinhas:1000, distanciaEntreLinhas:10);

            camera = new Camera(new Vector3d(30, 40, 30), Width, Height);
            skybox = new Skybox();

            ContentManager.CarregarTexturas();
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            //GL.PushMatrix();
            //{
            //    GL.Translate(10, 0, 0);
            //    GL.Scale(2,2,2);
            //    GL.Color4(1,1,1,1);
            //    Glut.glutStrokeString(Glut.GLUT_STROKE_ROMAN, "lalalala");
            //    Glut.glutStrokeCharacter(Glut.GLUT_STROKE_ROMAN, 'a');
            //}
            //GL.PopMatrix();

            if (ligarLuzes)
            {
                GL.Enable(EnableCap.Lighting);

                var lightColor0 = new[] { .8f, .8f, .8f, 1.0f };
                var lightPos0 = new[] { 5.0f, 10f, 10.0f, 0.0f };
                GL.Light(LightName.Light0, LightParameter.Diffuse, lightColor0);
                GL.Light(LightName.Light0, LightParameter.Position, lightPos0);
                GL.Enable(EnableCap.Light0);

                var lightColor1 = new[] { 1f, 1f, 1f, 1.0f };
                var lightPos1 = new[] { -10.0f, 10f, 10.0f, 0.0f };
                GL.Light(LightName.Light1, LightParameter.Diffuse, lightColor1);
                GL.Light(LightName.Light1, LightParameter.Position, lightPos1);
                GL.Enable(EnableCap.Light1);

                //GL.Light(LightName.Light2, LightParameter.Diffuse, lightColor1);
                //GL.Light(LightName.Light2, LightParameter.Position, lightPos1);
                //GL.Enable(EnableCap.Light2);

                GL.Enable(EnableCap.ColorMaterial); // https://www.opengl.org/sdk/docs/man2/xhtml/glColorMaterial.xml
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.White);

                GL.Color3(Color.White);
            }
            else
            {
                GL.Disable(EnableCap.ColorMaterial);
                //GL.Material(MaterialFace.Front, MaterialParameter.AmbientAndDiffuse, Color4.White);
            }

            camera.OlharPara(tabuleiro.Esfera.Posicao.X, tabuleiro.Esfera.Posicao.Y, tabuleiro.Esfera.Posicao.Z);

            skybox.Desenhar();
            tabuleiro.Desenhar();

            if(exibirLinhasReferencia)
                linhasReferencia.Desenhar();

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
            camera.Atualizar();
        }

        private bool rodando, exibirLinhasReferencia = false;
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

            if (e.Key == Key.Number1) camera.Comportamento = ComportamentoCamera.Estatico;
            if (e.Key == Key.Number2) camera.Comportamento = ComportamentoCamera.RotacionarAoRedor;
            
            if (e.Key == Key.Number3) camera.AproximarComEfeito(zoomIn:true, zoomOut:false);
            if (e.Key == Key.Number4) camera.AproximarComEfeito(zoomIn:false, zoomOut:true);
            if (e.Key == Key.Number5) camera.AproximarComEfeito(zoomIn:true, zoomOut:true); // Dolly effect, utilizada no filme Vertigo

            if (e.Key == Key.F1)
                exibirLinhasReferencia = !exibirLinhasReferencia;
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