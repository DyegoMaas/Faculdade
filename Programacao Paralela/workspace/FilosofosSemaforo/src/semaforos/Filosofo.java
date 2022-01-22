package semaforos;

import java.util.concurrent.Semaphore;
import java.util.concurrent.TimeUnit;

public class Filosofo extends Thread {

	private final Semaphore garfoEsquerda;
	private final Semaphore garfoDireita;
	private final String nome;
	private final int numeroRefeicoes;
	
	private int refeicoesRealizadas;
	
	public Filosofo(String nome, Semaphore garfoEsquerda, Semaphore garfoDireita, int numeroRefeicoes){
		this.garfoEsquerda = garfoEsquerda;
		this.garfoDireita = garfoDireita;
		this.nome = nome;
		this.numeroRefeicoes = numeroRefeicoes;
	}
	
	public void run(){
		while (getRefeicoesRealizadas() < numeroRefeicoes) {
			tentarComer();	
		}		
	}

	private void tentarComer() {
		try {
			if (garfoEsquerda.tryAcquire(100, TimeUnit.MILLISECONDS)){
				System.out.printf("O Filósofo %s pegou o garfo da esquerda.\n", nome);
			
				if (garfoDireita.tryAcquire(100, TimeUnit.MILLISECONDS)){
					System.out.printf("O Filósofo %s pegou o garfo da direita e comeu o espaguete.\n", nome);
					
					sleep((long)(Math.random() * 1000));				
					
					refeicoesRealizadas++;
					garfoDireita.release();
					System.out.printf("O Filósofo %s soltou o garfo da direita.\n", nome);
				}				
				else{
					System.out.printf("O Filósofo %s não conseguiu o garfo da direita e foi pensar.\n", nome);	
				}
				
				garfoEsquerda.release();
				System.out.printf("O Filósofo %s soltou o garfo da esquerda e foi pensar.\n", nome);
			}
			else{
				System.out.printf("O Filósofo %s não conseguiu garfos e foi pensar.\n", nome);
			}
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	public int getRefeicoesRealizadas(){
		return refeicoesRealizadas;
	}
	
	public String getNome(){
		return nome;
	}
}
