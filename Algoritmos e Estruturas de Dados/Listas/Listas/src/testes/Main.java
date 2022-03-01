package testes;

import java.util.ArrayList;

public class Main {

	/**
	 * @param args
	 * @author Dyego Maas
	 */
	public static void main(String[] args) {
		Main main = new Main();
		for(ITestadorLista teste : main.getTestes()){
			teste.testar();
			System.out.println();
		}
	}
	
	private ArrayList<ITestadorLista> getTestes(){
		ArrayList<ITestadorLista> testes = new ArrayList<ITestadorLista>();
		
		testes.add(new TesteListaSimplesmenteEncadeada());
		testes.add(new TesteListaDuplamenteEncadeada());
		
		return testes;
	}
	
}
