import java.io.IOException;
import java.util.Scanner;

import BoasVindas.*;
import org.omg.CORBA.*;
import org.omg.CosNaming.*;
import org.omg.CosNaming.NamingContextPackage.*;

public class clienteBoasVindas {

  public static void main(String args[]) {
    try {
      // Cria e inicializa o ORB
      ORB orb = ORB.init(args, null);

      // Obtem referencia para o servico de nomes
      org.omg.CORBA.Object objRef = orb.resolve_initial_references("NameService");
      NamingContextExt ncRef = NamingContextExtHelper.narrow(objRef);
 
      // Obtem referencia para o servidor
      String name = "Msg_Boas_Vindas";
      Msg_Boas_Vindas server = Msg_Boas_VindasHelper.narrow(ncRef.resolve_str(name));

      // Imprime mensagem de boas-vindas
      System.out.println(server.boas_vindas());
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

