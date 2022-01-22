package productionList;

public class Main {

	public static void main(String args[]) throws InterruptedException {
		Stock stock = new Stock();
		Consumer consumer = new Consumer(stock);
		Producer produtor = new Producer(stock);
		
		consumer.start();
		produtor.start();
		
		consumer.join();
		produtor.join();
		
		System.out.println("Saldo final: " + stock.getStackSize());

	}
}
