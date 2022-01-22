package colecoes;

import java.util.Random;

public class ThreadPuts extends Thread {

	private FilaThreadSafe<String> fila;
	
	public ThreadPuts(FilaThreadSafe<String> fila){
		this.fila = fila;
	}
	
	public void run(){		
		Random rand = new Random();
		for	(int i = 0; i < 10; i++){
			try {				
				fila.put("uma string de teste");
				
				System.out.printf("itens INSERIDOS %d\n", i + 1);	
				System.out.printf("fila.count: %d\n", fila.count());
	
				Thread.sleep(rand.nextInt(10));
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
}
