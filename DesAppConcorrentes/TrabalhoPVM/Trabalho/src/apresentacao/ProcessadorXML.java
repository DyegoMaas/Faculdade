package apresentacao;

import comunicacao.ComandosProcessamento;
import comunicacao.ComandosResposta;
import comunicacao.Escravo;
import jpvm.jpvmEnvironment;
import jpvm.jpvmException;

public class ProcessadorXML {

	public static void main(String[] args) throws jpvmException {
		Escravo escravo = new Escravo(ComandosProcessamento.ProcessarXML, new jpvmEnvironment());

		String respostaXML = escravo.Receber();
		if(respostaXML == null)
			return;
		
		//processar a resposta
		
		escravo.Enviar(ComandosResposta.RespostaXML, "resposta processamento XML");
	}

}
