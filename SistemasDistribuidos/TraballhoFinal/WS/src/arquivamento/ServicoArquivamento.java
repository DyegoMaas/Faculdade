package arquivamento;

import java.net.MalformedURLException;
import java.rmi.Naming;
import java.rmi.NotBoundException;
import java.rmi.RemoteException;

import javax.jws.WebService;

import org.omg.CORBA.ORB;
import org.omg.CORBA.ORBPackage.InvalidName;
import org.omg.CosNaming.NamingContextExt;
import org.omg.CosNaming.NamingContextExtHelper;
import org.omg.CosNaming.NamingContextPackage.CannotProceed;
import org.omg.CosNaming.NamingContextPackage.NotFound;

import com.google.gson.Gson;

import arquivos.Constantes;
import arquivos.ListaArquivos;
import arquivos.ServicoArquivosRemotos;
import autenticacao.Servico_Autenticacao;
import autenticacao.Servico_AutenticacaoHelper;

@WebService
public class ServicoArquivamento {
	public String login(String usuario, String senha){
		try {
			Servico_Autenticacao server = obterServicoAutenticacao();
			
	        System.out.println("Autenticando o usuário " + usuario);
	        String tokenRecebido = server.autenticar_usuario(usuario, senha);
	        
	        return tokenRecebido;
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

        return "";
	}
	
	public String criarUsuario(String usuario, String senha){
		try {
			Servico_Autenticacao server = obterServicoAutenticacao();
			
	        System.out.println("cadastrando o usuário " + usuario);
	        String tokenRecebido = server.cadastrar_usuario(usuario, senha);
	        
	        return tokenRecebido;
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

        return "";
	}
	
	public String criarPasta(String nomeDiretorio, String token){
		try {
			if(!validarToken(token))
				throw new IllegalArgumentException("token inválido");
			
			ServicoArquivosRemotos servico = obterServicoArquivos();			
			servico.criarPasta(nomeDiretorio);
			
		} catch (Exception e) {
			e.printStackTrace();
			return "Erro: " + e.getMessage();
		}
		return String.format("diretório %s criado com sucesso", nomeDiretorio);
	}
	
	public String listarDiretorios(String token){
		try {
			if(!validarToken(token))
				throw new IllegalArgumentException("token inválido");
			
			ServicoArquivosRemotos servico = obterServicoArquivos();			
			ListaArquivos listaDiretorios = servico.listaDiretorios();
			
			Gson gson = new Gson();
			return gson.toJson(listaDiretorios);
		} catch (Exception e) {
			e.printStackTrace();
			return e.getMessage();
		}
	}
	
	public String listarArquivos(String token){
		try {
			if(!validarToken(token))
				throw new IllegalArgumentException("token inválido");
			
			ServicoArquivosRemotos servico = obterServicoArquivos();			
			ListaArquivos listaArquivos = servico.listaArquivos();
			
			Gson gson = new Gson();
			return gson.toJson(listaArquivos);
		} catch (Exception e) {
			e.printStackTrace();
			return e.getMessage();
		}
	}
	
	
	private boolean validarToken(String token){
		Servico_Autenticacao servicoAutenticacao;
		try {
			servicoAutenticacao = obterServicoAutenticacao();
		} catch (Exception e) {
			e.printStackTrace();
			return false;
		}
		return servicoAutenticacao.validar_token(token);
	}

	private ServicoArquivosRemotos obterServicoArquivos()
			throws NotBoundException, MalformedURLException, RemoteException {
		String caminhoServico = String.format("//localhost:%d/ServicoArquivosRemotos", Constantes.Porta);
		ServicoArquivosRemotos servico = (ServicoArquivosRemotos)Naming.lookup(caminhoServico);
		return servico;
	}

	private Servico_Autenticacao obterServicoAutenticacao() throws InvalidName, NotFound,
			CannotProceed, org.omg.CosNaming.NamingContextPackage.InvalidName {
		// Cria e inicializa o ORB
        ORB orb = ORB.init();

        // Obtem referencia para o servico de nomes
        org.omg.CORBA.Object objRef = orb.resolve_initial_references("NameService");
        NamingContextExt ncRef = NamingContextExtHelper.narrow(objRef);
   
        // Obtem referencia para o servidor
        String name = "Servico_Autenticacao";
        Servico_Autenticacao server = Servico_AutenticacaoHelper.narrow(ncRef.resolve_str(name));
		return server;
	}
}
