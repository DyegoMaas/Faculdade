package apresentacao;

import java.io.BufferedReader;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.io.ObjectOutput;
import java.io.ObjectOutputStream;
import java.util.ArrayList;
import java.util.List;
import jpvm.jpvmEnvironment;
import jpvm.jpvmException;
import comunicacao.ComandosProcessamento;
import comunicacao.Mestre;
import comunicacao.Resposta;
import comunicacao.pacotes.Cabecalho;
import comunicacao.pacotes.matrizes.MatrizesProcessar;
import comunicacao.pacotes.Pacote;

public class ProcessadorGrafos {

	private static String caminhoDiretorioEntrada = "C:\\pvm\\entrada";
	private static String caminhoDiretorioSaida = "C:\\pvm\\saida";

	public static void main(String[] args) throws Exception, jpvmException,
			IOException {
		File diretorioEntrada = new File(caminhoDiretorioEntrada);
		final File[] arquivosParaProcessar = diretorioEntrada.listFiles();

		if (arquivosParaProcessar == null) {
			System.out.println("Nenhum arquivo para processar");
			return;
		}

		final Mestre mestre = construirMestre(arquivosParaProcessar);

		Thread processadorEntrada = new Thread(new Runnable() {
			@Override
			public void run() {
				for (File file : arquivosParaProcessar) {
					try {
						ComandosProcessamento comando = ComandosProcessamento
								.getComandoPorExtensao(getExtensao(file));

						Pacote pacote = new Pacote();
						pacote.cabecalho = new Cabecalho();
						pacote.cabecalho.nomeArquivo = file.getName();
						pacote.conteudo = getConteudo(file);

						byte[] bytesPacote = prepararPacoteParaEnvio(pacote);
						// empacotar conteudo arquivo

						mestre.Enviar(comando, bytesPacote);

					} catch (Exception | jpvmException e) {
						e.printStackTrace();
					}
				}
			}
		});

		final TratadorResposta tratadorResposta = new TratadorResposta(
				new File(caminhoDiretorioSaida));
		Thread processadorSaida = new Thread(new Runnable() {
			@Override
			public void run() {
				for (int i = 0; i < arquivosParaProcessar.length; i++) {
					try {
						Resposta resposta = mestre.Receber();
						tratadorResposta.Tratar(resposta);
					} catch (jpvmException | Exception e) {
						e.printStackTrace();
					}
				}
			}
		});

		processadorEntrada.start();
		processadorSaida.start();
	}

	private static Mestre construirMestre(final File[] arquivosParaProcessar)
			throws jpvmException, IOException, Exception {
		final Mestre mestre = new Mestre(new jpvmEnvironment());

		List<Configuracao> configuracoes = obterConfiguracoes(arquivosParaProcessar);
		for (Configuracao configuracao : configuracoes) {
			mestre.Adicionar(configuracao.getComando(),
					configuracao.getNumTarefas());
		}

		return mestre;
	}

	private static String getExtensao(File arquivo) throws IOException {
		String caminho = arquivo.getCanonicalPath();
		return caminho.substring(caminho.lastIndexOf('.') + 1);
	}

	private static String getConteudo(File arquivo) throws IOException {
		StringBuffer buffer = new StringBuffer();

		BufferedReader br = new BufferedReader(new FileReader(arquivo));
		while (br.ready()) {
			buffer.append(br.readLine());
		}
		br.close();

		return buffer.toString();
	}

	private static List<Configuracao> obterConfiguracoes(File[] arquivos) throws IOException, Exception {
		ArrayList<Configuracao> configuracoes = new ArrayList<Configuracao>();

		Configuracao configXml = new Configuracao(
				ComandosProcessamento.ProcessarXML);
		Configuracao configJson = new Configuracao(
				ComandosProcessamento.ProcessarJSON);

		for (File arquivo : arquivos) {
			ComandosProcessamento comando = ComandosProcessamento
					.getComandoPorExtensao(getExtensao(arquivo));

			switch (comando) {
			case ProcessarJSON:
				configJson.incNumTarefas();
				break;
			case ProcessarXML:
				configXml.incNumTarefas();
			default:
				break;
			}
		}

		configuracoes.add(configJson);
		configuracoes.add(configXml);

		return configuracoes;
	}
	/* EXEMPLOS DE MATRIZES PARA SERIALIZAR
		MatrizesProcessar m = new MatrizesProcessar();
		m.matriz1 = new double[][] {
				{1, 2.5d, 5},
				{1.2d, 2.5d, 5},
				{10d, 2.5d, 5}
			};
		
		m.matriz2 = new double[][] {
				{1, 2.5d, 3.5d},
				{12.2d, 1, 5},
				{10d, 2.5d, 1}
			};
		String s = x.toXML(m);*/
}