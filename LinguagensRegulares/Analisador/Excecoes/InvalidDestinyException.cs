using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Excecoes
{
    [Serializable]
    public class InvalidDestinyException : Exception
    {
        public InvalidDestinyException() { }
        public InvalidDestinyException(string message) : base(message) { }
        public InvalidDestinyException(string message, Exception inner) : base(message, inner) { }
        protected InvalidDestinyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
