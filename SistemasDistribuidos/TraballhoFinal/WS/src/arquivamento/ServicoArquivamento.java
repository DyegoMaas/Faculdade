package arquivamento;

import javax.jws.WebService;

import org.omg.CORBA.ORB;
import org.omg.CORBA.ORBPackage.InvalidName;
import org.omg.CosNaming.NamingContextExt;
import org.omg.CosNaming.NamingContextExtHelper;
import org.omg.CosNaming.NamingContextPackage.CannotProceed;
import org.omg.CosNaming.NamingContextPackage.NotFound;

import autenticacao.Servico_Autenticacao;
import autenticacao.Servico_AutenticacaoHelper;

@WebService
public class ServicoArquivamento {
	public String login(String usuario, String senha){
		try {
			Servico_Autenticacao server = obterServicoAutenticacao();
			
			// Imprime mensagem de boas-vindas
	        System.out.println("Autenticando o usuário vader");
	        String tokenRecebido = server.autenticar_usuario(usuario, senha);
	        
	        return tokenRecebido;
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

        return "";
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
