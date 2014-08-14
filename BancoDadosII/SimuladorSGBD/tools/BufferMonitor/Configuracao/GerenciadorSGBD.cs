using Autofac;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Core.IO;
using SimuladorSGBD.IoC;

namespace BufferMonitor.Configuracao
{
    public class GerenciadorSGBD
    {
        public static SGBB Inicializar()
        {
            var container = ConfiguradorAufofac.Configurar();
            return new SGBB
            {
                GerenciadorBuffer = container.Resolve<IGerenciadorBuffer>(),
                GerenciadorEspacoEmDisco = container.Resolve<IGerenciadorEspacoEmDisco>()
            };
        }
    }
}