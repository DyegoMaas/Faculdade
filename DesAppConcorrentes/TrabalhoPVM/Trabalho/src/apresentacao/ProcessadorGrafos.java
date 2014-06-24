package apresentacao;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.io.StringReader;
import java.util.ArrayList;
import java.util.List;

import jpvm.jpvmEnvironment;
import jpvm.jpvmException;
import comunicacao.ComandosProcessamento;
import comunicacao.ComandosResposta;
import comunicacao.Mestre;
import comunicacao.Resposta;

public class ProcessadorGrafos {
	
	private static String caminhoDiretorioEntrada = "C:\\pvm\\entrada";
	private static String caminhoDiretorioSaida = "C:\\pvm\\saida";

	//TODO utilizar wait/signal para controlar o processamento dos arquivos
	//TODO criar objeto Mensagem cop cabecalho para encapsular o conteúdo dos arquivos 
	public static void main(String[] args) throws Exception, jpvmException, IOException {
		final Mestre mestre = new Mestre(new jpvmEnvironment());
		
		File diretorioEntrada = new File(caminhoDiretorioEntrada);
		final File[] arquivosParaProcessar = diretorioEntrada.listFiles();
		
		if(arquivosParaProcessar == null){
			System.out.println("Nenhum arquivo para processar");
			return;
		}
				
		configurarMestre(mestre, arquivosParaProcessar);
		
		Thread processadorEntrada = new Thread(new Runnable() {
			@Override
			public void run() {				
				for (File file : arquivosParaProcessar) {					
					try {
						ComandosProcessamento comando = ComandosProcessamento.getComandoPorExtensao(getExtensao(file));
						String conteudoArquivo = getConteudo(file);
						
						
						
						mestre.Enviar(comando, conteudoArquivo);
						
					} catch (Exception | jpvmException e) {
						e.printStackTrace();
					}
				}
			}
		});
		
		final TratadorResposta tratadorResposta = new TratadorResposta(new File(caminhoDiretorioSaida));
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

	private static void configurarMestre(final Mestre mestre, final File[] arquivosParaProcessar) throws jpvmException, IOException, Exception {
		List<Configuracao> configuracoes = obterConfiguracoes(arquivosParaProcessar);
		
		for (Configuracao configuracao : configuracoes) {
			mestre.Adicionar(configuracao.getComando(), configuracao.getNumTarefas());	
		}
	}
	
	private static String getExtensao(File arquivo) throws IOException{
		String caminho = arquivo.getCanonicalPath();
		return caminho.substring(caminho.lastIndexOf('.') + 1);
	}
	
	private static String getConteudo(File arquivo) throws IOException{	
		StringBuffer buffer = new StringBuffer();
		
		 BufferedReader br = new BufferedReader(new FileReader(arquivo));  
         while(br.ready()){  
             buffer.append(br.readLine());
         }  
         br.close();
         
         return buffer.toString();
	}
	
	private static List<Configuracao> obterConfiguracoes(File[] arquivos) throws IOException, Exception{
		ArrayList<Configuracao> configuracoes = new ArrayList<Configuracao>();
				
		Configuracao configXml = new Configuracao(ComandosProcessamento.ProcessarXML);
		Configuracao configJson = new Configuracao(ComandosProcessamento.ProcessarJSON);
		
		for (File arquivo : arquivos) {
			ComandosProcessamento comando = ComandosProcessamento.getComandoPorExtensao(getExtensao(arquivo));
			
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
	
	
}
