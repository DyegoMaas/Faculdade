package trabalho1;

import com.google.gson.Gson;


public class Main {

	public static void main(String[] args) {
		System.out.println("Hello world!");

		
		Teste t = new Teste();
		t.Numero = 1;
		t.Texto = "fdsfdf";
		
		Gson gson = new Gson();
		String json = gson.toJson(t);
		
		Teste t2 = gson.fromJson(json, Teste.class);
		
		System.out.println(json);
		System.out.println(t2);
		
		
	}
	
}
