package arquivos;

import java.rmi.Remote;
import java.rmi.RemoteException;

public interface ServicoArquivosRemotos extends Remote{
	String helloWorld() throws RemoteException;
	ListaArquivos listaDiretorios() throws RemoteException;
	ListaArquivos listaArquivos() throws RemoteException;
	void criarPasta(String nomeDiretorio) throws RemoteException;
}
