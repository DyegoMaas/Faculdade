using JogoLabirinto.EngineGrafica;
using System.Drawing;

namespace JogoLabirinto.Regras
{
    public class Labirinto : ObjetoGrafico
    {
        private readonly ConfiguracaoLabirinto configuracaoLabirinto;

        public Ponto4D Centro
        {
            get
            {
                var quantidadeX = configuracaoLabirinto.MatrizConfiguracao.GetLength(0);
                var quantidadeZ = configuracaoLabirinto.MatrizConfiguracao.GetLength(1);

                var x = (quantidadeX * configuracaoLabirinto.TamanhoBlocosPiso) / 2f;
                var z = (quantidadeZ * configuracaoLabirinto.TamanhoBlocosPiso) / 2f;

                if (quantidadeX % 2 != 0)
                {
                    x += configuracaoLabirinto.TamanhoBlocosPiso / 2f;
                    z += configuracaoLabirinto.TamanhoBlocosPiso / 2f;
                }

                return new Ponto4D(Posicao.X + x, Posicao.Y, Posicao.Z + z);
            }
        }

        public Labirinto(ConfiguracaoLabirinto configuracao)
        {
            configuracaoLabirinto = configuracao;

            var matrizConfiguracao = configuracaoLabirinto.MatrizConfiguracao;
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

            SRU();
        }

        private void SRU()
        {
            var centro = Centro;
            var cuboSolido = new CuboSolido(Color.Coral);
            cuboSolido.Redimensionar(1, 50, 1, centro.InverterSinal());
            cuboSolido.Mover(centro.X, centro.Y, centro.Z);

            AdicionarObjetoGrafico(cuboSolido);
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
                case TipoBloco.Esfera:
                {
                    var esfera = ConstruirEsfera();
                    blocoPiso.AdicionarObjetoGrafico(esfera);
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
                case "e": return TipoBloco.Esfera;
                default: return TipoBloco.Chao;                    
            }
        }

        private CuboSolido ConstruirBlocoPiso(int x, int z)
        {
            var tamanho = configuracaoLabirinto.TamanhoBlocosPiso;

            var cuboSolido = new CuboSolido(Color.IndianRed);
            cuboSolido.Redimensionar(tamanho, cuboSolido.Posicao.InverterSinal());
            cuboSolido.Mover(x * tamanho, 0, z * tamanho);

            return cuboSolido;
        }

        private CuboSolido ConstruirBlocoParede()
        {
            var tamanho = configuracaoLabirinto.TamanhoParede;

            var cuboSolido = new CuboSolido(Color.Black);
            cuboSolido.Redimensionar(tamanho.X, tamanho.Y, tamanho.Z, cuboSolido.Posicao.InverterSinal());
            cuboSolido.Mover(0, 1, 0);

            return cuboSolido;
        }

        private EsferaSolida ConstruirEsfera()
        {
            var tamanho = configuracaoLabirinto.TamanhoParede;
            var esferaSolida = new EsferaSolida(Color.Blue);
            esferaSolida.Redimensionar(tamanho.X, tamanho.Y, tamanho.Z, esferaSolida.Posicao.InverterSinal());
            esferaSolida.Mover(0, 1, 0);

            return esferaSolida;
        }

        protected override void Desenhar()
        {
        }
    }

    public enum TipoBloco
    {
        Chao = 0,
        Parede = 1,
        Esfera = 2
    }
}