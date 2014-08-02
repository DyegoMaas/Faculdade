namespace SimuladorSGBD.Core
{
    public interface IManipuladorArquivos
    {
        IManipuladorArquivo Manipular(string caminhoArquivo);
    }
}