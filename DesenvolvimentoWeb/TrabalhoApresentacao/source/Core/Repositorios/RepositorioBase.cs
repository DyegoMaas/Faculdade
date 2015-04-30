using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Entidades;
using NHibernate;
using NHibernate.Criterion;

namespace Core.Repositorios
{
    public abstract class RepositorioBase<TId> : IRepositorioBase<TId>
        where TId : struct 
    {
        public T BuscarUm<T>(Expression<Func<T, bool>> filtros) where T : class, IEntidade<TId>
        {
            var query = Sessao.QueryOver<T>();
            if (filtros != null)
                query.Where(filtros);
            return query.List().FirstOrDefault();
        }

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

        public int Contar<T>() where T : class, IEntidade<TId>
        {
            return Contar<T>(null);
        }

        public int Contar<T>(Expression<Func<T, bool>> filtros) where T : class, IEntidade<TId>
        {
            var query = Sessao.QueryOver<T>();
            if (filtros != null)
            {
                query.Where(filtros);
            }
            return query.Select(Projections.RowCount()).List<int>().FirstOrDefault();
        }

        protected static ISession Sessao
        {
            get { return NHibernateSessionFactory.ObterSessaoAtual(); }
        }
    }
}