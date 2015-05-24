namespace Labirinto
{
    public class ConfiguracaoLabirinto
    {
        private readonly char[,] configuracao;

        public ConfiguracaoLabirinto(char[,] configuracao)
        {
            this.configuracao = configuracao;
        }

        public char[,] Configuracao
        {
            get { return configuracao; }
        }
    }
}