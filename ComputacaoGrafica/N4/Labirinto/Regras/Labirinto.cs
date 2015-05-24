using JogoLabirinto.EngineGrafica;
using System.Drawing;

namespace JogoLabirinto.Regras
{
    public class Labirinto : ObjetoGrafico
    {
        private readonly ConfiguracaoLabirinto configuracaoLabirinto;

        public Labirinto(ConfiguracaoLabirinto configuracao)
        {
            configuracaoLabirinto = configuracao;

            var matrizConfiguracao = configuracaoLabirinto.Configuracao;
            for (var x = 0; x < matrizConfiguracao.GetLength(0); x++)
            {
                for (var z = 0; z < matrizConfiguracao.GetLength(1); z++)
                {
                    var blocoPiso = ConstruirBlocoPiso(x, z);
                    AdicionarObjetoGrafico(blocoPiso);

                    var tipo = ObterConfiguracaoBloco(matrizConfiguracao, x, z);
                    AdicionarElementosAdicionais(tipo, blocoPiso);
                }
            }
        }

        private void AdicionarElementosAdicionais(TipoBloco tipo, ObjetoGrafico blocoPiso)
        {
            switch (tipo)
            {
                case TipoBloco.Parede:
                {
                    var parede = ConstruirBlocoParede();
                    blocoPiso.AdicionarObjetoGrafico(parede);
                    break;
                }
            }
        }

        private static TipoBloco ObterConfiguracaoBloco(char[,] matrizConfiguracao, int x, int z)
        {
            var configuracao = matrizConfiguracao[x, z].ToString().ToLower();
            switch (configuracao)
            {
                case "p": return TipoBloco.Parede;
                default: return TipoBloco.Chao;                    
            }
        }

        private CuboSolido ConstruirBlocoPiso(int x, int z)
        {
            var tamanho = configuracaoLabirinto.TamanhoBlocosPiso;

            var cuboSolido = new CuboSolido(Color.IndianRed);
            cuboSolido.Redimensionar(tamanho, cuboSolido.Posicao);
            cuboSolido.Mover(x * tamanho, 0, z * tamanho);

            return cuboSolido;
        }

        private CuboSolido ConstruirBlocoParede()
        {
            var tamanho = configuracaoLabirinto.TamanhoParede;

            var cuboSolido = new CuboSolido(Color.Black);
            cuboSolido.Redimensionar(tamanho.X, tamanho.Y, tamanho.Z, cuboSolido.Posicao);
            cuboSolido.Mover(0, 1, 0);

            return cuboSolido;
        }

        protected override void Desenhar()
        {
        }
    }

    public enum TipoBloco
    {
        Chao = 0,
        Parede = 1
    }
}