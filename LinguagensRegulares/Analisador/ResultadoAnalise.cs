using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador
{
    public class ResultadoAnalise
    {
        public int Linha { get; set; }
        public string Resultado { get; set; }
        public string Sequencia { get; set; }
        public string Reconhecimento { get; set; }

        public ResultadoAnalise(string resultado, string sequencia, string reconhecimento)
        {
            Resultado = resultado;
            Sequencia = sequencia;
            Reconhecimento = reconhecimento;
        }

        public ResultadoAnalise()
            : this("", "", "")
        { }
    }
}
