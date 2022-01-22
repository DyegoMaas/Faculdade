package bankAccount;

import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantReadWriteLock;

public class BankAccount {
	
	private final ReentrantReadWriteLock readWriteLock = new ReentrantReadWriteLock();
	private final Lock read = readWriteLock.readLock();
	private final Lock write = readWriteLock.writeLock();

	private final Condition temDinheiro = read.newCondition();
	
	private double balance = 0;
	
	public void deposit(double amount){
		write.lock();
		System.out.println("entrou depósito");
		
		try {
			System.out.printf("Depositing %f", amount);
			double newBalance = balance + amount;
			System.out.printf(", new balance is %f\n", newBalance);
			balance = newBalance;
			
			temDinheiro.signalAll();
		} 
		finally {
			write.unlock();
			System.out.println("saiu depósito");
		}
	}
	
	public void withdraw(double amount) throws InterruptedException {
		read.lock();
				
		while(balance < amount)
			temDinheiro.await();
		
		try {
			System.out.printf("Withdrawing %f", amount);
			double newBalance = balance - amount;
			System.out.printf(", new balance is %f\n", newBalance);
			balance = newBalance;
		} finally {
			read.unlock();
		}
	}
	
	public double getBalance(){
		read.lock();
		
		try {
			return balance;
		} finally {
			read.unlock();
		}
	}
}
