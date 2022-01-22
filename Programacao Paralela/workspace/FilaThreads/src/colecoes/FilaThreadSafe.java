package colecoes;

import java.util.LinkedList;
import java.util.Queue;

public class FilaThreadSafe<T> {

    private static final int minimoIdeal = 10;
	private Queue<T> fila = new LinkedList<T>();
    private int capacidade;

    public FilaThreadSafe(int capacidade) {
        this.capacidade = capacidade;
    }
    
    private boolean liberandoEspaco;

    public synchronized void put(T item) throws InterruptedException {
        while(fila.size() == capacidade) {
        	if(!liberandoEspaco)
        		liberandoEspaco = true;
        	
            wait();
        }
        
        if(liberandoEspaco)
        	if(fila.size() >= minimoIdeal)
        		wait();
        	else
        		liberandoEspaco = false;

        fila.add(item);
        notifyAll();
    }

    public synchronized T get() throws InterruptedException {
        while(fila.isEmpty()) {
            wait();
        }

        T item = fila.remove();
        notifyAll();
        return item;
    }
    
    public synchronized int count(){
    	return fila.size();
    }
}