using FluentNHibernate.Mapping;

namespace Dominio.Pessoas
{
    public class PessoaMap : MapeamentoBase<Pessoa>
    {
        public PessoaMap()
            : base("PESSOA")
        {
            Map(p => p.Nome, "NOME").Length(60).Not.Nullable();
            Map(p => p.Cpf, "CPF").Length(11).Not.Nullable();
        }
    }

    public class MapeamentoBase<T> : ClassMap<T>
        where T : IEntidade<long>
    {
        public MapeamentoBase(string nomeTabela)
        {
            Table(nomeTabela);
            Id(x => x.Id, "ID").GeneratedBy.Native("SEQ_" + nomeTabela);
            DynamicUpdate();
            DynamicInsert();
        }
    }

    public class Entidade : IEntidade<long>
    {
        public virtual long Id { get; set; }
    }

    public interface IEntidade<T>
    {
        T Id { get; set; }
    }
}