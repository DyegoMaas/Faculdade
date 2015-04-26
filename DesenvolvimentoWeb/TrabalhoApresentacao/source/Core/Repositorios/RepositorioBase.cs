using System.Collections.Generic;
using Core.Entidades;
using NHibernate;

namespace Core.Repositorios
{
    public abstract class RepositorioBase<TId> : IRepositorioBase<TId>
        where TId : struct 
    {
        public T Obter<T>(TId id) where T : class, IEntidade<TId>
        {
            return Sessao.Get<T>(id);
        }

        public void SalvarOuAtualizar<T>(T entidade) where T : class, IEntidade<TId>
        {
            Sessao.SaveOrUpdate(entidade);
        }

        public IList<T> ObterTodos<T>() where T : class, IEntidade<TId>
        {
            return Sessao.QueryOver<T>().List();
        }

        public void Remover<T>(TId id) where T : class, IEntidade<TId>
        {
            Sessao.Delete(Obter<T>(id));
        }

        public void Remover<T>(T entidade) where T : class, IEntidade<TId>
        {
            Sessao.Delete(entidade);
        }

        protected static ISession Sessao
        {
            get { return NHibernateSessionFactory.ObterSessaoAtual(); }
        }
    }
}