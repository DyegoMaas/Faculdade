using Core.Repositorios;
using Nancy.TinyIoc;

namespace ApresentacaoNancy
{
    internal class TinyIoCInstaller
    {
        public static void Instalar(TinyIoCContainer container)
        {
            container.Register<IRepositorio, Repositorio>();
        }
    }
}