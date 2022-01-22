using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Excecoes.Finalizacao
{
    [Serializable]
    public class ValidWordException : ProcessingEndException
    {
        public ValidWordException() { }
        public ValidWordException(string message) : base(message) { }
        public ValidWordException(string message, Exception inner) : base(message, inner) { }
        public ValidWordException(ResultadoAnalise resultado) : base(resultado) { }

        protected ValidWordException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
