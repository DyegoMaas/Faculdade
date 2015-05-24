using System.Drawing;
using JogoLabirinto.EngineGrafica;

namespace JogoLabirinto.Regras
{
    public class Labirinto : ObjetoGrafico
    {
        private readonly ConfiguracaoLabirinto configuracaoLabirinto;

        //private readonly List<ObjetoGrafico> blocosPiso = new List<ObjetoGrafico>();
        //public IEnumerable<ObjetoGrafico> BlocosPiso { get { return blocosPiso; } }

        public Labirinto(ConfiguracaoLabirinto configuracao)
        {
            configuracaoLabirinto = configuracao;

            var config = configuracaoLabirinto.Configuracao;
            for (var coluna = 0; coluna < config.GetLength(1); coluna++)
            {
                for (var linha = 0; linha < config.GetLength(0); linha++)
                {
                    var blocoPiso = ConstruirBlocoPiso(linha, coluna);
                    //blocosPiso.Add(blocoPiso);
                    AdicionarObjetoGrafico(blocoPiso);
                }
            }
        }

        private CuboSolido ConstruirBlocoPiso(int linha, int coluna)
        {
            var tamanho = configuracaoLabirinto.TamanhoBlocosPiso;

            var cuboSolido = new CuboSolido(Color.IndianRed);
            cuboSolido.Redimensionar(tamanho, cuboSolido.Posicao);
            cuboSolido.Mover(coluna * tamanho, linha * tamanho, 0);

            return cuboSolido;
        }

        protected override void Desenhar()
        {
        }
    }
}