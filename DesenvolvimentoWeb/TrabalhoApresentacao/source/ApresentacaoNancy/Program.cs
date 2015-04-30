using System;
using System.Text;
using System.Threading.Tasks;
using Core;
using Dominio.Pessoas;
using Nancy.Hosting.Self;
using Topshelf;

namespace ApresentacaoNancy
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = HostFactory.New(x =>
            {
                x.Service<InicializadorServico>(sc =>
                {
                    sc.ConstructUsing(name => new InicializadorServico());
                    sc.WhenStarted((servico, hostControl) =>
                    {
                        Task.Factory.StartNew(servico.Iniciar);
                        return true;
                    });
                    sc.WhenStopped((servico, hostControl) =>
                    {
                        servico.Parar();
                        return true;
                    });
                });
                x.RunAsLocalSystem();
                
                x.SetDescription("Aplicação para demonstração do NancyFX");
                x.SetDisplayName("NancyFX");
                x.SetServiceName("NancyFX");

                x.EnableServiceRecovery(rc => rc.RestartService(delayInMinutes: 1));
                x.SetStartTimeout(TimeSpan.FromMinutes(1));
                x.StartAutomatically();
            });

            host.Run();
        }
    }

    internal class InicializadorServico
    {
        public void Iniciar()
        {
            //NHibernateSessionFactory.IniciarSessionFactory(new [] { typeof(PessoaMap) });

            var hostConfiguration = new HostConfiguration
            {
                UrlReservations = new UrlReservations { CreateAutomatically = true }
            };

            var uri = new Uri("http://localhost:3579");
            var host = new NancyHost(hostConfiguration, uri);
            host.Start();
        }

        public void Parar()
        {
        }
    }
}
