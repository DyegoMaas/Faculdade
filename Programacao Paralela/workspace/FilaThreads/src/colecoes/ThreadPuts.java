package colecoes;

public class ThreadPuts extends Thread {

	private FilaThreadSafe<String> fila;
	
	public ThreadPuts(FilaThreadSafe<String> fila){
		this.fila = fila;
	}
	
	public void run(){		
		for	(int i = 0; i < 1000 && !isInterrupted(); i++){
			try {				
				fila.put("uma string de teste");
				
				System.out.printf("itens INSERIDOS %d\n", i + 1);	
				System.out.printf("fila.count: %d\n", fila.count());
	
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
}
