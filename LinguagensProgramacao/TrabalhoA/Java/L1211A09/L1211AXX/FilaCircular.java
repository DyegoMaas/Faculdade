package L1211AXX;

public class FilaCircular<T> {

	private T[] array = null; 
	private int tamanho = 0, tamanhoMax;
	private int cauda = 0, cabeca = 0;
	
	public FilaCircular(int pTamanho) {
		tamanhoMax = pTamanho;
	}
	
	@SuppressWarnings("unchecked")
	public void inserir(T pValor) {
		if(array == null)
			array = (T[])java.lang.reflect.Array.newInstance(pValor.getClass(), tamanhoMax);
		
		if(tamanho == tamanhoMax)
		{
			System.out.println("A fila está cheia");
			return;
		}
		
		array[cauda] = pValor;
		cauda = (cauda + 1) % tamanhoMax;
		tamanho++;		
	}
	
	public T remover() {
		if(tamanho <= 0)
			return null;
		
		T removido = array[cabeca];
		cabeca = (cabeca + 1) % tamanhoMax;
		tamanho--;
		
		return removido;		
	}
	
	public boolean criada() {
		return array != null;
	}
	
	public int getTamanho() {
		return tamanhoMax;
	}
	
	public int getTotalElementos() {
		return tamanho;
	}
	
	public String toString() {
		if(!criada())
			return "";
		
		StringBuffer buffer = new StringBuffer();
		
		int pos = cabeca;
		while(pos != cauda) {
			buffer.append(array[pos]).append(", ");
			
			pos = (pos + 1) % tamanhoMax;
		}		
		
		return buffer.toString();
	}
}
