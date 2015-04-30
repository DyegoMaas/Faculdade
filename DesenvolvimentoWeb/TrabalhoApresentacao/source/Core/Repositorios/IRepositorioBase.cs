using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Entidades;

namespace Core.Repositorios
{
    public interface IRepositorioBase<TId>
        where TId : struct
    {
        T BuscarUm<T>(Expression<Func<T, bool>> filtros) where T : class, IEntidade<TId>;
        T Obter<T>(TId id) where T: class, IEntidade<TId>;
        void SalvarOuAtualizar<T>(T entidade) where T: class, IEntidade<TId>;
        IList<T> ObterTodos<T>() where T: class, IEntidade<TId>;
        void Remover<T>(TId id) where T : class, IEntidade<TId>;
        void Remover<T>(T entidade) where T: class, IEntidade<TId>;
        int Contar<T>() where T : class, IEntidade<TId>;
        int Contar<T>(Expression<Func<T, bool>> filtros) where T : class, IEntidade<TId>;
    }
}