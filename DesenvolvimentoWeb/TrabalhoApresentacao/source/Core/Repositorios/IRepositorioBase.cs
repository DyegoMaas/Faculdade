using System.Collections.Generic;
using Core.Entidades;

namespace Core.Repositorios
{
    public interface IRepositorioBase<TId>
        where TId : struct
    {
        T Obter<T>(TId id) where T: class, IEntidade<TId>;
        void SalvarOuAtualizar<T>(T entidade) where T: class, IEntidade<TId>;
        IList<T> ObterTodos<T>() where T: class, IEntidade<TId>;
        void Remover<T>(TId id) where T : class, IEntidade<TId>;
        void Remover<T>(T entidade) where T: class, IEntidade<TId>;
    }
}