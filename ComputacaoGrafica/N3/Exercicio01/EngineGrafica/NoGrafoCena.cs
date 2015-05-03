using System.Collections.Generic;
using System.Linq;

namespace Exercicio01.EngineGrafica
{
    public abstract class NoGrafoCena
    {
        public NoGrafoCena Pai { get; set; }
        protected readonly List<NoGrafoCena> Filhos = new List<NoGrafoCena>();
        public IEnumerable<ObjetoGrafico> ObjetosGraficos { get { return Filhos.Cast<ObjetoGrafico>(); } }

        //TODO precisa???
        public IEnumerable<ObjetoGrafico> Irmaos
        {
            get
            {
                return Pai == null 
                    ? Enumerable.Empty<ObjetoGrafico>()
                    : Pai.Filhos.Where(filho => filho != this).Cast<ObjetoGrafico>();
            }
        }

        //TODO daria de setar o anterior ao montar a árvore
        public ObjetoGrafico Anterior
        {
            get
            {
                if (Pai == null)
                    return null;

                var indice = Pai.Filhos.IndexOf(this);
                if (indice > 0)
                    return Pai.Filhos[indice - 1] as ObjetoGrafico;
                return null;
            }
        }

        //TODO daria de setar o próximo ao montar a árvore
        public ObjetoGrafico Proximo
        {
            get
            {
                if (Pai == null)
                    return null;

                var indice = Pai.Filhos.IndexOf(this);
                if (indice < Pai.Filhos.Count - 1)
                    return Pai.Filhos[indice + 1] as ObjetoGrafico;
                return null;
            }
        }

        public ObjetoGrafico PrimeiroFilho
        {
            get
            {
                return Filhos.FirstOrDefault() as ObjetoGrafico;
            }
        }

        public ObjetoGrafico UltimoFilho
        {
            get
            {
                return Filhos.LastOrDefault() as ObjetoGrafico;
            }
        }

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