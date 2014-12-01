package arquivamento;

import javax.xml.ws.Endpoint;

public class Publisher {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		ServicoArquivamento servicoArquivamento = new ServicoArquivamento();
		Endpoint endpoint = Endpoint.publish("http://localhost:8080/servicoarquivamento", servicoArquivamento);
	}

}
