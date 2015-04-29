namespace Core.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Criar()
        {
            var sessao = NHibernateSessionFactory.ObterSessaoAtual();
            return new UnitOfWork(sessao);
        }
    }
}