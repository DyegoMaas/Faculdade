package bankAccount;

import java.util.concurrent.locks.ReentrantLock;

public class DepositThread extends Thread {

	private static final int REPETITIONS = 100;
	private static final long DELAY = 2;
	private BankAccount account;
	private double amount;
	
	private ReentrantLock lock;

	public DepositThread(BankAccount anAccount, double anAmount) {
		account = anAccount;
		amount = anAmount;
		
	}

	@Override
	public void run() {
		try {
			for (int i = 0; i < REPETITIONS && !isInterrupted(); i++) {
				account.deposit(amount);
				sleep(DELAY);
			}
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
	}
}
