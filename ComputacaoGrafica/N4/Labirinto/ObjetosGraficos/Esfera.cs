using JogoLabirinto.Jogo;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Tao.FreeGlut;

namespace JogoLabirinto.ObjetosGraficos
{
    public class Esfera : ComponenteTabuleiro
    {
        private const double FatorQueda = .03;
        public Vector3d Velocidade { get; set; }
        private bool visivel = true;
        
        public Esfera(Vector3d posicao)
            : base(posicao)
        {
            AntesDeDesenhar(() =>
            {
                if (!visivel)
                    return;

                GL.Color3(Color.Red);
                Glut.glutSolidSphere(.5f, 10, 10);
            });
        }

        protected override void DesenharObjeto()
        {
            if (visivel)
                DesenharVetorVelocidade();
        }

        private void DesenharVetorVelocidade()
        {
            const int tamahoEixos = 400;

            var velX = new Vector3d(Velocidade.X * tamahoEixos, 0, 0);
            var velY = new Vector3d(0, Velocidade.Y * tamahoEixos, 0);
            var velZ = new Vector3d(0, 0, Velocidade.Z * tamahoEixos);
            var direcao = velX + velY + velZ;

            GL.LineWidth(5);
            GL.Begin(PrimitiveType.Lines);
            {
                //x
                GL.Color3(Color.Red);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(velX);

                //y
                GL.Color3(Color.LawnGreen);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(velY);

                //z
                GL.Color3(Color.Blue);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(velZ);

                //dir
                GL.Color3(Color.SlateBlue);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(direcao);
            }
            GL.End();
        }

        private bool encontrouAlvo;
        public override void Atualizar()
        {
            if (encontrouAlvo)
            {
                if (Posicao.Y < -1)
                    visivel = false;
                else
                    Posicao -= Vector3d.UnitY * FatorQueda;
                return;
            }

            Posicao = MotorColisoes.NovaPosicaoDaEsfera(Posicao, Velocidade);

            if (MotorColisoes.EncontrouOAlvo(Posicao))
            {
                Console.WriteLine("Encontrou o alvo");
                encontrouAlvo = true;
            }
        }
    }
}