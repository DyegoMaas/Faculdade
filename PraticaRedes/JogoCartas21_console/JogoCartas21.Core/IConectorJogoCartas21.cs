using System.Collections.Generic;
using JogoCartas21.Core.Models;

namespace JogoCartas21.Core
{
    public interface IConectorJogoCartas21
    {
        IEnumerable<Player> GetPlayers(Usuario usuario);
        Carta GetCard(Usuario usuario);
        void EnviarComandoJogo(Usuario usuario, ComandosJogo comando);
    }
}