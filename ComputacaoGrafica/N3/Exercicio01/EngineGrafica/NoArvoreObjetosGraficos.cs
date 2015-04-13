using System.Collections.Generic;

namespace Exercicio01.EngineGrafica
{
    public abstract class NoArvoreObjetosGraficos
    {
        protected readonly List<ObjetoGrafico> Filhos = new List<ObjetoGrafico>();

        public void AdicionarObjetoGrafico(ObjetoGrafico objeto)
        {
            Filhos.Add(objeto);
        }

        public void RemoverObjetoGrafico(ObjetoGrafico objeto)
        {
            Filhos.Remove(objeto);
        }
    }
}