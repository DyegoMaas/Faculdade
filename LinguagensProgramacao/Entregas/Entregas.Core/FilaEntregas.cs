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
        private readonly TimeSpan intervaloTentativasAgendamento;
        private readonly TimeSpan intervaloTentativasEntrega;

        private readonly BlockingCollection<Produto> entregas;

        private int quantidadeEntregue;

        public int QuantidadeEntregue
        {
            get { return quantidadeEntregue; }
        }

        public FilaEntregas(int capacidadeEntrega, TimeSpan intervaloTentativasAgendamento, TimeSpan intervaloTentativasEntrega)
        {
            this.intervaloTentativasAgendamento = intervaloTentativasAgendamento;
            this.intervaloTentativasEntrega = intervaloTentativasEntrega;
            entregas = new BlockingCollection<Produto>(new ConcurrentQueue<Produto>(), capacidadeEntrega);
        }

        public void AgendarEntrega(Produto produto)
        {
            while(!entregas.TryAdd(produto))
            {
                Thread.Sleep(intervaloTentativasAgendamento);
            }
            Console.WriteLine("item INSERIDO");
        }

        public Produto EfetuarEntrega()
        {
            Produto produtoParaEntrega;
            while (!entregas.TryTake(out produtoParaEntrega))
            {
                Thread.Sleep(intervaloTentativasEntrega);
            }
            Console.WriteLine("item OBTIDO");
            Interlocked.Increment(ref quantidadeEntregue);

            return produtoParaEntrega;
        }

        public int ObterQuantidadeEntregasPendentes()
        {
            //lock (entregas)
            {
                if (!disposed)
                    return entregas.Count;
                return 0;
            }    
        }

        private bool disposed;
        public void Dispose()
        {
            disposed = true;
            entregas.Dispose();
        }
    }
}
