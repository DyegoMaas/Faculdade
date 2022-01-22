package colecoes;

public class ThreadGets extends Thread{
private FilaThreadSafe<String> fila;
	
	public ThreadGets(FilaThreadSafe<String> fila){
		this.fila = fila;
	}
	
	public void run(){
		System.out.println("y");
		for	(int i = 0; i < 1000 && !isInterrupted(); i++){
			try {
				fila.get();
			
				System.out.printf("itens RECUPERADOS %d\n", i + 1);
				System.out.printf("fila.count: %d\n", fila.count());
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
}
