package bankAccount;

public class BankAccount {

	private double balance = 0;
	
	public synchronized void deposit(double amount){
		System.out.printf("Depositing %f", amount);
		double newBalance = balance + amount;
		System.out.printf(", new balance is %f\n", newBalance);
		balance = newBalance;
		notifyAll();
	}
	
	public synchronized void withdraw(double amount) throws InterruptedException {
		while(balance < amount)
			wait();
		System.out.printf("Withdrawing %f", amount);
		double newBalance = balance - amount;
		System.out.printf(", new balance is %f\n", newBalance);
		balance = newBalance;
	}
	
	public double getBalance(){
		return balance;
	}
}
