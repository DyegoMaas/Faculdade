package teste;

import listas.Hashtable;

public class Teste {
	
	public static void main(String[] args){
		Hashtable h = new Hashtable(10);
		h.set(10, "Jo�o10", 7.5f);
		h.set(3, "Jo�o3", 7.5f);
		h.set(3, "Jo�o32", 7.5f);
		h.set(3, "Jo�o33", 7.5f);
		h.set(3, "Jo�o34", 7.5f);
		h.set(5, "Jo�o5", 7.5f);
		h.set(6, "Jo�o6", 7.5f);
		h.set(6, "Jo�o62", 7.5f);
		h.set(7, "Jo�o7", 7.5f);
		h.set(13, "James", 7.5f);
		
		h.remove(3);
		h.remove(7);
		
		System.out.println("\n" + h);
		
		System.out.println("\n" + h.get(10));
		
	}
	
}
