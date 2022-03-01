using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L1211B09
{
    /// <summary>
    /// Fila circular genérica (implementação com array)
    /// </summary>
    /// <author>Dyego Alekssander Maas</author>
    public class FilaCircular<T>
    {

        private T[] array = null;
        private int tamanho = 0, tamanhoMax;
        private int cauda = 0, cabeca = 0;

        public FilaCircular(int pTamanho)
        {
            tamanhoMax = pTamanho;
        }

        public void Inserir(T pValor)
        {
            if (array == null)
                array = new T[tamanhoMax];

            if (tamanho == tamanhoMax)
            {
                throw new IndexOutOfRangeException("A fila está cheia.");
            }

            array[cauda] = pValor;
            cauda = (cauda + 1) % tamanhoMax;
            tamanho++;
        }

        public T Remover()
        {
            if (tamanho <= 0)
                throw new IndexOutOfRangeException("Não há mais itens na fila.");

            T removido = array[cabeca];
            cabeca = (cabeca + 1) % tamanhoMax;
            tamanho--;

            return removido;
        }

        public bool Criada()
        {
            return array != null;
        }

        public bool Vazia()
        {
            return tamanho > 0;
        }

        public int GetTamanho()
        {
            return tamanhoMax;
        }

        public int GetTotalElementos()
        {
            return tamanho;
        }

        public override String ToString()
        {
            if (!Criada() || Vazia())
                return string.Empty;

            StringBuilder buffer = new StringBuilder();

            int pos = cabeca;
            do
            {
                buffer.Append(array[pos]).Append(", ");
                pos = (pos + 1) % tamanhoMax;
            } 
            while (pos != cauda);

            return buffer.ToString();
        }
    }
}
