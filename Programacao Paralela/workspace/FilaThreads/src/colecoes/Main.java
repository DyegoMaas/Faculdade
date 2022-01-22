package colecoes;

public class Main {

	public static void main(String[] args) throws InterruptedException {
		FilaThreadSafe<String> pilha = new FilaThreadSafe<String>(20);
		
		ThreadGets threadGets = new ThreadGets(pilha);
		ThreadPuts threadPuts = new ThreadPuts(pilha);
		
		threadGets.start();		
		threadPuts.start();
				
		threadGets.join();		
		threadPuts.join();
		
		System.out.println("acabou");
	}

}
