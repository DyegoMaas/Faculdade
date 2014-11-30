package rmi.helloworld;

import java.applet.Applet;
import java.awt.Graphics;
import java.rmi.Naming;

public class HelloApplet extends Applet { 

    String message = "nada"; 
    HelloWorld obj = null; 

    public void init() { 
        try { 
            obj = (HelloWorld)Naming.lookup("//localhost/HelloWorld"); 
            message = obj.hello(); 
        } catch (Exception e) { 
            System.out.println("HelloApplet exception: " + e.getMessage()); 
        } 
    } 

    public void paint(Graphics g) { 
        g.drawString(message, 25, 50); 
    } 
}


