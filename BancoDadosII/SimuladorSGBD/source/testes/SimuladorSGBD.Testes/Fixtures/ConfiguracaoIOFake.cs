using System;
using System.IO;
using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Testes.Fixtures
{
    public class ConfiguracaoIOFake : IConfiguracaoIO
    {
        public string CaminhoArquivoMestre
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "arquivoTeste.txt"); }
        }
    }
}