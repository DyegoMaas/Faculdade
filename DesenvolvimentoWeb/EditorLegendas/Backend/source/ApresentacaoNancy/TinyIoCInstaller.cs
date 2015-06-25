using Nancy.Authentication.Token;
using Nancy.TinyIoc;

namespace ApresentacaoNancy
{
    internal class TinyIoCInstaller
    {
        public static void Instalar(TinyIoCContainer container)
        {
            container.Register<ITokenizer>(new Tokenizer());
        }
    }
}