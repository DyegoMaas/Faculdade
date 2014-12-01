/** HelloServer.java **/
package arquivos;

import java.rmi.*;
import java.rmi.registry.LocateRegistry;
import java.rmi.server.*;

public class ServicoArquivosRemotosServidor extends UnicastRemoteObject implements ServicoArquivosRemotos {	
	
   public ServicoArquivosRemotosServidor() throws RemoteException {
      super();
   }

   // main()
	public static void main(String[] args) {
	   try {
		   //Runtime.getRuntime().exec("rmiregistry 2020");
		   LocateRegistry.createRegistry(Constantes.Porta);
		   Thread.sleep(500);
		   
		  ServicoArquivosRemotosServidor obj = new ServicoArquivosRemotosServidor();
		  String caminhoServico = String.format("//localhost:%d/ServicoArquivosRemotos", Constantes.Porta);
	      Naming.rebind(caminhoServico, obj);
	      
	      System.out.println("subiu");
	   } catch (Exception ex) {
	      System.out.println("Exception: " + ex.getMessage());
	   } 
	}

	@Override
	public String helloWorld() throws RemoteException {
		// TODO Auto-generated method stub
		return "Olá :)";
	}

	@Override
	public ListaDiretorios listaDiretorios(String diretorioBase) throws RemoteException {
		ListaDiretorios l = new ListaDiretorios();
		l.listaArquivos.add("a");
		l.listaArquivos.add("b");
		return l;
	}
}
