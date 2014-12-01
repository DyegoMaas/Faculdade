package arquivos;

import java.net.URL;
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
	        
	        obj.uploadArquivo(new byte[] {1,1,1,0,0,0}, "teste.data");
	        
	        Scanner s = new Scanner(System.in);
	        s.next();
	     } catch (Exception ex) {
	        System.out.println("Exception: " + ex.getMessage());
	     } 
	}
}
