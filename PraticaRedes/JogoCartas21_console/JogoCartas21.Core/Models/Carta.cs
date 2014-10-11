using System.Collections.Generic;

namespace JogoCartas21.Core.Models
{
    public class Carta  
    {
        private static readonly Dictionary<string, int> ValoresCarta = new Dictionary<string, int>
        {
            {"A", 1},
            {"2", 2},
            {"3", 3},
            {"4", 4},
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8},
            {"9", 9},
            {"J", 10},
            {"Q", 10},
            {"K", 10}
        };

        public string Num { get; private set; }
        public Naipe Suit { get; private set; }

        public Carta(string num, Naipe suit)
        {
            Num = num;
            Suit = suit;
        }

        public int ValorCarta
        {
            get { return ValoresCarta[Num]; }
        }
    }
}