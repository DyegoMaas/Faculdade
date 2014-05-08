package omp.exemplos.helloworld;

import jomp.runtime.*;
public class Main {

	public static void main(String[] args) {
		int meuId = 0;
		OMP.setNumThreads(15);
		
		//omp parallel private(meuId)
		{
			meuId = OMP.getThreadNum();
			System.out.println("Hello from " + meuId);
		}
	}

}
