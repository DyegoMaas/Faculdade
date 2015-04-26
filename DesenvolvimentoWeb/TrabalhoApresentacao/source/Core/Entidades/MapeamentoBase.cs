using FluentNHibernate.Mapping;

namespace Core.Entidades
{
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
}