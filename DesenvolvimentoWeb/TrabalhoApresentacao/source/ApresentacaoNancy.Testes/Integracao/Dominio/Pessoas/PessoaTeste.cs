using Dominio.Pessoas;
using FluentAssertions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;

namespace ApresentacaoNancy.Testes.Integracao.Dominio.Pessoas
{
    public class PessoaTeste : TestFixtureBase
    {
        [Test]
        public void persistindo_uma_pessoa()
        {
            var pessoa = new Pessoa
            {
                Cpf = 7172233964,
                Nome = "Dyego"
            };

            using (var session = SessionFactory.OpenSession())
            {
                using (var transacao = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pessoa);
                    transacao.Commit();
                }
            }
            pessoa.Id.Should().BeGreaterThan(0);

            using (var sessao = SessionFactory.OpenSession())
            {
                using (sessao.BeginTransaction())
                {
                    var pessoaPersistida = sessao.Get<Pessoa>(pessoa.Id);
                    pessoaPersistida.Nome.Should().Be(pessoa.Nome);
                    pessoaPersistida.Cpf.Should().Be(pessoa.Cpf);
                }
            }

        } 
    }

    public class TestFixtureBase
    {
        private static readonly string[] TabelasLimpar =
        {
            "PESSOA"
        };

        private const string ConnectionString = "Server=localhost;Database=apresentacao_nancy;User ID=postgres;Password=postgres";


        protected ISessionFactory SessionFactory { get; private set; }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            SessionFactory = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(ConnectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Pessoa>())
                .BuildSessionFactory();
        }

        [SetUp]
        public void SetUp()
        {
            using (var sessao = SessionFactory.OpenSession())
            {
                foreach (var tabela in TabelasLimpar)
                {
                    using (var transacao = sessao.BeginTransaction())
                    {
                        sessao
                            .CreateSQLQuery("delete from " + tabela)
                            .ExecuteUpdate();
                        transacao.Commit();
                    }
                }
            }
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            SessionFactory.Close();
        }
    }
}