namespace JogoLabirinto
{
    public class ConfiguracaoLabirinto
    {
        private readonly char[,] configuracao;
        public readonly int TamanhoBlocosPiso;

        public ConfiguracaoLabirinto(char[,] configuracao, int tamanhoBlocosPiso)
        {
            this.configuracao = configuracao;
            TamanhoBlocosPiso = tamanhoBlocosPiso;
        }

        public char[,] Configuracao
        {
            get { return configuracao; }
        }
    }
}