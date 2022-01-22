using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Linguagens.Excecoes.Finalizacao
{
    [Serializable]
    public class InvalidWordException : ProcessingEndException
    {
        public InvalidWordException() { }
        public InvalidWordException(string message) : base(message) { }
        public InvalidWordException(string message, Exception inner) : base(message, inner) { }
        public InvalidWordException(ResultadoAnalise resultado) : base(resultado) { }

        protected InvalidWordException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
