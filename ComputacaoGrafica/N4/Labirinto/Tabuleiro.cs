using Labirinto.EngineGrafica;
using System.Drawing;

namespace Labirinto
{
    public class Tabuleiro
    {
        public ObjetoGraficoSimples Chao { get; private set; }

        public Tabuleiro()
        {
            Chao = new CuboSolido(Color.Crimson);
        }
    }
}