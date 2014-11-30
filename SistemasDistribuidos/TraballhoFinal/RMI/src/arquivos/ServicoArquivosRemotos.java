package arquivos;

import java.rmi.Remote;
import java.rmi.RemoteException;

public interface ServicoArquivosRemotos extends Remote{
	String helloWorld() throws RemoteException;
}
