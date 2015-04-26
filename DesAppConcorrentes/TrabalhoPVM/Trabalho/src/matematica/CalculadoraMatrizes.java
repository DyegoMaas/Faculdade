package matematica;

import comunicacao.pacotes.matrizes.MatrizResposta;
import Jama.Matrix;

public class CalculadoraMatrizes {
	
	public MatrizResposta multiplicarMatrizes(double[][] matriz1, double[][] matriz2) {
		Matrix matrizUm = new Matrix(matriz1);
		Matrix matrizDois = new Matrix(matriz2);
		
		MatrizResposta resposta = new MatrizResposta();
		resposta.matriz = matrizUm.times(matrizDois).getArray(); 

		return resposta;
	}
}
