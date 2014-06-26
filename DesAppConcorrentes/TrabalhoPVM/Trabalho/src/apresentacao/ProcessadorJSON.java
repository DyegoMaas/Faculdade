package apresentacao;

import matematica.CalculadoraMatrizes;
import comunicacao.pacotes.matrizes.MatrizResposta;

import com.google.gson.Gson;

import jpvm.jpvmEnvironment;
import jpvm.jpvmException;
import comunicacao.ComandosProcessamento;
import comunicacao.ComandosResposta;
import comunicacao.Escravo;
import comunicacao.pacotes.Pacote;
import comunicacao.pacotes.matrizes.MatrizesProcessar;

public class ProcessadorJSON {
	public static void main(String[] args) throws jpvmException, Exception {
		Escravo escravo = new Escravo(ComandosProcessamento.ProcessarJSON, new jpvmEnvironment());

		Pacote pacoteRecebido = escravo.Receber();		
		if(pacoteRecebido == null)
			return;		

//		try{			
//			Gson gson = new Gson();
//			escravo.Enviar(ComandosResposta.RespostaJSON, dummy("x"));//TODO remover
//		}catch(Exception e){
//			escravo.Enviar(ComandosResposta.RespostaJSON, dummy(e.getMessage()));//TODO remover
//		}
		
		Gson gson = new Gson();		
		MatrizesProcessar matrizesProcessar = gson.fromJson(pacoteRecebido.conteudo, MatrizesProcessar.class);		

		CalculadoraMatrizes calculadora = new CalculadoraMatrizes();
		MatrizResposta matrizResposta = calculadora.multiplicarMatrizes(matrizesProcessar.matriz1, matrizesProcessar.matriz2);
		
		String matrizRespostaString = gson.toJson(matrizResposta);

		Pacote pacoteResposta = new Pacote();
		pacoteResposta.cabecalho = pacoteRecebido.cabecalho;
		pacoteResposta.conteudo = matrizRespostaString;

		escravo.Enviar(ComandosResposta.RespostaJSON, pacoteResposta);
	}

//	private static Pacote dummy(String msg) {
//		Pacote p = new Pacote();
//		p.conteudo = msg;
//		return p;
//	}
}
