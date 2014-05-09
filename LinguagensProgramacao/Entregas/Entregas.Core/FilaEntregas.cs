using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Entregas.Core
{
    public class FilaEntregas : IDisposable
    {
        private readonly int capacidadeEntrega;
        private readonly TimeSpan intervaloTentativas;
        private readonly BlockingCollection<Produto> entregas;

        public FilaEntregas(int capacidadeEntrega, TimeSpan intervaloTentativas)
        {
            this.capacidadeEntrega = capacidadeEntrega;
            this.intervaloTentativas = intervaloTentativas;
            entregas = new BlockingCollection<Produto>(new ConcurrentQueue<Produto>(), capacidadeEntrega);
        }

        public void AgendarEntrega(Produto produto)
        {
            while(!entregas.TryAdd(produto))
            {
                Thread.Sleep(intervaloTentativas);
            }
            Console.WriteLine("item INSERIDO");
        }

        public Produto EfetuarEntrega()
        {
            Produto produtoParaEntrega;
            while (!entregas.TryTake(out produtoParaEntrega))
            {
                Thread.Sleep(intervaloTentativas);
            }
            Console.WriteLine("item OBTIDO");

            return produtoParaEntrega;
        }

        public int ObterQuantidadeEntregasPendentes()
        {
            lock (entregas)
            {
                return entregas.Count;
            }    
        }

        public void Dispose()
        {
            entregas.Dispose();
        }
    }
}
