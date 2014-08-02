namespace SimuladorSGBD.Core.IO
{
    public class ManipuladorArquivoMestreFactory : IManipuladorArquivoMestreFactory
    {
        private readonly IConfiguracaoIO configuracao;

        public ManipuladorArquivoMestreFactory(IConfiguracaoIO configuracao)
        {
            this.configuracao = configuracao;
        }

        public IArquivoMestre Criar()
        {
            return new ArquivoMestre(configuracao.CaminhoArquivoMestre);
        }
    }
}