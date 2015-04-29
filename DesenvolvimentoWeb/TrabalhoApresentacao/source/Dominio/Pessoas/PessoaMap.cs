using Core.Entidades;

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
}