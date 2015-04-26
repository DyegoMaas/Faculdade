using Core;
using Dominio.Pessoas;
using NHibernate;
using NUnit.Framework;
using System;

namespace ApresentacaoNancy.Testes
{
    public class TesteIntegracaoBase
    {
        private static readonly string[] TabelasLimpar =
        {
            "PESSOA"
        };
        
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            NHibernateSessionFactory.IniciarSessionFactory(new [] {typeof(PessoaMap)});
        }

        [SetUp]
        public void SetUp()
        {
            using (var sessao = NHibernateSessionFactory.ObterSessaoAtual())
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

        [TearDown]
        public void Shutdown()
        {
            NHibernateSessionFactory.FecharTodasSessoes();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            NHibernateSessionFactory.RemoverTodasSessoes();
        }

        protected void ExecutarEmTransacao(Action<ISession> acao)
        {
            using (var sessao = NHibernateSessionFactory.ObterSessaoAtual())
            {
                using (var transacao = sessao.BeginTransaction())
                {
                    acao.Invoke(sessao);
                    transacao.Commit();
                }
                NHibernateSessionFactory.RemoverSessao();
            }
        }

        protected T ExecutarEmTransacao<T>(Func<ISession, T> funcao)
        {
            T retorno;
            using (var sessao = NHibernateSessionFactory.ObterSessaoAtual())
            {
                using (var transacao = sessao.BeginTransaction())
                {
                    retorno = funcao.Invoke(sessao);
                    transacao.Commit();
                }
                NHibernateSessionFactory.RemoverSessao();
            }

            return retorno;
        }
    }
}