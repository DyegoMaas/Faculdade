package apresentacao;

import Jama.Matrix;

import com.thoughtworks.xstream.XStream;

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

	public static void main(String[] args) throws jpvmException {
		Escravo escravo = new Escravo(ComandosProcessamento.ProcessarXML, new jpvmEnvironment());

		//retorna um Pacote???
		String respostaXML = escravo.Receber();
		if(respostaXML == null)
			return;
		
		escravo.Enviar(ComandosResposta.RespostaXML, "resposta");
		
		
//		Pacote pacote = new Pacote();
//		
//		ObjectSerializationToXML serializador = new ObjectSerializationToXML();
//		MatrizesProcessar pacotinho = null;
//		try {
//			pacotinho = (MatrizesProcessar)serializador.fromXML(pacote.conteudo);
//		} catch (Exception e1) {
//			// TODO Auto-generated catch block
//			e1.printStackTrace();
//		}
//		
////		XStream xstream = new XStream();
////		MatrizesProcessar pacotinho = (MatrizesProcessar)xstream.fromXML(pacote.conteudo);
//		
//		Matrix matriz1 = new Matrix(pacotinho.matriz1);
//		Matrix matriz2 = new Matrix(pacotinho.matriz2);
//		Matrix matrizResultante = matriz1.times(matriz2);
//		
//		MatrizesReposta resposta = new MatrizesReposta();
//		resposta.matriz = matrizResultante.getArray();
//		
//		
//		Pacote pacoteResposta = new Pacote();
//		
//		try {
//			String xml = serializador.toXML(pacoteResposta);
//			pacoteResposta.conteudo = xml;
//			escravo.Enviar(ComandosResposta.RespostaXML, xml);
//		} catch (Exception e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		}		
	}
}
