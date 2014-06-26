package matematica;

import Jama.Matrix;

public class CalculadoraMatrizes {
	
	public Matrix multiplicarMatrizes(double[][] matriz1, double[][] matriz2) {
		Matrix matrizUm = new Matrix(matriz1);
		Matrix matrizDois = new Matrix(matriz2);
		
		return matrizUm.times(matrizDois);
	}
}
