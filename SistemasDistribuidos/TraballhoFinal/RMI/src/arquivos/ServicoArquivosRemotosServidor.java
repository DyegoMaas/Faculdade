/** HelloServer.java **/
package arquivos;

import java.rmi.*;
import java.rmi.server.*;
import java.rmi.registry.*;

public class ServicoArquivosRemotosServidor extends UnicastRemoteObject implements ServicoArquivosRemotos {
   public ServicoArquivosRemotosServidor() throws RemoteException {
      super();
   }

   // main()
	public static void main(String[] args) {
	   try {
		   ServicoArquivosRemotosServidor obj = new ServicoArquivosRemotosServidor();
	      Naming.rebind("//localhost:2020/ServicoArquivosRemotos", obj);
	      
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
}
