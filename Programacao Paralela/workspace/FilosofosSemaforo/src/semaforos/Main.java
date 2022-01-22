package semaforos;

import java.util.concurrent.Semaphore;

public class Main {

	public static void main(String[] args) throws InterruptedException {
		Semaphore[] garfos = new Semaphore[]{
			new Semaphore(1),
			new Semaphore(1),
			new Semaphore(1),
			new Semaphore(1),
			new Semaphore(1)				
		};	
		
		final int refeicoes = 10;
		Filosofo[] filosofos = new Filosofo[]{
			new Filosofo("1", garfos[0], garfos[1], refeicoes),
			new Filosofo("2", garfos[1], garfos[2], refeicoes),
			new Filosofo("3", garfos[2], garfos[3], refeicoes),
			new Filosofo("4", garfos[3], garfos[4], refeicoes),
			new Filosofo("5", garfos[4], garfos[0], refeicoes)
		};
		
		for (Filosofo filosofo : filosofos) {
			filosofo.start();
		}
		
		for (Filosofo filosofo : filosofos) {
			filosofo.join();
		}
		
		System.out.println("");
		for (Filosofo filosofo : filosofos) {
			System.out.printf("O filósofo %s fez %d refeições.\n", filosofo.getNome(), filosofo.getRefeicoesRealizadas());
		}
	}

}
