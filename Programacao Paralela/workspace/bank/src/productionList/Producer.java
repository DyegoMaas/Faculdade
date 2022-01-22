package productionList;

import java.util.Date;

public class Producer extends Thread {

	private static final int REPETITIONS = 1000;
	private static final long DELAY = 2;
	private Stock stock;

	public Producer(Stock stock) {
		this.stock = stock;
		
	}
	
	@Override
	public void run() {
		try {
			for (int i = 0; i < REPETITIONS && !isInterrupted(); i++) {
				while(stock.getStackSize() >= 10)
					wait();
				stock.put(new Date(System.currentTimeMillis()).toString());
				notifyAll();
				if(stock.getStackSize() == 20)
					wait();
				sleep(DELAY);
			}
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
	}
}
