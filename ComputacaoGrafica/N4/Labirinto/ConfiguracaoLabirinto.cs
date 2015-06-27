using OpenTK;

namespace JogoLabirinto
{
    public class ConfiguracaoLabirinto
    {
        private readonly char[,] matrizConfiguracao;
        
        public readonly double Escala;
        public readonly Vector3d TamanhoParede;

        public ConfiguracaoLabirinto(char[,] matrizConfiguracao, double escala, Vector3d tamanhoParede)
        {
            this.matrizConfiguracao = matrizConfiguracao;
            Escala = escala;
            TamanhoParede = tamanhoParede;
        }

        public char[,] MatrizConfiguracao
        {
            get { return matrizConfiguracao; }
        }
    }
}