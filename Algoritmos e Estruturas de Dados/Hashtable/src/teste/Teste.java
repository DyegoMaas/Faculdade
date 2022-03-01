package teste;

import listas.Hashtable;

public class Teste {
	
	public static void main(String[] args){
		Hashtable h = new Hashtable(10);
		h.set(10, "João10", 7.5f);
		h.set(3, "João3", 7.5f);
		h.set(3, "João32", 7.5f);
		h.set(3, "João33", 7.5f);
		h.set(3, "João34", 7.5f);
		h.set(5, "João5", 7.5f);
		h.set(6, "João6", 7.5f);
		h.set(6, "João62", 7.5f);
		h.set(7, "João7", 7.5f);
		h.set(13, "James", 7.5f);
		
		h.remove(3);
		h.remove(7);
		
		System.out.println("\n" + h);
		
		System.out.println("\n" + h.get(10));
		
	}
	
}
