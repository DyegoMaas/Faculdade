using System;
using System.Collections.Generic;
using JogoCartas21.Core.IO;
using JogoCartas21.Core.Models;
using JogoCartas21.Core.Utils;

namespace JogoCartas21.Core.Jogo
{
    public class ConectorJogoCartas21
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
            var mensagem = "GET PLAYERS {0}:{1}".FormatWith(usuario.UserId, usuario.Senha);
            var resposta = clienteTcp.EnviarMensagem(mensagem);

            var listaJogadores = ConverterJogadores(resposta);
            return listaJogadores;
        }

        public Carta GetCard(Usuario usuario)
        {
            var mensagem = "GET CARD {0}:{1}".FormatWith(usuario.UserId, usuario.Senha);
            var resposta = clienteTcp.EnviarMensagem(mensagem);

            if (resposta == ":")
                throw new InvalidOperationException("resposta inválida (timeout?)");

            var partesResposta = resposta.Split(':');
            return new Carta(partesResposta[0], partesResposta[1].ToEnum<Naipe>());
        }

        public void EnviarComandoJogo(Usuario usuario, ComandosJogo comando)
        {
            var mensagem = "SEND GAME {0}:{1}:{2}".FormatWith(usuario.UserId, usuario.Senha, ConverterParaString(comando));
            clienteUdp.EnviarMensagem(mensagem);
        }

        private IEnumerable<Player> ConverterJogadores(string resposta)
        {
            resposta = resposta.TrimEnd('\n').TrimEnd(':');

            if(string.IsNullOrWhiteSpace(resposta))
                return new Player[0];

            var players = new List<Player>();
            var partesResposta = resposta.Split(':');
            for (int i = 0; i < partesResposta.Length; i += 3)
            {
                players.Add(new Player
                {
                    UserId = partesResposta[i],
                    Status = partesResposta[i + 1].ToEnum<PlayerStatus>()
                });
            }
            return players;
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
