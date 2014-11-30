/** HelloClient.java **/
package rmi.helloworld;

import java.rmi.Naming;

public class HelloClient {
   public static void main(String[] args) {
      try {
         HelloWorld obj = (HelloWorld)Naming.lookup("//localhost/HelloWorld"); 
         System.out.println("Mensagem do Servidor: " + obj.hello()); 
      } catch (Exception ex) {
         System.out.println("Exception: " + ex.getMessage());
      } 
   }
}
