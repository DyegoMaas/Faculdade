package apresentacao;

import Jama.Matrix;

import com.thoughtworks.xstream.XStream;

import comunicacao.ComandosProcessamento;
import comunicacao.ComandosResposta;
import comunicacao.Escravo;
import comunicacao.pacotes.Pacote;
import comunicacao.pacotes.matrizes.MatrizesProcessar;
import comunicacao.pacotes.matrizes.MatrizesReposta;
import jpvm.jpvmEnvironment;
import jpvm.jpvmException;

public class ProcessadorXML {

	public static void main(String[] args) throws jpvmException {
		Escravo escravo = new Escravo(ComandosProcessamento.ProcessarXML, new jpvmEnvironment());

		String respostaXML = escravo.Receber();
		if(respostaXML == null)
			return;
		/*
		Pacote pacote = new Pacote();
		 
		XStream xstream = new XStream();
		MatrizesProcessar pacotinho = (MatrizesProcessar)xstream.fromXML(pacote.conteudo);
		
		Matrix matriz1 = new Matrix(pacotinho.matriz1);
		Matrix matriz2 = new Matrix(pacotinho.matriz2);
		Matrix matrizResultante = matriz1.times(matriz2);
		
		MatrizesReposta resposta = new MatrizesReposta();
		resposta.matriz = matrizResultante.getArray();
		
		
		Pacote pacoteResposta = new Pacote();
		String xml = xstream.toXML(pacoteResposta);
		escravo.Enviar(ComandosResposta.RespostaXML, xml);
		*/
		escravo.Enviar(ComandosResposta.RespostaXML, "respot");
	}
}
