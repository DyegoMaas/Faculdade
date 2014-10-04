using System;
using System.Collections.Generic;
using JogoCartas21.Core.IO;
using JogoCartas21.Core.Models;

namespace JogoCartas21.Core
{
    public class ConectorJogoCartas21 : IConectorJogoCartas21
    {
        private readonly ClienteTCP clienteTcp;
        private readonly ClienteUDP clienteUdp;

        public ConectorJogoCartas21(ClienteTCP clienteTcp, ClienteUDP clienteUdp)
        {
            this.clienteTcp = clienteTcp;
            this.clienteUdp = clienteUdp;
        }

        public IEnumerable<Player> GetPlayers(Usuario usuario)
        {
            var mensagem = string.Format("GET PLAYERS {0}:{1}", usuario.UserId, usuario.Senha);
            var resposta = clienteTcp.EfetuarChamada(mensagem);

            var listaJogadores = ConverterJogadores(resposta);
            return listaJogadores;
        }

        public Carta GetCard(Usuario usuario)
        {
            var mensagem = string.Format("GET CARD {0}:{1}", usuario.UserId, usuario.Senha);
            var resposta = clienteTcp.EfetuarChamada(mensagem);

            var partesResposta = resposta.Split(';');
            return new Carta
            {
               Num = partesResposta[0],
               Suit = (Naipe)Enum.Parse(typeof(Naipe), partesResposta[1])
            };
        }

        public void EnviarComandoJogo(Usuario usuario, ComandosJogo comando)
        {
            var mensagem = string.Format("SEND GAME {0}:{1}:{2}", usuario.UserId, usuario.Senha, ConverterParaString(comando));
            clienteUdp.EnviarMensagem(mensagem);
        }

        private IEnumerable<Player> ConverterJogadores(string resposta)
        {
            resposta = resposta.TrimEnd('\n').TrimEnd(':');
            var partesResposta = resposta.Split(':');
            for (int i = 0; i < partesResposta.Length; i += 3)
            {
                yield return new Player
                {
                    UserId = partesResposta[i],
                    Status = (PlayerStatus)Enum.Parse(typeof(PlayerStatus), partesResposta[i + 1])
                };
            }
        }

        private string ConverterParaString(ComandosJogo comando)
        {
            switch (comando)
            {
                case ComandosJogo.Enter: return "ENTER";
                case ComandosJogo.Quit: return "QUIT";
                default: return "STOP";
            }
        }
    }
}
