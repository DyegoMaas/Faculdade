/** HelloServer.java **/
package rmi.helloworld;

import java.rmi.*;
import java.rmi.server.*;
import java.rmi.registry.*;

public class HelloServer extends UnicastRemoteObject implements HelloWorld {
   public HelloServer() throws RemoteException {
      super();
   }

   // main()
public static void main(String[] args) {
   try {
      HelloServer obj = new HelloServer();
      Naming.rebind("//localhost/HelloWorld", obj);
      System.out.println("subiu");
   } catch (Exception ex) {
      System.out.println("Exception: " + ex.getMessage());
   } 
}

   // hello()
public String hello() throws RemoteException {
   System.out.println("Executando hello()");
   return "Hello!!!";
}


}
