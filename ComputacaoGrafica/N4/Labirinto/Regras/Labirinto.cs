using Labirinto.EngineGrafica;
using System.Collections.Generic;
using System.Drawing;

namespace Labirinto.Regras
{
    public class Labirinto : ObjetoGrafico
    {
        private const int TamanhoBlocos = 10;

        private readonly List<ObjetoGrafico> blocosPiso = new List<ObjetoGrafico>();
        public IEnumerable<ObjetoGrafico> BlocosPiso { get { return blocosPiso; } }

        public Labirinto(ConfiguracaoLabirinto configuracao)
        {
            var config = configuracao.Configuracao;
            for (var coluna = 0; coluna < config.GetLength(1); coluna++)
            {
                for (var linha = 0; linha < config.GetLength(0); linha++)
                {
                    var blocoPiso = ConstruirBlocoPiso(linha, coluna);
                    blocosPiso.Add(blocoPiso);
                    AdicionarObjetoGrafico(blocoPiso);
                }
            }
        }

        private static CuboSolido ConstruirBlocoPiso(int linha, int coluna)
        {
            var cuboSolido = new CuboSolido(Color.IndianRed);
            cuboSolido.Redimensionar(TamanhoBlocos, cuboSolido.Posicao);
            cuboSolido.Mover(coluna * TamanhoBlocos, linha * TamanhoBlocos, 0);

            return cuboSolido;
        }

        protected override void Desenhar()
        {
        }
    }
}