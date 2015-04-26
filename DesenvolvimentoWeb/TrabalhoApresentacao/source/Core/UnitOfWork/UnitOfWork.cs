using NHibernate;

namespace Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ITransaction transaction;

        public UnitOfWork()
        {
            CurrentSession = NHibernateSessionFactory.ObterSessaoAtual(); 
            transaction = CurrentSession.BeginTransaction();
        }

        public ISession CurrentSession { get; private set; }

        public void Dispose()
        {
            CurrentSession.Dispose();
            NHibernateSessionFactory.RemoverSessao();
            CurrentSession = null;
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            if (transaction.IsActive) transaction.Rollback();
        }
    }
}