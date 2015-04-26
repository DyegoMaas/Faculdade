using System;
using NHibernate;

namespace Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ISession CurrentSession { get; }
        void Commit();
        void Rollback();
    }
}