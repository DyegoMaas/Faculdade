using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Linguagens.Excecoes
{
    [Serializable]
    public class NoEndException : Exception
    {
        public NoEndException() { }
        public NoEndException(string message) : base(message) { }
        public NoEndException(string message, Exception inner) : base(message, inner) { }
        protected NoEndException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
