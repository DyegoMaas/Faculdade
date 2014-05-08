package omp.exemplos.exemplo3_1;

import java.util.Random;

import jomp.runtime.OMP;
public class Main_jomp {


	public static void main(String[] args) {

		int[][] array = arrayValoresAleatorios(100, 25);
		
		int coluna = 0;
		OMP.setNumThreads(15);
		
		int somaTotal = 0;

// OMP PARALLEL BLOCK BEGINS
{
  __omp_Class0 __omp_Object0 = new __omp_Class0();
  // shared variables
  __omp_Object0.coluna = coluna;
  __omp_Object0.array = array;
  __omp_Object0.args = args;
  // firstprivate variables
  try {
    jomp.runtime.OMP.doParallel(__omp_Object0);
  } catch(Throwable __omp_exception) {
    System.err.println("OMP Warning: Illegal thread exception ignored!");
    System.err.println(__omp_exception);
  }
  // reduction variables
  somaTotal  += __omp_Object0._rd_somaTotal;
  // shared variables
  coluna = __omp_Object0.coluna;
  array = __omp_Object0.array;
  args = __omp_Object0.args;
}
// OMP PARALLEL BLOCK ENDS

		
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

// OMP PARALLEL REGION INNER CLASS DEFINITION BEGINS
private static class __omp_Class0 extends jomp.runtime.BusyTask {
  // shared variables
  int coluna;
  int [ ] [ ] array;
  String [ ] args;
  // firstprivate variables
  // variables to hold results of reduction
  int _rd_somaTotal;

  public void go(int __omp_me) throws Throwable {
  // firstprivate variables + init
  // private variables
  // reduction variables, init to default
  int somaTotal = 0;
    // OMP USER CODE BEGINS

		{
			coluna = OMP.getThreadNum();
			
			int[] linha = array[coluna];
			for (int i = 0; i < linha.length; i++) {
				somaTotal += linha[i];
			}
		}
    // OMP USER CODE ENDS
  // call reducer
  somaTotal = (int) jomp.runtime.OMP.doPlusReduce(__omp_me, somaTotal);
  // output to _rd_ copy
  if (jomp.runtime.OMP.getThreadNum(__omp_me) == 0) {
    _rd_somaTotal = somaTotal;
  }
  }
}
// OMP PARALLEL REGION INNER CLASS DEFINITION ENDS

}

