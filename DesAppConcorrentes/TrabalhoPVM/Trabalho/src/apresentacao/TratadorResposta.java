package apresentacao;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;

import comunicacao.Resposta;
import comunicacao.pacotes.Pacote;

public class TratadorResposta {

	private File diretorioDestino;

	public TratadorResposta(File diretorioDestino) {
		this.diretorioDestino = diretorioDestino;
	}

	public void Tratar(Resposta resposta) throws Exception {
		Pacote pacote = resposta.getPacote();
		
		switch (resposta.getTipoResposta()) {
		case RespostaJSON:
		case RespostaXML:
			GravarConteudo(pacote.cabecalho.nomeArquivo, resposta.getPacote().conteudo);
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
