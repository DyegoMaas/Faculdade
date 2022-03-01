package L1211G09;

import java.util.Date;
import java.util.Observable;
import java.util.Random;

public class Produtor extends Observable implements Runnable{

	private int numeroDescritor, ultimoProduto, totalProduzido, tempoProducao;
	private boolean processando;
	
	public Produtor(int numeroDescritor)
	{
		this.numeroDescritor = numeroDescritor;		
		this.processando = true;
	}
	
	public int getUltimoProduto() {
		return ultimoProduto;
	}

	public int getTotalProduzido() {
		return totalProduzido;
	}
	
	public int getNumero() {
		return numeroDescritor;
	}
		
	public int getTempoProducao() {
		return tempoProducao;
	}

	private void setUltimoProduto(int ultimoProduto) {
		this.ultimoProduto = ultimoProduto;
		
		setChanged();
		notifyObservers();
	}
	
	public void Stop() {
		processando = false;
	}
	
	@Override
	public void run() {
		while(processando) {
			int numeroGerado = gerarNumero();
			
			try {
				Thread.sleep(numeroGerado - 40);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
			}
					
			this.tempoProducao++;
			totalProduzido += numeroGerado;
			setUltimoProduto(numeroGerado);
			
			Armazem armazem = Armazem.getInstance();
			armazem.P();
			armazem.inserir(numeroGerado);
			armazem.V();
		}
	}
	
	private int gerarNumero() {
		Random random = new Random(new Date().getTime());
		return random.nextInt(251) + 50;
	}

}
