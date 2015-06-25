using System.Collections.Generic;

namespace ApresentacaoNancy
{
    public class ResultadoOperacao
    {
        public bool Sucesso { get; private set; }
        public object Dados { get; private set; }
        public IList<string> Erros { get; set; }

        public static ResultadoOperacao ComSucesso(object dados)
        {
            return new ResultadoOperacao
            {
                Sucesso = true,
                Dados = dados
            };
        }

        public static ResultadoOperacao ComErros(params string[] erros)
        {
            return new ResultadoOperacao
            {
                Sucesso = false,
                Erros = erros
            };
        }
    }
}