package L1211G09;

public class Armazem {

	private static Armazem instance = null;
	
	private int[] armazem = null;
	private int value = 0;
	
	public static void create(int size) {
		instance = new Armazem(size);
	}
	
	public static Armazem getInstance() {
		return instance;
	}
	
	private Armazem(int tamanho) {
		armazem = new int[tamanho];
		value = 0;
	}
	
	public synchronized void P() {
	      value--;
	      if (value < 0) {
	         while (true) {     
	            try {
	               wait();
	               break;       
	            } 
	            catch (InterruptedException e) {
	               if (value >= 0) 
	            	   break; 
	               else 
	            	   continue;         
	            }
	         }
	      }
	   }

	   public synchronized void V() { 
	      value++;                    
	      if (value <= 0) 
	    	  notify();   
	   }                              

	   public synchronized int value() {
	      return value;
	   }
	   
	   public void inserir(int v) {
		   armazem[0] = v;
	   }

	   public synchronized String toString() {
	      return String.valueOf(value);
	   }

	   public synchronized void tryP() throws Exception {
	      if (value > 0) 
	    	  this.P();  
	      else 
	    	  throw new Exception();
	   }

	   public synchronized void interruptibleP() throws InterruptedException {
	      value--;
	      if (value < 0) {
	         try { wait(); }
	         catch (InterruptedException e) {
	            value++;      
	            throw e;
	         }
	      }
	   }
}
