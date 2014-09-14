using System.Collections.Generic;

namespace SimuladorSGBD.Core.ArmazenamentoRegistros
{
    public class DiretorioRegistros
    {
        private int enderecoInicioAreaLivre;

        public int EnderecoInicioAreaLivre
        {
            get
            {
                if (NumeroEntradas == 0)
                    return 0;
                return enderecoInicioAreaLivre - 1;
            }
            private set { enderecoInicioAreaLivre = value; }
        }

        public int NumeroEntradas { get { return DiretorioSlots.Count; } }
        public IList<PonteiroRegistro> DiretorioSlots { get; private set; }

        public DiretorioRegistros()
        {
            DiretorioSlots = new List<PonteiroRegistro>();
        }
        
        public void InserirSlot(int tamanho)
        {
            enderecoInicioAreaLivre += tamanho;

            DiretorioSlots.Add(new PonteiroRegistro
            {
                Indice = DiretorioSlots.Count + 1, 
                Tamanho = tamanho
            });
        }
    }
}