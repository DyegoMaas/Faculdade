using Core.Repositorios;
using Core.UnitOfWork;
using Nancy.TinyIoc;

namespace ApresentacaoNancy
{
    internal class TinyIoCInstaller
    {
        public static void Instalar(TinyIoCContainer container)
        {
            container.Register<IRepositorio, Repositorio>();
            container.Register<IUnitOfWorkFactory, UnitOfWorkFactory>();
        }
    }
}