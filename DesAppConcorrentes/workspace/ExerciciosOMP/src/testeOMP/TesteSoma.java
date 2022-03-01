package testeOMP;

import jomp.runtime.*;

public class TesteSoma {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		int somaCorreta = 0;
		int[] array = new int[50];
		for (int i = 0; i < array.length; i++) {
			array[i] = i * 2;
			somaCorreta += array[i];
		}
		
		int myid, soma = 0;
		OMP.setNumThreads(array.length);
		
		//omp parallel private(mydid) reduction(+:soma) 
		{					
			myid = OMP.getThreadNum();
			soma = array[myid];		
			System.out.println("Eu (" + myid + ") somei " + array[myid]);
		}
		
		System.out.println("Soma: " + soma + ", somaCorreta: " + somaCorreta);
	}
}
