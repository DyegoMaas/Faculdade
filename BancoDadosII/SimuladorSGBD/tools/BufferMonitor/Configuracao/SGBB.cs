using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Core.IO;

namespace BufferMonitor.Configuracao
{
    public class SGBB
    {
        public IGerenciadorBuffer GerenciadorBuffer { get; set; }
        public IGerenciadorEspacoEmDisco GerenciadorEspacoEmDisco { get; set; }
    }
}