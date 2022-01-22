using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Linguagens.Excecoes.Finalizacao
{
    [Serializable]
    public class InvalidCharacterException : ProcessingEndException
    {
        public InvalidCharacterException() { }
        public InvalidCharacterException(string message) : base(message) { }
        public InvalidCharacterException(string message, Exception inner) : base(message, inner) { }
        public InvalidCharacterException(ResultadoAnalise resultado) : base(resultado) { }

        protected InvalidCharacterException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
