using System;

namespace SimuladorSGBD.Core.ArmazenamentoRegistros
{
    [Serializable]
    public class PonteiroRegistro
    {
        public int Tamanho { get; set; }
        public int Endereco { get; set; }
    }
}