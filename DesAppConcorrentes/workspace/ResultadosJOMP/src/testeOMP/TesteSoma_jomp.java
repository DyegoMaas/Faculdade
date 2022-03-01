package testeOMP;

import jomp.runtime.*;

public class TesteSoma_jomp {


//	/**
//	 * @param args
//	 */
//	public static void main(String[] args) {						
//		int somaCorreta = 0;
//		int[] array = new int[50];
//		for (int i = 0; i < array.length; i++) {
//			array[i] = i * 2;
//			somaCorreta += array[i];
//		}
//		
//		int myid, soma = 0;
//		OMP.setNumThreads(50);
//
//// OMP PARALLEL BLOCK BEGINS
//{
//  __omp_Class0 __omp_Object0 = new __omp_Class0();
//  // shared variables
//  __omp_Object0.array = array;
//  __omp_Object0.somaCorreta = somaCorreta;
//  __omp_Object0.args = args;
//  // firstprivate variables
//  try {
//    jomp.runtime.OMP.doParallel(__omp_Object0);
//  } catch(Throwable __omp_exception) {
//    System.err.println("OMP Warning: Illegal thread exception ignored!");
//    System.err.println(__omp_exception);
//  }
//  // reduction variables
//  soma  += __omp_Object0._rd_soma;
//  // shared variables
//  myid = __omp_Object0.myid;
//  array = __omp_Object0.array;
//  somaCorreta = __omp_Object0.somaCorreta;
//  args = __omp_Object0.args;
//}
//// OMP PARALLEL BLOCK ENDS
//
//		
//		int sum2 = 0;
//		for (int i = 0; i < array.length; i++) {
//			sum2 += array[i];
//		}
//		
//		System.out.println("Soma: " + soma + ", somaCorreta: " + somaCorreta);
//	}
//
//// OMP PARALLEL REGION INNER CLASS DEFINITION BEGINS
//private static class __omp_Class0 extends jomp.runtime.BusyTask {
//  // shared variables
//  int myid;
//  int [ ] array;
//  int somaCorreta;
//  String [ ] args;
//  // firstprivate variables
//  // variables to hold results of reduction
//  int _rd_soma;
//
//  public void go(int __omp_me) throws Throwable {
//  // firstprivate variables + init
//  // private variables
//  // reduction variables, init to default
//  int soma = 0;
//    // OMP USER CODE BEGINS
// 
//		{					
//			myid = OMP.getThreadNum();
//			soma = array[myid];		
//			System.out.println("Eu (" + myid + ") somei " + array[myid]);
//		}
//    // OMP USER CODE ENDS
//  // call reducer
//  soma = (int) jomp.runtime.OMP.doPlusReduce(__omp_me, soma);
//  // output to _rd_ copy
//  if (jomp.runtime.OMP.getThreadNum(__omp_me) == 0) {
//    _rd_soma = soma;
//  }
//  }
//}
//// OMP PARALLEL REGION INNER CLASS DEFINITION ENDS

}

