package testeOMP;

import jomp.runtime.*;

public class TesteMultiplicacao {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
			
		int[] array = new int[1000];
		for (int i = 0; i < array.length; i++) {
			array[i] = 2;
		}		
		
		int id = 0;
		int i = 0;
		OMP.setNumThreads(array.length / 20);
		//omp parallel private(id) shared(i)
		{
			//omp for
			for (i = 0; i < array.length; i++) {
				array[i] *= 2;
			}
		}
		
		for (i = 0; i < array.length; i++) {			 
			System.out.println("Multiplicacao: " + array[i]);
		}
	}
}
