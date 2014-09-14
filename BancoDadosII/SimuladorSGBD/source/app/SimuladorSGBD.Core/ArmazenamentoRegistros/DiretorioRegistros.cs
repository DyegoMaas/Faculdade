using System.Collections.Generic;

namespace SimuladorSGBD.Core.ArmazenamentoRegistros
{
    public class DiretorioRegistros
    {
        public int EnderecoInicioAreaLivre { get; set; }
        public int NumeroEntradas { get { return DiretorioSlots.Count; } }
        public IList<PonteiroRegistro> DiretorioSlots { get; private set; }

        public DiretorioRegistros()
        {
            DiretorioSlots = new List<PonteiroRegistro>();
        }
        
        public void InserirSlot(int tamanho)
        {
            DiretorioSlots.Add(new PonteiroRegistro
            {
                Endereco = EnderecoInicioAreaLivre,
                Tamanho = tamanho
            });

            EnderecoInicioAreaLivre += tamanho;
        }
    }
}