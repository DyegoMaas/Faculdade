using Core.Repositorios;
using Dominio.Pessoas;
using FluentAssertions;
using NUnit.Framework;

namespace ApresentacaoNancy.Testes.Integracao.Core
{
    public class RepositorioTeste : TesteIntegracaoBase
    {
        readonly Repositorio repositorio = new Repositorio();

        [Test]
        public void inserindo_uma_pessoa()
        {
            var pessoa = new Pessoa
            {
                Cpf = 7172233964,
                Nome = "Dyego"
            };

            ExecutarEmTransacao(sessao => repositorio.SalvarOuAtualizar(pessoa));

            pessoa.Id.Should().BeGreaterThan(0);
            ExecutarEmTransacao(sessao =>
            {
                var pessoaPersistida = sessao.Get<Pessoa>(pessoa.Id);
                pessoaPersistida.Cpf.Should().Be(pessoa.Cpf);
                pessoaPersistida.Nome.Should().Be(pessoa.Nome);
            });
        }

        [Test]
        public void atualizando_uma_pessoa()
        {
            var pessoa = DadaUmaPessoaNaBase();
            const long novoCpf = 92615477536;
            const string novoNome = "Novo nome";
            
            ExecutarEmTransacao(sessao =>
            {
                var pessoaPersistida = sessao.Load<Pessoa>(pessoa.Id);
                pessoaPersistida.Cpf = novoCpf;
                pessoaPersistida.Nome = novoNome;
                repositorio.SalvarOuAtualizar(pessoaPersistida);
            });

            ExecutarEmTransacao(sessao =>
            {
                var pessoaPersistida = sessao.Load<Pessoa>(pessoa.Id);
                pessoaPersistida.Cpf.Should().Be(novoCpf);
                pessoaPersistida.Nome.Should().Be(novoNome);
            });
        }

        [Test]
        public void obtendo_uma_pessoa_pelo_id()
        {
            var pessoa = DadaUmaPessoaNaBase();

            var pessoaPersistida = ExecutarEmTransacao(sessao => repositorio.Obter<Pessoa>(pessoa.Id));

            pessoaPersistida.Id.Should().Be(pessoa.Id);
            pessoaPersistida.Cpf.Should().Be(pessoa.Cpf);
            pessoaPersistida.Nome.Should().Be(pessoa.Nome);
        }

        [Test]
        public void obtendo_todas_as_pessoas()
        {
            var pessoas = new[]
            {
                DadaUmaPessoaNaBase(cpf:52678874125),
                DadaUmaPessoaNaBase(cpf:18357948235),
                DadaUmaPessoaNaBase(cpf:20127331239)
            };

            var pessoasPersistidas = ExecutarEmTransacao(sessao => repositorio.ObterTodos<Pessoa>());

            pessoasPersistidas.Should().HaveSameCount(pessoas);
            foreach (var pessoa in pessoas)
            {
                pessoasPersistidas.Should().ContainSingle(p => p.Id == pessoa.Id);
            }
        }

        [Test]
        public void removendo_uma_pessoa()
        {
            var pessoas = new[]
            {
                DadaUmaPessoaNaBase(cpf:52678874125),
                DadaUmaPessoaNaBase(cpf:18357948235),
                DadaUmaPessoaNaBase(cpf:20127331239)
            };

            ExecutarEmTransacao(sessao => repositorio.Remover<Pessoa>(pessoas[1].Id));

            var pessoasPersistidas = ExecutarEmTransacao(s => s.QueryOver<Pessoa>().List());

            pessoasPersistidas.Should().HaveCount(2);
            pessoasPersistidas.Should().ContainSingle(p => p.Id == pessoas[0].Id);
            pessoasPersistidas.Should().ContainSingle(p => p.Id == pessoas[2].Id);
        }

        private Pessoa DadaUmaPessoaNaBase(long cpf = 7172233964)
        {
            var pessoa = new Pessoa
            {
                Cpf = cpf,
                Nome = "Dyego"
            };
            ExecutarEmTransacao(sessao => sessao.SaveOrUpdate(pessoa));

            return pessoa;
        }
    }
}