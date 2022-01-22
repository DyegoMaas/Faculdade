package bankAccount;

public class Main {
	
	public static void main(String args[]) throws InterruptedException {
		BankAccount anAccount = new BankAccount();
		
		double anAmount = 10;
		WithdrawThread withdrawThread = new WithdrawThread(anAccount, anAmount);
		DepositThread depositThread = new DepositThread(anAccount, anAmount);
		
		withdrawThread.start();
		depositThread.start();
		
		withdrawThread.join();
		depositThread.join();
		
		System.out.println(anAccount.getBalance());
	}

}
