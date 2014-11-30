package BoasVindas;

import org.omg.CosNaming.*;
import org.omg.CORBA.*;
import org.omg.PortableServer.*;
import java.io.*;

public class servidor
{
  public static void main(String args[]) {
    try{
      // Cria e inicializa o ORB
      ORB orb = ORB.init(args, null);

      // Cria a implementação e registra no ORB
      Msg_Boas_VindasImpl impl = new Msg_Boas_VindasImpl();

      // Ativa o POA
      POA rootpoa = POAHelper.narrow(orb.resolve_initial_references("RootPOA"));
      rootpoa.the_POAManager().activate();

      // Pega a referência do servidor
      org.omg.CORBA.Object ref = rootpoa.servant_to_reference(impl);
      Msg_Boas_Vindas href = Msg_Boas_VindasHelper.narrow(ref);
	  
      // Obtém uma referência para o servidor de nomes
      org.omg.CORBA.Object objRef = orb.resolve_initial_references("NameService");
      NamingContextExt ncRef = NamingContextExtHelper.narrow(objRef);

      // Registra o servidor no servico de nomes
      String name = "Msg_Boas_Vindas";
      NameComponent path[] = ncRef.to_name( name );
      ncRef.rebind(path, href);

      System.out.println("Servidor aguardando requisicoes ....");

      // Aguarda chamadas dos clientes
      orb.run();
    } catch (Exception e) {
        System.err.println("ERRO: " + e);
        e.printStackTrace(System.out);
    }
    System.out.println("Encerrando o Servidor.");
  }
}
