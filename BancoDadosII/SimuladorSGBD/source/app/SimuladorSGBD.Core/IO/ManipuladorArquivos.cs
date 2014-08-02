namespace SimuladorSGBD.Core.IO
{
    //public class ManipuladorArquivos : IManipuladorArquivos
    //{
    //    public void CriarArquivoSeNaoExiste(string caminhoArquivo)
    //    {
    //        if (ArquivoExiste(caminhoArquivo))
    //            return;

    //        var arquivo = new FileInfo(caminhoArquivo);
    //        arquivo.Create();
    //    }

    //    public bool ArquivoExiste(string caminhoArquivo)
    //    {
    //        var arquivo = new FileInfo(caminhoArquivo);
    //        return arquivo.Exists;
    //    }

    //    public void CriarBlocoVazio(int bytes)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}

    public class ManipuladorArquivos : IManipuladorArquivos
    {
        public IManipuladorArquivo Manipular(string caminhoArquivo)
        {
            return new ManipuladorArquivo(caminhoArquivo);
        }
    }
}