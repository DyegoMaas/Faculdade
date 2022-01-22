using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Excecoes.Finalizacao
{
    [Serializable]
    public abstract class ProcessingEndException : Exception
    {
        public ResultadoAnalise Resultado { get; private set; }

        public ProcessingEndException() { }
        public ProcessingEndException(string message) : base(message) { }
        public ProcessingEndException(string message, Exception inner) : base(message, inner) { }
        public ProcessingEndException(ResultadoAnalise resultado)
            : base("")
        {
            this.Resultado = resultado;
        }

        protected ProcessingEndException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
