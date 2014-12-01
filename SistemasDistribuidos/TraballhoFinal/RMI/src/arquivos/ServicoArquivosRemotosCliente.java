package arquivos;

import java.rmi.Naming;

public class ServicoArquivosRemotosCliente {
	public static void main(String[] args) {
		try {
			String caminhoServico = String.format("//localhost:%d/ServicoArquivosRemotos", Constantes.Porta);
			ServicoArquivosRemotos obj = (ServicoArquivosRemotos)Naming.lookup(caminhoServico); 
	        System.out.println("Mensagem do Servidor: " + obj.helloWorld()); 
	     } catch (Exception ex) {
	        System.out.println("Exception: " + ex.getMessage());
	     } 
	}
}
