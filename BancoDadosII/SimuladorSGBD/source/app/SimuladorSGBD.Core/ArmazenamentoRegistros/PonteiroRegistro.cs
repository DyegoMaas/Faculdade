using System;

namespace SimuladorSGBD.Core.ArmazenamentoRegistros
{
    [Serializable]
    public class PonteiroRegistro
    {
        public int Tamanho { get; set; }
        public int Endereco { get; set; }

        public override bool Equals(Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var ponteiro = obj as PonteiroRegistro;
            if (ponteiro == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Tamanho == ponteiro.Tamanho) && (Endereco == ponteiro.Endereco);
        }

        public bool Equals(PonteiroRegistro ponteiro)
        {
            // If parameter is null return false:
            if (ponteiro == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Tamanho == ponteiro.Tamanho) && (Endereco == ponteiro.Endereco);
        }

        public static bool operator ==(PonteiroRegistro a, PonteiroRegistro b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Tamanho == b.Tamanho && a.Endereco == b.Endereco;
        }

        public static bool operator !=(PonteiroRegistro a, PonteiroRegistro b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Tamanho ^ Endereco;
        }
    }
}