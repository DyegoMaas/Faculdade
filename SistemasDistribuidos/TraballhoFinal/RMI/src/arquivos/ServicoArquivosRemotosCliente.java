package arquivos;

import java.rmi.Naming;

public class ServicoArquivosRemotosCliente {
	public static void main(String[] args) {
		try {
			ServicoArquivosRemotos obj = (ServicoArquivosRemotos)Naming.lookup("//localhost:2020/ServicoArquivosRemotos"); 
	        System.out.println("Mensagem do Servidor: " + obj.helloWorld()); 
	     } catch (Exception ex) {
	        System.out.println("Exception: " + ex.getMessage());
	     } 
	}
}
