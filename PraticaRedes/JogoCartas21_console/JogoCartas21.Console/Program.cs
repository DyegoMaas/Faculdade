using JogoCartas21.Core.IO;
using JogoCartas21.Core.Jogo;
using JogoCartas21.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace JogoCartas21.Console
{
    class Program
    {
        private const string EnderecoServidor = "larc.inf.furb.br";
        private const int PortaUDP = 1011;
        private const int PortaTCP = 1012;

        private static void Main(string[] args)
        {
            //const string userId = "6673";
            //const string senha = "yrskd";
            const string userId = "8065";
            const string senha = "egptq";

            var clienteTcp = new ClienteTCP(EnderecoServidor, PortaTCP);
            var clienteUdp = new ClienteUDP(EnderecoServidor, PortaUDP);
            var conector = new ConectorJogoCartas21(clienteTcp, clienteUdp);
            var jogo = new JogoCartas21(conector, new Usuario(userId, senha));

            System.Console.WriteLine(clienteTcp.EnviarMensagem(string.Format("GET USERS {0}:{1}", userId, senha)));

            jogo.EntrarNoJogo();
            while(jogo.Pontuacao < 21)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                var players = jogo.ObterJogadoresAtivos();
                
                foreach (var player in players)
                {
                    System.Console.WriteLine("{0} - {1}", player.UserId, player.Status);
                }

                var eu = players.FirstOrDefault(p => p.UserId == userId);
                if (eu != null)
                {
                    if (jogo.PossoPegarCarta())
                    {
                        var carta = jogo.PegarCarta();
                        System.Console.WriteLine("Carta obtida: {0} de {1}; Pontuação: {2}", carta.Num, carta.Suit, jogo.Pontuacao);
                    }
                }
            }
            jogo.PararDeJogar();
            jogo.SairDoJogo();
            
            System.Console.Read();
        }

        public class JogoCartas21
        {
            private readonly ConectorJogoCartas21 conector;
            private readonly Usuario usuario;
            private Player jogador;
            public int Pontuacao { get; private set; }

            public JogoCartas21(ConectorJogoCartas21 conector, Usuario usuario)
            {
                this.conector = conector;
                this.usuario = usuario;
            }

            public void EntrarNoJogo()
            {
                conector.EnviarComandoJogo(usuario, ComandosJogo.Enter);
                Pontuacao = 0;
            }

            public void PararDeJogar()
            {
                conector.EnviarComandoJogo(usuario, ComandosJogo.Stop);
                Pontuacao = 0;
            }

            public void SairDoJogo()
            {
                conector.EnviarComandoJogo(usuario, ComandosJogo.Quit);
                Pontuacao = 0;
            }

            public IList<Player> ObterJogadoresAtivos()
            {
                var jogadoresAtivos = conector.GetPlayers(usuario).ToArray();
                jogador = jogadoresAtivos.FirstOrDefault(p => p.UserId == usuario.UserId);

                return jogadoresAtivos;
            }

            public bool PossoPegarCarta()
            {
                if (jogador == null)
                    return false;

                return jogador.Status == PlayerStatus.GETTING;
            }

            public Carta PegarCarta()
            {
                if(!PossoPegarCarta())
                    throw new InvalidOperationException();

                var carta = conector.GetCard(usuario);
                Pontuacao += carta.ValorCarta;

                return carta;
            }
        }
    }
}
