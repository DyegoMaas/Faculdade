package colecoes;

import java.util.LinkedList;
import java.util.Queue;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;
import java.util.concurrent.locks.ReentrantReadWriteLock;

public class FilaThreadSafe<T> {

	private final ReentrantLock lock = new ReentrantLock();

	private final Condition podeRetirar = lock.newCondition();
	private final Condition podeInserir = lock.newCondition();
	
	
	private Queue<T> fila = new LinkedList<T>();
    private int capacidade;

    public FilaThreadSafe(int capacidade) {
        this.capacidade = capacidade;
    }
    
    public void put(T item) throws InterruptedException {
    	lock.lock();
    	
    	while(fila.size() == capacidade)        	
            podeInserir.await();
        
    	try {
            fila.add(item);
            podeRetirar.signalAll();
    		
    	} finally {
    		lock.unlock();    		
    	}    	
    }

    public T get() throws InterruptedException {
    	lock.lock();
    	
        while(fila.isEmpty()) 
            podeRetirar.await();
        
        try {
        	return fila.remove();        	
		} finally {
			podeInserir.signalAll();
			lock.unlock();
		}        
    }
    
    public int count(){
    	lock.lock();
    	
        try {
        	return fila.size();
		} finally {
			lock.unlock();
		}
    }
}