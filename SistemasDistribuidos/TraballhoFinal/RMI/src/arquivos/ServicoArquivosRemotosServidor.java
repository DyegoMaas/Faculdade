/** HelloServer.java **/
package arquivos;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FilenameFilter;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.rmi.*;
import java.rmi.registry.LocateRegistry;
import java.rmi.server.*;

public class ServicoArquivosRemotosServidor extends UnicastRemoteObject implements ServicoArquivosRemotos {	
	
	private static String caminhoStore = Paths.get("/store").toAbsolutePath().toString();
	
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
	public ListaArquivos listaDiretorios() throws RemoteException {
		ListaArquivos retorno = new ListaArquivos();
		
		File diretorio = new File(caminhoStore);
		if(diretorio.exists()) {
			String[] diretorios = diretorio.list(new FilenameFilter() {
				  @Override
				  public boolean accept(File current, String name) {
				    return new File(current, name).isDirectory();
				  }
				});
			if(diretorios != null)
			for(String dir : diretorios){
				retorno.lista.add(dir);
			}
		}
		return retorno;
	}
	
	@Override
	public ListaArquivos listaArquivos() throws RemoteException {
		ListaArquivos retorno = new ListaArquivos();
		
		File diretorio = new File(caminhoStore);
		if(diretorio.exists()){
			File[] arquivos = diretorio.listFiles(new FilenameFilter() {
				  @Override
				  public boolean accept(File current, String name) {
				    return new File(current, name).isFile();
				  }
				});
			if(arquivos != null)
			for(File file : arquivos){
				if(file.isFile())
					retorno.lista.add(file.getName());
			}
		}
		return retorno;
	}
	
	@Override
	public void criarPasta(String nomeDiretorio) throws RemoteException {
		inicializarDiretorioRaiz();
		
		Path caminhoNovoDiretorio = Paths.get(caminhoStore, nomeDiretorio);
		File novoDiretorio = new File(caminhoNovoDiretorio.toString());
		if(!novoDiretorio.exists())
			criarDiretorio(novoDiretorio);
	}
	
	@Override
	public void uploadArquivo(byte[] arquivo, String nomeArquivo) throws RemoteException {
		inicializarDiretorioRaiz();
		
		Path caminhoArquivo = Paths.get(caminhoStore, nomeArquivo);
		try {
			File file = new File(caminhoArquivo.toString());
			System.out.println(file.getAbsolutePath());
			
			FileOutputStream fos = new FileOutputStream(file);
			fos.write(arquivo);
			fos.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	@Override
	public byte[] downloadArquivo(String nomeArquivo) throws RemoteException, IOException {
		inicializarDiretorioRaiz();
		
		Path caminhoArquivo = Paths.get(caminhoStore, nomeArquivo);
		File file = new File(caminhoArquivo.toString());
		System.out.println(file.getAbsolutePath());
		
		return Files.readAllBytes(caminhoArquivo);
	}

	private void inicializarDiretorioRaiz() {
		File diretorioRaiz = new File(caminhoStore);
		if(!diretorioRaiz.exists()){
			criarDiretorio(diretorioRaiz);
		}
	}

	private void criarDiretorio(File diretorio) {
		try {
			diretorio.mkdir();
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
