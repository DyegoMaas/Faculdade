using System.IO;

namespace ManipulaImagem
{
    public class DiretorioTreinamento
    {
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
    }
}