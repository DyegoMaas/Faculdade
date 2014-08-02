namespace SimuladorSGBD.Core
{
    public interface IManipuladorArquivos
    {
        void CriarArquivo(string caminhoArquivo);
        bool ArquivoExiste(string caminhoArquivo);
        void CriarBlocoVazio(int bytes);
    }
}