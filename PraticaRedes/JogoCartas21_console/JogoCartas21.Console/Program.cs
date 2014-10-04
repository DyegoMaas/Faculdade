using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using JogoCartas21.Core;
using JogoCartas21.Core.IO;
using System.Linq;
using JogoCartas21.Core.Models;

namespace JogoCartas21.Console
{
    class Program
    {
        private static void Main(string[] args)
        {
            //const string userId = "6673";
            //const string senha = "yrskd";
            const string userId = "8065";
            const string senha = "egptq";

            var clienteTcp = new ClienteTCP("larc.inf.furb.br", 1012);
            var clienteUdp = new ClienteUDP("larc.inf.furb.br", 1011);
            var conector = new ConectorJogoCartas21(clienteTcp, clienteUdp);
            var jogo = new JogoCartas21(conector, new Usuario(userId, senha));

            jogo.EntrarNoJogo();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            var players = jogo.ObterJogadoresAtivos();
            jogo.PegarCarta();
            System.Console.Read();
        }

        public class JogoCartas21
        {
            private readonly ConectorJogoCartas21 conector;
            private readonly Usuario usuario;

            public JogoCartas21(ConectorJogoCartas21 conector, Usuario usuario)
            {
                this.conector = conector;
                this.usuario = usuario;
            }

            public void EntrarNoJogo()
            {
                conector.EnviarComandoJogo(usuario, ComandosJogo.Enter);
            }

            public void PararDeJogar()
            {
                conector.EnviarComandoJogo(usuario, ComandosJogo.Stop);
            }

            public void SairDoJogo()
            {
                conector.EnviarComandoJogo(usuario, ComandosJogo.Quit);
            }

            public IList<Player> ObterJogadoresAtivos()
            {
                return conector.GetPlayers(usuario).ToArray();
            }

            public Carta PegarCarta()
            {
                var jogadoresAtivos = ObterJogadoresAtivos();
                var jogador = jogadoresAtivos.First(j => j.UserId == usuario.UserId);

                if(jogador.Status == "GETTING") //todo USAR ENUM
                throw new NotImplementedException();
            }
        }
    }
}
