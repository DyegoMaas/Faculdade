using System.Collections.Generic;
using System.Linq;

namespace Exercicio01.EngineGrafica
{
    public abstract class NoGrafoCena
    {
        public NoGrafoCena Pai { get; set; }
        protected readonly List<NoGrafoCena> Filhos = new List<NoGrafoCena>();
        public IEnumerable<ObjetoGrafico> ObjetosGraficos { get { return Filhos.Cast<ObjetoGrafico>(); } } 

        public void AdicionarObjetoGrafico(ObjetoGrafico objeto)
        {
            objeto.Pai = this;
            Filhos.Add(objeto);
        }

        public void RemoverObjetoGrafico(ObjetoGrafico objeto)
        {
            Filhos.Remove(objeto);
        }
    }
}