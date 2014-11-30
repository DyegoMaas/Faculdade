import java.io.IOException;
import java.util.Scanner;
import java.util.UUID;

import org.omg.CORBA.*;
import org.omg.CosNaming.*;
import org.omg.CosNaming.NamingContextPackage.*;
import autenticacao.*;

public class clienteAutenticacaoCorba {

  public static void main(String args[]) {
    try {
      // Cria e inicializa o ORB
      ORB orb = ORB.init(args, null);

      // Obtem referencia para o servico de nomes
      org.omg.CORBA.Object objRef = orb.resolve_initial_references("NameService");
      NamingContextExt ncRef = NamingContextExtHelper.narrow(objRef);
 
      // Obtem referencia para o servidor
      String name = "Servico_Autenticacao";
      Servico_Autenticacao server = Servico_AutenticacaoHelper.narrow(ncRef.resolve_str(name));

      // Imprime mensagem de boas-vindas
      System.out.println("Autenticando o usuário vader");
      String tokenRecebido = server.autenticar_usuario("vader", "luke");
      GerenciadorToken.validarToken(tokenRecebido);
      
      System.out.println("Token recebido: " + tokenRecebido);
      Scanner in = new Scanner(System.in);
      in.next();

    } catch (Exception e) {
        System.out.println("ERROR : " + e) ;
        e.printStackTrace(System.out);
        
        Scanner in = new Scanner(System.in);
        in.next();
    }
    
    
  }
}

