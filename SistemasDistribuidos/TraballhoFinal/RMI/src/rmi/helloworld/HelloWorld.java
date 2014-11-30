/** HelloWorld.java **/
package rmi.helloworld;

import java.rmi.*;

public interface HelloWorld extends Remote {
   public String hello() throws RemoteException;
}
