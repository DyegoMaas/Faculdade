using System;
using NHibernate;

namespace Core
{
    public interface IUnitOfWork : IDisposable
    {
        ISession CurrentSession { get; }
        void Commit();
        void Rollback();
    }
}