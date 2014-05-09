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
        private readonly TimeSpan intervaloTentativasAgendamento;
        private readonly TimeSpan intervaloTentativasEntrega;

        private readonly BlockingCollection<Produto> entregas;

        public FilaEntregas(int capacidadeEntrega, TimeSpan intervaloTentativasAgendamento, TimeSpan intervaloTentativasEntrega)
        {
            this.capacidadeEntrega = capacidadeEntrega;
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
