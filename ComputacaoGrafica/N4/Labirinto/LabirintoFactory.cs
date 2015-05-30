
namespace JogoLabirinto
{
    public class LabirintoFactory
    {
    //    public static Labirinto GerarLabirinto(ConfiguracaoLabirinto configuracao)
    //    {
    //        var labirinto = new Labirinto();
    //        var matrizConfiguracao = configuracao.MatrizConfiguracao;
    //        for (var x = 0; x < matrizConfiguracao.GetLength(0); x++)
    //        {
    //            for (var z = 0; z < matrizConfiguracao.GetLength(1); z++)
    //            {
    //                var blocoPiso = ConstruirBlocoPiso(x, z, configuracao);
    //                labirinto.AdicionarObjetoGrafico(blocoPiso);

    //                var tipo = ObterConfiguracaoBloco(matrizConfiguracao, x, z);
    //                AdicionarElementosAdicionais(tipo, blocoPiso);
    //            }
    //        }
    //    }


    //    private CuboSolido ConstruirBlocoPiso(int x, int z, ConfiguracaoLabirinto configuracao)
    //    {
    //        var tamanho = configuracao.TamanhoBlocosPiso;

    //        var cuboSolido = new CuboSolido(Color.IndianRed);
    //        cuboSolido.Redimensionar(tamanho, cuboSolido.Posicao.InverterSinal());
    //        cuboSolido.Mover(x * tamanho, 0, z * tamanho);

    //        return cuboSolido;
    //    }

    //    private void AdicionarElementosAdicionais(TipoBloco tipo,  blocoPiso)
    //    {
    //        switch (tipo)
    //        {
    //            case TipoBloco.Parede:
    //                {
    //                    var parede = ConstruirBlocoParede();
    //                    blocoPiso.AdicionarObjetoGrafico(parede);
    //                    break;
    //                }
    //            case TipoBloco.Esfera:
    //                {
    //                    var esfera = ConstruirEsfera();
    //                    blocoPiso.AdicionarObjetoGrafico(esfera);
    //                    break;
    //                }
    //        }
    //    }

    //    private static TipoBloco ObterConfiguracaoBloco(char[,] matrizConfiguracao, int x, int z)
    //    {
    //        var configuracao = matrizConfiguracao[x, z].ToString().ToLower();
    //        switch (configuracao)
    //        {
    //            case "p": return TipoBloco.Parede;
    //            case "e": return TipoBloco.Esfera;
    //            default: return TipoBloco.Chao;
    //        }
    //    }

    //    private CuboSolido ConstruirBlocoParede()
    //    {
    //        var tamanho = configuracaoLabirinto.TamanhoParede;

    //        var cuboSolido = new CuboSolido(Color.Black);
    //        cuboSolido.Redimensionar(tamanho.X, tamanho.Y, tamanho.Z, cuboSolido.Posicao.InverterSinal());
    //        cuboSolido.Mover(0, 1, 0);

    //        return cuboSolido;
    //    }

    //    private EsferaSolida ConstruirEsfera()
    //    {
    //        var tamanho = configuracaoLabirinto.TamanhoParede;
    //        var esferaSolida = new EsferaSolida(Color.Blue);
    //        esferaSolida.Redimensionar(tamanho.X, tamanho.Y, tamanho.Z, esferaSolida.Posicao.InverterSinal());
    //        esferaSolida.Mover(0, 1, 0);

    //        return esferaSolida;
    //    }
    }
}