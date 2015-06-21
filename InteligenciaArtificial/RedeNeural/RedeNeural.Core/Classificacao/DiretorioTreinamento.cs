using System.IO;

namespace RedeNeural.Core.Classificacao
{
    public class DiretorioTreinamento
    {
        public const string NomeArquivoTreinamento = "treinamento.dat";

        private readonly DirectoryInfo diretorioBase;

        public DiretorioTreinamento(string diretorioBase)
        {
            this.diretorioBase = new DirectoryInfo(diretorioBase);
        }

        public DirectoryInfo Elipses
        {
            get { return new DirectoryInfo(Path.Combine(diretorioBase.FullName, "Elipses")); }
        }

        public DirectoryInfo Retangulos
        {
            get { return new DirectoryInfo(Path.Combine(diretorioBase.FullName, "Retangulos")); }
        }

        public DirectoryInfo Triangulos
        {
            get { return new DirectoryInfo(Path.Combine(diretorioBase.FullName, "Triangulos")); }
        }

        public DirectoryInfo DadosTreinamento
        {
            get { return new DirectoryInfo(Path.Combine(diretorioBase.FullName, "DadosTreinamento")); }
        }

        public string CaminhoArquivoTreinamento
        {
            get { return Path.Combine(DadosTreinamento.FullName, NomeArquivoTreinamento); }
        }
    }
}