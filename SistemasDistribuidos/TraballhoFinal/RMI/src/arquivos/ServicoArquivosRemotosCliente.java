package arquivos;

import java.rmi.Naming;
import java.util.Scanner;

public class ServicoArquivosRemotosCliente {
	public static void main(String[] args) {
		try {
			String caminhoServico = String.format("//localhost:%d/ServicoArquivosRemotos", Constantes.Porta);
			ServicoArquivosRemotos obj = (ServicoArquivosRemotos)Naming.lookup(caminhoServico); 
			
	        System.out.println("Mensagem do Servidor: " + obj.helloWorld());
	        ListaDiretorios l = obj.listaDiretorios("");
	        for(String dir : l.listaArquivos){
	        	System.out.println("Diretorio: " + dir);	
	        }
	        Scanner s = new Scanner(System.in);
	        s.next();
	     } catch (Exception ex) {
	        System.out.println("Exception: " + ex.getMessage());
	     } 
	}
}
