package apresentacao;

import jpvm.jpvmEnvironment;
import jpvm.jpvmException;
import comunicacao.ComandosProcessamento;
import comunicacao.ComandosResposta;
import comunicacao.Escravo;
import comunicacao.pacotes.Pacote;

public class ProcessadorJSON {
	public static void main(String[] args) throws jpvmException, Exception {
		Escravo escravo = new Escravo(ComandosProcessamento.ProcessarJSON, new jpvmEnvironment());

		Pacote pacote = escravo.Receber();
		if(pacote == null)
			return;

		//TODO processar a entrada
		
		escravo.Enviar(ComandosResposta.RespostaJSON, new Pacote());
	}
}
