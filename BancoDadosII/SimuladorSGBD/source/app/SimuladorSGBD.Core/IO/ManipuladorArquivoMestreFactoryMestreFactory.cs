namespace SimuladorSGBD.Core.IO
{
    public class ManipuladorArquivoMestreFactoryMestreFactory : IManipuladorArquivoMestreFactory
    {
        private readonly IConfiguracaoIO configuracao;

        public ManipuladorArquivoMestreFactoryMestreFactory(IConfiguracaoIO configuracao)
        {
            this.configuracao = configuracao;
        }

        public IManipuladorArquivoMestre Criar()
        {
            return new ManipuladorArquivoMestreMestre(configuracao.CaminhoArquivoMestre);
        }
    }
}