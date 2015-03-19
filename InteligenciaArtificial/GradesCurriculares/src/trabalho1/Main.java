package trabalho1;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.InputStream;
import java.io.StringReader;

import com.google.gson.Gson;
import com.google.gson.JsonIOException;
import com.google.gson.JsonSyntaxException;


public class Main {

	public static void main(String[] args) throws JsonSyntaxException, JsonIOException, FileNotFoundException {
		System.out.println("Hello world!");
	
		
		Gson gson = new Gson();
		GradeCurricular grade = gson.fromJson(new FileReader(new File("input.json")), GradeCurricular.class);
		
		System.out.println(grade);
		//System.out.println(t2);
		
		
		
		
		/**
		 * Agentes:
		 * 
		 * 
		 * Ações possíveis:
		 *	*Alocar materia em um horario de um dia da semana
		 *	*Desalocar matéria de um horário
		 * 
		 * Estado inicial:
		 * 
		 * 
		 * Estado obejtivo:
		 * 
		 * 
		 */
		
	}
	
}
