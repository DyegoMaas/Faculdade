using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Linguagens
{
    public struct ResultadoAnalise
    {
        public string Resultado;
        public string Sequencia;
        public string Reconhecimento;

        public ResultadoAnalise(string resultado, string sequencia, string reconhecimento)
        {
            Resultado = resultado;
            Sequencia = sequencia;
            Reconhecimento = reconhecimento;
        }
    }
}
