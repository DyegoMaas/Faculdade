using OpenTK;

namespace JogoLabirinto.AntigaImplementacao
{
    public class ConfiguracaoLabirinto
    {
        private readonly char[,] matrizConfiguracao;
        
        public readonly int TamanhoBlocosPiso;
        public readonly Vector3d TamanhoParede;

        public ConfiguracaoLabirinto(char[,] matrizConfiguracao, int tamanhoBlocosPiso, Vector3d tamanhoParede)
        {
            this.matrizConfiguracao = matrizConfiguracao;
            TamanhoBlocosPiso = tamanhoBlocosPiso;
            TamanhoParede = tamanhoParede;
        }

        public char[,] MatrizConfiguracao
        {
            get { return matrizConfiguracao; }
        }
    }
}