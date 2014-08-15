using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao;
using SimuladorSGBD.Core.IO;
using System;
using System.IO;

namespace SimuladorSGBD.IoC
{
    public class ConfiguradorAufofac
    {
        public static IContainer Configurar()
        {
            var builder = new ContainerBuilder();
            RegistrarCore(builder);
            RegistrarIO(builder);
            RegistrarGerenciamentoBuffer(builder);

            var container = builder.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            return container;
        }

        private static void RegistrarCore(ContainerBuilder builder)
        {
            builder.RegisterType<InicializadorArquivoMestre>()
                   .As<IInicializadorArquivoMestre>()
                   .SingleInstance();
        }

        private static void RegistrarIO(ContainerBuilder builder)
        {
            builder.RegisterInstance(new ConfiguracaoIO
            {
                CaminhoArquivoMestre = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "arquivoMestre.txt")
            }).As<IConfiguracaoIO>();

            builder.RegisterType<GerenciadorEspacoEmDisco>()
                   .As<IGerenciadorEspacoEmDisco>()
                   .SingleInstance();
        }

        private static void RegistrarGerenciamentoBuffer(ContainerBuilder builder)
        {
            builder.RegisterType<PoolDeBuffers>().As<IPoolDeBuffers>();

            builder.RegisterInstance(new ConfiguracaoBuffer
            {
                LimiteDePaginasEmMemoria = 10
            }).As<IConfiguracaoBuffer>();

            builder.RegisterType<LogicaSubstituicaoFactory>()
                   .As<ILogicaSubstituicaoFactory>()
                   .SingleInstance();

            builder.RegisterType<GerenciadorBuffer>()
                   .As<IGerenciadorBuffer>()
                   .SingleInstance()
                   .OnActivating(e => e.Context.Resolve<IInicializadorArquivoMestre>().Inicializar(blocos: 20, bytes: 128));
        }
    }
}
