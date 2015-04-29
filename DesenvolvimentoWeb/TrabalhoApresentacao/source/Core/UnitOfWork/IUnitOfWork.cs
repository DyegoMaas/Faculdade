using System;
using NHibernate;

namespace Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ISession SessaoAtual { get; }
        void ExecutarEmTransacao(Action acao);
    }
}