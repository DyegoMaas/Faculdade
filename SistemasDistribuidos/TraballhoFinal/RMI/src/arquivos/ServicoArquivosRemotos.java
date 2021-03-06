package arquivos;

import java.io.IOException;
import java.rmi.Remote;
import java.rmi.RemoteException;

public interface ServicoArquivosRemotos extends Remote{
	ListaArquivos listaDiretorios() throws RemoteException;
	ListaArquivos listaArquivos() throws RemoteException;
	void criarPasta(String nomeDiretorio) throws RemoteException;
	void uploadArquivo(byte[] arquivo, String nomeArquivo) throws RemoteException;
	byte[] downloadArquivo(String nomeArquivo) throws RemoteException, IOException;
}
