using JogoLabirinto.ObjetosGraficos;
using OpenTK;
using System;
using System.Collections.Generic;

namespace JogoLabirinto.Jogo
{
    public class MotorColisoes
    {
        public static Esfera Esfera { get; set; }
        public static IList<Parede> Paredes { get; set; }
        public static IList<Cacapa> Alvos { get; set; }

        static MotorColisoes()
        {
            Paredes = new List<Parede>();
            Alvos = new List<Cacapa>();
        }

        public static void Reiniciar()
        {
            Esfera = null;
            Paredes.Clear();
            Alvos.Clear();
        }

        public static Vector3d NovaPosicaoDaEsfera(Vector3d posicaoAtual, Vector3d velocidade)
        {
            var novaPosicaoDaEsfera = posicaoAtual + velocidade;
            for (int i = 0; i < Paredes.Count; i++)
            {
                var parede = Paredes[i];

                var count = 0;
                calculoColisao:
                var distancia = novaPosicaoDaEsfera - parede.Posicao;
                var distanciaAbs = new Vector3d(Math.Abs(distancia.X), Math.Abs(distancia.Y), Math.Abs(distancia.Z));
                
                if (distanciaAbs.X < 1 && distanciaAbs.Z < 1) //entrou no objeto
                {
                    Console.WriteLine("atravessou a parede {0} por {1}", i, distanciaAbs);

                    if (distanciaAbs.X > distanciaAbs.Z)
                        velocidade.X = 0;
                    if (distanciaAbs.X < distanciaAbs.Z)
                        velocidade.Z = 0;

                    novaPosicaoDaEsfera = posicaoAtual + velocidade;
                    if (++count > 1)
                        return posicaoAtual;

                    goto calculoColisao;
                }
            }
            return novaPosicaoDaEsfera;
        }

        public static bool EncontrouOAlvo(Vector3d posicaoAtual)
        {
            for (int i = 0; i < Alvos.Count; i++)
            {
                var alvo = Alvos[i];

                var distancia = posicaoAtual - alvo.Posicao;
                var distanciaAbs = new Vector3d(Math.Abs(distancia.X), Math.Abs(distancia.Y), Math.Abs(distancia.Z));

                if (distanciaAbs.X < .5 && distanciaAbs.Z < .5) //entrou no objeto
                {
                    return true;
                }
            }
            return false;
        }
    }
}