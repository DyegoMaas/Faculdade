package apresentacao;

import matematica.CalculadoraMatrizes;
import Jama.Matrix;
import comunicacao.ComandosProcessamento;
import comunicacao.ComandosResposta;
import comunicacao.Escravo;
import comunicacao.pacotes.Pacote;
import comunicacao.pacotes.matrizes.MatrizesProcessar;
import comunicacao.pacotes.matrizes.MatrizesReposta;
import comunicacao.utils.ObjectSerializationToXML;
import jpvm.jpvmEnvironment;
import jpvm.jpvmException;

public class ProcessadorXML {

	public static void main(String[] args) throws jpvmException, Exception {
		Escravo escravo = new Escravo(ComandosProcessamento.ProcessarXML, new jpvmEnvironment());

		Pacote pacote = escravo.Receber();
		if(pacote == null)
			return;
		
		ObjectSerializationToXML serializador = new ObjectSerializationToXML();
		MatrizesProcessar processar = (MatrizesProcessar)serializador.fromXML(pacote.conteudo);
				
		Matrix matrizResultante = multiplicarMatrizes(processar);
					
		MatrizesReposta resposta = new MatrizesReposta();
		resposta.matriz = matrizResultante.getArray();	
		
		Pacote pacoteResposta = serializarResposta(resposta);
		escravo.Enviar(ComandosResposta.RespostaXML, pacoteResposta);	
	}

	private static Matrix multiplicarMatrizes(MatrizesProcessar processar) {
		CalculadoraMatrizes calculadora = new CalculadoraMatrizes();
		return calculadora.multiplicarMatrizes(processar.matriz1, processar.matriz2);
	}

	private static Pacote serializarResposta(Object objeto) throws Exception {		
		ObjectSerializationToXML serializador = new ObjectSerializationToXML();
		String xml = serializador.toXML(objeto);
		
		Pacote pacote = new Pacote();
		pacote.conteudo = xml;
		
		return pacote;
	}
}
