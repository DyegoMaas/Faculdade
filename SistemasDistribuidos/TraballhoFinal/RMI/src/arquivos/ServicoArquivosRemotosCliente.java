package arquivos;

import java.rmi.Naming;
import java.util.Scanner;

public class ServicoArquivosRemotosCliente {
	public static void main(String[] args) {
		try {	
			String caminhoServico = String.format("//localhost:%d/ServicoArquivosRemotos", Constantes.Porta);
			ServicoArquivosRemotos obj = (ServicoArquivosRemotos)Naming.lookup(caminhoServico); 
				        
	        obj.criarPasta("pasta1");
	        obj.criarPasta("pasta2");
	        
	        
	        ListaArquivos l = obj.listaDiretorios();
	        for(String dir : l.lista){
	        	System.out.println("Diretorio: " + dir);	
	        }
	        
	        ListaArquivos l2 = obj.listaArquivos();
	        for(String arquivos : l2.lista){
	        	System.out.println("Arquivos: " + arquivos);	
	        }
	        
	        byte[] arquivoEnviado = new byte[] {1,1,1,0,0,0};
	        Imprimir("Arquivo enviado: ", arquivoEnviado);
			obj.uploadArquivo(arquivoEnviado, "pasta1/teste.data");
	        
	        byte[] bytes = obj.downloadArquivo("pasta1/teste.data");
	        Imprimir("Arquivo baixado: ", bytes);
	        
	        Scanner s = new Scanner(System.in);
	        s.next();
	     } catch (Exception ex) {
	        System.out.println("Exception: " + ex.getMessage());
	     } 
	}
	
	private static void Imprimir(String descricao, byte[] bytes){
		System.out.print(descricao);
		for(byte b : bytes)
			System.out.print(b + ",");
		System.out.println();
	}
}
