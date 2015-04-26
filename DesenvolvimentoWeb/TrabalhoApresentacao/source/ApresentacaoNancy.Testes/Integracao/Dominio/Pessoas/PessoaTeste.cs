using Dominio.Pessoas;
using FluentAssertions;
using NUnit.Framework;

namespace ApresentacaoNancy.Testes.Integracao.Dominio.Pessoas
{
    public class PessoaTeste : TesteIntegracaoBase
    {
        [Test]
        public void persistindo_uma_pessoa()
        {
            var pessoa = new Pessoa
            {
                Cpf = 7172233964,
                Nome = "Dyego"
            };

            ExecutarEmTransacao(sessao => sessao.SaveOrUpdate(pessoa));
            pessoa.Id.Should().BeGreaterThan(0);

            ExecutarEmTransacao(sessao =>
            {
                var pessoaPersistida = sessao.Get<Pessoa>(pessoa.Id);
                pessoaPersistida.Nome.Should().Be(pessoa.Nome);
                pessoaPersistida.Cpf.Should().Be(pessoa.Cpf);
            });
        } 
    }

    //public class UnitOfWorkFactory : IDisposable
    //{
    //    public UnitOfWorkFactory()
    //    {
    //        SessionFactory.OpenSession();
    //    }

    //    public void Dispose()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}