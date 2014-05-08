package omp.exemplos.exemplo3_1;

import java.util.Random;

import jomp.runtime.OMP;
public class Main {

	public static void main(String[] args) {

		int[][] array = arrayValoresAleatorios(100, 25);
		
		int coluna = 0;
		OMP.setNumThreads(15);
		
		int somaTotal = 0;
		//omp parallel private(meuId) reduction(+:somaTotal)
		{
			coluna = OMP.getThreadNum();
			
			int[] linha = array[coluna];
			for (int i = 0; i < linha.length; i++) {
				somaTotal += linha[i];
			}
		}
		
		System.out.println("Soma total: " + somaTotal);
	}

	private static int[][] arrayValoresAleatorios(int colunas, int linhas) {
		int[][] array = new int[colunas][linhas];
		
		int seed = 0;
		for (int x = 0; x < colunas; x++) {
			for (int y = 0; y < linhas; y++) {
				Random random = new Random(seed);
				
				int valorAleatorio = seed = random.nextInt(1000);
				array[x][y] = valorAleatorio;
			}
		}		
		
		return array;
	}

}