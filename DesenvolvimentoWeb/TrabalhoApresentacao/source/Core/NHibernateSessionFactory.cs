using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Core
{
    public class ThreadSessionStorage
    {
        private static readonly IList<string> factoryKeys = new List<string>();

        public IEnumerable<ISession> GetAllSessions()
        {
            return factoryKeys.Select(factoryKey => CallContext.GetData(factoryKey) as ISession).ToList();
        }

        public ISession GetSessionForKey(string factoryKey)
        {
            return CallContext.GetData(factoryKey) as ISession;
        }

        public void SetSessionForKey(string factoryKey, ISession session)
        {
            if (!factoryKeys.Contains(factoryKey))
                factoryKeys.Add(factoryKey);
            CallContext.SetData(factoryKey, session);
        }

        public void RemoveSession(string factoryKey)
        {
            CallContext.SetData(factoryKey, null);
        }

        public void RemoveAllSessions()
        {
            foreach (var key in factoryKeys)
            {
                CallContext.SetData(key, null);
            }
        }
    }

    public static class NHibernateSessionFactory
    {
        private const string DefaultFactoryKey = "nhibernate.current_session";
        private static ISessionFactory sessionFactory;
        public static ISessionFactory SessionFactory { get { return sessionFactory; }}
        
        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Base"].ConnectionString; }
        }

        public static void IniciarSessionFactory(IEnumerable<Type> tiposMapeamento)
        {
            sessionFactory = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(ConnectionString))
                .Mappings(m =>
                {
                    foreach (var tipoMapeamento in tiposMapeamento)
                    {
                        m.FluentMappings.AddFromAssembly(tipoMapeamento.Assembly);
                    }
                })
                .BuildSessionFactory();
        }

        private static readonly ThreadSessionStorage ThreadSessionStorage = new ThreadSessionStorage();

        public static ISession ObterSessaoAtual()
        {
            var session = ThreadSessionStorage.GetSessionForKey(DefaultFactoryKey);
            if (session != null && session.IsOpen)
            {
                return session;
            }

            session = sessionFactory.OpenSession();
            session.FlushMode = FlushMode.Commit;
            ThreadSessionStorage.SetSessionForKey(DefaultFactoryKey, session);

            return session;
        }

        public static void RemoverSessao()
        {
            ThreadSessionStorage.RemoveSession(DefaultFactoryKey);
        }

        public static void RemoverTodasSessoes()
        {
            ThreadSessionStorage.RemoveAllSessions();
        }

        public static void FecharTodasSessoes()
        {
            if (ThreadSessionStorage != null)
            {
                foreach (var session in ThreadSessionStorage.GetAllSessions())
                {
                    if (session != null && session.IsOpen)
                    {
                        session.Close();
                    }
                }
            }
        }
    }
}