using System;
using NHibernate;

namespace Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ITransaction transacao;
        public ISession SessaoAtual { get; private set; }

        public UnitOfWork(ISession sessao)
        {
            SessaoAtual = sessao; 
        }

        public void ExecutarEmTransacao(Action acao)
        {
            transacao = SessaoAtual.BeginTransaction();
            try
            {
                acao.Invoke();
                Commit();
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            SessaoAtual.Dispose();
            NHibernateSessionFactory.RemoverSessao();
            SessaoAtual = null;
        }

        private void Commit()
        {
            transacao.Commit();
        }

        private void Rollback()
        {
            if (transacao != null && transacao.IsActive)
            {
                transacao.Rollback();
            }
        }
    }
}