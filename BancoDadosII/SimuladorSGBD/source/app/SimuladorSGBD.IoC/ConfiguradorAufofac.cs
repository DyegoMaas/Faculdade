using Autofac;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.GerenciamentoBuffer;
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

            return builder.Build();
        }

        private static void RegistrarCore(ContainerBuilder builder)
        {
            builder.RegisterType<InicializadorArquivoMestre>().As<IInicializadorArquivoMestre>().SingleInstance();
        }

        private static void RegistrarIO(ContainerBuilder builder)
        {
            builder.RegisterInstance(new ConfiguracaoIO
            {
                CaminhoArquivoMestre = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "arquivoMestre.txt")
            }).As<IConfiguracaoIO>();

            builder.RegisterType<ArquivoMestre>().As<IArquivoMestre>();
        }

        private static void RegistrarGerenciamentoBuffer(ContainerBuilder builder)
        {
            builder.RegisterType<BufferEmMemoria>().As<IBufferEmMemoria>();
            builder.RegisterInstance(new ConfiguracaoBuffer
            {
                LimiteDePaginasEmMemoria = 10
            }).As<IConfiguracaoBuffer>();

            builder.RegisterType<GerenciadorBuffer>().As<IGerenciadorBuffer>().SingleInstance()
                .OnActivating(e => e.Context.Resolve<IInicializadorArquivoMestre>().Inicializar());
        }
    }
}
