package apresentacao;

import com.google.gson.Gson;

import matematica.CalculadoraMatrizes;
import comunicacao.ComandosProcessamento;
import comunicacao.ComandosResposta;
import comunicacao.Escravo;
import comunicacao.pacotes.Pacote;
import comunicacao.pacotes.matrizes.MatrizResposta;
import comunicacao.pacotes.matrizes.MatrizesProcessar;
import comunicacao.utils.ObjectSerializationToXML;
import jpvm.jpvmEnvironment;
import jpvm.jpvmException;

public class ProcessadorXML {

	public static void main(String[] args) throws jpvmException, Exception {
		Escravo escravo = new Escravo(ComandosProcessamento.ProcessarXML, new jpvmEnvironment());

		Pacote pacoteRecebido = escravo.Receber();
		if(pacoteRecebido == null)
			return;
				
		ObjectSerializationToXML serializador = new ObjectSerializationToXML();
		MatrizesProcessar processar = (MatrizesProcessar)serializador.fromXML(pacoteRecebido.conteudo);
						
		CalculadoraMatrizes calculadora = new CalculadoraMatrizes();		
		MatrizResposta matrizResultante = calculadora.multiplicarMatrizes(processar.matriz1, processar.matriz2);		
		
		Pacote pacoteResposta = new Pacote();
		pacoteResposta.cabecalho = pacoteRecebido.cabecalho;
		pacoteResposta.conteudo = serializador.toXML(matrizResultante);		
		
		escravo.enviar(ComandosResposta.RespostaXML, pacoteResposta);
		escravo.finalizar();
	}
}
