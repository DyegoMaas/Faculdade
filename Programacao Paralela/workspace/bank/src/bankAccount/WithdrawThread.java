package bankAccount;

public class WithdrawThread extends Thread {

	private static final int REPETITIONS = 100;
	private static final long DELAY = 2;
	private BankAccount account;
	private double amount;

	public WithdrawThread(BankAccount anAccount, double anAmount) {
		this.account = anAccount;
		this.amount = anAmount;
	}

	@Override
	public void run() {
		try {
			for (int i = 0; i < REPETITIONS && !isInterrupted(); i++) {
				account.withdraw(amount);
				sleep(DELAY);
			}
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
	}
	
}
