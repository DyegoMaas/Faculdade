package estoques;

import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;
import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

public class Entregas implements IFilaComPropertyChangeSupport {
	private PropertyChangeSupport changes = new PropertyChangeSupport(this);

	private final ReentrantLock lock = new ReentrantLock();
	private final Condition podeReceberEntregas = lock.newCondition();
	private final Condition podeEfetuarEntregas = lock.newCondition();
	
	private Queue<Produto> filaEntrega = new LinkedList<Produto>();
	private int capacidadeEntrega = 0;
	
	public Entregas(int capacidadeEntrega) {
		this.capacidadeEntrega = capacidadeEntrega;
	}
	
	public void addProduto(Produto produtoEntrega) throws InterruptedException {
		lock.lock();
		
		while (capacidadeMaximaAtingida())
			podeReceberEntregas.await();			
		
		try {
			filaEntrega.add(produtoEntrega);
			changes.firePropertyChange("produtos", null, filaEntrega);
			System.out.printf("produto %s selecionado para entrega\n", produtoEntrega.getNome());
		} finally {
			podeReceberEntregas.signalAll();
			lock.unlock();
		}
	}

	public Produto obterProdutoParaEntrega() throws InterruptedException {
		lock.lock();

		while (filaEntrega.isEmpty())
			podeEfetuarEntregas.await();

		try {
			Produto entrega = filaEntrega.poll();
			changes.firePropertyChange("pedidos", null, filaEntrega);
			System.out.printf("Produto %s saiu para entrega\n", entrega.toString());
			return entrega;
		} finally {
			podeEfetuarEntregas.signalAll();
			lock.unlock();
		}
	}

	private boolean capacidadeMaximaAtingida() {
		return filaEntrega.size() >= capacidadeEntrega;
	}
	
	@Override
	public synchronized int getNumeroItens() {
		return filaEntrega.size();
	}

	@Override
	public void addPropertyChangeListener(PropertyChangeListener l) {
		changes.addPropertyChangeListener(l);
	}

	@Override
	public void removePropertyChangeListener(PropertyChangeListener l) {
		changes.removePropertyChangeListener(l);
	}	
}

/*
package pedidos.distribuicao;

import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;
import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

import pedidos.IPedido;
import pedidos.entregas.Entrega;
import estrutura.IFilaComPropertyChangeSupport;

public class FilaPedidos implements IFilaComPropertyChangeSupport {

	private PropertyChangeSupport changes = new PropertyChangeSupport(this);

	private final ReentrantLock lock = new ReentrantLock();
	private final Condition podeGerarUmPedido = lock.newCondition();

	private Queue<IPedido> pedidos = new LinkedList<IPedido>();
	private int contadorNumeroPacotes = 0;
	private int numeroPacotesPorEntrega;

	public FilaPedidos(int numeroPacotesPorEntrega) {
		this.numeroPacotesPorEntrega = numeroPacotesPorEntrega;
	}

	public void addPedido(IPedido novoPedido) {
		lock.lock();

		try {
			pedidos.add(novoPedido);
			changes.firePropertyChange("pedidos", null, pedidos);
			contadorNumeroPacotes += novoPedido.getNumeroPacotes();
			System.out.printf("pedido %s adicionado na fila de entrega\n", novoPedido.getIdPedido());
		} finally {
			podeGerarUmPedido.signalAll();
			lock.unlock();
		}
	}

	public Entrega obterEntrega() throws InterruptedException {
		lock.lock();

		while (!possuiNumeroMinimoPacotes())
			podeGerarUmPedido.await();

		try {
			Entrega entrega = gerarEntrega();
			changes.firePropertyChange("pedidos", null, pedidos);
			System.out.printf("Gerada entrega %s\n", entrega.toString());
			return entrega;
		} finally {
			lock.unlock();
		}
	}

	private boolean possuiNumeroMinimoPacotes() {
		return contadorNumeroPacotes >= numeroPacotesPorEntrega;
	}

	private Entrega gerarEntrega() {
		List<IPedido> pedidosEntrega = new ArrayList<IPedido>();
		
		int pacotesNaEntrega = 0;
		
		while(pacotesNaEntrega < numeroPacotesPorEntrega){
			IPedido pedidoPoll = pedidos.poll();
			if(pedidoPoll != null){
				pacotesNaEntrega += pedidoPoll.getNumeroPacotes();
				pedidosEntrega.add(pedidoPoll);
			}
		}
		
		contadorNumeroPacotes -= pacotesNaEntrega;
		return new Entrega(pedidosEntrega);
	}

	@Override
	public synchronized int getNumeroItens() {
		return pedidos.size();
	}

	@Override
	public void addPropertyChangeListener(PropertyChangeListener l) {
		changes.addPropertyChangeListener(l);
	}

	@Override
	public void removePropertyChangeListener(PropertyChangeListener l) {
		changes.removePropertyChangeListener(l);
	}
}
*/