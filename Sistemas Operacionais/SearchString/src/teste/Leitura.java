package teste;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import leituraArquivo.LeitorArquivo;

/**
 * 
 * @author Dyego Alekssander Maas
 *
 */
public class Leitura{
	
	private boolean continueSearching = true;
	
	public void efetuarLeitura(){
		LeitorArquivo leitor = null;
		try {
			leitor = new LeitorArquivo();
			leitor.OpenFile("dic-0294.txt");
			
			try   
			{   
				while(continueSearching){
					BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
					
					System.out.print("Deseja localizar algum conteúdo no arquivo ('S' ou 'N')? R: ");
					String resposta = br.readLine();
					if(!resposta.equalsIgnoreCase("S")){
						System.out.print("Busca encerrada.");	
						break;
					}
					
					System.out.print("Pesquisar no arquivo: ");					
					String entrada = br.readLine();										
					leitor.SearchString(entrada);
				}
			}   
			catch (IOException io)   
			{   
				System.err.println(io.toString()); 
				return;
			}			
			
		} catch (Exception e) {
			// TODO: handle exception
		}
		finally{
			if(leitor != null)
				leitor.CloseFile();
		}
	}
	
}
