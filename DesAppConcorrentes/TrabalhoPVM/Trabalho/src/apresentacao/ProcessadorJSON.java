package apresentacao;

import jpvm.jpvmEnvironment;
import jpvm.jpvmException;
import comunicacao.ComandosProcessamento;
import comunicacao.ComandosResposta;
import comunicacao.Escravo;

public class ProcessadorJSON {
	public static void main(String[] args) throws jpvmException {
		Escravo escravo = new Escravo(ComandosProcessamento.ProcessarJSON, new jpvmEnvironment());

		String respostaJSON = escravo.Receber();
		if(respostaJSON == null)
			return;

		//TODO processar a resposta
		
		escravo.Enviar(ComandosResposta.RespostaJSON, "resposta processamento JSON");
	}
}
