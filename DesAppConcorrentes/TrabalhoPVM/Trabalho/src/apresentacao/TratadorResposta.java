package apresentacao;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;

import comunicacao.Resposta;

public class TratadorResposta {

	private File diretorioDestino;

	public TratadorResposta(File diretorioDestino) {
		this.diretorioDestino = diretorioDestino;
	}

	public void Tratar(Resposta resposta) throws Exception {
		// TODO imlememntar maneira de informar nome do arquivo para o escravo
		// para que seja retornado e criado de acordo
		switch (resposta.getTipoResposta()) {
		case RespostaJSON:
			GravarConteudo("nome.json", resposta.getPacote().conteudo);
			break;
		case RespostaXML:
			GravarConteudo("nome.xml", resposta.getPacote().conteudo);
			break;
		default:
			throw new Exception("tipo de resposta não reconhecida: " + resposta.getTipoResposta());
		}
	}

	private void GravarConteudo(String nomeArquivoDestino, String conteudo) throws IOException {
		File arquivoDestino = new File(diretorioDestino, nomeArquivoDestino);
		arquivoDestino.createNewFile();
		
		FileWriter fw = new FileWriter(arquivoDestino.getAbsoluteFile());
		BufferedWriter bw = new BufferedWriter(fw);
		bw.write(conteudo);
		bw.close();
		
		System.out.printf("[MESTRE] Conteudo: %s\n", conteudo);
	}
}
