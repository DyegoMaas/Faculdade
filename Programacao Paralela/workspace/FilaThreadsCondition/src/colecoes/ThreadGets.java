package colecoes;

import java.util.Random;

public class ThreadGets extends Thread{
private FilaThreadSafe<String> fila;
	
	public ThreadGets(FilaThreadSafe<String> fila){
		this.fila = fila;
	}
	
	public void run(){
		Random rand = new Random();
		for	(int i = 0; i < 10; i++){
			try {
				fila.get();
			
				System.out.printf("itens RECUPERADOS %d\n", i + 1);
				System.out.printf("fila.count: %d\n", fila.count());
				
				Thread.sleep(rand.nextInt(100));
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
}
