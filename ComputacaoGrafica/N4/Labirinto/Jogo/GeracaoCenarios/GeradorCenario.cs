using JogoLabirinto.ObjetosGraficos;
using OpenTK;

namespace JogoLabirinto.Jogo.GeracaoCenarios
{
    public static class GeradorCenario
    {
        public static Tabuleiro GerarCenario(ConfiguracaoLabirinto configuracao)
        {
            MotorColisoes.Reiniciar();

            var matrizConfiguracao = configuracao.MatrizConfiguracao;
            var numeroBlocosEmX = matrizConfiguracao.GetLength(1);
            var numeroBlocosEmZ = matrizConfiguracao.GetLength(0);
            var objetosCenario = new Tabuleiro(new SizeD(numeroBlocosEmX, numeroBlocosEmZ), configuracao.Escala);

            int nTochas = 0;
            for (var x = 0; x < numeroBlocosEmX; x++)
            {
                for (var z = 0; z < numeroBlocosEmZ; z++)
                {
                    var config = matrizConfiguracao[x, z];
                    var tipoConteudo = TipoConteudo(config);

                    var posicaoInicial = new Vector3d(x, 0, z);
                    switch (tipoConteudo)
                    {
                        case TipoConteudoCasaTabuleiro.Cacapa:
                            var cacapa = new Cacapa(posicaoInicial);
                            objetosCenario.Cacapas.Add(cacapa);
                            MotorColisoes.Alvos.Add(cacapa);
                            break;
                        case TipoConteudoCasaTabuleiro.Chao:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));
                            break;

                        case TipoConteudoCasaTabuleiro.ChaoComEsfera:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));

                            var posicaoEsfera = new Vector3d(posicaoInicial.X, posicaoInicial.Y + 1, posicaoInicial.Z);

                            var esfera = new Esfera(posicaoEsfera)
                            {
                                //BoundingBox = new BoundingBox()
                            };
                            objetosCenario.Esfera = esfera;
                            MotorColisoes.Esfera = esfera;
                            break;

                        case TipoConteudoCasaTabuleiro.ChaoComParede:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));
                            AdicionarParede(posicaoInicial, objetosCenario);
                            break;

                        case TipoConteudoCasaTabuleiro.ChaoComParedeETocha:
                            objetosCenario.BlocosChao.Add(new Chao(posicaoInicial));
                            AdicionarParede(posicaoInicial, objetosCenario);
                            AdicionarTocha(posicaoInicial, objetosCenario, nTochas++);

                            break;
                    }
                }
            }

            return objetosCenario;
        }

        private static void AdicionarParede(Vector3d posicaoInicial, Tabuleiro objetosCenario)
        {
            var posicaoParede = new Vector3d(posicaoInicial.X, posicaoInicial.Y + 1, posicaoInicial.Z);
            var parede = new Parede(posicaoParede)
            {
                //BoundingBox = new BoundingBox()
            };
            objetosCenario.Paredes.Add(parede);
            MotorColisoes.Paredes.Add(parede);
        }

        private static void AdicionarTocha(Vector3d posicaoInicial, Tabuleiro objetosCenario, int indiceTocha)
        {
            var posicaoParede = new Vector3d(posicaoInicial.X, posicaoInicial.Y + 2, posicaoInicial.Z);
            var parede = new Tocha(posicaoParede, indiceTocha);
            objetosCenario.Tochas.Add(parede);
        }

        private static TipoConteudoCasaTabuleiro TipoConteudo(char config)
        {
            switch (config)
            {
                case 'c': return TipoConteudoCasaTabuleiro.Chao;
                case 'p': return TipoConteudoCasaTabuleiro.ChaoComParede;
                case 'j': return TipoConteudoCasaTabuleiro.ChaoComEsfera;
                case 'f': return TipoConteudoCasaTabuleiro.ChaoComParedeETocha;
                default: return TipoConteudoCasaTabuleiro.Cacapa;
            }
        }

        private enum TipoConteudoCasaTabuleiro
        {
            Chao = 1,
            ChaoComParede = 2,
            ChaoComEsfera = 3,
            ChaoComParedeETocha = 4,
            Cacapa = 5
        }
    }
}