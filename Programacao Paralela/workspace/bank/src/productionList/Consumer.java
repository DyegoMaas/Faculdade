package productionList;


public class Consumer extends Thread {

	private static final int REPETITIONS = 1000;
	private static final long DELAY = 2;
	private Stock stock;

	public Consumer(Stock stock) {
		this.stock = stock;
		
	}
	
	@Override
	public void run() {
		try {
			for (int i = 0; i < REPETITIONS && !isInterrupted(); i++) {
				while(stock.getStackSize() == 0)
					wait();
				System.out.println("Product withdrawn: " + stock.get());
				notifyAll();
				sleep(DELAY);
			}
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
	}
	
}
