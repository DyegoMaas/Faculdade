namespace SimuladorSGBD.Core.IO
{
    public interface IManipuladorArquivos
    {
        IManipuladorArquivo Manipular(string caminhoArquivo);
    }
}