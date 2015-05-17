using System.Collections.Generic;
using System.Linq;

namespace Labirinto.EngineGrafica
{
    public abstract class NoGrafoCenaSimples
    {
        public NoGrafoCenaSimples Pai { get; set; }
        protected readonly List<NoGrafoCenaSimples> Filhos = new List<NoGrafoCenaSimples>();
        public IEnumerable<ObjetoGraficoSimples> ObjetosGraficos { get { return Filhos.Cast<ObjetoGraficoSimples>(); } }

        //TODO precisa???
        public IEnumerable<ObjetoGraficoSimples> Irmaos
        {
            get
            {
                return Pai == null
                    ? Enumerable.Empty<ObjetoGraficoSimples>()
                    : Pai.Filhos.Where(filho => filho != this).Cast<ObjetoGraficoSimples>();
            }
        }

        //TODO daria de setar o anterior ao montar a árvore
        public ObjetoGraficoSimples Anterior
        {
            get
            {
                if (Pai == null)
                    return null;

                var indice = Pai.Filhos.IndexOf(this);
                if (indice > 0)
                    return Pai.Filhos[indice - 1] as ObjetoGraficoSimples;
                return null;
            }
        }

        //TODO daria de setar o próximo ao montar a árvore
        public ObjetoGraficoSimples Proximo
        {
            get
            {
                if (Pai == null)
                    return null;

                var indice = Pai.Filhos.IndexOf(this);
                if (indice < Pai.Filhos.Count - 1)
                    return Pai.Filhos[indice + 1] as ObjetoGraficoSimples;
                return null;
            }
        }

        public ObjetoGraficoSimples PrimeiroFilho
        {
            get
            {
                return Filhos.FirstOrDefault() as ObjetoGraficoSimples;
            }
        }

        public ObjetoGraficoSimples UltimoFilho
        {
            get
            {
                return Filhos.LastOrDefault() as ObjetoGraficoSimples;
            }
        }

        public void AdicionarObjetoGrafico(ObjetoGraficoSimples objeto)
        {
            objeto.Pai = this;
            Filhos.Add(objeto);
        }

        public void RemoverObjetoGrafico(ObjetoGraficoSimples objeto)
        {
            Filhos.Remove(objeto);
        }
    }
}