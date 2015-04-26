package omp.exemplos.exemplo3_3;

import java.util.Random;

import jomp.runtime.OMP;

public class Main_jomp {


	public static void main(String[] args) {

		int[][] array = arrayValoresAleatorios(100, 25);
	
		OMP.setNumThreads(15);
		
		int somaTotal = 0;
		int coluna = 0;

// OMP PARALLEL BLOCK BEGINS
{
  __omp_Class0 __omp_Object0 = new __omp_Class0();
  // shared variables
  __omp_Object0.somaTotal = somaTotal;
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
  // shared variables
  somaTotal = __omp_Object0.somaTotal;
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
  int somaTotal;
  int [ ] [ ] array;
  String [ ] args;
  // firstprivate variables
  // variables to hold results of reduction

  public void go(int __omp_me) throws Throwable {
  // firstprivate variables + init
  // private variables
  int coluna;
  // reduction variables, init to default
    // OMP USER CODE BEGINS

                          { // OMP FOR BLOCK BEGINS
                          // copy of firstprivate variables, initialized
                          // copy of lastprivate variables
                          // variables to hold result of reduction
                          int _cp_somaTotal;
                          boolean amLast=false;
                          {
                            // firstprivate variables + init
                            // [last]private variables
                            // reduction variables + init to default
                            int  somaTotal = 0;
                            // -------------------------------------
                            jomp.runtime.LoopData __omp_WholeData2 = new jomp.runtime.LoopData();
                            jomp.runtime.LoopData __omp_ChunkData1 = new jomp.runtime.LoopData();
                            __omp_WholeData2.start = (long)( 0);
                            __omp_WholeData2.stop = (long)( array.length);
                            __omp_WholeData2.step = (long)(1);
                            jomp.runtime.OMP.setChunkStatic(__omp_WholeData2);
                            while(!__omp_ChunkData1.isLast && jomp.runtime.OMP.getLoopStatic(__omp_me, __omp_WholeData2, __omp_ChunkData1)) {
                            for(;;) {
                              if(__omp_WholeData2.step > 0) {
                                 if(__omp_ChunkData1.stop > __omp_WholeData2.stop) __omp_ChunkData1.stop = __omp_WholeData2.stop;
                                 if(__omp_ChunkData1.start >= __omp_WholeData2.stop) break;
                              } else {
                                 if(__omp_ChunkData1.stop < __omp_WholeData2.stop) __omp_ChunkData1.stop = __omp_WholeData2.stop;
                                 if(__omp_ChunkData1.start > __omp_WholeData2.stop) break;
                              }
                              for( coluna = (int)__omp_ChunkData1.start; coluna < __omp_ChunkData1.stop; coluna += __omp_ChunkData1.step) {
                                // OMP USER CODE BEGINS
 {
			for (int linha = 0; linha < array[coluna].length; linha++) {
				somaTotal += array[coluna][linha];	
			}			
		}
                                // OMP USER CODE ENDS
                                if (coluna == (__omp_WholeData2.stop-1)) amLast = true;
                              } // of for 
                              if(__omp_ChunkData1.startStep == 0)
                                break;
                              __omp_ChunkData1.start += __omp_ChunkData1.startStep;
                              __omp_ChunkData1.stop += __omp_ChunkData1.startStep;
                            } // of for(;;)
                            } // of while
                            // call reducer
                            _cp_somaTotal = (int) jomp.runtime.OMP.doPlusReduce(__omp_me, somaTotal);
                            jomp.runtime.OMP.doBarrier(__omp_me);
                            // copy lastprivate variables out
                            if (amLast) {
                            }
                          }
                          // set global from lastprivate variables
                          if (amLast) {
                          }
                          // set global from reduction variables
                          if (jomp.runtime.OMP.getThreadNum(__omp_me) == 0) {
                            somaTotal+= _cp_somaTotal;
                          }
                          } // OMP FOR BLOCK ENDS

    // OMP USER CODE ENDS
  // call reducer
  // output to _rd_ copy
  if (jomp.runtime.OMP.getThreadNum(__omp_me) == 0) {
  }
  }
}
// OMP PARALLEL REGION INNER CLASS DEFINITION ENDS

}

