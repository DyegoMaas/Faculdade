using OpenTK;

namespace JogoLabirinto
{
    public class ConfiguracaoLabirinto
    {
        private readonly char[,] configuracao;
        
        public readonly int TamanhoBlocosPiso;
        public readonly Vector3d TamanhoParede;

        public ConfiguracaoLabirinto(char[,] configuracao, int tamanhoBlocosPiso, Vector3d tamanhoParede)
        {
            this.configuracao = configuracao;
            TamanhoBlocosPiso = tamanhoBlocosPiso;
            TamanhoParede = tamanhoParede;
        }

        public char[,] Configuracao
        {
            get { return configuracao; }
        }
    }
}