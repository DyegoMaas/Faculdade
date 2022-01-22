using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Excecoes
{
    [Serializable]
    public class MultipleStartException : Exception
    {
        public MultipleStartException() { }
        public MultipleStartException(string message) : base(message) { }
        public MultipleStartException(string message, Exception inner) : base(message, inner) { }
        protected MultipleStartException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
