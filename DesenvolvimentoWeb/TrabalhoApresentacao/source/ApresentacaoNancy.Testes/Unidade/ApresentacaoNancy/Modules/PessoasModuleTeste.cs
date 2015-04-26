using ApresentacaoNancy.Modules;
using Core.Repositorios;
using CsQuery.ExtensionMethods;
using Dominio.Pessoas;
using FluentAssertions;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace ApresentacaoNancy.Testes.Unidade.ApresentacaoNancy.Modules
{
    public class PessoasModuleTeste
    {
        private IRepositorio repositorio;
        private Browser browser;

        [SetUp]
        public void SetUp()
        {
            repositorio = Substitute.For<IRepositorio>();

            var bootstrapper = new ConfigurableBootstrapper(with =>
            {
                with.ResponseProcessor<JsonProcessor>()
                    .Module<PessoasModule>().Dependency(repositorio);
                //TODO configurar modelbinder
            });
            browser = new Browser(bootstrapper);
        }

        [Test]
        public void obtendo_todas_as_pessoas()
        {
            var pessoasNaBase = DadasPessoasNaBase();
            
            // When
            var response = browser.Get("/pessoas", with => with.HttpRequest());

            // Then
            var pessoasResposta = response.Body.AsString().ParseJSON<IList<Pessoa>>();
            foreach (var pessoa in pessoasNaBase)
            {
                pessoasResposta.Should().ContainSingle(p => p.Id == pessoa.Id);
                pessoasResposta.Should().ContainSingle(p => p.Cpf == pessoa.Cpf);
                pessoasResposta.Should().ContainSingle(p => p.Nome == pessoa.Nome);
            }
        }

        [Test]
        public void obtendo_uma_pessoa_especifica()
        {
            var pessoaNaBase = DadaUmaPessoaNaBaseComId(1);

            // When
            var response = browser.Get("/pessoas/1", with => with.HttpRequest());

            // Then
            var pessoaResponse = response.Body.AsString().ParseJSON<Pessoa>();
            pessoaResponse.Id.Should().Be(pessoaNaBase.Id);
            pessoaResponse.Cpf.Should().Be(pessoaNaBase.Cpf);
            pessoaResponse.Nome.Should().Be(pessoaNaBase.Nome);
        }

        [Test]
        public void cadastrando_uma_pessoa()
        {
            const long idGerado = 10;
            repositorio.When(r => r.SalvarOuAtualizar(Arg.Any<Pessoa>()))
                       .Do(callInfo => callInfo.Arg<Pessoa>().Id = idGerado);

            // When
            var response = browser.Post("/pessoas", with =>
            {
                with.FormValue("cpf", "85914051533");
                with.FormValue("nome", "teste");
                with.HttpRequest();
            });

            // Then
            response.Body.AsString().Should().Be(idGerado.ToString());
        }

        private Pessoa DadaUmaPessoaNaBaseComId(long id)
        {
            var pessoa = new Pessoa { Id = id, Cpf = 34258774707, Nome = "Joao" };
            repositorio.Obter<Pessoa>(pessoa.Id).Returns(pessoa);

            return pessoa;
        }

        private IEnumerable<Pessoa> DadasPessoasNaBase()
        {
            var pessoasNaBase = new[]
            {
                new Pessoa {Id = 1, Cpf = 34258774707, Nome = "Joao"},
                new Pessoa {Id = 2, Cpf = 33918919129, Nome = "Dyego"}
            };
            repositorio.ObterTodos<Pessoa>().Returns(pessoasNaBase);

            return pessoasNaBase;
        }
    }
}