using System;

namespace Core
{
    public class ErroNegocioException : Exception
    {
        public ErroNegocioException(string mensagem) : base(mensagem)
        {
        }
    }
}